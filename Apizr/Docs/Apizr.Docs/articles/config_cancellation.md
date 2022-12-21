## Configuring Cancellation

You may want to provide a CancellationToken, thanks to `WithCancellation` builder option, available at request time.
```csharp
// With static builder here but works the same with the extended one
var reqResManager = ApizrBuilder.CreateManagerFor<IReqResUserService>();

// Create your token source
var cts = new CancellationTokenSource();

// Send the request with your token into options
var users = await reqResManager.ExecuteAsync((options, api) => api.GetUsersAsync(options), options => 
    options.WithCancellation(cts.Token));
```

You may notice that you don't have to pass the token itself directly to the request anymore. Just pass the resulting options insteed and everything will be wired for you.
Don't forget to handle the exception.