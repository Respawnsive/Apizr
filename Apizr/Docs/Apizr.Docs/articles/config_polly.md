## Configuring Polly

If you are referencing the `Apizr.Extensions.Microsoft.DependencyInjection` package (extended registration), you may want to reference the `Microsoft.Extensions.Http.Resilience` optional package too, so that you can use all the Microsoft Resilience goodness.

Anyway, both extended and static registrations let you configure Polly behaviors straight the way with the yet referenced `Polly.Extensions` package.

### Using Microsoft Resilience

With the extended registration approach only (not available with the static one), the `Microsoft.Extensions.Http.Resilience` optional package offers a pre-configured way to handle requests resilience, applied globally to all methods of an api interface.

#### Installing

First, you should read more about it from the [official documentation](https://learn.microsoft.com/en-us/dotnet/core/resilience/http-resilience).

Then, please install this package:

|Project|Current|Upcoming|
|-------|-----|-----|
|Microsoft.Extensions.Http.Resilience|[![NuGet](https://img.shields.io/nuget/v/Microsoft.Extensions.Http.Resilience.svg)](https://www.nuget.org/packages/Microsoft.Extensions.Http.Resilience/)|[![NuGet Pre Release](https://img.shields.io/nuget/vpre/Microsoft.Extensions.Http.Resilience.svg)](https://www.nuget.org/packages/Microsoft.Extensions.Http.Resilience/)|

#### Registering

Finally, just register it using `ConfigureHttpClientBuilder` then `AddStandardResilienceHandler` methods like so:
```csharp
options => options.ConfigureHttpClientBuilder(builder => builder
    .AddStandardResilienceHandler())
```

If you need more control over pipeline scope, like per method tunning, you should use the `Polly.Extensions` integration instead or even mix both approaches, applying some global resilience handling with `Microsoft.Extensions.Http.Resilience` and some specific ones with `Polly.Extensions`.

#### Configuring

If you need more control over resilience settings, you can provide your configuration.

You can do it either automatically from settings or manually with options.

##### Automatically

First, define your resilience settings like so:
```json
"ResilienceOptions": {
    "Retry": {
        "BackoffType": "Exponential",
        "UseJitter": true,
        "MaxRetryAttempts": 3
    }
}
```

Then provide it to the Resilience Handler:
```csharp
options => options.ConfigureHttpClientBuilder(builder => builder
	.AddStandardResilienceHandler(configuration.GetSection("ResilienceOptions")))
```

##### Manually

Just provide your configuration thanks to the dedicated builder:
```csharp
options => options.ConfigureHttpClientBuilder(builder => builder
    .AddStandardResilienceHandler(resilienceOptions =>
    {
        resilienceOptions.CircuitBreaker.MinimumThroughput = 10;
        // and so on...
    }))
```

### Using Polly Extensions

With both extended and static registrations, the `Polly.Extensions` integration offers many ways to handle requests resilience, individually or globally, and can be configured at design, register or request time.

Apizr comes with a `ResiliencePipeline` attribute to apply some resilience strategies on apis, handled by [Polly](https://github.com/App-vNext/Polly).

You’ll find also resilience pipeline attributes dedicated to each Http methods like `GetResiliencePipeline`, `PostResiliencePipeline` and so on, and some others to CRUD apis like `CreateResiliencePipeline`, `ReadResiliencePipeline` and so on…

Polly will help you to manage some retry scenarios but can do more. Please refer to its [official documentation](https://www.pollydocs.org/) if you’d like to know more about it.

First, add the request options parameter `[RequestOptions] IApizrRequestOptions options` to your api methods to ensure your pipelines will be applied and don't forget to pass the options to your api methods at request time.

#### Registering

Here is how to define a resilience pipeline with some strategies.

```csharp
var resiliencePipelineBuilder = new ResiliencePipelineBuilder<HttpResponseMessage>()
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

#### [Static](#tab/tabid-static)

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

#### [Extended](#tab/tabid-extended)

There's nothing specific to do with Apizr about Polly when using the extended approach.

Just don't forget to register it into your container like you usualy do:

```csharp
// (Polly) Add the resilience pipeline with its key to your container
services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
    builder => builder.AddPipeline(resiliencePipelineBuilder.Build()));
```

Note that `TransientHttpError` here is a key that will be used to identify the pipeline to apply to apis.

***

#### Activating

You can activate resiliencing either at:
- Design time by attribute decoration
- Register time by fluent configuration
- Request time by fluent configuration

##### ResiliencePipeline attribute

Apizr comes with a `ResiliencePipeline` attribute which activate resiliencing at any level (all Assembly apis, classic interface/crud class apis or specific classic interface api method).

Here is classic api an example:
```csharp
[assembly:ResiliencePipeline("TransientHttpError")]
namespace Apizr.Sample
{
    [BaseAddress("https://reqres.in/api")]
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
    [BaseAddress("https://reqres.in/api/users")]
    [ReadAllResiliencePipeline("TransientHttpError")]
    [ReadResiliencePipeline("AnotherHttpError")]
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

As usual, you can mix levels and pipelines as all will be wrapped in the end.

##### Fluent configuration

###### Automatically

Resiliencing could be activated automatically by providing an `IConfiguration` instance containing resilience pipeline settings to Apizr:
```csharp
options => options.WithConfiguration(context.Configuration)
```

We can activate it at common level (to all apis), specific level (dedicated to a named api) or even request level (dedicated to a named api's method).

Please heads to the [Settings](config_settings.md))  doc article to see how to configure resiliencing automatically from settings.

###### Manually

You can activate resiliencing at any levels by providing pipeline keys with this fluent option :
```csharp
// pipeline keys
options => options.WithResiliencePipelineKeys(["TransientHttpError"])

// OR the same with method scope
options => options.WithResiliencePipelineKeys(["TransientHttpError"], [ApizrRequestMethod.HttpGet, ApizrRequestMethod.CrudRead])
```

#### Using

Apizr will automatically tell Polly to handle request with pipelines that get a key matching the one provided by attributes or fluent options.

### Tunning Polly Context

#### Automatically

Context parameters could be set automatically by providing an `IConfiguration` instance containing the context settings:
```csharp
options => options.WithConfiguration(context.Configuration)
```

We can set it at common level (to all apis), specific level (dedicated to a named api) or even request level (dedicated to a named api's method).

Please heads to the [Settings](config_settings.md))  doc article to see how to configure context automatically from loaded settings configuration.

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