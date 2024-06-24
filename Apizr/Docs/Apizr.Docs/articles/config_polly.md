## Configuring Polly

Apizr comes with a `ResiliencePipeline` attribute to apply some resilience strategies on apis, handled by [Polly](https://github.com/App-vNext/Polly).

You’ll find also resilience pipeline attributes dedicated to each Http methods like `GetResiliencePipeline`, `PostResiliencePipeline` and so on, and some others to CRUD apis like `CreateResiliencePipeline`, `ReadResiliencePipeline` and so on…

Polly will help you to manage some retry scenarios but can do more. Please refer to its [official documentation](https://github.com/App-vNext/Polly) if you’d like to know more about it.

### Registering

Here is how to define a resilience pipeline with some strategies.

```csharp
var resiliencePipelineBuilder = new ResiliencePipelineBuilder<HttpResponseMessage>()
    // Configure telemetry to get some logs from Polly process
    .ConfigureTelemetry(LoggerFactory.Create(loggingBuilder =>
        loggingBuilder.Debug()))
    // Add a retry strategy with some options
    .AddRetry(
        new RetryStrategyOptions<HttpResponseMessage>
        {
            ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                .Handle<HttpRequestException>()
                .HandleResult(response =>
                    response.StatusCode is >= HttpStatusCode.InternalServerError
                        or HttpStatusCode.RequestTimeout),
            Delay = TimeSpan.FromSeconds(1),
            MaxRetryAttempts = 3,
            UseJitter = true,
            BackoffType = DelayBackoffType.Exponential
        }));
```

Now we have to register our pipeline:

### [Static](#tab/tabid-static)

First, build a registry and register your pipeline into it:

```csharp
var resiliencePipelineRegistry = new ResiliencePipelineRegistry<string>();
resiliencePipelineRegistry.TryAddBuilder<HttpResponseMessage>("TransientHttpError", 
    (builder, _) => builder.AddPipeline(resiliencePipelineBuilder.Build()));
```

Note that `TransientHttpError` here is a key that will be used to identify the pipeline to apply to apis.

You'll be able to provide your registry to Apizr with this option:

```csharp
// direct configuration
options => options.WithResiliencePipelineRegistry(resiliencePipelineRegistry)

// OR factory configuration
options => options.WithResiliencePipelineRegistry(() => resiliencePipelineRegistry)
```

### [Extended](#tab/tabid-extended)

There's nothing specific to do with Apizr about Polly when using the extended approach.

Just don't forget to register it into your container like you usualy do:

```csharp
// (Polly) Add the resilience pipeline with its key to your container
services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
    builder => builder.AddPipeline(resiliencePipelineBuilder.Build()));
```

Note that `TransientHttpError` here is a key that will be used to identify the pipeline to apply to apis.

***

### Activating

You can activate resiliencing either at:
- Design time by attribute decoration
- Register time by fluent configuration
- Request time by fluent configuration

#### ResiliencePipeline attribute

Apizr comes with a `ResiliencePipeline` attribute which activate resiliencing at any level (all Assembly apis, classic interface/crud class apis or specific classic interface api method).

Here is classic api an example:
```csharp
[assembly:ResiliencePipeline("TransientHttpError")]
namespace Apizr.Sample
{
    [WebApi("https://reqres.in/api")]
    public interface IReqResService
    {
        [Get("/users")]
        Task<UserList> GetUsersAsync();
    }
}
```

Here we are using the `ResiliencePipeline` attribute at assembly level (all methods of all apis), 
but you can use it at interface/class level (all methods of one api) or method level (decorated api methods only).

You may want to set pipelines scoped to a group of methods instead, like all Get http methods or Post ones.
You can do it at assembly or interface/class levels thanks to one of the provided scoped attributes:
- Http methods grouping: 
  - `GetResiliencePipeline`
  - `PostResiliencePipeline`
  - `PutResiliencePipeline`
  - `DeleteResiliencePipeline`
  - `PatchResiliencePipeline`
  - `OptionsResiliencePipeline`
  - `HeadResiliencePipeline`

You’ll find some more resilience pipeline attributes but dedicated to CRUD apis (the ones starting with `Read`, `ReadAll`, `Create`, `Update` or `Delete` prefix), so you could activate resiliencing at method/request level for CRUD apis too.

Here is CRUD api an example:
```csharp
namespace Apizr.Sample.Models
{
    [CrudEntity("https://reqres.in/api/users", typeof(int), typeof(PagedResult<>))]
    [ReadAllResiliencePipeline("TransientHttpError")]
    [ReadResiliencePipeline("AnotherHttpError")]
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

As usual, you can mix levels and pipelines as all will be wrapped in the end.

#### Fluent configuration

##### Automatically

Resiliencing could be activated automatically by providing an `IConfiguration` instance containing resilience pipeline settings to Apizr:
```csharp
options => options.WithConfiguration(context.Configuration)
```

We can activate it at common level (to all apis) or specific level (dedicated to a named one).

Please heads to the Settings doc article to see how to configure resiliencing automatically from settings.

##### Manually

You can activate resiliencing at any levels by providing pipeline keys with this fluent option :
```csharp
// pipeline keys
options => options.WithResiliencePipelineKeys(["TransientHttpError"])

// OR the same with method scope
options => options.WithResiliencePipelineKeys(["TransientHttpError"], [ApizrRequestMethod.HttpGet, ApizrRequestMethod.CrudRead])
```

### Using

Apizr will automatically tell Polly to handle request with pipelines that get a key matching the one provided by attributes or fluent options.

### Tunning context

#### Automatically

Context parameters could be set automatically by providing an `IConfiguration` instance containing the context settings:
```csharp
options => options.WithConfiguration(context.Configuration)
```

We can set it at common level (to all apis) or specific level (dedicated to a named one).

Please heads to the Settings doc article to see how to configure context automatically from loaded settings configuration.

#### Manually

Some advanced options are also available to configure Polly context itself at any level:

```csharp
options => options.WithResilienceContextOptions(contextOptions =>
    contextOptions.ReturnToPoolOnComplete(false) // true by default
        .ContinueOnCapturedContext(false))
```

Here is the one to provide Resilience Properties to Polly context at any level:

```csharp
// direct configuration
options => options.WithResilienceProperty(testKey2, "testValue2.2")

// OR factory configuration
options => options.WithResilienceProperty(testKey2, () => "testValue2.2")

// OR extended factory configuration
options => options.WithResilienceProperty(testKey2, serviceProvider => 
    serviceProvider.GetRequiredService<ISettingsService>().MyTestValue)
```

Note that if you provide a property with the same key at different levels, the closest one to the request will be the one used by Apizr.