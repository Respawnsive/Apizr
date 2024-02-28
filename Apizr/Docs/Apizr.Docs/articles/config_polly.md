## Configuring Polly

Apizr comes with a `ResiliencePipeline` attribute to apply some resilience strategies on apis, handled by [Polly](https://github.com/App-vNext/Polly).

You’ll find also resilience pipeline attributes dedicated to CRUD apis like `CreateResiliencePipeline`, `ReadResiliencePipeline` and so on…

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

### Defining

Now we can use it thanks to attribute decoration:

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

Here we are using it at assembly level, telling Apizr to apply `TransiantHttpError` policy to all apis.

You can mix levels and mix pipelines as all will be wrapped in the end.

### Using

Apizr will automatically tell Polly to handle request of any decorated api method or parent interface or assembly.

### Configuring

Some advanced options are available to configure Polly context itself at any level:

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