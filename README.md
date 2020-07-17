# Apizr
Refit based web api client management, but resilient (retry, connectivity, cache, auth, log, priority, etc...)

## Libraries

[Change Log - July 17, 2020](https://github.com/Respawnsive/Apizr/blob/master/CHANGELOG.md)

|Project|NuGet|
|-------|-----|
|Apizr|[![NuGet](https://img.shields.io/nuget/v/Apizr.svg)](https://www.nuget.org/packages/Apizr/)|
|Apizr.Extensions.Microsoft.DependencyInjection|[![NuGet](https://img.shields.io/nuget/v/Apizr.Extensions.Microsoft.DependencyInjection.svg)](https://www.nuget.org/packages/Apizr.Extensions.Microsoft.DependencyInjection/)|
|Apizr.Integrations.Akavache|[![NuGet](https://img.shields.io/nuget/v/Apizr.Integrations.Akavache.svg)](https://www.nuget.org/packages/Apizr.Integrations.Akavache/)|
|Apizr.Integrations.MonkeyCache|[![NuGet](https://img.shields.io/nuget/v/Apizr.Integrations.MonkeyCache.svg)](https://www.nuget.org/packages/Apizr.Integrations.MonkeyCache/)|
|Apizr.Integrations.Shiny|[![NuGet](https://img.shields.io/nuget/v/Apizr.Integrations.Shiny.svg)](https://www.nuget.org/packages/Apizr.Integrations.Shiny/)|
|Apizr.Integrations.MediatR|[![NuGet](https://img.shields.io/nuget/v/Apizr.Integrations.MediatR.svg)](https://www.nuget.org/packages/Apizr.Integrations.MediatR/)|
|Apizr.Integrations.Optional|[![NuGet](https://img.shields.io/nuget/v/Apizr.Integrations.Optional.svg)](https://www.nuget.org/packages/Apizr.Integrations.Optional/)|
|Apizr.Integrations.AutoMapper|[![NuGet](https://img.shields.io/nuget/v/Apizr.Integrations.AutoMapper.svg)](https://www.nuget.org/packages/Apizr.Integrations.AutoMapper/)|

Install the NuGet package of your choice:

   - **Apizr** package comes with the For and CrudFor static instantiation approach (wich you can register in your DI container then)
   - **Apizr.Extensions.Microsoft.DependencyInjection** package extends your IServiceCollection with AddApizrFor and AddApizrCrudFor registration methods (ASP.Net Core, etc)
   - **Apizr.Integrations.Akavache** package brings an ICacheHandler method mapping implementation for [Akavache](https://github.com/reactiveui/Akavache)
   - **Apizr.Integrations.MonkeyCache** package brings an ICacheHandler method mapping implementation for [MonkeyCache](https://github.com/jamesmontemagno/monkey-cache)
   - **Apizr.Integrations.Shiny** package brings ICacheHandler, ILogHandler and IConnectivityHandler method mapping implementations for [Shiny](https://github.com/shinyorg/shiny), extending your IServiceCollection with a UseApizr registration method
   - **Apizr.Integrations.MediatR** package enables Crud request auto handling with CQRS mediation using [MediatR](https://github.com/jbogard/MediatR)
   - **Apizr.Integrations.Optional** package enables Optional result for Crud request (requires MediatR integration) using [Optional.Async](https://github.com/dnikolovv/optional-async)
   - **Apizr.Integrations.AutoMapper** package enables auto mapping for Crud request and result (requires MediatR integration and could work with Optional integration) using [AutoMapper](https://github.com/AutoMapper/AutoMapper)

Definitly, Apizr make use of well known nuget packages to make the magic appear:

|Package|Features|
|-------|--------|
|[Refit](https://github.com/reactiveui/refit)|Auto-implement web api interface and deal with HttpClient|
|[Polly](https://github.com/App-vNext/Polly)|Apply some policies like Retry, CircuitBreaker, etc...|
|[Fusillade](https://github.com/reactiveui/Fusillade)|Play with request priority|
|[HttpTracer](https://github.com/BSiLabs/HttpTracer)|Trace Http(s) request/response traffic to log it|

It also comes with some handling interfaces to let you provide your own services for:
- **Caching** with ICacheHandler, wich comes with its default VoidCacheHandler (no cache), but also with:
  - AkavacheCacheHandler: [Akavache](https://github.com/reactiveui/Akavache) method mapping interface (Integration package referenced above)
  - MonkeyCacheHandler: [MonkeyCache](https://github.com/jamesmontemagno/monkey-cache) method mapping interface (Integration package referenced above)
  - ShinyCacheHandler: [Shiny](https://github.com/shinyorg/shiny) chaching method mapping interface (Integration package referenced above)
- **Logging** with ILogHandler, wich comes with its default DefaultLogHandler (Console and Debug), but also with:
  - ShinyLogHandler: [Shiny](https://github.com/shinyorg/shiny) logging method mapping interface (Integration package referenced above)
- **Connectivity** with IConnectivityHandler, wich comes with its default VoidConnectivityHandler (no connectivity check), but also with:
  - ShinyConnectivityHandler: [Shiny](https://github.com/shinyorg/shiny) connectivity method mapping interface (Integration package referenced above)
- **Mapping** with IMappingHandler, wich comes with its default VoidMappingHandler (no mapping conversion), but also with:
  - AutoMapperMappingHandler: [AutoMapper](https://github.com/AutoMapper/AutoMapper) mapping method mapping interface (Integration package referenced above)

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

        [Post("/api/users")]
        Task<User> CreateUser(User user, CancellationToken cancellationToken);
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
myContainer.SomeInstanceRegistrationMethod(Apizr.For<IReqResService>(optionsBuilder => optionsBuilder.WithPolicyRegistry(registry)
                    .WithCacheHandler(new AkavacheCacheHandler())));
```

I provided a policy registry and a cache handler here as I asked for it with cache and policy attributes in my web api example.

#### Extensions approach

For this one, two options :
- Manually: register calling ```AddApizrFor<TWebApi>``` service collection extension method or overloads for each service you want to manage
- Automatically: decorate your services with WebApiAttribute and let Apizr auto register it all for you

##### Manually

Here is an example:
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
    services.AddApizrFor<IReqResService>(optionsBuilder => optionsBuilder.WithCacheHandler<AkavacheCacheHandler>());
    
    // Or if you use Shiny
    //services.UseApizrFor<IReqResService>();
}
```

##### Automatically

Decorate your api services like we did before (but with your own settings):
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

        [Post("/api/users")]
        Task<User> CreateUser(User user, CancellationToken cancellationToken);
    }
}
```

Then, register in your Startup class like so:
```csharp
public override void ConfigureServices(IServiceCollection services)
{
    // Apizr registration
    services.AddApizrFor(typeof(AnyClassFromServicesAssembly));
    
    // Or if you use Shiny
    //services.UseApizrFor(typeof(AnyClassFromServicesAssembly));
}
```

There are 4 AddApizrFor/UseApizrFor flavors for classic automatic registration, depending on what you want to do and provide.
This one is the simplest.

### Using

Sending web request from your app - e.g. using Apizr in a Xamarin.Forms mobile app.

Inject ```IApizrManager<YourWebApiInterface>``` where you need it - e.g. into your ViewModel constructor
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

## CRUD

When playing with RESTful CRUD api, you've got a couple of options:
- Define a web api interface like we just did before with each crud method (each entity into one interface or one interface for each entity)
- Use the built-in ICrudApi

As the first option is described already, here we'll talk about the ICrudApi option

### Defining

As we'll use the built-in yet defined ICrudApi, there's no more definition to do.

Here is what it looks like then:
```csharp
[Policy("TransientHttpError"), Cache]
public interface ICrudApi<T, in TKey, TReadAllResult, in TReadAllParams> where T : class
{
    [Post("")]
    Task<T> Create([Body] T payload, CancellationToken cancellationToken = default);

    [Get("")]
    Task<TReadAllResult> ReadAll([CacheKey] TReadAllParams readAllParams, CancellationToken cancellationToken = default);

    [Get("")]
    Task<TReadAllResult> ReadAll(CancellationToken cancellationToken = default);

    [Get("/{key}")]
    Task<T> Read([CacheKey] TKey key, CancellationToken cancellationToken = default);

    [Put("/{key}")]
    Task Update(TKey key, [Body] T payload, CancellationToken cancellationToken = default);

    [Delete("/{key}")]
    Task Delete(TKey key, CancellationToken cancellationToken = default);
}
```

We can see that it comes with some attribute decorations, like Cache or Policy. 
If you don't want it for your crud scenario, just don't provide any CacheHandler and/or TransientHttpError policy, it will be ignored.

About generic types:
- T and TKey meanings are abvious
- TReadAllResult is there to handle cases where ReadAll doesn't return an ```IEnumerable<T>``` or derived, but a paged result with some statistics
- TReadAllParams is there to handle cases where you don't want to provide an ```IDictionary<string, object>``` for a ReadAll reaquest, but a custom class

But again, nothing to do around here.

### Registering

#### Static approach

Somewhere where you can add services to your container, add the following:
```csharp
// Apizr registration
myContainer.SomeInstanceRegistrationMethod(Apizr.CrudFor<T, TKey, TReadAllResult, TReadAllParams>(optionsBuilder => optionsBuilder.WithBaseAddress("your specific T entity crud base uri")));
```

T must be a class.

TKey must be primitive. If you don't provide it here, it will be defined as ```int```.

TReadAllResult must inherit from ```IEnumerable<>``` or be a class.
If you don't use paged result, just don't provide any TReadAllResult here and it will be defined as ```IEnumerable<T>```.

TReadAllParams must be a class.
If you don't use a custom class holding your query parameters, just don't provide any TReadAllParams here and it will be defined as ```IDictionary<string, object>```.

You have to provide the specific entity crud base uri with the options builder.

There are 5 CrudFor flavors, depending on what you want to do and provide.
One of it is the simple ```Apizr.CrudFor<T>()```, wich as you can expect, define TKey as ```int```, TReadAllResult as ```IEnumerable<T>``` and TReadAllParams as ```IDictionary<string, object>```.

#### Extensions approach

Ok, for this one, two options again:
- Manually: register calling AddApizrCrudFor<T, TKey, TReadAllResult, TReadAllParams> service collection extension method or overloads for each entity you want to manage
- Automatically: decorate your entities with CrudEntityAttribute and let Apizr auto register it all for you

##### Manually

In your Startup class, add the following:
```csharp
public override void ConfigureServices(IServiceCollection services)
{
    // Apizr registration
    services.AddApizrCrudFor<T, TKey, TReadAllResult, TReadAllParams>(optionsBuilder => optionsBuilder.WithBaseAddress("your specific T entity crud base uri"));
    
    // Or if you use Shiny
    //services.UseApizrCrudFor<T, TKey, TReadAllResult, TReadAllParams>(optionsBuilder => optionsBuilder.WithBaseAddress("your specific T entity crud base uri"));
}
```

Again, T must be a class.

TKey must be primitive. If you don't provide it here, it will be defined as ```int```.

TReadAllResult must inherit from ```IEnumerable<>``` or be a class.
If you don't use paged result, just don't provide any TReadAllResult here and it will be defined as ```IEnumerable<T>```.

TReadAllParams must be a class.
If you don't use a custom class holding your query parameters, just don't provide any TReadAllParams here and it will be defined as ```IDictionary<string, object>```.

You have to provide the specific entity crud base uri with the options builder.

There are 10 AddApizrCrudFor/UseApizrCrudFor flavors for crud manual registration, depending on what you want to do and provide.
One of it is the simple ```services.AddApizrCrudFor<T>()``` or ```services.UseApizrCrudFor<T>()```, wich as you can expect, define TKey as ```int```, TReadAllResult as ```IEnumerable<T>``` and TReadAllParams as ```IDictionary<string, object>```.

##### Automatically

You need to have access to your entity model classes for this option.

Decorate your crud entities like so (but with your own settings):
```csharp
[CrudEntity("https://myapi.com/api/myentity", typeof(int), typeof(PagedResult<>), typeof(ReadAllUsersParams))]
public class MyEntity
{
    [JsonProperty("id")]
    public int Id { get; set; }

    ...
}
```

Thanks to this attribute:
- (Mandatory) We have to provide the specific entity crud base uri
- (Optional) We can set TKey type to any primitive type (default to int)
- (Optional) We can set TReadAllResult to any class or must inherit from ```IEnumerable<>``` (default to ```IEnumerable<T>```)
- (Optional) We can set TReadAllParams to any class (default to ```IDictionary<string, object>```)

Then, register in your Startup class like so:
```csharp
public override void ConfigureServices(IServiceCollection services)
{
    // Apizr registration
    services.AddApizrCrudFor(typeof(MyEntity));
    
    // Or if you use Shiny
    //services.UseApizrCrudFor(typeof(MyEntity));
}
```

There are 4 AddApizrCrudFor/UseApizrCrudFor flavors for crud automatic registration, depending on what you want to do and provide.
This is the simplest.

### Using

Sending web request from your app - e.g. using Apizr in a Xamarin.Forms mobile app.

Inject ```IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>>``` where you need it - e.g. into your ViewModel constructor
```csharp
public class YourViewModel
{
    private readonly IApizrManager<ICrudApi<User, int, PagedResult<User>, ReadAllUsersParams>> _userCrudManager;
	
    public YouViewModel(IApizrManager<ICrudApi<User, int, PagedResult<User>, ReadAllUsersParams>> userCrudManager)
    {
		_userCrudManager = userCrudManager;
    }
    
    public ObservableCollection<User>? Users { get; set; }

    private async Task GetUsersAsync()
    {
        IList<User>? users;
        try
        {
            var pagedUsers = await _userCrudManager.ExecuteAsync((ct, api) => api.ReadAll(ct), CancellationToken.None);
            users = pagedUsers.Data?.ToList();
        }
        catch (ApizrException<PagedResult<User>> e)
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
Each registration approach comes with its optionsBuilder optional parameter:
```csharp
optionsBuilder => optionsBuilder.SomeOptionsHere(someParametersThere)
```

### Service handlers

The options builder let you provide your own method mapping implementations for:
- ICacheHandler (thanks to WithCacheHandler)
- ILogHandler (thanks to WithLogHandler)
- IConnectivityHandler (thanks to WithConnectivityHandler)
- IMappingHandler (thanks to WithMappingHandler).

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

## Advanced

### Mediation

In extensions registration approach and with the dedicated integration nuget package referenced, the options builder let you enable mediation by calling:
```csharp
optionsBuilder => optionsBuilder.WithMediation()
```

Don't forget to register MediatR itself as usual:
```csharp
services.AddMediatR(typeof(Startup));
```

When activated, you don't have to inject/resolve anything else than an ```IMediator``` instance, in order to play with your api services (both classic and crud).

Then, when playing with classic api interfaces, you'll be able to send requests, among:
- ```ExecuteRequest<TWebApi>```: execute any method from ```TWebApi``` defined by an expression parameter
- ```ExecuteRequest<TWebApi, TApiResponse>```: execute any method from ```TWebApi``` with a ```TApiResponse``` result and defined by an expression parameter


Also, when playing with crud, you'll be able to send queries and commands from anywhere, among:
- ```ReadQuery<T>```: get the T entity with int
- ```ReadQuery<T, TKey>```: get the T entity with TKey
- ```ReadAllQuery<TReadAllResult>```: get TReadAllResult with IDictionary<string, object> optional query parameters
- ```ReadAllQuery<TReadAllParams, TReadAllResult>```: get TReadAllResult with TReadAllParams optional query parameters
- ```CreateCommand<T>```: create a T entity
- ```UpdateCommand<T>```: update the T entity with int
- ```UpdateCommand<TKey, T>```: update the T entity with TKey
- ```DeleteCommand<T>```: delete the T entity with int
- ```DeleteCommand<T, TKey>```: delete the T entity with TKey


In both cases, Apizr will intercept it and handle it to send the result back to you, thanks to MediatR.

From there, our ViewModel can look like:
```csharp
public class YourViewModel
{
    private readonly IMediator _mediator;
	
    public YouViewModel(IMediator mediator)
    {
		_mediator = mediator;
    }
    
    public ObservableCollection<User>? Users { get; set; }

    private async Task GetUsersAsync()
    {
        IList<User>? users;
        try
        {
            // The classic api interface way
            //var userList = await _mediator.Send(new ExecuteRequest<IReqResService, UserList>((ct, api) => api.GetUsersAsync(ct)), CancellationToken.None);
            //users = userList.Data;
            
            // The crud api interface way
            var pagedUsers = await _mediator.Send(new ReadAllQuery<PagedResult<User>>(), CancellationToken.None);
            users = pagedUsers.Data?.ToList();
        }
        catch (ApizrException<PagedResult<User>> e)
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

### Optional

In extensions registration approach and with the dedicated integration nuget package referenced, the options builder let you enable mediation with Optional result by calling:
```csharp
optionsBuilder => optionsBuilder.WithOptionalMediation()
```

Again, don't forget to register MediatR itself as usual :
```csharp
services.AddMediatR(typeof(Startup));
```

When activated, you don't have to inject/resolve anything else than an ```IMediator``` instance, in order to play with your apis and get some Optional result.

Then, when playing with classic api interfaces, you'll be able to send requests, among:
- ```ExecuteOptionalRequest<TWebApi>```: execute any method from ```TWebApi``` defined by an expression parameter wich returns with a ```Option<Unit, ApizrException>``` result
- ```ExecuteOptionalRequest<TWebApi, TApiResponse>```: execute any method from ```TWebApi``` defined by an expression parameter wich returns with a ```Option<TApiResponse, ApizrException<TApiResponse>>``` result

Also, you'll be able to send queries and commands from anywhere, among:
- ```ReadOptionalQuery<T>```: get the T entity with int and returns ```Option<T, ApizrException<T>>```
- ```ReadOptionalQuery<T, TKey>```: get the T entity with TKey and returns ```Option<T, ApizrException<T>>```
- ```ReadAllOptionalQuery<TReadAllResult>```: get TReadAllResult with IDictionary<string, object> optional query parameters and returns ```Option<TReadAllResult, ApizrException<TReadAllResult>>```
- ```ReadAllOptionalQuery<TReadAllParams, TReadAllResult>```: get TReadAllResult with TReadAllParams optional query parameters and returns ```Option<TReadAllResult, ApizrException<TReadAllResult>>```
- ```CreateOptionalCommand<T>```: create a T entity and returns ```Option<Unit, ApizrException>```
- ```UpdateOptionalCommand<T>```: update the T entity with int and returns ```Option<Unit, ApizrException>```
- ```UpdateOptionalCommand<TKey, T>```: update the T entity with TKey and returns ```Option<Unit, ApizrException>```
- ```DeleteOptionalCommand<T>```: delete the T entity with int and returns ```Option<Unit, ApizrException>```
- ```DeleteOptionalCommand<T, TKey>```: delete the T entity with TKey and returns ```Option<Unit, ApizrException>```

Apizr will intercept it and handle it to send the result back to you, thanks to MediatR and Optional.

From there, our ViewModel can look like:
```csharp
public class YourViewModel
{
    private readonly IMediator _mediator;
	
    public YouViewModel(IMediator mediator)
    {
		_mediator = mediator;
    }
    
    public ObservableCollection<User>? Users { get; set; }

    private async Task GetUsersAsync()
    {
        // The classic api interface way with Optional
        //var optionalUserList = await _mediator.Send(new ExecuteOptionalRequest<IReqResService, UserList>((ct, api) => api.GetUsersAsync(ct)), CancellationToken.None);
        //optionalPagedResult.Match(userList =>
        //{
        //    if (userList.Data != null && userList.Data.Any())
        //        Users = new ObservableCollection<User>(userList.Data);
        //}, e =>
        //{
        //    var message = e.InnerException is IOException ? "No network" : (e.Message ?? "Error");
        //    UserDialogs.Instance.Toast(new ToastConfig(message) { BackgroundColor = Color.Red, MessageTextColor = Color.White });

        //    if (e.CachedResult?.Data != null && e.CachedResult.Data.Any())
        //        Users = new ObservableCollection<User>(e.CachedResult.Data);
        //});
            
        // The crud api interface way with Optional
        var optionalPagedResult = await _mediator.Send(new ReadAllOptionalQuery<PagedResult<User>>(), CancellationToken.None);
        optionalPagedResult.Match(pagedUsers =>
        {
            if (pagedUsers.Data != null && pagedUsers.Data.Any())
                Users = new ObservableCollection<User>(pagedUsers.Data);
        }, e =>
        {
            var message = e.InnerException is IOException ? "No network" : (e.Message ?? "Error");
            UserDialogs.Instance.Toast(new ToastConfig(message) { BackgroundColor = Color.Red, MessageTextColor = Color.White });

            if (e.CachedResult?.Data != null && e.CachedResult.Data.Any())
                Users = new ObservableCollection<User>(e.CachedResult.Data);
        });
    }
}
```

Same advantages than classic mediation but with exception handling.
Both "classic" and "optional" mediation are compatibles with each other.
It means that if you call both methods during registration, both request collection will be available, so you can decide wich one suits to you when you need it.

### AutoMapper

You can define your own model entities and then, your AutoMapper mapping profiles between api entities and model entities.

Then, you have to tell Apizr wich entities must use the mapping feature.

#### AutoMapper with Crud apis

##### Manually

```csharp
services.AddApizrCrudFor<MappedEntity<TModelEntity, TApiEntity>>(optionsBuilder =>
    optionsBuilder.WithBaseAddress("https://myapi.com/api/myentity")
        .WithCrudMediation()
        .WithMappingHandler<AutoMapperMappingHandler>());
```

Manual registration makes use of MappedEntity<TModelEntity, TApiEntity> just in place of our usual T.
You'll have to enable one or both mediation feature to handle requests (classic and/or optional) and provide a mapping handler.
You'll have to repeat this registration for each crud mapping.

Don't forget to register AutoMapper itself as usual :
```csharp
services.AddAutoMapper(typeof(Startup));
```

##### Automatically

Why not let Apizr do it for you?
To do so, you have do decorate one of those two entities (api vs model) with corresponding attribute:
- ```CrudEntityAttribute``` above the api entity, with ```modelEntityType``` parameter set to the mapped model entity type
- ```MappedCrudEntityAttribute``` above the model entity, with ```apiEntityType``` parameter set to the mapped api entity type

If you get access to both entities, it doesn't matter wich one you decorate, just do it for one of it (if you decorate both, it will take the first found).
If you don't get any access to the api entities, just decorate your model one with the ```MappedCrudEntityAttribute```

From here, let's write:
```csharp
services.AddApizrCrudFor(optionsBuilder => optionsBuilder
        .WithCrudMediation()
        .WithMappingHandler<AutoMapperMappingHandler>(), 
        typeof(AnyTApiEntity), typeof(AnyTModelEntity));
```

In this example, I provided both api entity and model entity assemblies to the attribute scanner, but actually you just have to provide the one containing your attribute decorated entities (api or model, depending of your scenario/access rights).

Don't forget to register AutoMapper itself as usual :
```csharp
services.AddAutoMapper(typeof(Startup));
```

##### Using

Nothing different here but direct using of your model entities when sending mediation requests, like:
```csharp
var createdModelEntity = await _mediator.Send(new CreateCommand<TModelEntity>(myModelEntity), CancellationToken.None);
```

Apizr will map myModelEntity to TApiEntity, send it to the server, map the result to TModelEntity and send it back to you.
And yes, it works also with Optional.

#### AutoMapper with classic apis

You have do decorate one among the api method, the model entity or the api entity with ```MappedWithAttribute```, with ```mappedWithType``` set to the other mapped entity.

From here, let's write:
```csharp
services.AddApizrFor(optionsBuilder => optionsBuilder
        .WithMediation()
        .WithMappingHandler<AutoMapperMappingHandler>(), 
        typeof(AnyTApiEntity), typeof(AnyTModelEntity), typeof(AnyTWebApi));
```
Actually, the number of ```typeof``` depends on where your attribute decorations are defined.

Don't forget to register AutoMapper itself as usual :
```csharp
services.AddAutoMapper(typeof(Startup));
```

#### Using

Nothing different here but direct using of your model entities when sending mediation requests, like:
```csharp
// Classic auto mapped result only
var userInfos = await _mediator.Send(new ExecuteRequest<IReqResService, UserInfos, UserDetails>((ct, api) => 
    api.GetUserAsync(userChoice, ct)), CancellationToken.None);
```

Apizr will send the request to the server, map the api result from ```UserDetails``` to ```UserInfos``` and send it back to you.

You can also map the request before being sent, like so:
```csharp
// Classic auto mapped request and result
var minUser = new MinUser {Name = "John"};
var createdMinUser = await _mediator.Send(
    new ExecuteRequest<IReqResService, MinUser, User>((ct, api, mapper) =>
        api.CreateUser(mapper.Map<MinUser, User>(minUser), ct)), CancellationToken.None);
```

```minUser``` will be mapped from ```MinUser``` to ```User``` just before being sent, then Apizr will map the api result back from ```User``` to ```MinUser``` and send it back to you.

And yes, all the mapping feature works also with Optional.