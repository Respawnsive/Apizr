## Configuring Headers

You can configure headers at:
- Design time by attribute decoration
- Register time by fluent configuration
- Request time by fluent configuration

### [Designing](#tab/tabid-designing)

You can set headers at design time, decorating interfaces or methods with the `Headers` attribute provided by Refit.

You definitly can set a global header by decorating an interface, then manage specific scenarios at method level.
Apizr will apply the closest header value to the request it could find. 

>[!TIP]
>
> Please refer to Refit official documentation about header attribute. Note that decorating assembly is not available with Headers attribute.

### [Registering](#tab/tabid-registering)

Configuring the headers fluently at register time allows you to set it dynamically (e.g. based on settings).

First, please add the request options parameter to your api methods: ```[RequestOptions] IApizrRequestOptions options```

Now you can set headers thanks to this option:

```csharp
// direct configuration
options => options.AddHeaders("HeaderKey1: HeaderValue1", "HeaderKey2: HeaderValue2")

// OR factory configuration
options => options.AddHeaders(() => $"HeaderKey3: {YourHeaderValue3}")

// OR extended factory configuration with the service provider instance
options => options.AddHeaders(serviceProvider => $"HeaderKey3: {serviceProvider.GetRequiredService<IYourSettingsService>().YourHeaderValue3}")
```

All headers fluent options are available with or without using registry. 
It means that you can share headers configuration, setting it at registry level and/or set some specific one at api level.

### [Requesting](#tab/tabid-requesting)

Configuring the headers fluently at request time allows you to set it at the very end, just before sending the request.

First, please add the request options parameter to your api methods: ```[RequestOptions] IApizrRequestOptions options```

You can now set headers thanks to this option:
```csharp
// direct configuration
options => options.AddHeaders("HeaderKey1: HeaderValue1", "HeaderKey2: HeaderValue2")
```

***