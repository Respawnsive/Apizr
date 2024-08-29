## Configuring Timeout

There's actually two kind of client timeout:
- Request timeout which is set to each request try
- Operation timeout which is set to overall request tries

Both of it will throw a TimeoutRejectedException when time is out.
- If you configured a retry policy handling TimeoutRejectedException:
  - with a request timeout, the request will be sent again by Polly with your defined request timeout set to each individual try.
  - with an operation timeout, Polly will stop sending retries if the operation timeout timed out.
- Otherwise, if you didn't configure any retry policy handling TimeoutRejectedException, request timeout will behave like an operation timeout, so it doesn't matter which one you defined.

You can configure a timeout at:
- Design time by attribute decoration
- Register time by fluent configuration
- Request time by fluent configuration

### [Designing](#tab/tabid-designing)

Apizr comes with a `RequestTimeout` and an `OperationTimeout` attribute which set a timeout at any level (all Assembly apis, interface apis or specific api method).

Also, please add the request options parameter `[RequestOptions] IApizrRequestOptions options` to your api methods to ensure your timeouts will be applied and don't forget to pass the options to your api methods at request time.

Here is classic api an example:
```csharp
namespace Apizr.Sample
{
    [BaseAddress("https://reqres.in/api"), OperationTimeout("00:03:00")]
    public interface IReqResService
    {
        [Get("/users"), RequestTimeout("00:01:00")]
        Task<UserList> GetUsersAsync([RequestOptions] IApizrRequestOptions options);

        [Get("/users/{userId}"), RequestTimeout("00:00:30")]
        Task<UserDetails> GetUserAsync([CacheKey] int userId, [RequestOptions] IApizrRequestOptions options);
    }
}
```

You’ll find the same timeout attributes dedicated to CRUD apis (the ones starting with `Read`, `ReadAll`, `Create`, `Update` and `Delete` prefix), so you could define timeout settings at any level for CRUD apis too.

Here is CRUD api an example:
```csharp
namespace Apizr.Sample.Models
{
    [BaseAddress("https://reqres.in/api/users")]
    [OperationTimeout("00:03:00")]
    [ReadAllRequestTimeout("00:01:00")]
    [ReadRequestTimeout("00:00:30")]
    public record User
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("first_name")]
        public string FirstName { get; init; }

        [JsonPropertyName("last_name")]
        public string LastName { get; init; }

        [JsonPropertyName("avatar")]
        public string Avatar { get; init; }

        [JsonPropertyName("email")]
        public string Email { get; init; }
    }
}
```

Both (classic and CRUD) attributes define the same thing about timeout.

The attribute value is actually a `TimeSpan` string representation which is parsed then.

You definitly can set a global timeout by decorating the assembly or interface, then manage specific scenarios at method level.
Apizr will apply the closest timeout settings to the request it could find. 

Back to previous examples, we are saying that:
- in details, any request try shouldn't take longer than:
  - 1 min for GetUsers/ReadAll
  - 30 sec for GetUser/Read
- in general, we don't want the user to wait too much, so let's retry if needed, but not longer than 3 min overall.

### [Registering](#tab/tabid-registering)

#### Automatically

Timeout could be set automatically by providing an `IConfiguration` instance containing the timeout settings:
```csharp
options => options.WithConfiguration(context.Configuration)
```

We can set it at common level (to all apis), specific level (dedicated to a named api) or even request level (dedicated to a named api's method).

Please heads to the [Settings](config_settings.md))  doc article to see how to configure timeouts automatically from loaded settings configuration.

#### Manually

You can set a request timeout thanks to this option:

```csharp
// direct configuration
options => options.WithRequestTimeout(YOUR_TIMESPAN)

// OR factory configuration
options => options.WithRequestTimeout(() => YOUR_TIMESPAN)

// OR extended factory configuration with the service provider instance
options => options.WithRequestTimeout(serviceProvider => serviceProvider.GetRequiredService<IYourSettingsService>().YOUR_TIMESPAN)
```

And/or you can set an operation timeout thanks to this option:

```csharp
// direct configuration
options => options.WithOperationTimeout(YOUR_TIMESPAN)

// OR factory configuration
options => options.WithOperationTimeout(() => YOUR_TIMESPAN)

// OR extended factory configuration with the service provider instance
options => options.WithOperationTimeout(serviceProvider => serviceProvider.GetRequiredService<IYourSettingsService>().YOUR_TIMESPAN)
```

All timeout fluent options are available with or without using registry. 
It means that you can share timeout configuration, setting it at registry level and/or set some specific one at api level.

### [Requesting](#tab/tabid-requesting)

Configuring a timeout fluently at request time allows you to set it at the very end, just before sending the request.

First, please add the request options parameter to your api methods: ```[RequestOptions] IApizrRequestOptions options```

You can set a request timeout thanks to this option:

```csharp
// direct configuration
options => options.WithRequestTimeout(YOUR_TIMESPAN)
```

And/or you can set an operation timeout thanks to this option:

```csharp
// direct configuration
options => options.WithOperationTimeout(YOUR_TIMESPAN)
```

***