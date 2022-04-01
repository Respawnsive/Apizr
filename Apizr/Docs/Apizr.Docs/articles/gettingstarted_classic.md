## Defining

We could define our web api service just like:
```csharp
[assembly:Policy("TransientHttpError")]
namespace Apizr.Sample
{
    [WebApi("https://reqres.in/"), Cache, Log]
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

And that's all.

Every attributes here will inform Apizr on how to manage each web api request. No more boilerplate.

## Registering

It's not required to register anything in a container for DI purpose (you can use the returned static instance directly), but we'll describe here how to use it with DI anyway.

### Registering a single interface

#### [Static](#tab/tabid-static)

Here is an example of how to register a managed instance of an api interface:
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
myContainer.RegistrationMethodFactory(() => 
    Apizr.CreateFor<IReqResService>(options => options
        .WithPolicyRegistry(registry)
        .WithAkavacheCacheHandler())
);
```

We provided a policy registry and a cache handler here as we asked for it with cache and policy attributes while designing the api interface.
Also, you could use the manager directly instead of registering it.

#### [Extended](#tab/tabid-extended)

Here is an example of how to register a managed api interface:
```csharp
public override void ConfigureServices(IServiceCollection services)
{
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
    services.AddPolicyRegistry(registry);

    // Apizr registration
    services.AddApizrFor<IReqResService>(options => options.WithAkavacheCacheHandler());
}
```

We registered a policy registry and provided a cache handler here as we asked for it with cache and policy attributes while designing the api interface.

***

### Registering multiple interfaces

#### [Static](#tab/tabid-static)

You may want to register multiple managed api interfaces within the same project.
Also, you may want to share some common configuration between apis without repeating yourself, but at the same time, you may need to set some specific ones for some of it.
This is where the ApizrRegistry comes on stage.

Here is an example of how to register a managed instance of multiple api interfaces:
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

// Apizr registry
var apizrRegistry = Apizr.Create(
    registry => registry
        .AddFor<IReqResService>()
        .AddFor<IHttpBinService>(
            options => options
                .WithLogging(
                    HttpTracerMode.Everything, 
                    HttpMessageParts.All, 
                    LogLevel.Trace)),
    
    config => config
        .WithPolicyRegistry(registry)
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
  - Providing a policy registry
  - Providing a cache handler
  - Providing some logging settings (won't apply to IHttpBinService's manager as we set some specific ones)

It's an example, meaning if you don't need common and/or specific configuration, just don't provide it.
And yes you can mix classic and CRUD manager registration into the same registry.

Also, you could register the registry itslef, instead of its populated managers and then use its managers directly.

Or, you could use the managers directly from the registry instead of registering anything.

Here's how to get a manager from the registry:

```csharp
var reqResManager = apizrRegistry.GetFor<IReqResService>();

var httpBinManager = apizrRegistry.GetFor<IHttpBinService>();
```

#### [Extended](#tab/tabid-extended)

You may want to register multiple managed api interfaces within the same project.
Also, you may want to share some common configuration between apis without repeating yourself, but at the same time, you may need to set some specific ones for some of it.
This is where the ApizrRegistry comes on stage.

Here is an example of how to register multiple managed api interfaces manually:
```csharp
public override void ConfigureServices(IServiceCollection services)
{
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
    services.AddPolicyRegistry(registry);

    // Apizr registration
    services.AddApizr(
        registry => registry
            .AddFor<IReqResService>()
            .AddFor<IHttpBinService>(
                options => options
                    .WithLogging(
                        HttpTracerMode.Everything, 
                        HttpMessageParts.All, 
                        LogLevel.Trace)),
    
        config => config
            .WithPolicyRegistry(registry)
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
  - Providing a policy registry
  - Providing a cache handler
  - Providing some logging settings (won't apply to IHttpBinService's manager as we set some specific ones)

It's an example, meaning if you don't need common and/or specific configuration, just don't provide it.
And yes you can mix classic and CRUD manager registration into the same registry.

Of course, each managers will be regitered into the container so that you can use it directly.

Also, the registry itslef will be registered into the container, so you could use it to get its managers, instead of using each managers.

Here's how to get a manager from the registry:

```csharp
var reqResManager = apizrRegistry.GetFor<IReqResService>();

var httpBinManager = apizrRegistry.GetFor<IHttpBinService>();
```
***

### Registering all scanned interfaces

#### [Static](#tab/tabid-static)

Not available.

#### [Extended](#tab/tabid-extended)

Here is an example of how to auto register all scanned interfaces:
```csharp
public override void ConfigureServices(IServiceCollection services)
{
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
    services.AddPolicyRegistry(registry);

    // Apizr registration
    services.AddApizrFor(options => options.WithAkavacheCacheHandler(), ASSEMBLIES_CONTAINING_INTERFACES);
}
```

Apizr will scan assemblies to auto register managers for decorated api interfaces.

We registered a policy registry and provided a cache handler here as we asked for it with cache and policy attributes while designing the api interface.

***

## Using

Here is an example of how to send a web request from an app - e.g. using Apizr in a Xamarin.Forms mobile app.

Inject ```IApizrManager<IYourDefinedInterface>``` where you need it - e.g. into your ViewModel constructor
```csharp
public class YourViewModel
{
    private readonly IApizrManager<IReqResService> _reqResManager;
	
    public YouViewModel(IApizrManager<IReqResService> reqResManager)
    // Or registry injection
    //public YouViewModel(IApizrRegistry apizrRegistry)
    {
        _reqResManager = reqResManager;

        // Or registry injection
        //_reqResManager = apizrRegistry.GetFor<IReqResService>();
    }
    
    public ObservableCollection<User>? Users { get; set; }

    private async Task GetUsersAsync()
    {
        IList<User>? users;
        try
        {
            var userList = await _reqResManager.ExecuteAsync(api => api.GetUsersAsync());
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
