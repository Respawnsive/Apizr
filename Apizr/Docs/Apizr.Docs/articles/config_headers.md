## Configuring Headers

You can configure headers at:
- Design time by attribute decoration
- Register time by fluent configuration
- Request time by fluent configuration

### [Designing](#tab/tabid-designing)

You can set headers at design time, decorating interfaces or methods with the `Headers` attribute provided by Refit.
    
```csharp
[Headers("HeaderKey1: HeaderValue1", "HeaderKey2: HeaderValue2")]
public interface IYourApi
{
    [Headers("HeaderKey3: HeaderValue3")]
    [Get("/your-endpoint")]
    Task<YourData> GetYourDataAsync();
}
```

>[!TIP]
>
> Please refer to Refit official documentation about Headers attribute. Note that decorating assembly is not available with Refit's Headers attribute.

You may want to set key-only headers at design time to provide values fluently at register or request time:

```csharp
[Headers("HeaderKey1:", "HeaderKey2:")]
public interface IYourApi
{
    [Headers("HeaderKey3:")]
    [Get("/your-endpoint")]
    Task<YourData> GetYourDataAsync();
}
```

>[!WARNING]
>
> Key-only headers need its ':' symbol so that Refit include it and then let Apizr process it.

Key-only headers attributes are mostly here to provide some common headers values fluently at register time, using the "Store" regististration mode (instead of "Set"), and deciding to use it or not at design time.

### [Registering](#tab/tabid-registering)

Configuring headers fluently at register time allows you to:
- Set it right the way to the request (default "Set" registration mode) or store it for further attribute key-only match use ("Store" registration mode)
- Load values dynamically (e.g. factory pointing to settings) or not.
- Refresh values at request time ("Request" lifetime scope) or not (default "Api" lifetime scope).

First, please add the request options parameter to your api methods: ```[RequestOptions] IApizrRequestOptions options```

Now you can set headers thanks to this option:

```csharp
// direct configuration
options => options.AddHeaders(["HeaderKey1: HeaderValue1", "HeaderKey2: HeaderValue2"])

// OR factory configuration
options => options.AddHeaders(() => [$"HeaderKey3: {YourHeaderValue3}"])

// OR extended factory configuration with the service provider instance
options => options.AddHeaders(serviceProvider => [$"HeaderKey3: {serviceProvider.GetRequiredService<IYourSettingsService>().YourHeaderValue3}"])

// OR extended factory configuration with your service instance
options => options.AddHeaders<IYourSettingsService>([settings => $"HeaderKey3: {settings.YourHeaderValue3}"])
```

There're many more overloads available with some optional parameters to make it suit your needs (duplicate strategy, lifetime scope and registration mode).
You definitly can mix it all by calling the AddHeaders method multiple times but different ways.
All headers fluent options are available with or without using registry. 
It means that you can share headers configuration, setting it at registry level and/or set some specific one at api level.

### [Requesting](#tab/tabid-requesting)

Configuring the headers fluently at request time allows you to set it at the very end, just before sending the request.

First, please add the request options parameter to your api methods: ```[RequestOptions] IApizrRequestOptions options```

You can now set headers thanks to this option:
```csharp
// direct configuration
options => options.AddHeaders(["HeaderKey1: HeaderValue1", "HeaderKey2: HeaderValue2"])
```

***