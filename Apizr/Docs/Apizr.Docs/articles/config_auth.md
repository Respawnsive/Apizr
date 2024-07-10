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

To activate this feature, you have to configure it thanks to the options builder:

#### [Static](#tab/tabid-static)

```csharp
options => options.WithAuthenticationHandler<YourSettingsService, YourSignInService>(
    YourSettingsServiceInstance, settingsService => settingsService.Token, 
    YourSignInServiceInstance, signInService => signInService.SignInAsync)

// OR with service factory
options => options.WithAuthenticationHandler<YourSettingsService, YourSignInService>(
    () => YourSettingsServiceInstance, settingsService => settingsService.Token, 
    () => YourSignInServiceInstance, signInService => signInService.SignInAsync)
```

- `YourSettingsServiceInstance` should be replaced by whatever settings manager instance of your choice
- `YourSignInServiceInstance` should be replaced by your service managing your login flow.

#### [Extended](#tab/tabid-extended)

```csharp
options => options.WithAuthenticationHandler<ISettingsService, ISignInService>(
    settingsService => settingsService.Token, 
    signInService => signInService.SignInAsync)
```

- `settingsService` is your service managing settings
- `signInService` is your service managing your login flow.

Both services should be container registered as it will be resolved.

***

In details:

- `settingsService.Token` should be a public string property, saved locally on device.
- `signInService.SignInAsync` should be a method taking an HttpRequestMessage parameter and returning a refreshed access token.

#### More

You may want to deal with authentication configuration in some other ways.
Here are all other authentication options:

##### [Static](#tab/tabid-static)

- When you don't want Apizr to save the token anywhere neither refresh it, but just want to load it when needed:
```csharp
options => options.WithAuthenticationHandler<YourSettingsService>(
    YourSettingsServiceInstance, settingsService => settingsService.Token)

// OR with service factory
options => options.WithAuthenticationHandler<YourSettingsService>(
    () => YourSettingsServiceInstance, settingsService => settingsService.Token)
```
`settingsService.Token` should be here a public string property with a private setter, containing the token.

- When you don't want Apizr to save the token anywhere but want to deal with the refresh token call with a method:
```csharp
options => options.WithAuthenticationHandler(OnRefreshToken)
...
private string OnRefreshTokden(HttpRequestMessage message)
{
    // whatever returning a refreshed string token
}
```

- When you want to deal with the refresh token call with a method:
```csharp
options => options.WithAuthenticationHandler<YourSettingsService>(
    YourSettingsServiceInstance, settingsService => settingsService.Token,
    OnRefreshToken)

// Or with service factory
options => options.WithAuthenticationHandler<YourSettingsService>(
    () => YourSettingsServiceInstance, settingsService => settingsService.Token,
    OnRefreshToken)
...
private string OnRefreshTokden(HttpRequestMessage message)
{
    // whatever returning a refreshed string token
}
```

- When you want to provide your own AuthenticationHandlerBase implementation:
```csharp
options => options.WithAuthenticationHandler<YourAuthenticationHandler>(
    (logger, options) => new YourAuthenticationHandler(...))
```

##### [Extended](#tab/tabid-extended)

- When you don't want Apizr to save the token anywhere neither refresh it, but just want to load it when needed:
```csharp
options => options.WithAuthenticationHandler<ISettingsService>(
    settingsService => settingsService.Token)
```
`settingsService.Token` should be here a public string property with a private setter, containing the token.

- When you don't want Apizr to save the token anywhere but want to deal with the refresh token call with a method:
```csharp
options => options.WithAuthenticationHandler(OnRefreshToken)
...
private string OnRefreshTokden(HttpRequestMessage message)
{
    // whatever returning a refreshed string token
}
```

- When you want to deal with the refresh token call with a method:
```csharp
options => options.WithAuthenticationHandler<ISettingsService>(
    settingsService => settingsService.Token, OnRefreshToken)
...
private string OnRefreshTokden(HttpRequestMessage message)
{
    // whatever returning a refreshed string token
}
```

- When you want to provide your own AuthenticationHandlerBase implementation:
```csharp
options => options.WithAuthenticationHandler<YourAuthenticationHandler>(
    (serviceProvider, options) => new YourAuthenticationHandler(...))
```

***

### Processing

There's nothing more to deal with.
Protected requests will be authenticated by Apizr, otherwise it will ask user to sign in.
 
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
        token = GetToken();
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
    this.SetToken(token);
    logger?.Log(logLevel, $"{context.OperationKey}: Token saved");

    return response;
}
```

The workflow:

- We check if the request needs to be authenticated
- If so, we try to load a previously saved token
  - If there’s one, we clone the request in case we need to re-issue it with a refreshed token (as token could be rejected server side)
  - If there’s not, we ask for a refreshed one (launching your signin feature and waiting for the resulting token)
- We set the authentication header with the token
- We finally send the request
- We check if we get an Unauthorized response
  - If so and if it was sent with a saved token, we ask for a refreshed one (launching your signin feature and waiting for the resulting token)
  - We set the authentication header of the cloned request with the refreshed token
  - We send the cloned request
- We save the token if succeed or clear it if not
- We return the response