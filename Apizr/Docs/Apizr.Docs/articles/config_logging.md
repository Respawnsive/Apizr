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
  - `RequestAll` = `RequestBody | RequestHeaders | RequestCookies`
  - `ResponseBody`
  - `ResponseHeaders`
  - `ResponseAll` = `ResponseBody | ResponseHeaders`
  - `All` = `ResponseAll | RequestAll`
- `logLevels` (default: [Low] `Trace`, [Medium] `Information` and [High] `Critical`) Log levels to apply while writing logs (see Microsoft.Enxtension.Logging), with:
  - `Trace`
  - `Debug`
  - `Information`
  - `Warning`
  - `Error`
  - `Critical`
  - `None`

You can configure logging either by attribute decoration or by fluent configuration.

In both cases, logLevels is a parameter array. It lets you provide from 0 to 3 different levels, as Apizr needs to get corresponding log level to each internal severity:
- Low: logs any internal and normal execution step
- Medium: logs all missconfigured things, like asking for cache without providing any cache provider
- High: logs errors and exceptions

Obviously, providing more than 3 log levels would be pointlees.

It means that:
- if you don't provide any log level at all, default levels will be applied ([Low] `Trace`, [Medium] `Information` and [High] `Critical`)
- if you provide only 1 log level like `Information`, it will be applied to all log entries ([Low] `Information`, [Medium] `Information` and [High] `Information`). Up to you to catch exceptions and to log it at any level of your choice.
- if you provide only 2 log levels like `Debug` and `Error`, the lowest will be applied to both Low and Medium ([Low] `Debug`, [Medium] `Debug` and [High] `Error`)
- if you provide 3 log levels like `Debug`, `Warning` and `Critical`, it will be applied like you said ([Low] `Debug`, [Medium] `Warning` and [High] `Critical`)
- if you provide more than 3 log levels, the lowest goes to Low, the highest to High and it will take the middle one for Medium
- if you provide a `None` at some point, it will disable logging for corresponding severity

### [Attribute](#tab/tabid-attribute)

You can set logging configuration thanks to `Log` attribute.
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
    [WebApi("https://reqres.in/")]
    public interface IReqResService
    {
        [Get("/api/users")]
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

### [Fluent](#tab/tabid-fluent)

Configuring the logging fluently allows you to set it dynamically (e.g. based on settings)

You can set it thanks to this option:

```csharp
// direct configuration
options => options.WithLogging(HttpTracerMode.Everything, HttpMessageParts.All, LogLevel.Information)

// OR static factory configuration
options => options.WithLogging(() => Settings.HttpTracerMode, () => Settings.TrafficVerbosity, () => Settings.LogLevels)

// OR extended factory configuration
options => options.WithLogging(
    serviceProvider => serviceProvider.GetRequiredService<IYourSettingsService>().HttpTracerMode,
    serviceProvider => serviceProvider.GetRequiredService<IYourSettingsService>().TrafficVerbosity
    serviceProvider => serviceProvider.GetRequiredService<IYourSettingsService>().LogLevels)
```

All logging fluent options are available with and without using registry. 
It means that you can share logging configuration, setting it at registry level and/or set some specific one at api level.

***