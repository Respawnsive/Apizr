## Watching

Please find this getting started tutorial video on YouTube about how to get started with Apizr:

> [!Video https://www.youtube.com/embed/9qXekjZepLA]

## Defining

We could define our web api service just like:
```csharp
// (Polly) Define a resilience pipeline key
// OR use Microsoft Resilience instead
[assembly:ResiliencePipeline("TransientHttpError")]
namespace Apizr.Sample
{
    // (Apizr) Define your web api base url and ask for cache and logs
    [BaseAddress("https://reqres.in/"), 
    Cache(CacheMode.FetchOrGet, "01:00:00"), 
    Log(HttpMessageParts.AllButBodies)]
    public interface IReqResService
    {
        // (Refit) Define your web api interface methods
        [Get("/api/users")]
        Task<UserList> GetUsersAsync([RequestOptions] IApizrRequestOptions options);

        [Get("/api/users/{userId}")]
        Task<UserDetails> GetUserAsync([CacheKey] int userId, [RequestOptions] IApizrRequestOptions options);

        [Post("/api/users")]
        Task<User> CreateUser(User user, [RequestOptions] IApizrRequestOptions options);
    }
}
```

And that's all.

Every attributes here will inform Apizr on how to manage each web api request. No more boilerplate.

## Registering

It's not required to register anything in a container for DI purpose (you can use the returned static instance directly), but we'll describe here how to use it with DI anyway.

### Registering a single interface

#### [Extended](#tab/tabid-extended)

Here is an example of how to register a managed api interface:
```csharp
public override void ConfigureServices(IServiceCollection services)
{
    // (Logger) Configure logging the way you want, like
    services.AddLogging(loggingBuilder => loggingBuilder.AddDebug());

    // (Apizr) Add an Apizr manager for the defined api to your container
    services.AddApizrManagerFor<IReqResService>(
        options => options
            // With a cache handler
            .WithAkavacheCacheHandler()
            // If using Microsoft Resilience
            .ConfigureHttpClientBuilder(builder => builder
                .AddStandardResilienceHandler()));

    // (Polly) Add the resilience pipeline (if not using Microsoft Resilience)
    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
        builder => builder.AddRetry(
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
}
```

We registered a resilience pipeline registry and a logger factory and provided a cache handler here as we asked for it with cache, log and resilience pipeline attributes while designing the api interface.

#### [Static](#tab/tabid-static)

Here is an example of how to register a managed instance of an api interface:
```csharp
// (Polly) Create a resilience pipeline registry with some strategies
var resiliencePipelineRegistry = new ResiliencePipelineRegistry<string>();
resiliencePipelineRegistry.TryAddBuilder<HttpResponseMessage>("TransientHttpError", (builder, _) =>
    // Configure telemetry to get some logs from Polly process
    builder.ConfigureTelemetry(LoggerFactory.Create(loggingBuilder =>
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

// Apizr registration
myContainer.RegistrationMethodFactory(() => 
    ApizrBuilder.Current.CreateManagerFor<IReqResService>(options => options
        // With a logger
        .WithLoggerFactory(LoggerFactory.Create(loggingBuilder =>
            loggingBuilder.Debug()))
        // With the defined resilience pipeline registry
        .WithResiliencePipelineRegistry(resiliencePipelineRegistry)
        // And with a cache handler
        .WithAkavacheCacheHandler())
);
```

We provided a resilience pipeline registry, a cache handler and a logger factory here as we asked for it with cache, log and resilience pipeline attributes while designing the api interface.
Also, you could use the manager directly instead of registering it.

***

### Registering multiple interfaces

You may want to register multiple managed api interfaces within the same project.
Also, you may want to share some common configuration between apis without repeating yourself, but at the same time, you may need to set some specific ones for some of it.
This is where the ApizrRegistry comes on stage.

#### Single common configuration

Here is an example of how to register a managed instance of multiple api interfaces, sharing a single common configuration:

##### [Extended](#tab/tabid-extended)

```csharp
public override void ConfigureServices(IServiceCollection services)
{
    // Apizr registration
    services.AddApizr(
        registry => registry
            .AddManagerFor<IReqResService>()
            .AddManagerFor<IHttpBinService>(
                options => options
                    .WithLogging(
                        HttpTracerMode.Everything, 
                        HttpMessageParts.All, 
                        LogLevel.Trace)),
    
        config => config
            .WithAkavacheCacheHandler()
            .WithLogging(
                HttpTracerMode.ExceptionsOnly, 
                HttpMessageParts.ResponseAll, 
                LogLevel.Error)
    );
}
```

Here is what we're saying in this example:
- Add a manager for IReqResService api interface into the registry, to register it into the container
- Add a manager for IHttpBinService api interface into the registry, to register it into the container
  - Set some specific logging settings dedicated to IHttpBinService's manager
- Apply common configuration to all managers by:
  - Providing a cache handler
  - Providing some logging settings (won't apply to IHttpBinService's manager as we set some specific ones)

It's an example, meaning if you don't need common and/or specific configuration, just don't provide it.
And yes you can mix classic and CRUD manager registration into the same registry.

Of course, each managers will be regitered into the container so that you can use it directly.

Also, the registry itslef will be registered into the container, so you could resolve it to get its managers, instead of resolving each managers.

##### [Static](#tab/tabid-static)

```csharp
// Apizr registry
var apizrRegistry = ApizrBuilder.Current.CreateRegistry(
    registry => registry
        .AddManagerFor<IReqResService>()
        .AddManagerFor<IHttpBinService>(
            options => options
                .WithLogging(
                    HttpTracerMode.Everything, 
                    HttpMessageParts.All, 
                    LogLevel.Trace)),
    
    config => config
        .WithAkavacheCacheHandler()
        .WithLogging(
            HttpTracerMode.ExceptionsOnly, 
            HttpMessageParts.ResponseAll, 
            LogLevel.Error)
);

// Container registration
apizrRegistry.Populate((type, factory) => 
    myContainer.RegistrationMethodFactory(type, factory)
);
```

Here is what we're saying in this example:
- Add a manager for IReqResService api interface into the registry
- Add a manager for IHttpBinService api interface into the registry
  - Set some specific logging settings dedicated to IHttpBinService's manager
- Apply common configuration to all managers by:
  - Providing a cache handler
  - Providing some logging settings (won't apply to IHttpBinService's manager as we set some specific ones)

It's an example, meaning if you don't need common and/or specific configuration, just don't provide it.
And yes you can mix classic and CRUD manager registration into the same registry.

Also, you could register the registry itslef, instead of its populated managers and then use its managers directly.

Or, you could use the managers directly from the registry instead of registering anything.

***

Here is how to get a manager from the registry:

```csharp
var reqResManager = apizrRegistry.GetManagerFor<IReqResService>();

var httpBinManager = apizrRegistry.GetManagerFor<IHttpBinService>();
```

#### Multiple common configurations

Here is an example of how to register a managed instance of multiple api interfaces, sharing multiple common configurations at different group level.
It could be usefull when requesting mutliple apis (multiple base address) comming with multiple endpoints (multiple base path).

##### [Extended](#tab/tabid-extended)

```csharp
public override void ConfigureServices(IServiceCollection services)
{
    // Apizr registration
    services.AddApizr(
        registry => registry
            .AddGroup(
                group => group
                    .AddManagerFor<IReqResUserService>(
                        config => config.WithBasePath("users"))
                    .AddManagerFor<IReqResResourceService>(
                        config => config.WithBasePath("resources")),
                config => config.WithBaseAddress("https://reqres.in/api"))

            .AddManagerFor<IHttpBinService>(
                config => config.WithBaseAddress("https://httpbin.org")),
    
        config => config
            .WithAkavacheCacheHandler()
            .WithLogging(
                HttpTracerMode.ExceptionsOnly, 
                HttpMessageParts.ResponseAll, 
                LogLevel.Error)
    );
}
```

Here is what we're saying in this example:
- Add a manager for IReqResUserService api interface into the registry with a common base address (https://reqres.in/api) and a specific base path (users), to register it into the container
- Add a manager for IReqResResourceService api interface into the registry with a common base address (https://reqres.in/api) and a specific base path (resources), to register it into the container
- Add a manager for IHttpBinService api interface into the registry with a speific base address (https://httpbin.org), to register it into the container
- Apply common configuration to all managers by:
  - Providing a cache handler
  - Providing some logging settings

It's an example, meaning if you don't need common and/or specific configuration, just don't provide it.
And yes you can mix classic and CRUD manager registration into the same registry.
You can add mutliple group at the same level and go deeper with group into group itself.

Of course, each managers will be regitered into the container so that you can use it directly.

Also, the registry itslef will be registered into the container, so you could use it to get its managers, instead of using each managers.

##### [Static](#tab/tabid-static)

```csharp
// Apizr registry
var apizrRegistry = ApizrBuilder.Current.CreateRegistry(
    registry => registry
        .AddGroup(
            group => group
                .AddManagerFor<IReqResUserService>(
                    config => config.WithBasePath("users"))
                .AddManagerFor<IReqResResourceService>(
                    config => config.WithBasePath("resources")),
            config => config.WithBaseAddress("https://reqres.in/api"))

        .AddManagerFor<IHttpBinService>(
            config => config.WithBaseAddress("https://httpbin.org")),
    
    config => config
        .WithAkavacheCacheHandler()
        .WithLogging(HttpTracerMode.ExceptionsOnly, HttpMessageParts.ResponseAll, LogLevel.Error)
);

// Container registration
apizrRegistry.Populate((type, factory) => 
    myContainer.RegistrationMethodFactory(type, factory)
);
```

Here is what we're saying in this example:
- Add a manager for IReqResUserService api interface into the registry with a common base address (https://reqres.in/api) and a specific base path (users)
- Add a manager for IReqResResourceService api interface into the registry with a common base address (https://reqres.in/api) and a specific base path (resources)
- Add a manager for IHttpBinService api interface into the registry with a speific base address (https://httpbin.org)
- Apply common configuration to all managers by:
  - Providing a cache handler
  - Providing some logging settings

It's an example, meaning if you don't need common and/or specific configuration, just don't provide it.
And yes you can mix classic and CRUD manager registration into the same registry/group.
You can add mutliple group at the same level and go deeper with group into group itself.

Also, you could register the registry itslef, instead of its populated managers and then use its managers directly.

Or, you could use the managers directly from the registry instead of registering anything.

***

Here's how to get a manager from the registry:

```csharp
var reqResUserManager = apizrRegistry.GetManagerFor<IReqResUserService>();

var reqResResourceManager = apizrRegistry.GetManagerFor<IReqResResourceService>();

var httpBinManager = apizrRegistry.GetManagerFor<IHttpBinService>();
```

### Registering all scanned interfaces

#### [Extended](#tab/tabid-extended)

First you have to tell Apizr which api to auto register by assembly scanning thanks to the `AutoRegister` attribute:
```csharp
[AutoRegister("YOUR_API_INTERFACE_BASE_ADDRESS_OR_PATH")]
public interface IYourApiInterface
{
    // Your api interface methods
}
```

Then fluently just write:
```csharp
public override void ConfigureServices(IServiceCollection services)
{
    // Apizr registration
    services.AddApizrManagerFor([ASSEMBLIES_CONTAINING_INTERFACES]);
}
```

Apizr will scan assemblies to auto register managers for decorated api interfaces.

#### [Static](#tab/tabid-static)

Not available.

***

## Using

Here is an example of how to send a web request from an app - e.g. using Apizr in a MAUI mobile app.

Inject ```IApizrManager<IYourDefinedInterface>``` where you need it - e.g. into your ViewModel constructor
```csharp
public class YourViewModel
{
    private readonly IApizrManager<IReqResService> _reqResManager;
    //private readonly IApizrRegistry _apizrRegistry;
	
    public YouViewModel(IApizrManager<IReqResService> reqResManager)
    // OR registry injection
    //public YouViewModel(IApizrRegistry apizrRegistry)
    {
        _reqResManager = reqResManager;

        // OR registry injection
        //_apizrRegistry = apizrRegistry;

        // Or registry injection AND getting the manager
        //_reqResManager = apizrRegistry.GetManagerFor<IReqResService>();
    }
    
    public ObservableCollection<User>? Users { get; set; }

    private async Task GetUsersAsync()
    {
        IList<User>? users;
        try
        {
            var userList = await _reqResManager.ExecuteAsync(api => api.GetUsersAsync()); 

            // OR with dedicated registry shortcut extension
            // var userList = await _apizrRegistry.ExecuteAsync<IReqResService>(api => api.GetUsersAsync());
 
            // OR with some option adjustments
            // var userList = await _reqResManager.ExecuteAsync((options, api) => api.GetUsersAsync(options),
            //                  options => options.WithPriority(Priority.Background)); 

            users = userList.Data;
        }
        catch (ApizrException<UserList> e)
        {
            var message = e.InnerException is IOException ? "No network" : (e.Message ?? "Error");
            UserDialogs.Instance.Toast(new ToastConfig(message) { BackgroundColor = Color.Red, MessageTextColor = Color.White });

            users = e.CachedResult?.Data;
        }

        if(users != null)
            Users = new ObservableCollection<User>(users);
    }
}
```

We catch any ApizrException as it will contain the original inner exception, but also the previously cached result if some.
If you provided an IConnectivityHandler implementation and there's no network connectivity before sending request, Apizr will throw an IO inner exception without sending the request.
