## Configuring Timeout

You can configure a timeout at:
- Design time by attribute decoration
- Register time by fluent configuration
- Request time by fluent configuration

### [Designing](#tab/tabid-designing)

Apizr comes with a `Timeout` attribute which set a request timeout at any level (all Assembly apis, interface apis or specific api method).

Here is classic api an example:
```csharp
namespace Apizr.Sample
{
    [WebApi("https://reqres.in/api")]
    public interface IReqResService
    {
        [Get("/users"), Timeout("00:01:00")]
        Task<UserList> GetUsersAsync();

        [Get("/users/{userId}"), Timeout("00:00:30")]
        Task<UserDetails> GetUserAsync([CacheKey] int userId);
    }
}
```

You’ll find also Timeout attributes dedicated to CRUD apis like `TimeoutRead`, `TimeoutReadAll`, `TimeoutCreate`, `TimeoutUpdate` and `TimeoutDelete`, so you could define timeout settings at any level (all Assembly apis, interface apis or specific CRUD method).

Here is CRUD api an example:
```csharp
namespace Apizr.Sample.Models
{
    [CrudEntity("https://reqres.in/api/users", typeof(int), typeof(PagedResult<>))]
    [TimeoutReadAll("00:01:00")]
    [TimeoutRead("00:00:30")]
    public class User
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
```

Both (classic and CRUD) attributes define the same thing about timeout.

The attribute value is actually a `TimeSpan` string representation which is parsed then.

Note that setting a timeout at assembly or interface level actually set the timeout to the HttpClient itslef, where setting it at method level it will be managed by an auto cancelling token.

You definitly can set a global timeout by decorating the assembly or interface, then manage specific scenarios at method level.
Apizr will apply the closest timeout settings to the request it could find. 
But in such scenario, your global timeout (HttpClient) should be longer than your request one (CancellationToken) obviously, otherwise the request one would be useless.

Back to the example, we are saying:

- When getting all users, let’s admit we could have many users registered, so let's set a 1 minute timeout.
- When getting a specific user, the request should be quick, so let's set a 30 secondes timeout.

### [Registering](#tab/tabid-registering)

Configuring a timeout fluently at register time allows you to set it dynamically (e.g. based on settings).

Note that setting a timeout at register time actually set the timeout to the HttpClient itslef, where setting it at request time it will be managed by an auto cancelling token.

You can set a timeout thanks to this option:

```csharp
// direct configuration
options => options.WithTimeout(YOUR_TIMESPAN)

// OR factory configuration
options => options.WithTimeout(() => YOUR_TIMESPAN)

// OR extended factory configuration with the service provider instance
options => options.WithTimeout(serviceProvider => serviceProvider.GetRequiredService<IYourSettingsService>().YOUR_TIMESPAN)
```

All timeout fluent options are available with or without using registry. 
It means that you can share timeout configuration, setting it at registry level and/or set some specific one at api level.

### [Requesting](#tab/tabid-requesting)

Configuring a timeout fluently at request time allows you to set it at the very end, just before sending the request.

First, please add the request options parameter to your api methods: ```[RequestOptions] IApizrRequestOptions options```

You can now set a timeout thanks to this option:
```csharp
// direct configuration
options => options.WithTimeout(YOUR_TIMESPAN)
```

***