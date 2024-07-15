## Configuring logging

You can adjust logging configuration with:
- `httpTracerMode` (default: `Everything`) Http traffic tracing mode:
  - `ExceptionsOnly` logs only when an exception occurs
  - `ErrorsAndExceptionsOnly` logs only when an exception or any error occurs
  - `Everything` logs all, anytime
- `trafficVerbosity` (default: `All`) Http traffic tracing verbosity (see HttpTracer), with:
  - `None`
  - `RequestBody`
  - `RequestHeaders`
  - `RequestCookies`
  - `RequestAllButBody` = `RequestHeaders | RequestCookies`,
  - `RequestAll` = `RequestBody | RequestHeaders | RequestCookies`
  - `ResponseBody`
  - `ResponseHeaders`
  - `ResponseAll` = `ResponseBody | ResponseHeaders`
  - `HeadersOnly` = `ResponseHeaders | RequestHeaders`,
  - `AllButRequestBody` = `RequestAllButBody` | `ResponseAll`,
  - `AllButResponseBody` = `RequestAll` | `ResponseHeaders`,
  - `AllButBodies` = `ResponseAll | RequestAllButBody`,
  - `All` = `ResponseAll | RequestAll`
- `logLevels` (default: [Low] `Trace`, [Medium] `Information` and [High] `Critical`) Log levels to apply while writing logs (see Microsoft.Enxtension.Logging), with:
  - `Trace`
  - `Debug`
  - `Information`
  - `Warning`
  - `Error`
  - `Critical`
  - `None`

Note that parameter logLevels is an array. It lets you provide from 0 to 3 different levels, as Apizr needs to get corresponding log level to each internal severity:
- Low: logs any internal and normal execution step
- Medium: logs all missconfigured things, like asking for cache without providing any cache provider
- High: logs errors and exceptions

Obviously, providing more than 3 log levels would be pointless.

It means that:
- if you don't provide any log level at all, default levels will be applied ([Low] `Trace`, [Medium] `Information` and [High] `Critical`)
- if you provide only 1 log level like `Information`, it will be applied to all log entries ([Low] `Information`, [Medium] `Information` and [High] `Information`). Up to you to catch exceptions and to log it at any level of your choice.
- if you provide only 2 log levels like `Debug` and `Error`, the lowest will be applied to both Low and Medium ([Low] `Debug`, [Medium] `Debug` and [High] `Error`)
- if you provide 3 log levels like `Debug`, `Warning` and `Critical`, it will be applied like you said ([Low] `Debug`, [Medium] `Warning` and [High] `Critical`)
- if you provide more than 3 log levels, the lowest goes to Low, the highest to High and it will take the middle one for Medium
- if you provide a `None` at some point, it will disable logging for corresponding severity

You can configure logging at:
- Design time by attribute decoration
- Register time by fluent configuration
- Request time by fluent configuration.

### [Designing](#tab/tabid-designing)

You can set logging configuration at design time, decorating with the provided `Log` attribute.
Configuring logging with attribute allows you to use assembly scanning auto registration feature.

The `Log` attribute could decorate:
- Assembly: to set logging configuration to all assembly api interfaces/entities
- Interface/Class: to set logging configuration to all request methods of the decorated api interface/entity
- Method: to set logging configuration to a specific request method of an api interface or entity (with dedicated attribtes)

You also can mix decoration levels to set a common logging configuration to all assembly api interfaces, 
and/or a specific to all api interface methods, 
and/or a specific to an api interface method.

```csharp
[assembly:Log]
namespace Apizr.Sample
{
    [BaseAddress("https://reqres.in/"), Log(HttpMessageParts.RequestAll, 
        HttpTracerMode.ErrorsAndExceptionsOnly, 
        LogLevel.Information)]
    public interface IReqResService
    {
        [Get("/api/users"), Log(HttpMessageParts.RequestBody, 
            HttpTracerMode.ExceptionsOnly, 
            LogLevel.Warning)]
        Task<UserList> GetUsersAsync();

        [Get("/api/users/{userId}")]
        Task<UserDetails> GetUserAsync([CacheKey] int userId);

        [Post("/api/users")]
        Task<User> CreateUser(User user);
    }
}
```

In this classic api example, we decided to apply the default logging configuration ([Low] `Trace`, [Medium] `Information` and [High] `Critical`) to all assembly api interfaces/entities. 
Then some custom log settings about this specific api.

You’ll find some more log attributes but dedicated to CRUD apis (the ones ending with `Read`, `ReadAll`, `Create`, `Update` or `Delete` suffix), so you could define log settings at any level for CRUD apis too.

Here is CRUD api an example:
```csharp
[assembly:Log]
namespace Apizr.Sample.Models
{
    [CrudEntity("https://reqres.in/api/users", typeof(int), typeof(PagedResult<>))]
    [LogReadAll(HttpMessageParts.RequestAll, 
        HttpTracerMode.ErrorsAndExceptionsOnly, 
        LogLevel.Information)]
    [LogRead(HttpMessageParts.AllButBodies, 
        HttpTracerMode.ExceptionsOnly, 
        LogLevel.Debug)]
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

Again, in this CRUD api example, we decided to apply the default logging configuration ([Low] `Trace`, [Medium] `Information` and [High] `Critical`) to all assembly api interfaces/entities. 
Then some custom log settings about this specific api.

### [Registering](#tab/tabid-registering)

#### Automatically

Logging parameters could be set automatically by providing an `IConfiguration` instance containing the logging settings:
```csharp
options => options.WithConfiguration(context.Configuration)
```

We can set it at common level (to all apis) or specific level (dedicated to a named one).

Please heads to the Settings doc article to see how to configure logging automatically from loaded settings configuration.

#### Manually

Configuring the logging fluently at register time allows you to set it dynamically.

You can set it thanks to this option:

```csharp
// direct configuration
options => options.WithLogging(HttpTracerMode.Everything, HttpMessageParts.All, LogLevel.Information)

// OR static individual factory configuration
options => options.WithLogging(() => Settings.HttpTracerMode, () => Settings.TrafficVerbosity, () => Settings.LogLevels)

// OR static single factory configuration
options => options.WithLogging(() => (Settings.HttpTracerMode, Settings.TrafficVerbosity, Settings.LogLevels))

// OR extended individual factory configuration
options => options.WithLogging(
    serviceProvider => serviceProvider.GetRequiredService<IYourSettingsService>().HttpTracerMode,
    serviceProvider => serviceProvider.GetRequiredService<IYourSettingsService>().TrafficVerbosity
    serviceProvider => serviceProvider.GetRequiredService<IYourSettingsService>().LogLevels)

// OR extended single factory configuration
options => options.WithLogging(servieProvider =>
    {
        var settings = servieProvider.GetRequiredService<IYourSettingsService>();
        return (settings.HttpTracerMode, settings.TrafficVerbosity, settings.LogLevels);
    });
```

All logging fluent options are available with or without using registry. 
It means that you can share logging configuration, setting it at registry level and/or set some specific one at api level.

### [Requesting](#tab/tabid-requesting)

Configuring the logging fluently at request time allows you to set it at the very end, just before sending the request.

First, add the request options parameter `[RequestOptions] IApizrRequestOptions options` to your api methods to provide your logging configuration at request time. 

Then, you can set it thanks to this option:
```csharp
// direct configuration
options => options.WithLogging(HttpTracerMode.Everything, HttpMessageParts.All, LogLevel.Information)
```

***

Note that you can mix design, register and request time logging configurations.
In case of mixed configurations, the internal duplicate strategy will be to take the closest one to the request.

Logging configuration duplicate strategy order:
- take fluent request configuration first if defined (request options)
- otherwise the request attribute decoration one (method)
- otherwise the fluent proper resgistration one (api proper options)
- otherwise the api attribute decoration one (interface)
- otherwise the fluent common resgistration one (registry common options)
- otherwise the global attribute decoration one (assembly)

### Redacting

You may want to hide some header sensitive data from logs.

You can set which header values should be redacted before logging with this option:

```csharp
// direct configuration
options => options.WithLoggedHeadersRedactionNames(new[]{ "MyHeaderKey" })

// factory configuration
options => options.WithLoggedHeadersRedactionRule(header => header == "MyHeaderKey")
```

From there, you should see your logs redacted like so:
```csharp
 ==================== HTTP REQUEST: [GET] ==================== 
GET https://reqres.in/api/users
Request Headers:
MyHeaderKey: *
```

Note that you can mix register and request time redaction configurations. 
Also, the ApizrDuplicateStrategy optional parameter let you tell Apizr whether to override or not any parent redaction rules.