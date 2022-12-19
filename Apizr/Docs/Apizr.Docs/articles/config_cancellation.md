## Configuring Cancellation

You may want to provide a CancellationToken, thanks to `WithCancellation` builder option, available at request time.

```csharp
var reqResManager = apizrRegistry.GetManagerFor<IReqResUserService>();
var token = new CancellationToken();
var users = await reqResManager.ExecuteAsync(api => api.GetUsersAsync(), options => 
    options.WithCancellation());
```

You may notice that:
- ```strategy``` parameter let you adjust the behavior in case of mixing (default: ```Merge```):
  - ```Ignore```: if there's another context yet configured, ignore this one
  - ```Add```: add or merge this context with any yet configured ones
  - ```Replace```: replace all yet configured context by this one
  - ```Merge```: add or merge this context with any yet configured ones