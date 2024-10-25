## Configuring authentication

Apizr provides its own AuthenticationHandler to manage the authentication workflow .

### Defining

As Apizr is based on Refit, you can decorate your authenticated apis like so (here with bearer authorization):

```csharp
namespace Apizr.Sample
{
    [BaseAddress("https://httpbin.org/")]
    public interface IHttpBinService
    {
        [Get("/bearer")]
        [Headers("Authorization: Bearer")]
        Task<HttpResponseMessage> AuthBearerAsync();
    }
}
```

>[!TIP]
>
>**Fluent Headers**
>
>Note that you can either define headers at registration time with some fluent options.


### Configuring

To activate this feature, you have to configure it thanks to the options builder.

You can do it with both extended and static registrations using local handling methods:
```csharp
options => options.WithAuthenticationHandler(OnGetTokenAsync, OnSetTokenAsync, OnRefreshTokenAsync)

...

private Task<string> OnGetTokenAsync()
{
    // Return local stored token
}

private Task OnSetTokenAsync(string tk)
{
    // Save token to local store
}

private Task<string> OnRefreshTokenAsync(HttpRequestMessage message)
{
    // Refresh the unauthorized token by sending a refreshing request, 
    // or processing a login flow that returns a fresh token.
}

```

#### More

You may want to deal with authentication configuration with deeper control and customizations.
Here are some other authentication options:

##### [Extended](#tab/tabid-extended)

- When you want the token to be saved to and load from a property by Apizr, as well as be refreshed when needed:
```csharp
// by service mappings (both services should be registered in DI)
options => options.WithAuthenticationHandler<ISettingsService, IAuthService>(
    settingsService => settingsService.Token, 
    authService => authService.RefreshAsync)
```

- When you don't want Apizr to refresh the token neither save it, but just load its constant value when needed:
```csharp
// by local handling methods
options => options.WithAuthenticationHandler(OnGetTokenAsync)

// OR by service hanling methods
options => options.WithAuthenticationHandler<ISettingsService>(
    settingsService => settingsService.GetTokenAsync())

// OR by property mapping expression with public getter only
options => options.WithAuthenticationHandler<ISettingsService>(
    settingsService => settingsService.Token)
```

- When you don't want Apizr to refresh the token, but just save and load it when needed:
```csharp
// by local handling methods
options => options.WithAuthenticationHandler(OnGetTokenAsync, OnSetTokenAsync)

// OR by service hanling methods
options => options.WithAuthenticationHandler<ISettingsService>(
    settingsService => settingsService.GetTokenAsync(),
    (settingsService, token) => settingsService.SetTokenAsync(token))

// OR by property mapping expression with public getter and setter
options => options.WithAuthenticationHandler<ISettingsService>(
    settingsService => settingsService.Token)
```

- When you don't want Apizr to save the token anywhere, but just deal with the refresh token method:
```csharp
// by local handling methods
options => options.WithAuthenticationHandler(OnRefreshTokenAsync)

// OR by service hanling methods
options => options.WithAuthenticationHandler<IAuthService>(
    (authService, message) => authService.RefreshTokenAsync(message))
```

- When you want to provide your own `AuthenticationHandlerBase<TWebApi>` open generic implementation:
```csharp
// by open generic auto resolving (need to be registered in DI)
options => options.WithAuthenticationHandler(typeof(YourAuthenticationHandler<>))
...
service.AddTransient(typeof(YourAuthenticationHandler<>)))
```

- When you want to provide your own `AuthenticationHandlerBase` implementation:
```csharp
// by manual instantiation
options => options.WithAuthenticationHandler<YourAuthenticationHandler>(
    (serviceProvider, options) => new YourAuthenticationHandler(...))
```

##### [Static](#tab/tabid-static)

- When you want the token to be saved to and load from a property by Apizr, as well as be refreshed when needed:
```csharp
// by service mappings
options => options.WithAuthenticationHandler<YourSettingsService, YourAuthService>(
    YourSettingsServiceInstance, settingsService => settingsService.Token, 
    YourAuthServiceInstance, authService => authService.RefreshAsync)

// OR by service mapping factory
options => options.WithAuthenticationHandler<YourSettingsService, YourAuthService>(
    () => YourSettingsServiceInstance, settingsService => settingsService.Token, 
    () => YourAuthServiceInstance, authService => authService.RefreshAsync)
```

- When you don't want Apizr to refresh the token neither save it, but just load its constant value when needed:
```csharp
// by local handling methods
options => options.WithAuthenticationHandler(OnGetTokenAsync)

// OR by service hanling methods
options => options.WithAuthenticationHandler<YourSettingsService>(
    YourSettingsServiceInstance, settingsService => settingsService.GetTokenAsync())

// OR by property mapping expression with public getter only
options => options.WithAuthenticationHandler<YourSettingsService>(
    YourSettingsServiceInstance, settingsService => settingsService.Token)

// OR by property mapping expression factory with public getter only
options => options.WithAuthenticationHandler<YourSettingsService>(
    () => YourSettingsServiceInstance, settingsService => settingsService.Token)
```

- When you don't want Apizr to refresh the token, but just save and load it when needed:
```csharp
// by local handling methods
options => options.WithAuthenticationHandler(OnGetTokenAsync, OnSetTokenAsync)

// OR by service hanling methods
options => options.WithAuthenticationHandler<YourSettingsService>(
    YourSettingsServiceInstance, 
    settingsService => settingsService.GetTokenAsync(),
    (settingsService, token) => settingsService.SetTokenAsync(token))

// OR by service factory hanling methods
options => options.WithAuthenticationHandler<YourSettingsService>(
    () => YourSettingsServiceInstance, 
    settingsService => settingsService.GetTokenAsync(),
    (settingsService, token) => settingsService.SetTokenAsync(token))

// OR by property mapping expression with public getter and setter
options => options.WithAuthenticationHandler<YourSettingsService>(
    YourSettingsServiceInstance, settingsService => settingsService.Token)

// OR by property mapping expression factory with public getter and setter
options => options.WithAuthenticationHandler<YourSettingsService>(
    () => YourSettingsServiceInstance, settingsService => settingsService.Token)
```

- When you don't want Apizr to save the token anywhere, but just deal with the refresh token method:
```csharp
// by local handling methods
options => options.WithAuthenticationHandler(OnRefreshTokenAsync)

// OR by service hanling methods
options => options.WithAuthenticationHandler<YourAuthService>(
    YourAuthServiceInstance, (authService, message) => authService.RefreshTokenAsync(message))

// OR by service hanling methods
options => options.WithAuthenticationHandler<YourAuthService>(
    () => YourAuthServiceInstance, (authService, message) => authService.RefreshTokenAsync(message))
```ice.AddTransient(typeof(YourAuthenticationHandler<>)))
```

- When you want to provide your own `AuthenticationHandlerBase` implementation:
```csharp
// by manual instantiation
options => options.WithAuthenticationHandler<YourAuthenticationHandler>(
    (logger, options) => new YourAuthenticationHandler(...))
```

***

### Processing

There's nothing more to deal with.
Protected requests will be authenticated by Apizr thnaks to the get/set methods, otherwise it will call the refresh one.
 
Anyway, here is the AuthenticationHandler's SendAsync method FYI:

```csharp
protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
{
    HttpRequestMessage clonedRequest = null;
    string token = null;

    var context = request.GetOrBuildApizrResilienceContext();
    if (!context.TryGetLogger(out var logger, out var logLevel, out _, out _))
    {
        logger = _logger;
        logLevel = _apizrOptions.LogLevel;
    }

    // See if the request has an authorize header
    var auth = request.Headers.Authorization;
    if (auth != null)
    {
        // Authorization required! Get the token from saved settings if available
        logger?.Log(logLevel, $"{context.OperationKey}: Authorization required with scheme {auth.Scheme}");
        token = await GetTokenAsync();
        if (!string.IsNullOrWhiteSpace(token))
        {
            // We have one, then clone the request in case we need to re-issue it with a refreshed token
            logger?.Log(logLevel, $"{context.OperationKey}: Saved token will be used");
            clonedRequest = await this.CloneHttpRequestMessageAsync(request);
        }
        else
        {
            // Refresh the token
            logger?.Log(logLevel, $"{context.OperationKey}: No token saved yet. Refreshing token...");
            token = await this.RefreshTokenAsync(request).ConfigureAwait(false);
        }

        // Set the authentication header
        request.Headers.Authorization = new AuthenticationHeaderValue(auth.Scheme, token);
        logger?.Log(logLevel, $"{context.OperationKey}: Authorization header has been set");
    }

    // Send the request
    logger?.Log(logLevel, $"{context.OperationKey}: Sending request with authorization header...");
    var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

    // Check if we get an Unauthorized response with token from settings
    if (response.StatusCode == HttpStatusCode.Unauthorized && auth != null && clonedRequest != null)
    {
        logger?.Log(logLevel, $"{context.OperationKey}: Unauthorized !");

        // Refresh the token
        logger?.Log(logLevel, $"{context.OperationKey}: Refreshing token...");
        token = await this.RefreshTokenAsync(request).ConfigureAwait(false);

        // Set the authentication header with refreshed token 
        clonedRequest.Headers.Authorization = new AuthenticationHeaderValue(auth.Scheme, token);
        logger?.Log(logLevel, $"{context.OperationKey}: Authorization header has been set with refreshed token");

        // Send the request
        logger?.Log(logLevel, $"{context.OperationKey}: Sending request again but with refreshed authorization header...");
        response = await base.SendAsync(clonedRequest, cancellationToken).ConfigureAwait(false);
    }

    // Clear the token if unauthorized
    if (response.StatusCode == HttpStatusCode.Unauthorized)
    {
        token = null;
        logger?.Log(logLevel, $"{context.OperationKey}: Unauthorized ! Token has been cleared");
    }

    // Save the refreshed token if succeed or clear it if not
    await SetTokenAsync(token);
    logger?.Log(logLevel, $"{context.OperationKey}: Token saved");

    return response;
}
```

The workflow:

- We check if the request needs to be authenticated
- If so, we try to load a previously saved token
  - If there’s one, we clone the request in case we need to re-issue it with a refreshed token (as token could be rejected server side)
  - If there’s not, we ask for a refreshed one, depending on your `RefreshTokenAsync` implementation (at this stage, a login flow)
- We set the authentication header with the token
- We finally send the request
- We check if we get an Unauthorized response
  - If so and if it was sent with a saved token, we ask for a refreshed one, depending on your `RefreshTokenAsync` implementation (at this stage, a refresh request or a login flow)
  - We set the authentication header of the cloned request with the refreshed token
  - We send the cloned request
- We save the token if succeed or clear it if not
- We return the response
