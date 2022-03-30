## Configuring Exception handling

### Using `Try/Catch`

Here is an example of how to send a web request from an app - e.g. using Apizr in a Xamarin.Forms mobile app.

Inject ```IApizrManager<IYourDefinedInterface>``` where you need it - e.g. into your ViewModel constructor
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
    UserDialogs.Instance.Toast(new ToastConfig(message) { BackgroundColor = Color.Red, MessageTextColor = Color.White });

    users = e.CachedResult?.Data;
}

if(users != null)
    Users = new ObservableCollection<User>(users);
```

We catch any ApizrException as it will contain the original inner exception, but also the previously cached result if some.
If you provided an IConnectivityHandler implementation and there's no network connectivity before sending request, Apizr will throw an IO inner exception without sending the request.

### Using `Action<Exception> onException`

Instead of trycatching all the things, you may want to provide an exception handling action on call, thanks to `Action<Exception> onException` optional parameter.

Something like:
```csharp
reqResManager.ExecuteAsync(api => api.GetUsersAsync(), clearCache: false, onException: OnGetUsersException);

...

private void OnGetUsersException(Exception ex)
{
    ...
}
```

### Using `Optional.Async`

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
    - letThrowOnExceptionWithEmptyCache is True? (witch is the case here)
        - Apizr will throw the inner exception that will be catched further by AsyncErrorHander (this is its normal behavior)
    - letThrowOnExceptionWithEmptyCache is False! (default)
        - Apizr will return the empty cache data (null) which has to be handled then

One line of code to get all the thing done safely and shorter than ever!