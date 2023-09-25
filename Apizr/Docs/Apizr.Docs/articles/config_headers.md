## Configuring Headers

You can configure headers at:
- Design time by attribute decoration
- Register time by fluent configuration
- Request time by fluent configuration

### [Designing](#tab/tabid-designing)

You can set headers at design time, decorating interfaces or methods with the `Headers` attribute provided by Refit.

But for now, this attribute approach won't let you:
- set global/shared headers (other than repeating yourself decorating each interfaces or inheriting from a decorated one)
- set computed/calculated headers (other than intercepting it on the handler side and changing its value before sending or providing it as a request parameter)

### [Registering](#tab/tabid-registering)

Configuring the headers fluently at register time allows you to set it dynamically (e.g. based on settings).

Note that setting headers at register time actually set the headers to the HttpClient itslef, where setting it at request time it will be added on sending.

You can set headers thanks to this option:

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

You can do the same thing with the Refit's Header parameter attribute, but you'll have to design your api with this special parameter.

***