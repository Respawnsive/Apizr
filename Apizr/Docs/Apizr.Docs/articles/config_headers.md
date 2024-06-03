## Configuring Headers

You can set headers with static or dynamic values, with clear or redacted logged values (if logging is enabled with headers included).

>[!TIP]
> You should add the request options parameter `[RequestOptions] IApizrRequestOptions options` to your api methods to get all the Apizr goodness.

### Static headers

#### [Designing](#tab/tabid-design)

You can set headers with static values at design time by decorating interfaces or methods with the `Headers` attribute provided by Refit.
    
```csharp
[Headers("HeaderKey1: HeaderValue1", "HeaderKey2: HeaderValue2")]
public interface IYourApi
{
    [Headers("HeaderKey3: HeaderValue3")]
    [Get("/your-endpoint")]
    Task<YourData> GetYourDataAsync([RequestOptions] IApizrRequestOptions options);
}
```

>[!NOTE]
>
> Please refer to Refit official documentation about Headers attribute with static values. 
> Note that decorating assembly to share headers between several api interfaces is not available with Refit's Headers attribute, 
> but you can do it with fluent configuration at register time.

>[!TIP]
> CRUD api headers could be set using provided dedicted method headers attributes (`ReadAllHeaders, ...`) 

#### [Registering](#tab/tabid-register)

You can set headers with static values at register time by configuring fluent options:

```csharp
// direct configuration
options => options.WithHeaders(["HeaderKey1: HeaderValue1", "HeaderKey2: HeaderValue2"])

// OR static factory configuration
options => options.WithHeaders(() => ["HeaderKey1: HeaderValue1", "HeaderKey2: HeaderValue2"])

// OR extended factory configuration
options => options.WithHeaders(_ => ["HeaderKey1: HeaderValue1", "HeaderKey2: HeaderValue2"])
```

>[!TIP]
>
> You can share headers between several api interfaces by configuring fluent options at registry common level.

#### [Requesting](#tab/tabid-request)

You can set headers with static values at request time by configuring fluent options:

```csharp
options => options.WithHeaders(["HeaderKey1: HeaderValue1", "HeaderKey2: HeaderValue2"])
```

***

You definitly can mix it all as Apiz will merge your headers at the very end while sending the request.

### Dynamic headers

#### Setting dynamic headers

#### [Designing](#tab/tabid-design)

You can set headers with dynamic values at design time by:
- Decorating an api method parameter with the `Header` or `HeaderCollection` attribute
- Decorating interfaces or methods with the `Headers` attribute using key matching

##### Parameter header

You can set headers with dynamic values at design time by decorating an api method parameter with the `Header` attribute:
```csharp
public interface IYourApi
{
    [Get("/your-endpoint")]
    Task<YourData> GetYourDataAsync([Header("HeaderKey1")] string headerValue1, [RequestOptions] IApizrRequestOptions options);
}
```

 or `HeaderCollection` attribute:
```csharp
public interface IYourApi
{
    [Get("/your-endpoint")]
    Task<YourData> GetYourDataAsync([HeaderCollection] IDictionary<string, string> headers, [RequestOptions] IApizrRequestOptions options);
}
```

Please refer to Refit official documentation about `Header` and `HeaderCollection` attributes with dynamic values.

##### Key matching header

You can set headers with dynamic values at design time by decorating interfaces or methods with the `Headers` attribute and using key matching:
```csharp
[Headers("HeaderKey1: {0}", "HeaderKey2: {0}")]
public interface IYourApi
{
    [Headers("HeaderKey3: {0}")]
    [Get("/your-endpoint")]
    Task<YourData> GetYourFirstDataAsync([RequestOptions] IApizrRequestOptions options);

    [Get("/your-endpoint")]
    Task<YourData> GetYourSecondDataAsync([RequestOptions] IApizrRequestOptions options);
}
```

Here we are asking Apizr to set headers 1, 2 and 3 to `GetYourFirstDataAsync` and the same but 3 to `GetYourSecondDataAsync`.
It's here to let you choose at design time which request needs which headers, but provide values later in one place.
So we don't provide any value here but the `{0}` string placeholder and let Apizr set it at request time from its headers store if keys match.

>[!TIP]
> CRUD api headers could be set using provided dedicted method headers attributes (`ReadAllHeaders, ...`) 

>[!WARNING]
>
> Key matching headers need you to provide values fluently at register time with `Store` registration mode (see Registering tab).

#### [Registering](#tab/tabid-register)

##### Setting headers

You can set headers with dynamic values at register time by configuring fluent options:
```csharp
// expression factory configuration
options => options.WithHeaders(settingsService, [settings => settings.Header1, settings => settings.Header2])

// OR extended expression factory configuration
options => options.WithHeaders<IOptions<TestSettings>>([settings => settings.Value.Header1, settings => settings.Value.Header2])
```

>[!TIP]
>
> You can share headers between several api interfaces by configuring fluent options at registry common level.

##### Storing headers

By storing headers, Apizr will set it at request time only if keys match from Headers attribute decoration. It lets you provide values once fluently at register time, but apply it only where you actually decided to put the Headers attribute with the same key.

>[!WARNING]
>
> Don't forget to design your api interfaces with key matching headers (see Designing tab).

To store values for further headers attribute key match use, you have to tell it to Apizr by registering it with the `Store` registration mode:
```csharp
// expression factory configuration
options => options.WithHeaders(settingsService, [settings => settings.Header1, 
    settings => settings.Header2], 
    mode: ApizrRegistrationMode.Store)

// OR extended expression factory configuration
options => options.WithHeaders<IOptions<TestSettings>>([settings => settings.Value.Header1, 
    settings => settings.Value.Header2], 
    mode: ApizrRegistrationMode.Store)
```

***

You definitly can mix it all as Apiz will merge your headers at the very end while sending the request.

#### Refreshing dynamic header values

You may want to refresh your dynamic header values on each request.

If so, you can set your header values at register time with the `Request` lifetime scope (instead of the `Api` default one):
```csharp
// expression factory configuration
options => options.WithHeaders(settingsService, [settings => settings.Header1, 
    settings => settings.Header2], 
    scope: ApizrLifetimeScope.Request)

// OR extended expression factory configuration
options => options.WithHeaders<IOptions<TestSettings>>([settings => settings.Value.Header1, 
    settings => settings.Value.Header2], 
    scope: ApizrLifetimeScope.Request)
```

### Redacting logged header values

You may want to log http traces, including headers, but be concerned about its values sensitivity.

In such ssenario, you should redact header values so that logs would never contain any header sensitive value but a `*` replacement.

#### [Designing](#tab/tabid-design)

You can tell Apizr to do so by surrounding values with a `*` and `*` star symbol into the `Headers` attribute:
```csharp
[Headers("HeaderKey1: *HeaderValue1*", "HeaderKey2: HeaderValue2")]
public interface IYourApi
{
    [Headers("HeaderKey3: *HeaderValue3*", "HeaderKey4: *{0}*)]
    [Get("/your-endpoint")]
    Task<YourData> GetYourDataAsync([RequestOptions] IApizrRequestOptions options);
}
```

Here we are asking Apizr to redact both headers 1 and 3 values, but also key matching header 4 value.

>[!TIP]
> CRUD api headers could be set using provided dedicted method headers attributes (`ReadAllHeaders, ...`) 

#### [Registering](#tab/tabid-register)

You can tell Apizr to do so by surrounding values with a `*` and `*` star symbol while adding headers fluently:
```csharp
options => options.WithHeaders(["HeaderKey1: *HeaderValue1*", "HeaderKey2: HeaderValue2"])
```

Here we are asking Apizr to redact header 1 value, but leave the 2 clear in logs.

Or you can tell the same with dedicated fluent options:
```csharp
// By header names
options => options.WithLoggedHeadersRedactionNames(["testKey2"])

// OR by any rules
options => options.WithLoggedHeadersRedactionRule(header => header == "testKey3")
```

#### [Requesting](#tab/tabid-request)

You can tell Apizr to do so by surrounding values with `*` and `*` star symbol while adding headers fluently:
```csharp
options => options.WithHeaders(["HeaderKey1: *HeaderValue1*", "HeaderKey2: HeaderValue2"])
```

Here we are asking Apizr to redact header 1 value, but leave the 2 clear in logs.

Or you can tell the same with dedicated fluent options:
```csharp
// By header names
options => options.WithLoggedHeadersRedactionNames(["testKey2"])

// OR by any rules
options => options.WithLoggedHeadersRedactionRule(header => header == "testKey3")
```

***