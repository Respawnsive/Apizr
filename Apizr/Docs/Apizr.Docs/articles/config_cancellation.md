## Configuring Cancellation

You may want to provide a CancellationToken, thanks to `WithCancellation` builder option, available at request time.
```csharp
try
{
	// With static builder here but works the same with the extended one
	var reqResManager = ApizrBuilder.Current.CreateManagerFor<IReqResUserService>();

	// Create your token source
	var cts = new CancellationTokenSource();

	// Send the request with your token into options
	var users = await reqResManager.ExecuteAsync((opt, api) => api.GetUsersAsync(opt), options => 
		options.WithCancellation(cts.Token));
	
	// Do whatever with users here...
}
// Catching ApizrException with data caching
catch (ApizrException<ApiResult<User>> ex) when (ex.InnerException is OperationCanceledException cancelEx)
{
	// Handle canceled exception here with cached data
}
// OR catching ApizrException without data caching
catch (ApizrException ex) when (ex.InnerException is OperationCanceledException cancelEx)
{
	// Handle canceled exception here
}
// AND catching other exceptions
catch (Exception ex)
{
	// Handle other exceptions here
}
```

You may notice that you don't have to pass the token itself directly to the request anymore. Just pass the resulting options instead and everything will be wired for you.
Don't forget to handle the right exception, depending if you enabled data caching feature or not.