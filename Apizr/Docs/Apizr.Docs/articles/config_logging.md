## Configuring logging

You can adjust logging configuration with:
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
- `httpTracerMode` (default: `Everything`) Http traffic tracing mode:
  - `ExceptionsOnly` logs only when an exception occurs
  - `ErrorsAndExceptionsOnly` logs only when an exception or any error occurs
  - `Everything` logs all, anytime
- `logLevel` (default: `Trace`) Log level to apply while writing logs (see Microsoft.Enxtension.Logging), with:
  - `Trace`
  - `Debug`
  - `Information`
  - `Warning`
  - `Error`
  - `Critical`
  - `None`

You can configure logging either by attribute decoration or by fluent configuration.

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

In this example, we decided to apply the default logging configuration to all assembly api interfaces/entities. 
But you can adjust logging configuration thanks to attribute parameters.

### [Fluent](#tab/tabid-fluent)

Configuring the logging fluently allows you to set it dynamically (e.g. based on settings)

You can set it thanks to this option:

```csharp
// direct configuration
options => options.WithLogging(HttpTracerMode.Everything,, HttpMessageParts.All, LogLevel.Information)

// OR static factory configuration
options => options.WithLogging(() => Settings.HttpTracerMode, () => Settings.TrafficVerbosity, () => Settings.LogLevel)

// OR extended factory configuration
options => options.WithLogging(
    serviceProvider => serviceProvider.GetRequiredService<IYourSettingsService>().HttpTracerMode,
    serviceProvider => serviceProvider.GetRequiredService<IYourSettingsService>().TrafficVerbosity
    serviceProvider => serviceProvider.GetRequiredService<IYourSettingsService>().LogLevel)
```

All logging fluent options are available with and without using registry. 
It means that you can share logging configuration setting it at registry level and/or set some specific one at api level.

***