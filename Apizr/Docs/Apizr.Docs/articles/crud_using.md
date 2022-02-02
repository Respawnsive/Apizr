## Using the manager:

Here is an example of how to send a web request from an app - e.g. using Apizr in a Xamarin.Forms mobile app.

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
