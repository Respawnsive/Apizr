## Settings

Most of Apizr settings could be set by providing an `IConfiguration` instance.
We can set them at common level (shared by all apis) or specific level (dedicated to named apis).
The following doc acrticle will focus on appsettings.json configuration.

>[!TIP]
> You should add the request options parameter `[RequestOptions] IApizrRequestOptions options` to your api methods to get all the Apizr goodness. 
>
>If not, some configurations may not be applied (such as Polly, Cancellation, Timeout, Priority, etc...).

### Defining

Here is an example of an appsettings.json file with some of the settings that could be set:

```json
{
  "Logging": {
    "LogLevel": { // No provider, LogLevel applies to all the enabled providers.
      "Default": "Trace", // Default, application level if no other level applies
      "Microsoft": "Warning", // Log level for log category which starts with text 'Microsoft' (i.e. 'Microsoft.*')
      "Microsoft.Extensions.Http.DefaultHttpClientFactory": "Information"
    }
  },
  "Apizr": {
    "Common": {
      "Logging": {
        "HttpTracerMode": "Everything",
        "TrafficVerbosity": "All",
        "LogLevels": [ "Trace", "Information", "Critical" ]
      },
      "OperationTimeout": "00:00:10",
      "LoggedHeadersRedactionNames": [ "testSettingsKey1" ],
      "ResilienceContext": {
        "ContinueOnCapturedContext": false,
        "ReturnContextToPoolOnComplete": true
      },
      "Headers": [
        "testSettingsKey6: testSettingsValue6.1"
      ],
      "ResiliencePipelineOptions": {
        "HttpGet": [ "TestPipeline3" ]
      },
      "Caching": {
        "Mode": "GetAndFetch",
        "LifeSpan": "00:15:00",
        "ShouldInvalidateOnError": false
      }
    },
    "IReqResSimpleService": {
      "BaseAddress": "https://reqres.in/api",
      "RequestTimeout": "00:00:03",
      "Headers": [
        "testSettingsKey2: testSettingsValue2.1",
        "testSettingsKey3: *testSettingsValue3.1*",
        "testSettingsKey4: {0}",
        "testSettingsKey5: *{0}*"
      ],
      "Caching": {
        "Mode": "GetAndFetch",
        "LifeSpan": "00:12:00",
        "ShouldInvalidateOnError": true
      },
      "ResiliencePipelineKeys": [ "TestPipeline3" ]
    },
    "User": {
      "BaseAddress": "https://reqres.in/api/users",
      "RequestTimeout": "00:00:05",
      "Headers": [
        "testSettingsKey8: testSettingsValue8.1"
      ]
    }
  }
}
```

- You first have to start with the `Apizr` root section key.
- Then you can define settings at common level with the `Common` section key, or specific level with the name of apis as section keys (here `IReqResSimpleService` classic api and `User` CRUD api).
- Finally you can adjust following available settings:
  - `BaseAddress` (string): specifies the base API address
  - `BasePath` (string): specifies the base API address path
  - `Logging` (section): contains settings related to logging
    - `HttpTracerMode` (enum member name): specifies the [mode](/api/Apizr.Logging.HttpTracerMode.html) for HTTP tracing
    - `TrafficVerbosity` (enum member name): specifies the [verbosity](/api/Apizr.Logging.HttpMessageParts.html) level for traffic logging
    - `LogLevels` (enum member names array): specifies the [log levels](https://learn.microsoft.com/fr-fr/dotnet/api/microsoft.extensions.logging.loglevel) to use
  - `OperationTimeout` (TimeSpan representation): specifies the timeout for an API operation of multiple requests
  - `RequestTimeout` (TimeSpan representation): specifies the timeout for an API request
  - `LoggedHeadersRedactionNames` (string array): specifies the header keys to be redacted in logs
  - `ResilienceContext` (section): contains settings related to the resilience context
    - `ContinueOnCapturedContext` (bool): specifies whether to continue on the captured context
    - `ReturnContextToPoolOnComplete` (bool): specifies whether to return the context to the pool on completion
  - `Headers` (string array): specifies custom headers to be added to requests
  - `ResiliencePipelineKeys` (string array): specifies the resilience pipeline keys to use
  - `ResiliencePipelineOptions` (dictionary): specifies the resilience pipeline keys to use but scoped to specific request [method groups](/api/Apizr.Configuring.ApizrRequestMethod.html)
  - `Caching` (section): contains settings related to caching
    - `Mode` (enum member name): specifies the [caching mode](/api/Apizr.Caching.CacheMode.html)
    - `LifeSpan` (TimeSpan representation): specifies the lifespan of cached responses
    - `ShouldInvalidateOnError` (bool): specifies whether to invalidate the cache on error

### Registering

Once settings has been adjusted to your needs, you just have to provide an `IConfiguration` instance to Apizr with the dedicated fluent option at any registration level:

```csharp
options => options.WithConfiguration(context.Configuration)
```

>[!NOTE]
>
> - Apizr will first load common settings, then specific settings, so specific settings will override the common ones. The same behavior as usual with fluent options registration actually.
> - Order matters, meaning that you should first register the configuration from settings, then override it if needed with fluent options.

If you want to organize your settings in a more custom way, you can provide custom configuration section keys to Apizr at any registration level:

```csharp
options => options.WithConfiguration(context.Configuration.GetSection("My:Custom:Section"))
```

But you still have to conform to the settings structure described above.