# Apizr
Refit based web api client, but resilient (retry, connectivity, cache, auth, log, priority, etc...)

You'll find a [blog post series here](https://www.respawnsive.com/category/blog-en/apizr/) about Apizr.

## Libraries

[Change Log - Jan 04, 2021](https://github.com/Respawnsive/Apizr/blob/master/CHANGELOG.md)

|Project|NuGet|
|-------|-----|
|Apizr|[![NuGet](https://img.shields.io/nuget/v/Apizr.svg)](https://www.nuget.org/packages/Apizr/)|
|Apizr.Extensions.Microsoft.DependencyInjection|[![NuGet](https://img.shields.io/nuget/v/Apizr.Extensions.Microsoft.DependencyInjection.svg)](https://www.nuget.org/packages/Apizr.Extensions.Microsoft.DependencyInjection/)|
|Apizr.Integrations.Shiny|[![NuGet](https://img.shields.io/nuget/v/Apizr.Integrations.Shiny.svg)](https://www.nuget.org/packages/Apizr.Integrations.Shiny/)|
|Apizr.Integrations.Akavache|[![NuGet](https://img.shields.io/nuget/v/Apizr.Integrations.Akavache.svg)](https://www.nuget.org/packages/Apizr.Integrations.Akavache/)|
|Apizr.Integrations.MonkeyCache|[![NuGet](https://img.shields.io/nuget/v/Apizr.Integrations.MonkeyCache.svg)](https://www.nuget.org/packages/Apizr.Integrations.MonkeyCache/)|
|Apizr.Integrations.MediatR|[![NuGet](https://img.shields.io/nuget/v/Apizr.Integrations.MediatR.svg)](https://www.nuget.org/packages/Apizr.Integrations.MediatR/)|
|Apizr.Integrations.Optional|[![NuGet](https://img.shields.io/nuget/v/Apizr.Integrations.Optional.svg)](https://www.nuget.org/packages/Apizr.Integrations.Optional/)|
|Apizr.Integrations.AutoMapper|[![NuGet](https://img.shields.io/nuget/v/Apizr.Integrations.AutoMapper.svg)](https://www.nuget.org/packages/Apizr.Integrations.AutoMapper/)|

Install the NuGet package of your choice:

   - **Apizr** package comes with the For and CrudFor static instantiation approach (witch you can register in your DI container then)
   - **Apizr.Extensions.Microsoft.DependencyInjection** package extends your IServiceCollection with AddApizrFor and AddApizrCrudFor registration methods (ASP.Net Core, etc)
   - **Apizr.Integrations.Shiny** package brings ICacheHandler, ILogHandler and IConnectivityHandler method mapping implementations for [Shiny](https://github.com/shinyorg/shiny), extending your IServiceCollection with a UseApizr and UseApizrCrudFor registration methods
   - **Apizr.Integrations.Akavache** package brings an ICacheHandler method mapping implementation for [Akavache](https://github.com/reactiveui/Akavache)
   - **Apizr.Integrations.MonkeyCache** package brings an ICacheHandler method mapping implementation for [MonkeyCache](https://github.com/jamesmontemagno/monkey-cache)
   - **Apizr.Integrations.MediatR** package enables request auto handling with mediation using [MediatR](https://github.com/jbogard/MediatR)
   - **Apizr.Integrations.Optional** package enables Optional result from mediation requests (requires MediatR integration) using [Optional.Async](https://github.com/dnikolovv/optional-async)
   - **Apizr.Integrations.AutoMapper** package enables auto mapping for mediation requests (requires MediatR integration and could work with Optional integration) using [AutoMapper](https://github.com/AutoMapper/AutoMapper)

Apizr core package make use of well known nuget packages to make the magic appear:

|Package|Features|
|-------|--------|
|[Refit](https://github.com/reactiveui/refit)|Auto-implement web api interface and deal with HttpClient|
|[Polly](https://github.com/App-vNext/Polly)|Apply some policies like Retry, CircuitBreaker, etc...|
|[Fusillade](https://github.com/reactiveui/Fusillade)|Play with request priority|
|[HttpTracer](https://github.com/BSiLabs/HttpTracer)|Trace Http(s) request/response traffic to log it|

It also comes with some handling interfaces to let you provide your own services for:
- **Caching** with ICacheHandler, witch comes with its default VoidCacheHandler (no cache), but also with:
  - AkavacheCacheHandler: [Akavache](https://github.com/reactiveui/Akavache) method mapping interface (Integration package referenced above)
  - MonkeyCacheHandler: [MonkeyCache](https://github.com/jamesmontemagno/monkey-cache) method mapping interface (Integration package referenced above)
  - ShinyCacheHandler: [Shiny](https://github.com/shinyorg/shiny) chaching method mapping interface (Integration package referenced above)
- **Logging** with ILogHandler, witch comes with its default DefaultLogHandler (Console and Debug), but also with:
  - ShinyLogHandler: [Shiny](https://github.com/shinyorg/shiny) logging method mapping interface (Integration package referenced above)
- **Connectivity** with IConnectivityHandler, witch comes with its default VoidConnectivityHandler (no connectivity check), but also with:
  - ShinyConnectivityHandler: [Shiny](https://github.com/shinyorg/shiny) connectivity method mapping interface (Integration package referenced above)
- **Mapping** with IMappingHandler, witch comes with its default VoidMappingHandler (no mapping conversion), but also with:
  - AutoMapperMappingHandler: [AutoMapper](https://github.com/AutoMapper/AutoMapper) mapping method mapping interface (Integration package referenced above)

## How to:
   - [Intro](#intro)
   - [Classic APIs](#classic)
     - [Defining](#classic-defining)
     - [Registering](#classic-registering)
       - [Static approach](#classic-static-approach)
       - [Extensions approach](#classic-extensions-approach)
         - [Manually](#classic-manually)
         - [Automatically](#classic-automatically)
     - [Using](#classic-using)
   - [CRUD APIs](#crud)
     - [Defining](#crud-defining)
     - [Registering](#crud-registering)
       - [Static approach](#crud-static-approach)
       - [Extensions approach](#crud-extensions-approach)
         - [Manually](#crud-manually)
         - [Automatically](#crud-automatically)
     - [Using](#crud-using)
   - [Advanced configurations](#advanced-configurations)
     - [Service handlers](#configuration-service-handlers)
     - [Authentication DelegatingHandler](#configuration-authentication-delegatinghandler)
     - [Custom DelegatingHandler](#configuration-custom-delegatinghandler)
     - [Refit settings](#configuration-refit-settings)
     - [Policy registry](#configuration-policy-registry)
     - [HttpClient](#configuration-httpclient)
   - [External integrations](#external-integrations)
     - [Mediation](#mediation)
     - [Optional](#optional)
       - [Optional helper extentions](#optional-helper-extentions)
         - [OnResultAsync](#optional-onresultasync)
         - [CatchAsync](#optional-catchasync)
     - [AutoMapper](#automapper)
       - [AutoMapper with Crud apis](#automapper-with-crud-apis)
         - [Manually](#automapper-crud-manually)
         - [Automatically](#automapper-crud-automatically)
         - [Using](#automapper-crud-using)
       - [AutoMapper with classic apis](#automapper-with-classic-apis)
         - [Using](#automapper-classic-using)
   - [Pitfalls](#pitfalls)

<h2 id="intro">
Intro
</h2>

Clearly inspired by [Refit.Insane.PowerPack](https://github.com/thefex/Refit.Insane.PowerPack) but extended with a lot more features, the goal of Apizr is to get all ready to use for web api requesting, with the more resiliency we can, but without the boilerplate.

Examples here are based on a Xamarin.Forms app working with Shiny. 
You'll find a sample Xamarin.Forms app browsing code, implementing Apizr with Shiny, Prism and MS DI all together.

> **NOTE - Xamarin/Shiny/Prism/Container**: 
> 
>Please use any Prism extended container of your choice starting v8.0.48 to handle multiple instance registration/resolution

You'll find another sample app but .Net Core console this time, implementing Apizr without anything else (static) and also with MS DI (extensions).

So please, take a look at the samples :)

<h2 id="classic">
Classic APIs:
</h2>

<h3 id="classic-defining">
Defining:
</h3>

We could define our web api service just like:
```csharp
[assembly:Policy("TransientHttpError")]
namespace Apizr.Sample.Api
{
    [WebApi("https://reqres.in/"), CacheIt, LogIt]
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

<h3 id="classic-registering">
Registering:
</h3>

As it's not mandatory to register anything in a container for DI purpose (you can use a static instance directly), I'll describe here how to use it with DI.

<h4 id="classic-static-approach">
Static approach:
</h4>

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

<h4 id="classic-extensions-approach">
Extensions approach:
</h4>

For this one, two options :
- Manually: register calling ```AddApizrFor<TWebApi>``` service collection extension method or overloads for each service you want to manage
- Automatically: decorate your services with WebApiAttribute and let Apizr auto register it all for you

<h5 id="classic-manually">
Manually:
</h5>

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

<h5 id="classic-automatically">
Automatically:
</h5>

Decorate your api services like we did before (but with your own settings):
```csharp
[assembly:Policy("TransientHttpError")]
namespace Apizr.Sample.Api
{
    [WebApi("https://reqres.in/"), CacheIt, LogIt]
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

<h3 id="classic-using">
Using:
</h3>

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

<h2 id="crud">
CRUD APIs:
</h2>

When playing with RESTful CRUD api, you've got a couple of options:
- Define a web api interface like we just did before with each crud method (each entity into one interface or one interface for each entity)
- Use the built-in ICrudApi

As the first option is described already, here we'll talk about the ICrudApi option

<h3 id="crud-defining">
Defining:
</h3>

As we'll use the built-in yet defined ICrudApi, there's no more definition to do.

Here is what it looks like then:
```csharp
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

We can see that it comes with some CacheKey attribute decorations, but it won't cache anything until you ask Apizr to. 
Caching, Logging, Policing... everything is activable fluently with the options builder.

About generic types:
- T and TKey (optional - default: ```int```) meanings are obvious
- TReadAllResult (optional - default: ```IEnumerable<T>```) is there to handle cases where ReadAll doesn't return an ```IEnumerable<T>``` or derived, but a paged result with some statistics
- TReadAllParams (optional - default: ```IDictionary<string, object>```) is there to handle cases where you don't want to provide an ```IDictionary<string, object>``` for a ReadAll reaquest, but a custom class

But again, nothing to do around here.

<h3 id="crud-registering">
Registering:
</h3>

<h4 id="crud-static-approach">
Static approach:
</h4>

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
One of it is the simple ```Apizr.CrudFor<T>()```, witch as you can expect, define TKey as ```int```, TReadAllResult as ```IEnumerable<T>``` and TReadAllParams as ```IDictionary<string, object>```.

<h4 id="crud-extensions-approach">
Extensions approach:
</h4>

Ok, for this one, two options again:
- Manually: register calling AddApizrCrudFor<T, TKey, TReadAllResult, TReadAllParams> service collection extension method or overloads for each entity you want to manage
- Automatically: decorate your entities with CrudEntityAttribute and let Apizr auto register it all for you

<h5 id="crud-manually">
Manually:
</h5>

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
One of it is the simple ```services.AddApizrCrudFor<T>()``` or ```services.UseApizrCrudFor<T>()```, witch as you can expect, define TKey as ```int```, TReadAllResult as ```IEnumerable<T>``` and TReadAllParams as ```IDictionary<string, object>```.

<h5 id="crud-automatically">
Automatically:
</h5>

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

<h3 id="crud-using">
Using:
</h3>

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

<h2 id="advanced-configurations">
Advanced configurations:
</h2>

There're some advanced scenarios where you want to adjust some settings and behaviors.
This is where the options builder comes in.
Each registration approach comes with its optionsBuilder optional parameter:
```csharp
optionsBuilder => optionsBuilder.SomeOptionsHere(someParametersThere)
```

<h3 id="configuration-service-handlers">
Service handlers:
</h3>

The options builder let you provide your own method mapping implementations for:
- ICacheHandler (thanks to WithCacheHandler)
- ILogHandler (thanks to WithLogHandler)
- IConnectivityHandler (thanks to WithConnectivityHandler)
- IMappingHandler (thanks to WithMappingHandler).

<h3 id="configuration-authentication-delegatinghandler">
Authentication DelegatingHandler:
</h3>

For autorized request calls, you can provide some properties and/or methods (thanks to WithAuthenticationHandler) to help Apizr to authenticate user when needed.

<h3 id="configuration-custom-delegatinghandler">
Custom DelegatingHandler:
</h3>

The options builder let you add any custom delegating handler thanks to AddDelegatingHandler method

<h3 id="configuration-refit-settings">
Refit settings:
</h3>

You can adjust some specific Refit settings providing an instance of RefitSettings (thanks to WithRefitSettings).
Note that for this one, only constructor parameters will be used (IContentSerializer, IUrlParameterFormatter and IFormUrlEncodedParameterFormatter).

Please don't use AuthorizationHeaderValueGetter, AuthorizationHeaderValueWithParamGetter and HttpMessageHandlerFactory, as they'll be ignored.

Prefer using WithAuthenticationHandler builder method to manage request authorization and AddDelegatingHandler builder method to add some other custom delegating handlers.

<h3 id="configuration-policy-registry">
Policy registry:
</h3>

If you plan to use the PoliciesAttribute, Apizr needs to know where to find your policy registry:

- With static instantiation, you have to provide it thanks to WithPolicyRegistry builder method.

- With extensions registration, you have to register it thanks to AddPolicyRegistry service collection extension method.

In any case, you may want to log what's going on during policies excecution.
To do so, there's an OnRetry helper action witch provide your ILogHandler method mapping implementation to Polly.

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

<h3 id="configuration-httpclient">
HttpClient:
</h3>

With extensions registration, you can adjust some more HttpClient settings thanks to ConfigureHttpClientBuilder builder method.
This one could interfere with all Apizr http client auto configuration, so please use it with caution.

<h2 id="external-integrations">
External integrations:
</h2>

<h3 id="mediation">
Mediation:
</h3>

In extensions registration approach and with the dedicated integration nuget package referenced, the options builder let you enable mediation by calling:
```csharp
optionsBuilder => optionsBuilder.WithMediation()
```

Don't forget to register MediatR itself as usual:
```csharp
services.AddMediatR(typeof(Startup));
```

When activated, you don't have to inject/resolve anything else than an ```IMediator``` instance, in order to play with your api services (both classic and crud).
Everything you need to do then, is sending your request calling:
```csharp
var result = await _mediator.Send(YOUR_REQUEST_HERE);
```

Where YOUR_REQUEST_HERE could be, with classic api interfaces:
- ```ExecuteRequest<TWebApi>```: execute any method from ```TWebApi``` defined by an expression parameter
- ```ExecuteRequest<TWebApi, TApiResponse>```: execute any method from ```TWebApi``` with a ```TApiResponse``` result and defined by an expression parameter
- ```ExecuteRequest<TWebApi, TModelResponse, TApiResponse>```: execute any method from ```TWebApi``` with a ```TApiResponse``` mapped to a ```TModelResponse``` result and defined by an expression parameter

> **NOTE - Mapping**:
> When I say "mapped", I talk about the mapping integration feature
>
> Please refer to AutoMapper section for more info

Or with crud api interfaces:
- ```ReadQuery<T>```: get the T entity with int
- ```ReadQuery<T, TKey>```: get the T entity with TKey
- ```ReadAllQuery<TReadAllResult>```: get TReadAllResult with IDictionary<string, object> optional query parameters
- ```ReadAllQuery<TReadAllParams, TReadAllResult>```: get TReadAllResult with TReadAllParams optional query parameters
- ```CreateCommand<T>```: create a T entity
- ```UpdateCommand<T>```: update the T entity with int
- ```UpdateCommand<TKey, T>```: update the T entity with TKey
- ```DeleteCommand<T>```: delete the T entity with int
- ```DeleteCommand<T, TKey>```: delete the T entity with TKey

There's also a typed mediator available for each api interface (classic or crud), to help you write things shorter.

With classic api interfaces, resolving ```IMediator<TWebApi>``` give you access to:
- ```SendFor(YOUR_API_METHOD_EXPRESSION)```: send an ```ExecuteRequest<TWebApi>``` for you
- ```SendFor<TApiResponse>(YOUR_API_METHOD_EXPRESSION)```: send an ```ExecuteRequest<TWebApi, TApiResponse>``` for you
- ```SendFor<TModelResponse, TApiResponse>(YOUR_API_METHOD_EXPRESSION)```: send an ```ExecuteRequest<TWebApi, TModelResponse, TApiResponse>``` for you

With crud api interfaces, resolving ```ICrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>``` give you access to:
- ```SendReadQuery(TApiEntityKey key)```: send a ```ReadQuery<TApiEntity, TApiEntityKey>``` for you
- ```SendReadQuery<TModelEntity>(TApiEntityKey key)```: send a ```ReadQuery<TModelEntity, TApiEntityKey>``` for you, with ```TModelEntity``` mapped with ```TApiEntity```
- ```SendReadAllQuery()```: send a ```ReadAllQuery<TReadAllResult>``` for you
- ```SendReadAllQuery<TModelEntityReadAllResult>()```: send a ```ReadAllQuery<TModelEntityReadAllResult>``` for you, with ```TModelEntityReadAllResult``` mapped with ```TReadAllResult```
- ```SendCreateCommand(TApiEntity payload)```: send a ```CreateCommand<TApiEntity>``` for you
- ```SendCreateCommand<TModelEntity>(TModelEntity payload)```: send a ```CreateCommand<TModelEntity>``` for you, with ```TModelEntity``` mapped with ```TApiEntity```
- ```SendUpdateCommand(TApiEntityKey key, TApiEntity payload)```: send an ```UpdateCommand<TApiEntityKey, TApiEntity>``` for you
- ```SendUpdateCommand<TModelEntity>(TApiEntityKey key, TModelEntity payload)```: send an ```UpdateCommand<TApiEntityKey, TModelEntity>``` for you, with ```TModelEntity``` mapped with ```TApiEntity```
- ```SendDeleteCommand(TApiEntityKey key)```: send a ```DeleteCommand<TApiEntity, TApiEntityKey>``` for you

Most of all requests get some overloads to provide some more parameters.

Apizr will intercept your request and handle it to send the result back to you, thanks to MediatR.

From there, our ViewModel can look like (only one interface necessary in real world):
```csharp
public class YourViewModel
{
    private readonly IMediator _mediator;
    private readonly IMediator<IReqResService> _reqResMediator;
    private readonly ICrudMediator<User, int, PagedResult<User>, IDictionary<string, object>> _userMediator;
	
    public YouViewModel(IMediator mediator, 
        IMediator<IReqResService> reqResMediator,
        ICrudMediator<User, int, PagedResult<User>, IDictionary<string, object>> userMediator)
    {
		_mediator = mediator;
       _reqResMediator = reqResMediator;
       _userMediator = userMediator;
    }
    
    public ObservableCollection<User>? Users { get; set; }

    // This won't compile obviously
    // It's an example presenting all ways to play with MediatR
    // You should choose one of these ways
    private async Task GetUsersAsync()
    {
        IList<User>? users;
        try
        {
            // The classic api interface way
            var userList = await _mediator.Send(new ExecuteRequest<IReqResService, UserList>((ct, api) => api.GetUsersAsync(ct)), CancellationToken.None);
            users = userList.Data;

            // The classic api interface way with typed mediator
            var userList = await _reqResMediator.SendFor(api => api.GetUsersAsync());
            users = userList.Data;
            
            // The crud api interface way
            var pagedUsers = await _mediator.Send(new ReadAllQuery<PagedResult<User>>(), CancellationToken.None);
            users = pagedUsers.Data?.ToList();
            
            // The crud api interface way with typed mediator
            var pagedUsers = await _userMediator.SendReadAllQuery();
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

<h3 id="optional">
Optional:
</h3>

In extensions registration approach and with the dedicated integration nuget package referenced, the options builder let you enable mediation with Optional result by calling:
```csharp
optionsBuilder => optionsBuilder.WithOptionalMediation()
```

Again, don't forget to register MediatR itself as usual :
```csharp
services.AddMediatR(typeof(Startup));
```

When activated, you don't have to inject/resolve anything else than an ```IMediator``` instance, in order to play with your api services (both classic and crud).
Everything you need to do then, is sending your request calling:
```csharp
var result = await _mediator.Send(YOUR_REQUEST_HERE);
```

Where YOUR_REQUEST_HERE could be, with classic api interfaces:
- ```ExecuteOptionalRequest<TWebApi>```: execute any method from ```TWebApi``` defined by an expression parameter witch returns ```Option<Unit, ApizrException>```
- ```ExecuteOptionalRequest<TWebApi, TApiResponse>```: execute any method from ```TWebApi``` defined by an expression parameter witch returns ```Option<TApiResponse, ApizrException<TApiResponse>>```
- ```ExecuteOptionalRequest<TWebApi, TModelResponse, TApiResponse>```: execute any method from ```TWebApi``` defined by an expression parameter witch returns ```Option<TModelResponse, ApizrException<TModelResponse>>``` where ```TModelResponse``` mapped from ```TApiResponse```

> **NOTE - Mapping**:
> When I say "mapped", I talk about the mapping integration feature
>
> Please refer to AutoMapper section for more info

Or with crud api interfaces:
- ```ReadOptionalQuery<T>```: get the T entity with int and returns ```Option<T, ApizrException<T>>```
- ```ReadOptionalQuery<T, TKey>```: get the T entity with TKey and returns ```Option<T, ApizrException<T>>```
- ```ReadAllOptionalQuery<TReadAllResult>```: get TReadAllResult with IDictionary<string, object> optional query parameters and returns ```Option<TReadAllResult, ApizrException<TReadAllResult>>```
- ```ReadAllOptionalQuery<TReadAllParams, TReadAllResult>```: get TReadAllResult with TReadAllParams optional query parameters and returns ```Option<TReadAllResult, ApizrException<TReadAllResult>>```
- ```CreateOptionalCommand<T>```: create a T entity and returns ```Option<Unit, ApizrException>```
- ```UpdateOptionalCommand<T>```: update the T entity with int and returns ```Option<Unit, ApizrException>```
- ```UpdateOptionalCommand<TKey, T>```: update the T entity with TKey and returns ```Option<Unit, ApizrException>```
- ```DeleteOptionalCommand<T>```: delete the T entity with int and returns ```Option<Unit, ApizrException>```
- ```DeleteOptionalCommand<T, TKey>```: delete the T entity with TKey and returns ```Option<Unit, ApizrException>```

There's also a typed optional mediator available for each api interface (classic or crud), to help you write things shorter.

With classic api interfaces, resolving ```IOptionalMediator<TWebApi>``` give you access to:
- ```SendFor(YOUR_API_METHOD_EXPRESSION)```: send an ```ExecuteOptionalRequest<TWebApi>``` for you
- ```SendFor<TApiResponse>(YOUR_API_METHOD_EXPRESSION)```: send an ```ExecuteOptionalRequest<TWebApi, TApiResponse>``` for you
- ```SendFor<TModelResponse, TApiResponse>(YOUR_API_METHOD_EXPRESSION)```: send an ```ExecuteOptionalRequest<TWebApi, TModelResponse, TApiResponse>``` for you

With crud api interfaces, resolving ```ICrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>``` give you access to:
- ```SendReadOptionalQuery(TApiEntityKey key)```: send a ```ReadOptionalQuery<TApiEntity, TApiEntityKey>``` for you
- ```SendReadOptionalQuery<TModelEntity>(TApiEntityKey key)```: send a ```ReadOptionalQuery<TModelEntity, TApiEntityKey>``` for you, with ```TModelEntity``` mapped with ```TApiEntity```
- ```SendReadAllOptionalQuery()```: send a ```ReadAllOptionalQuery<TReadAllResult>``` for you
- ```SendReadAllOptionalQuery<TModelEntityReadAllResult>()```: send a ```ReadAllOptionalQuery<TModelEntityReadAllResult>``` for you, with ```TModelEntityReadAllResult``` mapped with ```TReadAllResult```
- ```SendCreateOptionalCommand(TApiEntity payload)```: send a ```CreateOptionalCommand<TApiEntity>``` for you
- ```SendCreateOptionalCommand<TModelEntity>(TModelEntity payload)```: send a ```CreateOptionalCommand<TModelEntity>``` for you, with ```TModelEntity``` mapped with ```TApiEntity```
- ```SendUpdateOptionalCommand(TApiEntityKey key, TApiEntity payload)```: send an ```UpdateOptionalCommand<TApiEntityKey, TApiEntity>``` for you
- ```SendUpdateOptionalCommand<TModelEntity>(TApiEntityKey key, TModelEntity payload)```: send an ```UpdateOptionalCommand<TApiEntityKey, TModelEntity>``` for you, with ```TModelEntity``` mapped with ```TApiEntity```
- ```SendDeleteOptionalCommand(TApiEntityKey key)```: send a ```DeleteOptionalCommand<TApiEntity, TApiEntityKey>``` for you

Apizr will intercept it and handle it to send the result back to you, thanks to MediatR and Optional.

From there, our ViewModel can look like (only one interface necessary in real world):
```csharp
public class YourViewModel
{
    private readonly IMediator _mediator;
    private readonly IOptionalMediator<IReqResService> _reqResOptionalMediator;
    private readonly ICrudOptionalMediator<User, int, PagedResult<User>, IDictionary<string, object>> _userOptionalMediator;
	
    public YouViewModel(IMediator mediator, 
        IOptionalMediator<IReqResService> reqResOptionalMediator,
        ICrudOptionalMediator<User, int, PagedResult<User>, IDictionary<string, object>> userOptionalMediator)
    {
		_mediator = mediator;
       _reqResOptionalMediator = reqResOptionalMediator;
       _userOptionalMediator = userOptionalMediator;
    }
    
    public ObservableCollection<User>? Users { get; set; }

    // This won't compile obviously
    // It's an example presenting all ways to play with Optional
    // You should choose one of these ways
    private async Task GetUsersAsync()
    {
        // The classic api interface way with mediator and optional request
        var optionalUserList = await _mediator.Send(new ExecuteOptionalRequest<IReqResService, UserList>((ct, api) => api.GetUsersAsync(ct)), CancellationToken.None);
        
        // The classic api interface way with typed optional mediator (the same but shorter)
        var optionalUserList = await _reqResOptionalMediator.SendFor(api => api.GetUsersAsync());

        // Handling the optional result for both previous ways
        optionalPagedResult.Match(userList =>
        {
            if (userList.Data != null && userList.Data.Any())
                Users = new ObservableCollection<User>(userList.Data);
        }, e =>
        {
            var message = e.InnerException is IOException ? "No network" : (e.Message ?? "Error");
            UserDialogs.Instance.Toast(new ToastConfig(message) { BackgroundColor = Color.Red, MessageTextColor = Color.White });

            if (e.CachedResult?.Data != null && e.CachedResult.Data.Any())
                Users = new ObservableCollection<User>(e.CachedResult.Data);
        });
            
        // The crud api interface way with mediator and optional request
        var optionalPagedResult = await _mediator.Send(new ReadAllOptionalQuery<PagedResult<User>>(), CancellationToken.None);
        
        // The crud api interface way with typed crud optional mediator
        var optionalPagedResult = await _userOptionalMediator.SendReadAllOptionalQuery();
        
        // Handling the optional result for both previous ways
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
It means that if you call both methods during registration, both request collection will be available, so you can decide witch one suits to you when you need it.

<h4 id="optional-helper-extentions">
Optional helper extentions:
</h4>

Optional and MediatR are pretty cool.

But even if we use the typed optional mediator or typed crud optional mediator to get things shorter, we still have to deal with the result boilerplate:
```csharp
// The classic api interface way with typed optional mediator (the same but shorter)
var optionalUserList = await _reqResOptionalMediator.SendFor(api => api.GetUsersAsync());

// Handling the optional result for both previous ways
optionalPagedResult.Match(userList =>
{
    if (userList.Data != null && userList.Data.Any())
        Users = new ObservableCollection<User>(userList.Data);
}, e =>
{
    var message = e.InnerException is IOException ? "No network" : (e.Message ?? "Error");
    UserDialogs.Instance.Toast(new ToastConfig(message) { BackgroundColor = Color.Red, MessageTextColor = Color.White });

    if (e.CachedResult?.Data != null && e.CachedResult.Data.Any())
        Users = new ObservableCollection<User>(e.CachedResult.Data);
});
```

Let's cut down the optional result handling thing, to get something as short as we can.

```OnResultAsync``` and ```CatchAsync``` are extension methods to handle optional result fluently.

<h5 id="optional-onresultasync">
OnResultAsync:
</h5>

```OnResultAsync``` ask you to provide one of these parameters:
- ```Action<TResult> onResult```: this action will be invoked just before throwing any exception that might have occurred during request execution
- ```Func<TResult, ApizrException<TResult>, bool> onResult```: this function will be invoked with the returned result and potential occurred exception
- ```Func<TResult, ApizrException<TResult>, Task<bool>> onResult```: this function will be invoked async with the returned result and potential occurred exception

All give you a result returned from fetch if succeed, or cache if failed.
The main goal here is to set any binded property with the returned result (fetched or cached), no matter of exceptions.
Then the Action will let the exception throw, where the Func will let you decide to throw manually or return a success boolean flag.

Here is what our final request looks like with Action (auto throwing after invocation on excpetion):
```csharp
await _reqResOptionalMediator.SendFor(api => api.GetUsersAsync()).OnResultAsync(userList => { users = userList?.Data; });
```

Or with Func and throw:
```csharp
await _reqResOptionalMediator.SendFor(api => api.GetUsersAsync()).OnResultAsync((userList, exception) => 
{ 
    users = userList?.Data; 
    
    if(exception != null)
        throw exception;

    return true;
});
```

Or with Func and success flag:
```csharp
var success = await _reqResOptionalMediator.SendFor(api => api.GetUsersAsync()).OnResultAsync((userList, exception) => 
{ 
    users = userList?.Data; 
    
    return exception != null;
});
```

We could combine the first two with [AsyncErrorHandler](https://github.com/Fody/AsyncErrorHandler), to catch them all globally and show any information dialog to the user, like:
```csharp
public static class AsyncErrorHandler
{
    public static void HandleException(Exception exception)
    {
        var message = exception  is IOException || exception.InnerException is IOException ? "No network" : (exception.Message ?? "Error");
        UserDialogs.Instance.Toast(new ToastConfig(message) { BackgroundColor = Color.Red, MessageTextColor = Color.White });

        Log.Write(exception);
    }
}
```

<h5 id="optional-catchasync">
CatchAsync:
</h5>

```CatchAsync``` let you provide these parameters:
- ```Action<Exception> onException```: this action will be invoked just before returning the result from cache if fetch failed. Useful to inform the user of the api call failure and that data comes from cache.
- ```letThrowOnExceptionWithEmptyCache```: True to let it throw the inner exception in case of empty cache, False to handle it with ```onException``` action and return empty cache result (default: False)

This one is to return result from fetch or cache, no matter of execption handled on the other side by an action callback to inform the user
```csharp
var users = await _reqResOptionalMediator.SendFor(api => api.GetUsersAsync()).CatchAsync(AsyncErrorHandler.HandleException, true);
```

Here we ask the api to get users and if it fails:
- There's some cached data?
  - AsyncErrorHandler will handle the exception to inform the user call just failed
  - Apizr will return the previous result from cache
- There's no cached data yet!
  - ```letThrowOnExceptionWithEmptyCache``` is True? (witch is the case here)
    - Apizr will throw the inner exception that will be catched further by AsyncErrorHander (this is its normal Fody usage)
  - ```letThrowOnExceptionWithEmptyCache``` is False! (default)
    - Apizr will return the empty cache data (null) witch has to be handled further

Safe and shorter than ever!

<h3 id="automapper">
AutoMapper:
</h3>

You can define your own model entities and then, your AutoMapper mapping profiles between api entities and model entities.

Then, you have to tell Apizr witch entities must use the mapping feature.

<h4 id="automapper-with-crud-apis">
AutoMapper with Crud apis:
</h4>

<h5 id="automapper-crud-manually">
Manually:
</h5>

```csharp
services.AddApizrCrudFor<MappedEntity<TModelEntity, TApiEntity>>(optionsBuilder =>
    optionsBuilder.WithBaseAddress("https://myapi.com/api/myentity")
        .WithMediation()
        .WithMappingHandler<AutoMapperMappingHandler>());
```

Manual registration makes use of MappedEntity<TModelEntity, TApiEntity> just in place of our usual T.
You'll have to enable one or both mediation feature to handle requests (classic and/or optional) and provide a mapping handler.
You'll have to repeat this registration for each crud mapping.

Don't forget to register AutoMapper itself as usual :
```csharp
services.AddAutoMapper(typeof(Startup));
```

<h5 id="automapper-crud-automatically">
Automatically:
</h5>

Why not let Apizr do it for you?
To do so, you have do decorate one of those two entities (api vs model) with corresponding attribute:
- ```CrudEntityAttribute``` above the api entity, with ```modelEntityType``` parameter set to the mapped model entity type
- ```MappedCrudEntityAttribute``` above the model entity, with ```apiEntityType``` parameter set to the mapped api entity type

If you get access to both entities, it doesn't matter witch one you decorate, just do it for one of it (if you decorate both, it will take the first found).
If you don't get any access to the api entities, just decorate your model one with the ```MappedCrudEntityAttribute```

From here, let's write:
```csharp
services.AddApizrCrudFor(optionsBuilder => optionsBuilder
        .WithMediation()
        .WithMappingHandler<AutoMapperMappingHandler>(), 
        typeof(AnyTApiEntity), typeof(AnyTModelEntity));
```

In this example, I provided both api entity and model entity assemblies to the attribute scanner, but actually you just have to provide the one containing your attribute decorated entities (api or model, depending of your scenario/access rights).

Don't forget to register AutoMapper itself as usual :
```csharp
services.AddAutoMapper(typeof(Startup));
```

<h5 id="automapper-crud-using">
Using:
</h5>

Nothing different here but direct using of your model entities when sending mediation requests, like:
```csharp
var createdModelEntity = await _mediator.Send(new CreateCommand<TModelEntity>(myModelEntity), CancellationToken.None);
```

Apizr will map myModelEntity to TApiEntity, send it to the server, map the result to TModelEntity and send it back to you.
And yes, it works also with Optional.

<h4 id="automapper-with-classic-apis">
AutoMapper with classic apis:
</h4>

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

<h5 id="automapper-classic-using">
Using:
</h5>

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

<h2 id="pitfalls">
Pitfalls:
</h2>

Like [Refit.Insane.PowerPack](https://github.com/thefex/Refit.Insane.PowerPack) does, Apizr depends on ```Expression<>``` to wrap refit API interfaces and analyse method calls with the same pitfall: 
**allways pass method agruments as local variables**

Instead of:
```csharp
var result = await _myManager.ExecuteAsync(api => api.UpdateObjectAsync(MyObject.IdProperty, new MyObjectDetails{ Details = details }));
```

Use:
```csharp
var myObjectId = MyObject.IdProperty;
var myObjectDetails = new MyObjectDetails{ Details = details };

var result = await _myManager.ExecuteAsync(api => api.UpdateObjectAsync(myObjectId, myObjectDetails));
```