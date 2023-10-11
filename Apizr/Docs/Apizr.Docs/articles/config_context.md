## Configuring Context

You may want to provide a Polly Context, thanks to `WithContext` builder option, available at both register and request time.

### [Registering](#tab/tabid-registering)

Configuring a context at register time allows you to get a pre-defined one while requesting.

`WithContext` builder option is available with or without using registry.
It means that you can share a context globally by setting it at registry level and/or set some specific one at api level.
As it's not recomended to share the same context instance between requests, `WithContext` registration option comes with a factory registration only.

Here is a quite simple scenario:
```csharp
var reqResUserManager = ApizrBuilder.Current.CreateManagerFor<IReqResUserService>(options => options
                    .WithContext(() => new Context { { "testKey1", "testValue1" } }));
```

And here is a pretty complexe scenario:
```csharp
private Context FirstContextFactory() => new() { { "testKey1", "testValue1" } };
private Context SecondContextFactory() => new() { { "testKey2", "testValue2" } };
private Context ThirdContextFactory() => new() { { "testKey3", "testValue3" } };

var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
        .AddGroup(group => group
                .AddManagerFor<IReqResUserService>(options => options
                    .WithContext(ThirdContextFactory))
                .AddManagerFor<IReqResResourceService>(),
            options => options.WithContext(SecondContextFactory))
        .AddManagerFor<IHttpBinService>()
        .AddCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>(),
    options => options.WithContext(FirstContextFactory));
```

Here I'm telling Apizr to:
- Merge all 3 context together and pass it while requesting with ```IReqResUserService``` api
- Merge first and second context and pass it while requesting with ```IReqResResourceService``` api
- Pass the first context while requesting with ```IHttpBinService``` api or ```User``` CRUD api

Feel free to configure your context at the level of your choice, depending on your needs.
You definitly can mix it all with request option context providing. 
Keep in mind that the closest key/value to the request will be the one used by Apizr.

### [Requesting](#tab/tabid-requesting)

Configuring a context at request time allows you to set it at the very end, just before sending the request.

```csharp
var reqResManager = apizrRegistry.GetManagerFor<IReqResUserService>();

var users = await reqResManager.ExecuteAsync(api => api.GetUsersAsync(), options => 
    options.WithContext(() => new Context { { testKey4, testValue4 } }));
```

You definitly can mix it with registration option context.

***

You may notice that:
- ```strategy``` parameter let you adjust the behavior in case of mixing (default: ```Merge```):
  - ```Ignore```: if there's another context yet configured, ignore this one
  - ```Add```: add or merge this context with any yet configured ones
  - ```Replace```: replace all yet configured context by this one
  - ```Merge```: add or merge this context with any yet configured ones