## Configuring data caching

You may want to cache data returned from your API.
Apizr could handle it for you by providing an `ICachingHandler` interface implementation to it.
Fortunately, there are some integration Nuget packages to do so.
Of course, you can implement your own integration, but here we'll talk about the provided ones.

Please first install the integration package of your choice:

|Project|Current|Upcoming|
|-------|-----|-----|
|Apizr.Extensions.Microsoft.Caching|[![NuGet](https://img.shields.io/nuget/v/Apizr.Extensions.Microsoft.Caching.svg)](https://www.nuget.org/packages/Apizr.Extensions.Microsoft.Caching/)|[![NuGet Pre Release](https://img.shields.io/nuget/vpre/Apizr.Extensions.Microsoft.Caching.svg)](https://www.nuget.org/packages/Apizr.Extensions.Microsoft.Caching/)|
|Apizr.Integrations.Akavache|[![NuGet](https://img.shields.io/nuget/v/Apizr.Integrations.Akavache.svg)](https://www.nuget.org/packages/Apizr.Integrations.Akavache/)|[![NuGet Pre Release](https://img.shields.io/nuget/vpre/Apizr.Integrations.Akavache.svg)](https://www.nuget.org/packages/Apizr.Integrations.Akavache/)|
|Apizr.Integrations.MonkeyCache|[![NuGet](https://img.shields.io/nuget/v/Apizr.Integrations.MonkeyCache.svg)](https://www.nuget.org/packages/Apizr.Integrations.MonkeyCache/)|[![NuGet Pre Release](https://img.shields.io/nuget/vpre/Apizr.Integrations.MonkeyCache.svg)](https://www.nuget.org/packages/Apizr.Integrations.MonkeyCache/)|

Where:
   - **Apizr.Extensions.Microsoft.Caching** package brings an ICacheHandler implementation for [MS Extensions Caching](https://www.nuget.org/packages/Microsoft.Extensions.Caching.Abstractions)
   - **Apizr.Integrations.Akavache** package brings an ICacheHandler implementation for [Akavache](https://github.com/reactiveui/Akavache)
   - **Apizr.Integrations.MonkeyCache** package brings an ICacheHandler implementation for [MonkeyCache](https://github.com/jamesmontemagno/monkey-cache)

>[!WARNING]
>
>**Apizr.Extensions.Microsoft.Caching**
>
>This integration package offers you to work with any of MS Extension Caching compatible caching engines. It means that you still have to install the one of your choice right after Apizr.Extensions.Microsoft.Caching.


### Defining

Apizr comes with a `Cache` attribute which activate result data caching at any level (all Assembly apis, interface apis or specific api method).

Here is classic api an example:
```csharp
namespace Apizr.Sample
{
    [WebApi("https://reqres.in/api")]
    public interface IReqResService
    {
        [Get("/users"), Cache(CacheMode.GetAndFetch, "01:00:00")]
        Task<UserList> GetUsersAsync();

        [Get("/users/{userId}"), Cache(CacheMode.GetOrFetch, "1.00:00:00")]
        Task<UserDetails> GetUserAsync([CacheKey] int userId);
    }
}
```

You’ll find also cache attributes dedicated to CRUD apis like `CacheRead` and `CacheReadAll`, so you could define cache settings at any level (all Assembly apis, interface apis or specific CRUD method).

Here is CRUD api an example:
```csharp
namespace Apizr.Sample.Models
{
    [CrudEntity("https://reqres.in/api/users", typeof(int), typeof(PagedResult<>))]
    [CacheReadAll(CacheMode.GetAndFetch, "01:00:00")]
    [CacheRead(CacheMode.GetOrFetch, "1.00:00:00")]
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

Both (classic and CRUD) define the same thing about cache life time and cache mode.

Life time is actually a `TimeSpan` string representation which is parsed then. Its optional and if you don’t provide it, the default cache provider settings will be applyed.

Cache mode could be set to:

  - `GetAndFetch` (default): the result is returned from request if it succeed, otherwise from cache if there’s some data already cached. In this specific case of request failing, cached data will be wrapped with the original exception into an ApizrException thrown by Apizr, so don’t forget to catch it.
  - `GetOrFetch`: the result is returned from cache if there’s some data already cached, otherwise from the request.

In both cases, cached data is updated after each successful request call.

You also can define global caching settings by decorating the assembly or interface, then manage specific scenarios at method level. 
Apizr will apply the lowest level settings it could find.

Back to the example, we are saying:

- When getting all users, let’s admit we could have many new users registered each hour, so:
    - Try to fetch it from web first
        - if fetch failed, try to load it from previous cached result
        - if fetch succeed, update cached data but make it expire after 1 hour
- When getting a specific user, let’s admit its details won’t change so much each day, so:
    - Try to load it from cache first
        - if no previous cached data or cache expired after 1 day, fetch it and update cached data but make it expire after 1 day

### Registering

Please register the one corresponding to the package you just installed

#### MS Extensions Caching

As you can guess, MS Extensions Caching is available only with extended registration flavor. 
That said, you'll be able to register with one of the folowing options

##### [In-Memory](#tab/tabid-inmemory)

```csharp
// direct short configuration
options => options.WithInMemoryCacheHandler()

// OR closed type configuration
options => options.WithCacheHandler<InMemoryCacheHandler>()

// OR type configuration
options => options.WithCacheHandler(typeof(InMemoryCacheHandler))

// OR direct configuration
options => options.WithCacheHandler(new InMemoryCacheHandler(new YOUR_INMEMORY_CACHING_ENGINE()))

// OR factory configuration with the service provider instance
options => options.WithCacheHandler(serviceProvider => new InMemoryCacheHandler(serviceProvider.GetRequiredService<IMemoryCache>()))
```

##### [Distributed](#tab/tabid-distributed)

```csharp
// direct short configuration
options => options.WithDistributedCacheHandler<TCacheType>()

// OR closed type configuration
options => options.WithCacheHandler<DistributedCacheHandler<TCacheType>>()

// OR type configuration
options => options.WithCacheHandler(typeof(DistributedCacheHandler<TCacheType>))

// OR direct configuration
options => options.WithCacheHandler(new DistributedCacheHandler<TCacheType>(new YOUR_DISTRIBUTED_CACHING_ENGINE()))

// OR factory configuration
options => options.WithCacheHandler(serviceProvider => new DistributedCacheHandler<TCacheType>(
    serviceProvider.GetRequiredService<IDistributedCache>(), 
    serviceProvider.GetRequiredService<IHttpContentSerializer>()))
```

Where `TCacheType` could be either `string` or `byte[]`, conforming to MS Extensions Distributed Cache definition.

>[!WARNING]
>
>**Distributed cache**
>
>Registering MS Extension Distributed Cache means that you have to install the distributed cache of your choice and register it too.

***

#### Akavache

You'll be able to register with one of the folowing options:

##### [Static](#tab/tabid-static)

```csharp
// direct short configuration
options => options.WithAkavacheCacheHandler()

// OR direct configuration
options => options.WithCacheHandler(new AkavacheCacheHandler())

// OR factory configuration
options => options.WithCacheHandler(() => new AkavacheCacheHandler())
```

##### [Extended](#tab/tabid-extended)

```csharp
// direct short configuration
options => options.WithAkavacheCacheHandler()

// OR closed type configuration
options => options.WithCacheHandler<AkavacheCacheHandler>()

// OR type configuration
options => options.WithCacheHandler(typeof(AkavacheCacheHandler))

// OR direct configuration
options => options.WithCacheHandler(new AkavacheCacheHandler())

// OR factory configuration
options => options.WithCacheHandler(serviceProvider => new AkavacheCacheHandler())
```

***

Where most of it get overloads so you could set:
- `blobCacheFactory`: The factory to init the blob cache of your choice (default: LocalMachine)
- `applicationName`: The application name used by Akavache (default: ApizrAkavacheCacheHandler)

#### MonkeyCache

Start by initializing `Barrel.ApplicationId` as you used to do with MonkeyCache:
```csharp
Barrel.ApplicationId = "YOUR_APPLICATION_NAME";
```

Then you'll be able to register with one of the folowing options:

##### [Static](#tab/tabid-static)

```csharp
// direct configuration
options => options.WithCacheHandler(new MonkeyCacheHandler(Barrel.Current))

// OR factory configuration
options => options.WithCacheHandler(() => new MonkeyCacheHandler(Barrel.Current))
```

##### [Extended](#tab/tabid-extended)

```csharp
// closed type configuration
options => options.WithCacheHandler<MonkeyCacheHandler>()

// OR type configuration
options => options.WithCacheHandler(typeof(MonkeyCacheHandler))

// OR direct configuration
options => options.WithCacheHandler(new MonkeyCacheHandler(Barrel.Current))

// OR factory configuration
options => options.WithCacheHandler(serviceProvider => new MonkeyCacheHandler(Barrel.Current))
```

>[!WARNING]
>
>**Barrel.Current**
>
>If you don't provide Barrel.Current to the MonkeyCacheHandler, don't forget to register it into your DI container.


***

### Using

#### Reading

##### From thrown `ApizrException<T>`

Using Apizr caching feature is just about catching exceptions like for example:
```csharp
IList<User>? users = null;
try
{
    var userList = await _reqResManager.ExecuteAsync(api => api.GetUsersAsync());
    users = userList?.Data;
}
catch (ApizrException<UserList> e)
{
    users = e.CachedResult?.Data;
}
finally
{
    if (users != null && users.Any())
        Users = new ObservableCollection<User>(users);
}
```

Here we catch an `ApizrException<UserList>` meaning that in case of exception, it will bring a typed object to you loaded from cache.

##### From returned `IApizrResponse<T>`

If your api methods return an `IApiResponse<T>` provided by Refit, you can handle the `IApizrResponse<T>` returned by Apizr to get your data from the cache, the safe way without throwing any exception.

```csharp
// Here we wrap the response into an IApiResponse<T> provided by Refit
[WebApi("https://reqres.in/api")]
public interface IReqResService
{
    [Get("/users")]
    Task<IApiResponse<UserList>> GetUsersAsync();
}

...

// Then we can handle the IApizrResponse<T> response comming from Apizr
var response = await _reqResManager.ExecuteAsync(api => api.GetUsersAsync());

// Log potential errors and maybe inform the user about it
if(!response.IsSuccess)
{
   _logger.LogError(response.Exception);
    Alert.Show("Error", response.Exception.Message);
}

// Use the data, no matter the source
if(response.Result?.Data?.Any() == true)
{
    Users = new ObservableCollection<User>(response.Result.Data);

    // Inform the user that data comes from cache if so
    if(response.DataSource == ApizrResponseDataSource.Cache)
        Toast.Show("Data comes from cache");
}
```

Read the exception handling documentation to get more details about it.

#### Clearing

You may need to clear cache.
Remeber that cache will be cleared when it will expire, thanks to what you set within the `Cache` attribute.
Anyway, sometime we need to clear it explicitly, like in a Refresh scenario.

Here are different ways to clear cache:
```csharp
// Clear on call to force fetch and update cache
var userList = await _reqResManager.ExecuteAsync(api => api.GetUsersAsync(), options => options.WithCacheClearing(true));

// Clear a specific request cache
var succeed = await _reqResManager.ClearCacheAsync(api => api.GetUsersAsync());

// Clear all cache
var succeed = await _reqResManager.ClearCacheAsync();
```

Clearing all cache of all managers of entire app could also be done thanks to your cache engine api.