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
    [WebApi("https://reqres.in/"), Log(HttpMessageParts.RequestAll, 
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

In this example, we decided to apply the default logging configuration ([Low] `Trace`, [Medium] `Information` and [High] `Critical`) to all assembly api interfaces/entities. 
But you can adjust logging configuration thanks to attribute parameters.

### [Registering](#tab/tabid-registering)

Configuring the logging fluently at register time allows you to set it dynamically (e.g. based on settings)

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

You can set it thanks to this option:

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