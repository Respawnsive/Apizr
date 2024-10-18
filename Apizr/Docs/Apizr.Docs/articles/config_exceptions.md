## Configuring Exception handling

### By Try-Catching

Here is an example of how to send a request.

Inject ```IApizrManager<IYourDefinedInterface>``` where you need it
```csharp
IList<User>? users;
try
{
    var userList = await _reqResManager.ExecuteAsync(api => api.GetUsersAsync());
    users = userList.Data;
}
catch (ApizrException<UserList> e)
{
    var message = e.InnerException is IOException ? "No network" : (e.Message ?? "Error");
    Alert.Show("Error", message);

    users = e.CachedResult?.Data;
}

if(users != null)
    Users = new ObservableCollection<User>(users);
```

We catch any ApizrException as it will contain the original inner exception, but also the previously cached result if some.
If you provided an IConnectivityHandler implementation and there's no network connectivity before sending request, Apizr will throw an IO inner exception without sending the request.

Note that you can mix it with other handling solutions.

### By returning an Api Response

Refit has different exception handling behavior depending on if your Refit interface methods return `Task<T>` or if they return `Task<IApiResponse>`, `Task<IApiResponse<T>>`, or `Task<ApiResponse<T>>`.

When returning `Task<IApiResponse>`, `Task<IApiResponse<T>>`, or `Task<ApiResponse<T>>` **(not `Apizr` but `Api`)**,
Refit traps any `ApiException` raised by the `ExceptionFactory` when processing the response, and any errors that occur when attempting to deserialize the response to `ApiResponse<T>`, and populates the exception into the `Error` property on `ApiResponse<T>` without throwing the exception.

Then, Apizr will wrap the `ApiResponse<T>` into an `ApizrResponse<T>` plus some cached data if any and some more infos and return it as a final response.
You can then decide what to do like so:

```csharp
// Here we wrap the response into an IApiResponse<T> provided by Refit
[BaseAddress("https://reqres.in/api")]
public interface IReqResService
{
    [Get("/users")]
    Task<IApiResponse<UserList>> GetUsersAsync();
}

...

// Then we can handle the IApizrResponse<T> response comming from Apizr
var response = await _reqResManager.ExecuteAsync(api => api.GetUsersAsync());

// Log potential unhandled exceptions and maybe inform the user about it
if(!response.IsSuccess && !response.Exception.Handled)
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

Note that you can mix it with other handling solutions.

### By using a handling callback

Instead of trycatching everything everywhere or even managing each Api Reponse locally, you may want to provide a handling callback, thanks to `WithExCatching` builder option, available at both register and request time.

You can set it thanks to this option:

```csharp
// direct configuration
options => options.WithExCatching(OnException)
```

#### While registering

Configuring an exception handling callback at register time allows you to get some Global Exception Handling concepts right in place.

`WithExCatching` builder option is available with or without using registry.
It means that you can share your handling callback globally by setting it at registry level and/or set some specific one at api level.

### [Extended](#tab/tabid-extended)

Here is a quite simple scenario:
```csharp
services.AddApizrManagerFor<IReqResUserService>(options => options
                    .WithExCatching(OnException));

private bool OnException(ApizrException ex)
{
    // this is a global exception handling callback 
    if (ex.InnerException is IOException)
	{
		// handle no network exception globally for example
        Alert.Show("No network", "Please check your connection and try again");

        // Tell other exception handling callbacks that we handled it yet
		return true;
	}

    return false;
}
```

And here is a more complexe scenario:
```csharp
services.AddApizr(
    registry => registry
        .AddManagerFor<IHttpBinService>()
        .AddCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>(),
        .AddGroup(
            group => group
                .AddManagerFor<IReqResResourceService>()
                .AddManagerFor<IReqResUserService>(
                    // IReqResUserService dedicated exception handling callback
                    options => options.WithExCatching(OnReqResUserException, strategy: ApizrDuplicateStrategy.Add)),

            // Group exception handling callback common to IReqResUserService & IReqResResourceService apis
            options => options.WithExCatching(OnGroupException, strategy: ApizrDuplicateStrategy.Add))

    // Global exception handling callback common to all apis
    options => options.WithExCatching(OnGlobalException, strategy: ApizrDuplicateStrategy.Add));

private bool OnGlobalException(ApizrException ex)
{
    // this is a global exception handling callback 
    // called back in case of exception thrown 
    // while requesting with any managed api from the registry
    if (ex.InnerException is IOException)
	{
		// handle no network exception globally for example
        Alert.Show("No network", "Please check your connection and try again");

        // Tell other exception handling callbacks that we handled it yet
		return true;
	}

    return false;
}

private bool OnGroupException(IServiceProvider serviceProvider, ApizrException ex)
{
    // this is a group exception handling callback 
    // called back in case of exception thrown 
    // while requesting with any managed api from the group
    if(!ex.Handled) // Not yet handled ?
    {
		// handle it here at group level, like logging things
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred");

        return true;
	}

    return false;
}

private async Task<bool> OnReqResUserException(ApizrException ex)
{
    // this is a dedicated exception handling callback 
    // called back in case of exception thrown 
    // while requesting with IReqResUserService managed api
    if(!ex.Handled) // Not yet handled ?
    {
		// handle it here at api level
        await whateverService.DoSomethingAsync();
        ...
        return true;
	}

    return false;
}
```

### [Static](#tab/tabid-static)

Here is a quite simple scenario:
```csharp
var reqResUserManager = ApizrBuilder.Current.CreateManagerFor<IReqResUserService>(options => options
                    .WithExCatching(OnException));

private bool OnException(ApizrException ex)
{
    // this is a global exception handling callback 
    if (ex.InnerException is IOException)
	{
		// handle no network exception globally for example
        Alert.Show("No network", "Please check your connection and try again");

        // Tell other exception handling callbacks that we handled it yet
		return true;
	}

    return false;
}
```

And here is a more complexe scenario:
```csharp
var apizrRegistry = ApizrBuilder.Current.CreateRegistry(
    registry => registry
        .AddManagerFor<IHttpBinService>()
        .AddCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>(),
        .AddGroup(
            group => group
                .AddManagerFor<IReqResResourceService>()
                .AddManagerFor<IReqResUserService>(
                    // IReqResUserService dedicated exception handling callback
                    options => options.WithExCatching(OnReqResUserException, strategy: ApizrDuplicateStrategy.Add)),

            // Group exception handling callback common to IReqResUserService & IReqResResourceService apis
            options => options.WithExCatching(OnGroupException, strategy: ApizrDuplicateStrategy.Add))

    // Global exception handling callback common to all apis
    options => options.WithExCatching(OnGlobalException, strategy: ApizrDuplicateStrategy.Add));

private bool OnGlobalException(ApizrException ex)
{
    // this is a global exception handling callback 
    // called back in case of exception thrown 
    // while requesting with any managed api from the registry
    if (ex.InnerException is IOException)
	{
		// handle no network exception globally for example
        Alert.Show("No network", "Please check your connection and try again");

        // Tell other exception handling callbacks that we handled it yet
		return true;
	}

    return false;
}

private bool OnGroupException(ApizrException ex)
{
    // this is a group exception handling callback 
    // called back in case of exception thrown 
    // while requesting with any managed api from the group
    if(!ex.Handled) // Not yet handled ?
    {
		// handle it here at group level
        ...
        return true;
	}

    return false;
}

private async Task<bool> OnReqResUserException(ApizrException ex)
{
    // this is a dedicated exception handling callback 
    // called back in case of exception thrown 
    // while requesting with IReqResUserService managed api
    if(!ex.Handled) // Not yet handled ?
    {
		// handle it here at api level
        await whateverService.DoSomethingAsync();
        ...
        return true;
	}

    return false;
}
```

***

Here, as I registered callbacks with `Add` strategy, I'm telling Apizr to:
- Call back `OnGlobalException` then `OnGroupException` then `OnReqResUserException` in case of any exception thrown while requesting with ```IReqResUserService``` api
- Call back ```OnGlobalException``` then ```OnGroupException``` in case of any exception thrown while requesting with ```IReqResResourceService``` api
- Call back only ```OnGlobalException``` in case of any exception thrown while requesting with ```IHttpBinService``` api or ```User``` CRUD api

Feel free to configure your exception handling callbacks at the level of your choice, depending on your needs.
You definitly can mix it all with request option exception handling.

As I leaved the `letThrowOnHandledException` parameter to its default `true` value, Apizr will throw back the exception in the end to let you catch it for final specific handling.
But you definitly can tell Apizr to not throw the final exception if yet handled, by setting `letThrowOnHandledException` parameter to `false` and then dealing with result default value.

Note that you can mix it with other handling solutions.

#### While requesting

Configuring an exception handling callback at request time allows you to set it at the very end, just before sending the request, like trycatching does.

```csharp
public ObservableCollection<User> Users { get; set; }
...
var reqResManager = apizrRegistry.GetManagerFor<IReqResUserService>();
...
try
{
	var users = await reqResManager.ExecuteAsync((options, api) => api.GetUsersAsync(options), 
		options => options.WithExCatching<ApiResult<User>>(OnGetUsersException, strategy: ApizrDuplicateStrategy.Add));

    Users = new ObservableCollection<User>(users);
}
catch (ApizrException<ApiResult<User>> ex)
{
	// handle it here at request level
	if(!ex.Handled) // Not yet handled ?
    {
		// handle it here at result level
        ...
	}
}
...
private async Task<bool> OnGetUsersException(ApizrException<ApiResult<User>> ex)
{
    // this is a method dedicated exception handling callback 
    // called back in case of exception thrown 
    // while requesting with a specific managed api's request
    if(!ex.Handled) // Not yet handled ?
    {
		// handle it here at request level
        await NavigationService.ShowAlertAsync("Error", ex.InnerException.Message ?? "An error occurred");

        return true;
	}

    return false;
}
```

Here, as I set the callback with `Add` strategy, I'm telling Apizr to:
- Call back any other registered exception handling callbacks (see Registering tab)
- Then call back ```OnGetUsersException``` (e.g. to display a dedicated message or something)
- Then throw back the final exception to catch it for specific handling

As I leaved the `letThrowOnHandledException` parameter to its default `true` value, Apizr will throw back the exception in the end to let you catch it for final specific handling.
But you definitly can tell Apizr to not throw the final exception if yet handled, by setting `letThrowOnHandledException` parameter to `false` and then dealing with result default value.

Note that you can mix it with other handling solutions.

You may notice that:
- ```strategy``` parameter let you adjust the behavior in case of mixing (default: ```Replace```):
  - ```Ignore```: if there's another callback yet configured, ignore this one
  - ```Add```: add/queue this callback, no matter of yet configured ones
  - ```Replace```: replace all yet configured callbacks by this one
  - ```Merge```: add/queue this callback, no matter of yet configured ones
- ```letThrowOnHandledException``` parameter tells Apizr to throw back the final exception even if it's been handled by callbacks (default: `true`)

### By registering an exception handler

You may want to provide an exception handler class, thanks to `WithExCatching` builder option, available at register time.

You can define it like so:

```csharp
public class MyExHandler : IApizrExceptionHandler
{
    private readonly ILogger<MyExHandler> _logger;
    private readonly ICustomService _customService;

    public MyExHandler(ILogger<MyExHandler> logger, ICustomService customService)
	{
		_logger = logger;
        _customService = customService;
	}
    
    /// <inheritdoc />
    public Task<bool> HandleAsync(ApizrException ex)
    {
        if(!ex.Handled)
        {
            // Maybe log the exception
			_logger.LogError(ex, "An error occurred");

            // Do some other stuff here
            await _customService.DoSomethingAsync();

			return true;
		}

        return false;
    }
}
```

Then you can register it like so:

```csharp
// static configuration
var reqResUserManager = ApizrBuilder.Current.CreateManagerFor<IReqResUserService>(options => options
                    .WithExCatching(new MyExHandler(LOGGER, CUSTOM_SERVICE)));

// OR extended configuration
services.AddApizrManagerFor<IReqResUserService>(options => options
        .WithExCatching<MyExHandler>());

services.AddSingleton<MyExHandler>();
```

You may notice that:
- ```strategy``` parameter let you adjust the behavior in case of mixing (default: ```Replace```):
  - ```Ignore```: if there's another handler yet configured, ignore this one
  - ```Add```: add/queue this handler, no matter of yet configured ones
  - ```Replace```: replace all yet configured handlers by this one
  - ```Merge```: add/queue this handler, no matter of yet configured ones
- ```letThrowOnHandledException``` parameter tells Apizr to throw back the final exception even if it's been handled by callbacks (default: `true`)

Note that you can mix it with other handling solutions.

### By relying on `Optional.Async`

Here is how we could handle exceptions using Optional.Async:

```csharp
var optionalUserList = await _reqResOptionalMediator.SendFor(api => api.GetUsersAsync());

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

Optional is pretty cool when trying to handle nullables and exceptions, but what if we still want to write it shorter to get our request done and managed with as less code as possible.
Even if we use the typed optional mediator or typed crud optional mediator to get things shorter, we still have to deal with the result matching boilerplate.
Fortunately, Apizr provides some dedicated extensions to help getting things as short as we can with exceptions handled.

#### With `OnResultAsync`

`OnResultAsync` ask you to provide one of these parameters:

- `Action<TResult> onResult`: this action will be invoked just before throwing any exception that might have occurred during request execution
    ```csharp
    await _reqResOptionalMediator.SendFor(api => api.GetUsersAsync())
        .OnResultAsync(userList => 
        { 
            users = userList?.Data; 
        });
    ```
- `Func<TResult, ApizrException<TResult>, bool> onResult`: this function will be invoked with the returned result and potential occurred exception
    ```csharp
    await _reqResOptionalMediator.SendFor(api => api.GetUsersAsync())
        .OnResultAsync((userList, exception) => 
        { 
            users = userList?.Data; 
    
            if(exception != null)
                throw exception;

            return true;
        });
    ```
- `Func<TResult, ApizrException<TResult>, Task<bool>> onResult`: this function will be invoked async with the returned result and potential occurred exception
    ```csharp
    var success = await _reqResOptionalMediator.SendFor(api => api.GetUsersAsync())
        .OnResultAsync((userList, exception) => 
        { 
            users = userList?.Data; 
    
            return exception != null;
        });
    ```

All give you a result returned from fetch if succeed, or cache if failed (if configured). The main goal here is to set any binded property with the returned result (fetched or cached), no matter of exceptions. Then the Action will let the exception throw, where the Func will let you decide to throw manually or return a success boolean flag.
Of course, remember to catch throwing exceptions.

#### With `CatchAsync`

`CatchAsync` let you provide these parameters:

- `Action<Exception> onException`: this action will be invoked just before returning the result from cache if fetch failed. Useful to inform the user of the api call failure and that data comes from cache.
- `letThrowOnExceptionWithEmptyCache`: True to let it throw the inner exception in case of empty cache, False to handle it with onException action and return empty cache result (default: False)

This one returns result from fetch or cache (if configured), no matter of potential exception handled on the other side by an action callback

```csharp
var userList = await _reqResOptionalMediator.SendFor(api => api.GetUsersAsync())
    .CatchAsync(AsyncErrorHandler.HandleException, true);
```

Here we ask the api to get users and if it fails:

- There’s some cached data?
    - [AsyncErrorHandler](https://github.com/Fody/AsyncErrorHandler) will handle the exception like to inform the user that call just failed
    - Apizr will return the previous result from cache
- There’s no cached data yet!
    - letThrowOnExceptionWithEmptyCache is True? (which is the case here)
        - Apizr will throw the inner exception that will be catched further by AsyncErrorHander (this is its normal behavior)
    - letThrowOnExceptionWithEmptyCache is False! (default)
        - Apizr will return the empty cache data (null) which has to be handled then

One line of code to get all the thing done safely and shorter than ever!