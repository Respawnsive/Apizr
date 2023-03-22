## Defining

As we'll use the built-in yet defined ICrudApi, there's no more definition to do.

Here is what the provided interface looks like then:
```csharp
public interface ICrudApi<T, in TKey, TReadAllResult, in TReadAllParams> where T : class
{
    #region Create

    [Post("")]
    Task<T> Create([Body] T payload);

    [Post("")]
    Task<T> Create([Body] T payload, [Context] Context context);

    [Post("")]
    Task<T> Create([Body] T payload, CancellationToken cancellationToken);

    [Post("")]
    Task<T> Create([Body] T payload, [Context] Context context, CancellationToken cancellationToken);

    #endregion

    #region ReadAll

    [Get("")]
    Task<TReadAllResult> ReadAll();

    [Get("")]
    Task<TReadAllResult> ReadAll([CacheKey] TReadAllParams readAllParams);

    [Get("")]
    Task<TReadAllResult> ReadAll([Property(Constants.PriorityKey)] int priority);

    [Get("")]
    Task<TReadAllResult> ReadAll([Context] Context context);

    [Get("")]
    Task<TReadAllResult> ReadAll(CancellationToken cancellationToken);

    [Get("")]
    Task<TReadAllResult> ReadAll([CacheKey] TReadAllParams readAllParams, [Property(Constants.PriorityKey)] int priority);

    [Get("")]
    Task<TReadAllResult> ReadAll([CacheKey] TReadAllParams readAllParams, [Context] Context context);

    [Get("")]
    Task<TReadAllResult> ReadAll([CacheKey] TReadAllParams readAllParams, CancellationToken cancellationToken);

    [Get("")]
    Task<TReadAllResult> ReadAll([Property(Constants.PriorityKey)] int priority, [Context] Context context);

    [Get("")]
    Task<TReadAllResult> ReadAll([Property(Constants.PriorityKey)] int priority, CancellationToken cancellationToken);

    [Get("")]
    Task<TReadAllResult> ReadAll([Context] Context context, CancellationToken cancellationToken);

    [Get("")]
    Task<TReadAllResult> ReadAll([CacheKey] TReadAllParams readAllParams, [Property(Constants.PriorityKey)] int priority, [Context] Context context);

    [Get("")]
    Task<TReadAllResult> ReadAll([CacheKey] TReadAllParams readAllParams, [Property(Constants.PriorityKey)] int priority, CancellationToken cancellationToken);

    [Get("")]
    Task<TReadAllResult> ReadAll([CacheKey] TReadAllParams readAllParams, [Context] Context context, CancellationToken cancellationToken);

    [Get("")]
    Task<TReadAllResult> ReadAll([CacheKey] TReadAllParams readAllParams, [Property(Constants.PriorityKey)] int priority, [Context] Context context, CancellationToken cancellationToken);

    #endregion

    #region Read

    [Get("/{key}")]
    Task<T> Read([CacheKey] TKey key);

    [Get("/{key}")]
    Task<T> Read([CacheKey] TKey key, [Property(Constants.PriorityKey)] int priority);

    [Get("/{key}")]
    Task<T> Read([CacheKey] TKey key, [Context] Context context);

    [Get("/{key}")]
    Task<T> Read([CacheKey] TKey key, CancellationToken cancellationToken);

    [Get("/{key}")]
    Task<T> Read([CacheKey] TKey key, [Property(Constants.PriorityKey)] int priority, [Context] Context context);

    [Get("/{key}")]
    Task<T> Read([CacheKey] TKey key, [Property(Constants.PriorityKey)] int priority, CancellationToken cancellationToken);

    [Get("/{key}")]
    Task<T> Read([CacheKey] TKey key, [Property(Constants.PriorityKey)] int priority, [Context] Context context, CancellationToken cancellationToken);

    #endregion

    #region Update

    [Put("/{key}")]
    Task Update(TKey key, [Body] T payload);

    [Put("/{key}")]
    Task Update(TKey key, [Body] T payload, [Context] Context context);

    [Put("/{key}")]
    Task Update(TKey key, [Body] T payload, CancellationToken cancellationToken);

    [Put("/{key}")]
    Task Update(TKey key, [Body] T payload, [Context] Context context, CancellationToken cancellationToken);

    #endregion

    #region Delete

    [Delete("/{key}")]
    Task Delete(TKey key);

    [Delete("/{key}")]
    Task Delete(TKey key, [Context] Context context);

    [Delete("/{key}")]
    Task Delete(TKey key, CancellationToken cancellationToken);

    [Delete("/{key}")]
    Task Delete(TKey key, [Context] Context context, CancellationToken cancellationToken); 

    #endregion
}
```

We can see that it comes with many parameter combinations, but it won't do anything until you ask Apizr to. 
Caching, Logging, Policing, Prioritizing... everything is activable fluently with the options builder.

About generic types:
- T and TKey (optional - default: ```int```) meanings are obvious
- TReadAllResult (optional - default: ```IEnumerable<T>```) is there to handle cases where ReadAll doesn't return an ```IEnumerable<T>``` or derived, but a paged result with some statistics
- TReadAllParams (optional - default: ```IDictionary<string, object>```) is there to handle cases where you don't want to provide an ```IDictionary<string, object>``` for a ReadAll reaquest, but a custom class

But again, nothing to do around here.

## Registering

It's not mandatory to register anything in a container for DI purpose (you can use the returned static instance directly), but we'll describe here how to use it with DI anyway.

### Registering a single interface

#### [Static](#tab/tabid-static)

Here is an example of how to register a managed instance of the CRUD api interface:
```csharp
// Apizr registration
myContainer.RegistrationMethod(() =>
    ApizrBuilder.CreateCrudManagerFor<T, TKey, TReadAllResult, TReadAllParams>(options => options
        .WithBaseAddress("your specific T entity crud base uri"))
);
```

T must be a class.

TKey must be primitive. If you don't provide it here, it will be defined as ```int```.

TReadAllResult must inherit from ```IEnumerable<>``` or be a class.
If you don't use paged result, just don't provide any TReadAllResult here and it will be defined as ```IEnumerable<T>```.

TReadAllParams must be a class.
If you don't use a custom class holding your query parameters, just don't provide any TReadAllParams here and it will be defined as ```IDictionary<string, object>```.

You have to provide the specific entity crud base uri with the options builder (if you don't plan to use entity crud attribute).

Also, you could use the manager directly instead of registering it.

#### [Extended](#tab/tabid-extended)

Here is an example of how to register a managed CRUD api interface:
```csharp
public override void ConfigureServices(IServiceCollection services)
{
    // Apizr registration
    services.AddApizrCrudManagerFor<T, TKey, TReadAllResult, TReadAllParams>(options => options
        .WithBaseAddress("your specific T entity crud base uri"));
}
```

Again, T must be a class.

TKey must be primitive. If you don't provide it here, it will be defined as ```int```.

TReadAllResult must inherit from ```IEnumerable<>``` or be a class.
If you don't use paged result, just don't provide any TReadAllResult here and it will be defined as ```IEnumerable<T>```.

TReadAllParams must be a class.
If you don't use a custom class holding your query parameters, just don't provide any TReadAllParams here and it will be defined as ```IDictionary<string, object>```.

You have to provide the specific entity crud base uri with the options builder (if you don't plan to use entity crud attribute).

***

### Registering multiple interfaces

#### [Static](#tab/tabid-static)

You may want to register multiple managed api interfaces within the same project.
Also, you may want to share some common configuration between apis without repeating yourself, but at the same time, you may need to set some specific ones for some of it.
This is where the ApizrRegistry comes on stage.

Here is an example of how to register a managed instance of multiple api interfaces:
```csharp
// Apizr registry
var apizrRegistry = ApizrBuilder.CreateRegistry(
    registry => registry
        .AddCrudManagerFor<T1>(
            options => options
                .WithBaseAddress("your specific T1 entity crud base uri")
        .AddCrudManagerFor<T2, T2Key, T2ReadAllResult, T2ReadAllParams>(
            options => options
                .WithBaseAddress("your specific T2 entity crud base uri"),
    
    config => config
        .WithAkavacheCacheHandler()
);

// Container registration
apizrRegistry.Populate((type, factory) => 
    myContainer.RegistrationMethodFactory(type, factory)
);
```

Here is what we're saying in this example:
- Add a manager for T1 entity with CRUD api interface and default types into the registry
  - Set a specific address dedicated to T1's manager
- Add a manager for T2 entity with CRUD api interface and custom types into the registry
  - Set a specific address dedicated to T2's manager
- Apply common configuration to all managers by:
  - Providing a cache handler

It's an example, meaning if you don't need common and/or specific configuration, just don't provide it.
And yes you can mix classic and CRUD manager registration into the same registry.

Also, you could register the registry itslef, instead of its populated managers and then use its managers directly.

Or, you could use the managers directly from the registry instead of registering anything.

Here's how to get a manager from the registry:

```csharp
// T1 with default registered types
var t1Manager = apizrRegistry.GetCrudManagerFor<T1>();

// T2 with custom registered types
var t2Manager = apizrRegistry.GetCrudManagerFor<T2, T2Key, T2ReadAllResult, T2ReadAllParams>();
```

#### [Extended](#tab/tabid-extended)

You may want to register multiple managed CRUD api interfaces within the same project.
Also, you may want to share some common configuration between apis without repeating yourself, but at the same time, you may need to set some specific ones for some of it.
This is where the ApizrRegistry comes on stage.

Here is an example of how to register multiple managed CRUD api interfaces manually:
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
            .AddCrudManagerFor<T1>(
                options => options
                    .WithBaseAddress("your specific T1 entity crud base uri")
            .AddCrudManagerFor<T2, T2Key, T2ReadAllResult, T2ReadAllParams>(
                options => options
                    .WithBaseAddress("your specific T2 entity crud base uri"),
    
        config => config
            .WithAkavacheCacheHandler()
    );
}
```

Here is what we're saying in this example:
- Add a manager for T1 entity with CRUD api interface and default types into the registry, to register it into the container
  - Set a specific address dedicated to T1's manager
- Add a manager for T2 entity with CRUD api interface and custom types into the registry, to register it into the container
  - Set a specific address dedicated to T2's manager
- Apply common configuration to all managers by:
  - Providing a cache handler

It's an example, meaning if you don't need common and/or specific configuration, just don't provide it.
And yes you can mix classic and CRUD manager registration into the same registry.

Of course, each managers will be regitered into the container so that you can use it directly.

Also, the registry itslef will be registered into the container, so you could use it to get its managers, instead of using each managers.

Here's how to get a manager from the registry:

```csharp
// T1 with default registered types
var t1Manager = apizrRegistry.GetCrudManagerFor<T1>();

// T2 with custom registered types
var t2Manager = apizrRegistry.GetCrudManagerFor<T2, T2Key, T2ReadAllResult, T2ReadAllParams>();
```

***

### Registering all scanned interfaces

#### [Static](#tab/tabid-static)

Not available.

#### [Extended](#tab/tabid-extended)

You need to have access to your entity model classes for this option.

Decorate your crud entities like so (but with your own settings):
```csharp
[CrudEntity("https://mybaseuri.com/api/myentity", typeof(int), typeof(PagedResult<>), typeof(ReadAllUsersParams))]
public class MyEntity
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    ...
}
```

Thanks to this attribute:
- (Mandatory) We have to provide the specific entity crud base uri (no more fluent declaration)
- (Optional) We can set TKey type to any primitive type (default to int)
- (Optional) We can set TReadAllResult to any class or must inherit from ```IEnumerable<>``` (default to ```IEnumerable<T>```)
- (Optional) We can set TReadAllParams to any class (default to ```IDictionary<string, object>```)

Then, here is a registration example:
```csharp
public override void ConfigureServices(IServiceCollection services)
{
    // Apizr registration
    services.AddApizrCrudManagerFor(options => options.WithAkavacheCacheHandler(), ASSEMBLIES_CONTAINING_ENTITIES);
}
```

Apizr will scan assemblies to auto register managers for decorated entities.

***

## Using

Here is an example of how to send a web request from an app - e.g. using Apizr in a Xamarin.Forms mobile app.

Inject ```IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>>``` where you need it - e.g. into your ViewModel constructor
```csharp
public class YourViewModel
{
    private readonly IApizrManager<ICrudApi<User, int, PagedResult<User>, ReadAllUsersParams>> _userCrudManager;
	
    public YouViewModel(IApizrManager<ICrudApi<User, int, PagedResult<User>, ReadAllUsersParams>> userCrudManager)
    // Or registry injection
    //public YouViewModel(IApizrRegistry apizrRegistry)
    {
		_userCrudManager = userCrudManager;

        // Or registry injection
        //_userCrudManager = apizrRegistry.GetCrudManagerFor<User, int, PagedResult<User>, ReadAllUsersParams>>();
    }
    
    public ObservableCollection<User>? Users { get; set; }

    private async Task GetUsersAsync()
    {
        IList<User>? users;
        try
        {
            var pagedUsers = await _userCrudManager.ExecuteAsync(api => api.ReadAll());
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

We catch any ApizrException as it will contain the original inner exception, but also the previously cached result if some.
If you provided an IConnectivityHandler implementation and there's no network connectivity before sending request, Apizr will throw with an IO inner exception without sending the request.