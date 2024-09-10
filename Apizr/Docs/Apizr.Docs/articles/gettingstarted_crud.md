## Watching

Please find this getting started video tutorial on YouTube about how to get started with Apizr:

> [!Video https://youtu.be/9qXekjZepLA]

## Defining

As we'll use the built-in yet defined ICrudApi, there's no more definition to do.

Here is what the provided interface looks like then:
```csharp
public interface ICrudApi<T, in TKey, TReadAllResult, in TReadAllParams> where T : class
{
    // ==== Create ==== //
    [Post("")]
    Task<T> Create([Body] T payload);

    [Post("")]
    Task<IApiResponse<T>> SafeCreate([Body] T payload);

    [Post("")]
    Task<T> Create([Body] T payload, [RequestOptions] IApizrRequestOptions options);

    [Post("")]
    Task<IApiResponse<T>> SafeCreate([Body] T payload, [RequestOptions] IApizrRequestOptions options);

    // ==== ReadAll ==== //
    [Get("")]
    Task<TReadAllResult> ReadAll();

    [Get("")]
    Task<IApiResponse<TReadAllResult>> SafeReadAll();

    [Get("")]
    Task<TReadAllResult> ReadAll([RequestOptions] IApizrRequestOptions options);

    [Get("")]
    Task<IApiResponse<TReadAllResult>> SafeReadAll([RequestOptions] IApizrRequestOptions options);

    [Get("")]
    Task<TReadAllResult> ReadAll([CacheKey] TReadAllParams readAllParams);

    [Get("")]
    Task<IApiResponse<TReadAllResult>> SafeReadAll([CacheKey] TReadAllParams readAllParams);

    [Get("")]
    Task<TReadAllResult> ReadAll([CacheKey] TReadAllParams readAllParams, [RequestOptions] IApizrRequestOptions options);

    [Get("")]
    Task<IApiResponse<TReadAllResult>> SafeReadAll([CacheKey] TReadAllParams readAllParams, [RequestOptions] IApizrRequestOptions options);

    // ==== Read ==== //
    [Get("/{key}")]
    Task<T> Read([CacheKey] TKey key);

    [Get("/{key}")]
    Task<IApiResponse<T>> SafeRead([CacheKey] TKey key);

    [Get("/{key}")]
    Task<T> Read([CacheKey] TKey key, [RequestOptions] IApizrRequestOptions options);

    [Get("/{key}")]
    Task<IApiResponse<T>> SafeRead([CacheKey] TKey key, [RequestOptions] IApizrRequestOptions options);

    // ==== Update ==== //
    [Put("/{key}")]
    Task Update(TKey key, [Body] T payload);

    [Put("/{key}")]
    Task<IApiResponse> SafeUpdate(TKey key, [Body] T payload);

    [Put("/{key}")]
    Task Update(TKey key, [Body] T payload, [RequestOptions] IApizrRequestOptions options);

    [Put("/{key}")]
    Task<IApiResponse> SafeUpdate(TKey key, [Body] T payload, [RequestOptions] IApizrRequestOptions options);

    // ==== Delete ==== //
    [Delete("/{key}")]
    Task Delete(TKey key);

    [Delete("/{key}")]
    Task<IApiResponse> SafeDelete(TKey key);

    [Delete("/{key}")]
    Task Delete(TKey key, [RequestOptions] IApizrRequestOptions options);

    [Delete("/{key}")]
    Task<IApiResponse> SafeDelete(TKey key, [RequestOptions] IApizrRequestOptions options);
}
```

We can see that it comes with or without request options, allowing some option adjustments later at request time.
It comes with or whithout an ApiResponse too, allowing to handle errors or throw exceptions.

About generic types:
- T and TKey (optional - default: ```int```) meanings are obvious
- TReadAllResult (optional - default: ```IEnumerable<T>```) is there to handle cases where ReadAll doesn't return an ```IEnumerable<T>``` or derived, but a paged result with some statistics
- TReadAllParams (optional - default: ```IDictionary<string, object>```) is there to handle cases where you don't want to provide an ```IDictionary<string, object>``` for a ReadAll reaquest, but a custom class

But again, nothing to do around here.
You still can read the api documentation about it [here](/api/Apizr.Requesting.ICrudApi-4.html).

## Registering

It's not mandatory to register anything in a container for DI purpose (you can use the returned static instance directly), but we'll describe here how to use it with DI anyway.

### Registering a single interface

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

#### [Static](#tab/tabid-static)

Here is an example of how to register a managed instance of the CRUD api interface:
```csharp
// Apizr registration
myContainer.RegistrationMethod(() =>
    ApizrBuilder.Current.CreateCrudManagerFor<T, TKey, TReadAllResult, TReadAllParams>(options => options
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
            .AddCrudManagerFor<T1>(
                options => options.WithBaseAddress("your specific T1 entity crud base uri")
            .AddCrudManagerFor<T2, T2Key, T2ReadAllResult, T2ReadAllParams>(
                options => options.WithBaseAddress("your specific T2 entity crud base uri"),
    
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

##### [Static](#tab/tabid-static)

```csharp
// Apizr registry
var apizrRegistry = ApizrBuilder.Current.CreateRegistry(
    registry => registry
        .AddCrudManagerFor<T1>(
            options => options.WithBaseAddress("your specific T1 entity crud base uri")
        .AddCrudManagerFor<T2, T2Key, T2ReadAllResult, T2ReadAllParams>(
            options => options.WithBaseAddress("your specific T2 entity crud base uri"),
    
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

***

Here's how to get a manager from the registry:

```csharp
// T1 with default registered types
var t1Manager = apizrRegistry.GetCrudManagerFor<T1>();

// T2 with custom registered types
var t2Manager = apizrRegistry.GetCrudManagerFor<T2, T2Key, T2ReadAllResult, T2ReadAllParams>();
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
                    .AddCrudManagerFor<T1>(
                        config => config.WithBasePath("t1")
                    .AddCrudManagerFor<T2, T2Key, T2ReadAllResult, T2ReadAllParams>(
                        config => config.WithBasePath("t2"),
                config => config.WithBaseAddress("https://crud.io/api"))

            .AddCrudManagerFor<T3>(
                config => config.WithBaseAddress("https://crud.com/api"),
    
        config => config.WithAkavacheCacheHandler()
    );
}
```

Here is what we're saying in this example:
- Add a manager for T1 entity with CRUD api interface and default types into the registry, to register it into the container
  - Set a common base address (https://crud.io/api) dedicated to T1's manager
  - Set a specific base path (t1) dedicated to T1's manager
- Add a manager for T2 entity with CRUD api interface and custom types into the registry, to register it into the container
  - Set a common base address (https://crud.io/api) dedicated to T2's manager
  - Set a specific base path (t1) dedicated to T2's manager
- Add a manager for T3 entity with CRUD api interface and default types into the registry, to register it into the container
  - Set a specific base address (https://crud.com/api) dedicated to T3's manager
- Apply common configuration to all managers by:
  - Providing a cache handler

It's an example, meaning if you don't need common and/or specific configuration, just don't provide it.
And yes you can mix classic and CRUD manager registration into the same registry/group.
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
                .AddCrudManagerFor<T1>(
                    config => config.WithBasePath("t1")
                .AddCrudManagerFor<T2, T2Key, T2ReadAllResult, T2ReadAllParams>(
                    config => config.WithBasePath("t2"),
            config => config.WithBaseAddress("https://crud.io/api"))

        .AddCrudManagerFor<T3>(
            config => config.WithBaseAddress("https://crud.com/api"),
    
    config => config.WithAkavacheCacheHandler()
);

// Container registration
apizrRegistry.Populate((type, factory) => 
    myContainer.RegistrationMethodFactory(type, factory)
);
```

Here is what we're saying in this example:
- Add a manager for T1 entity with CRUD api interface and default types into the registry
  - Set a common base address (https://crud.io/api) dedicated to T1's manager
  - Set a specific base path (t1) dedicated to T1's manager
- Add a manager for T2 entity with CRUD api interface and custom types into the registry
  - Set a common base address (https://crud.io/api) dedicated to T2's manager
  - Set a specific base path (t1) dedicated to T2's manager
- Add a manager for T3 entity with CRUD api interface and default types into the registry
  - Set a specific base address (https://crud.com/api) dedicated to T3's manager
- Apply common configuration to all managers by:
  - Providing a cache handler

It's an example, meaning if you don't need common and/or specific configuration, just don't provide it.
And yes you can mix classic and CRUD manager registration into the same registry/group.
You can add mutliple group at the same level and go deeper with group into group itself.

Also, you could register the registry itslef, instead of its populated managers and then use its managers directly.

Or, you could use the managers directly from the registry instead of registering anything.

***

Here's how to get a manager from the registry:

```csharp
// T1 with default registered types
var t1Manager = apizrRegistry.GetCrudManagerFor<T1>();

// T2 with custom registered types
var t2Manager = apizrRegistry.GetCrudManagerFor<T2, T2Key, T2ReadAllResult, T2ReadAllParams>();

// T3 with default registered types
var t3Manager = apizrRegistry.GetCrudManagerFor<T3>();
```

### Registering all scanned interfaces

#### [Extended](#tab/tabid-extended)

First you have to tell Apizr which entity to auto register a crud api for by assembly scanning thanks to the `AutoRegister` attribute:
```csharp
[AutoRegister("https://mybaseuri.com/api/myentity")]
public record MyEntity
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    ...
}

 // OR with custom arguments
[AutoRegister<ICrudApi<MyEntity, string, MyPagedResult<MyEntity>, MyReadAllParamsType>>("https://mybaseuri.com/api/myentity")]
public record MyEntity
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    ...
}
```

Thanks to this attribute:
- (Optional) We can specify the crud api with custom arguments if needed (otherwise default arguments will be `int`, `IEnumerable<T>` and `IDictionary<string, object>`)
- (Mandatory) We have to provide the specific entity crud base uri (no more fluent declaration)

Then fluently just write:
```csharp
public override void ConfigureServices(IServiceCollection services)
{
    // Apizr registration
    services.AddApizrCrudManagerFor([ASSEMBLIES_CONTAINING_ENTITIES]);
}
```

Apizr will scan assemblies to auto register managers for decorated entities.

#### [Static](#tab/tabid-static)

Not available.

***

## Using

Here is an example of how to send a web request from an app - e.g. using Apizr in a MAUI mobile app.

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
 
            // OR with some option adjustments
            // var userList = await _userCrudManager.ExecuteAsync((options, api) => api.ReadAll(options),
            //                  options => options.WithPriority(Priority.Background)); 

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