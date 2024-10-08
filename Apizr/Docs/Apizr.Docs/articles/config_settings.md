﻿## Settings

Most of Apizr settings could be set by providing an `IConfiguration` instance.
We can set it at common level (to all apis), specific level (dedicated to a named api) or even request level (dedicated to a named api's method).
The following doc article will focus on appsettings.json configuration.

>[!TIP]
> - You must add the request options parameter `[RequestOptions] IApizrRequestOptions options` to your api methods to get all the Apizr goodness. 
>If not, some configurations may not be applied (such as Polly, Cancellation, Timeout, Priority, etc...).
>
>- Non hosted environments (like MAUI) could definitly use appsettings.json goodness too, but by using both embedded resource file loading and compile time conditional file copying. 
>Look at both csproj and MauiProgram files from the [MAUI Sample](https://github.com/Respawnsive/Apizr/tree/master/Apizr/Samples/Apizr.Sample.MAUI) to get a picture of the workaround and note that appsettings files won't be merged in that case but replaced.

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
    "ResilienceOptions": { // Root Microsoft Resilience configuration section key (see Polly article)
      "Retry": { // Retry configuration section key
        "BackoffType": "Exponential",
        "UseJitter": true,
        "MaxRetryAttempts": 3
      }
    },
    "Apizr": { // Root Apizr configuration section key
        "CommonOptions": { // Common options shared by all apis
            "Logging": { // Common logging settings
                "HttpTracerMode": "Everything",
                "TrafficVerbosity": "All",
                "LogLevels": ["Trace", "Information", "Critical"]
            },
            "OperationTimeout": "00:00:10", // Common operation timeout
            "LoggedHeadersRedactionNames": ["testSettingsKey1"], // Headers to common redact in logs
            "ResilienceContext": { // Common resilience context settings
                "ContinueOnCapturedContext": false,
                "ReturnContextToPoolOnComplete": true
            },
            "Headers": [// Common headers applied to all apis
                "testSettingsKey6: testSettingsValue6.1"
            ],
            "ResiliencePipelineOptions": { // Common resilience pipeline applied to all apis
                "HttpGet": ["TestPipeline3"]// Resilience pipelines scoped to specific request method group
            },
            "Caching": { // Common caching settings
                "Mode": "FetchOrGet",
                "LifeSpan": "00:15:00",
                "ShouldInvalidateOnError": false
            },
            "Priority": "UserInitiated"
        },
        "ProperOptions": { // Options specific to some apis
            "IReqResSimpleService": { // Options specific to IReqResSimpleService api
                "BaseAddress": "https://reqres.in/api", // Specific base address
                "RequestTimeout": "00:00:03", // Specific request timeout
                "Headers": [// Specific headers applied to the IReqResSimpleService api
                    "testSettingsKey2: testSettingsValue2.1", // Clear static header
                    "testSettingsKey3: *testSettingsValue3.1*", // Redacted static header
                    "testSettingsKey4: {0}", // Clear runtime header
                    "testSettingsKey5: *{0}*" // Redacted runtime header
                ],
                "Caching": { // Specific caching settings overriding common ones
                    "Mode": "FetchOrGet",
                    "LifeSpan": "00:12:00",
                    "ShouldInvalidateOnError": true
                },
                "ResiliencePipelineKeys": ["TestPipeline3"], // Specific resilience pipelines applied to all IReqResSimpleService api methods
                "RequestOptions": { // Options specific to some IReqResSimpleService api methods
                    "GetUsersAsync": { // Options specific to GetUsersAsync method
                        "Caching": {
                            "Mode": "FetchOrGet",
                            "LifeSpan": "00:10:00",
                            "ShouldInvalidateOnError": false
                        },
                        "Headers": [
                            "testSettingsKey7: testSettingsValue7.1"
                        ],
                        "Priority": "Speculative"
                    }
                },
                "Priority": "Background"
            },
            "User": { // Options specific to User CRUD api
                "BaseAddress": "https://reqres.in/api/users", // Specific base address
                "RequestTimeout": "00:00:05", // Specific request timeout
                "Headers": [// Specific headers applied to the User CRUD api
                    "testSettingsKey8: testSettingsValue8.1" // Clear static header
                ],
                "Priority": 70
            }
        }
    }
}
```

- You first have to start with the `Apizr` root section key.
- Then you can define settings at:
  - Common level to set shared settings with the `CommonOptions` section key
  - Proper level to set api specific settings with the `ProperOptions` section key followed by the name of apis as section keys (here `IReqResSimpleService` classic api and `User` CRUD api)
  - Request level to set api method settings with the `RequestOptions` section key into the named api section (here `GetUsersAsync` method of `IReqResSimpleService` api)
- Finally you can set following available settings:
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
  - `Headers` (string array): specifies custom headers to be added to requests (supporting either clear static, redacted static, clear runtime and redacted runtime values)
  - `ResiliencePipelineKeys` (string array): specifies the resilience pipeline keys to use
  - `ResiliencePipelineOptions` (dictionary): specifies the resilience pipeline keys to use but scoped to specific request [method groups](/api/Apizr.Configuring.ApizrRequestMethod.html)
  - `Caching` (section): contains settings related to caching
    - `Mode` (enum member name): specifies the [caching mode](/api/Apizr.Caching.CacheMode.html)
    - `LifeSpan` (TimeSpan representation): specifies the lifespan of cached responses
    - `ShouldInvalidateOnError` (bool): specifies whether to invalidate the cache on error
  - `Priority` (enum member name or int): specifies the [priority](/api/Apizr.Configuring.Priority.html) level for the request

### Registering

Once settings has been adjusted to your needs, you just have to provide an `IConfiguration` instance to Apizr with the dedicated fluent option at any registration level:

```csharp
options => options.WithConfiguration(context.Configuration)
```

>[!NOTE]
>
> - Apizr will first load common settings, then specific settings, so specific settings will override or be merged with the common ones. The same behavior as usual with fluent options registration actually.
> - Order matters, meaning that you should first register the configuration from settings, then override it with fluent options if needed .

If you want to organize your settings in a more custom way, you can provide custom configuration section keys to Apizr at any registration level:

```csharp
options => options.WithConfiguration(context.Configuration.GetSection("My:Custom:Section"))
```

But you still have to conform to the settings structure described above.