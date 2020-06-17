# Apizr
Refit based web api client management, but resilient (retry, connectivity, cache, auth, log, priority, etc...)

## Libraries

|Project|NuGet|
|-------|-----|
|Apizr|[![NuGet](https://img.shields.io/nuget/v/Apizr.svg)](https://www.nuget.org/packages/Apizr/)|
|Apizr.Extensions.Microsoft.DependencyInjection|[![NuGet](https://img.shields.io/nuget/v/Apizr.Extensions.Microsoft.DependencyInjection.svg)](https://www.nuget.org/packages/Apizr.Extensions.Microsoft.DependencyInjection/)|
|Apizr.Integrations.Akavache|[![NuGet](https://img.shields.io/nuget/v/Apizr.Integrations.Akavache.svg)](https://www.nuget.org/packages/Apizr.Integrations.Akavache/)|
|Apizr.Integrations.MonkeyCache|[![NuGet](https://img.shields.io/nuget/v/Apizr.Integrations.MonkeyCache.svg)](https://www.nuget.org/packages/Apizr.Integrations.MonkeyCache/)|
|Apizr.Integrations.Shiny|[![NuGet](https://img.shields.io/nuget/v/Apizr.Integrations.Shiny.svg)](https://www.nuget.org/packages/Apizr.Integrations.Shiny/)|

Install the NuGet package of your choice:

   - Apizr package comes with the static instantiation approach (wich you can register in your DI container then)
   - Apizr.Extensions.Microsoft.DependencyInjection package extends your IServiceCollection with an AddApizr registration method (ASP.Net Core, etc)
   - Apizr.Integrations.Akavache package brings an ICacheHandler method mapping implementation for [Akavache](https://github.com/reactiveui/Akavache)
   - Apizr.Integrations.MonkeyCache package brings an ICacheHandler method mapping implementation for [MonkeyCache](https://github.com/jamesmontemagno/monkey-cache)
   - Apizr.Integrations.Shiny package brings ICacheHandler, ILogHandler and IConnectivityHandler method mapping implementations for [Shiny](https://github.com/shinyorg/shiny), extending your IServiceCollection with a UseApizr registration method

Definitly, Apizr make use of well known nuget packages to make the magic appear:

|Package|Features|
|-------|--------|
|[Refit](https://github.com/reactiveui/refit)|Auto-implement web api interface and deal with HttpClient|
|[Polly](https://github.com/App-vNext/Polly)|Apply some policies like Retry, CircuitBreaker, etc...|
|[Fusillade](https://github.com/reactiveui/Fusillade)|Play with request priority|
|[HttpTracer](https://github.com/BSiLabs/HttpTracer)|Trace Http(s) request/response traffic to log it|

It also comes with some handling interfaces to let you provide your own services for:
- Caching with ICacheHandler, wich comes with its default VoidCacheHandler (no cache), but also with:
  - AkavacheCacheHandler: [Akavache](https://github.com/reactiveui/Akavache) method mapping interface (Integration package referenced above)
  - MonkeyCacheHandler: [MonkeyCache](https://github.com/jamesmontemagno/monkey-cache) method mapping interface (Integration package referenced above)
  - ShinyCacheHandler: [Shiny](https://github.com/shinyorg/shiny) chaching method mapping interface (Integration package referenced above)
- Logging with ILogHandler, wich comes with its default DefaultLogHandler (Console and Debug), but also with:
  - ShinyLogHandler: [Shiny](https://github.com/shinyorg/shiny) logging method mapping interface (Integration package referenced above)
- Connectivity with IConnectivityHandler, wich comes with its default VoidConnectivityHandler (no connectivity check), but also with:
  - ShinyConnectivityHandler: [Shiny](https://github.com/shinyorg/shiny) connectivity method mapping interface (Integration package referenced above)

## Getting started

Clearly inspired by [Refit.Insane.PowerPack](https://github.com/thefex/Refit.Insane.PowerPack) but extended with a lot more features, the goal of Apizr is to get all ready to use for web api requesting, with the more resiliency we can, but without the boilerplate.

Examples here are based on a Xamarin.Forms app working with Shiny. 
You'll find a sample Xamarin.Forms app browsing code, implementing Apizr with Shiny, Prism and MS DI all together.

You'll find another sample app but .Net Core console this time, implementing Apizr without anything else (static) and also with MS DI (extensions).

So please, take a look at the samples :)

### Defining:
We could define our web api service just like:
```csharp
[assembly:Policy("TransientHttpError")]
namespace Apizr.Sample.Api
{
    [WebApi("https://reqres.in/"), Cache, Trace]
    public interface IReqResService
    {
        [Get("/api/users")]
        Task<UserList> GetUsersAsync(CancellationToken cancellationToken);

        [Get("/api/users/{userId}")]
        Task<UserDetails> GetUserAsync([CacheKey] int userId, CancellationToken cancellationToken);
    }
}
```

And that's all.

Every attributes here will inform Apizr on how to manage each web api request. No more boilerplate.

### Registering

As it's not mandatory to register anything in a container for DI purpose (you can use a static instance directly), I'll describe here how to use it with DI.

#### Static approach

Somewhere where you can add services to your container, add the following:
```csharp
// Some policies
var registry = new PolicyRegistry
{
    {
        "TransientHttpError", HttpPolicyExtensions.HandleTransientHttpError().WaitAndRetryAsync(new[]
        {
            TimeSpan.FromSeconds(1),
            TimeSpan.FromSeconds(5),
            TimeSpan.FromSeconds(10)
        })
    }
};

// Apizr registration
myContainer.SomeInstanceRegistrationMethod<IApizrManager<IReqResService>>(Apizr.For<IReqResService>(optionsBuilder => optionsBuilder.WithPolicyRegistry(registry)
                    .WithCacheHandler(new AkavacheCacheHandler())));
```

I provided a policy registry and a cache handler here as I asked for it with cache and policy attributes in my web api example.

#### Extensions approach

In your Startup class, add the following:
```csharp
public override void ConfigureServices(IServiceCollection services)
{
    var registry = new PolicyRegistry
    {
        {
            "TransientHttpError", HttpPolicyExtensions.HandleTransientHttpError().WaitAndRetryAsync(new[]
            {
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(10)
            })
        }
    };
    services.AddPolicyRegistry(registry);

    // Apizr registration
    services.AddApizr<IReqResService>(optionsBuilder => optionsBuilder.WithCacheHandler<AkavacheCacheHandler>());
    
    // Or if you use Shiny
    //services.UseApizr<IReqResService>();
}
```

### Using

Sending web request from your app - e.g. using Apizr in a Xamarin.Forms mobile app.

Inject IApizrManager< YourWebApiInterface > where you need it - e.g. into your ViewModel constructor
```csharp
public class YourViewModel
{
    private readonly IApizrManager<IReqResService> _reqResManager;
	
    public YouViewModel(IApizrManager<IReqResService> reqResManager)
    {
		_reqResManager = reqResManager;
    }
    
    public ObservableCollection<User>? Users { get; set; }

    private async Task GetUsersAsync()
    {
        IList<User>? users;
        try
        {
            var userList = await _reqResManager.ExecuteAsync((ct, api) => api.GetUsersAsync(ct), CancellationToken.None);
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

I catch execution into an ApizrException as it will contain the original inner exception, but also the previously cached result if some.
If you provided an IConnectivityHandler implementation and there's no network connectivity before sending request, Apizr will throw with an IO inner exception without sending the request.

## Configuring

There're some advanced scenarios where you want to adjust some settings and behaviors.
This is where the options builder comes in.
Each registration approach comes with its optionsBuilder:
```csharp
something.WhateverRegistrationApproach<WhateverWebApi>(optionsBuilder =>
                optionsBuilder.SomeOptionsHere(someParametersThere));
```

### Service handlers

The options builder let you provide your own method mapping implementations for ICacheHandler (thanks to WithCacheHandler), ILogHandler (thanks to WithLogHandler) and IConnectivityHandler (thanks to WithConnectivityHandler).

### Authentication DelegatingHandler

For autorized request calls, you can provide some properties and/or methods (thanks to WithAuthenticationHandler) to help Apizr to authenticate user when needed.

### Custom DelegatingHandler

The options builder let you add any custom delegating handler thanks to AddDelegatingHandler method

### RefitSettings

You can adjust some specific Refit settings providing an instance of RefitSettings (thanks to WithRefitSettings).
Note that for this one, only constructor parameters will be used (IContentSerializer, IUrlParameterFormatter and IFormUrlEncodedParameterFormatter).

Please don't use AuthorizationHeaderValueGetter, AuthorizationHeaderValueWithParamGetter and HttpMessageHandlerFactory, as they'll be ignored.

Prefer using WithAuthenticationHandler builder method to manage request authorization and AddDelegatingHandler builder method to add some other custom delegating handlers.

### PolicyRegistry

If you plan to use the PoliciesAttribute, Apizr needs to know where to find your policy registry:

- With static instantiation, you have to provide it thanks to WithPolicyRegistry builder method.

- With extensions registration, you have to register it thanks to AddPolicyRegistry service collection extension method.

In any case, you may want to log what's going on during policies excecution.
To do so, there's an OnRetry helper action wich provide your ILogHandler method mapping implementation to Polly.

Here's how to use it:
```csharp
// Some policies
var registry = new PolicyRegistry
{
    {
        "TransientHttpError", HttpPolicyExtensions.HandleTransientHttpError().WaitAndRetryAsync(new[]
        {
            TimeSpan.FromSeconds(1),
            TimeSpan.FromSeconds(5),
            TimeSpan.FromSeconds(10)
        }, LoggedPolicies.OnLoggedRetry).WithPolicyKey("TransientHttpError")
    }
};
```

LoggedPolicies.OnLoggedRetry could also execute your own specific action if needed.

### HttpClient

With extensions registration, you can adjust some more HttpClient settings thanks to ConfigureHttpClientBuilder builder method.
This one could interfere with all Apizr http client auto configuration, so please use it with caution.