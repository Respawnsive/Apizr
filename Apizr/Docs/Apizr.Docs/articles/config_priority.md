## Configuring Priority

Apizr could use [Fusillade](https://github.com/reactiveui/Fusillade) to offer some api priority management on calls.

To be short, Fusillade is about:

- Auto-deduplication of relevant requests
- Request Limiting
- Request Prioritization
- Speculative requests

Please refer to its [official documentation](https://github.com/reactiveui/Fusillade) if you’d like to know more about it.

### Installing

Please first install this integration package:

|Project|Current|V-Next|
|-------|-----|-----|
|Apizr.Integrations.Fusillade|[![NuGet](https://img.shields.io/nuget/v/Apizr.Integrations.Fusillade.svg)](https://www.nuget.org/packages/Apizr.Integrations.Fusillade/)|[![NuGet Pre Release](https://img.shields.io/nuget/vpre/Apizr.Integrations.Fusillade.svg)](https://www.nuget.org/packages/Apizr.Integrations.Fusillade/)|

You can configure priority at:
- Design time by attribute decoration
- Register time by fluent configuration
- Request time by fluent configuration

### [Designing](#tab/tabid-designing)

The first thing to do while designing your api interfaces using Apizr to send a request, is to add an `IApizrRequestOptions` param decorated with the provided `RequestOptions` attribute to your methods like:

```csharp
[WebApi("https://reqres.in/api")]
public interface IReqResService
{
    [Get("/users")]
    Task<UserList> GetUsersAsync([RequestOptions] IApizrRequestOptions options);

    [Get("/users/{userId}")]
    Task<UserDetails> GetUserAsync(int userId, [RequestOptions] IApizrRequestOptions options);
}
```

This way you'll make sure to pass your priority to the priority handler, defined thanks to request options builder at request time.

Another way to deal with priority at design time is to use the `PriorityAttribute`:
```csharp
[assembly:Priority(Priority.UserInitiated)]
namespace Your.Namespace
{
    [WebApi("https://reqres.in/api"), Priority(Priority.Background)]
    public interface IReqResService
    {
        [Get("/users"), Priority(Priority.Speculative)]
        Task<UserList> GetUsersAsync([RequestOptions] IApizrRequestOptions options);

        [Get("/users/{userId}")]
        Task<UserDetails> GetUserAsync(int userId, [RequestOptions] IApizrRequestOptions options);
    }
}
```
Here I'm saying:
- Send all requests of all apis with a default `UserInitiated` priority (the assembly one)
- Excepted for all the requests of the `IReqResService` with a `Background` priority instead (the interface one)
- Excepted for any GetUsersAsync request with a `Speculative` priority instead (the method one)

Of course, you could (should) mix it with the `RequestOptions` method parameter implementation, so you could change your mind at request time with the request options builder.

### [Registering](#tab/tabid-registering)

Designing your apis using PriorityAttribute or not, you still have to activate priority management at register time.
By activating it, you're free to provide a priority or not.

Here is how to activate it, thanks to the `WithPriority` extension method:
```csharp
// activation configuration only
options => options.WithPriority()

// activation with default priority configuration
options => options.WithPriority(Priority.Background)

// activation with default custom priority configuration
options => options.WithPriority(70)
```

All priority fluent options are available with and without using registry. 
It means that you can share priority configuration, setting it at registry level and/or set some specific one at api level, something like:
```csharp
var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
        .AddGroup(group => group
                .AddManagerFor<IReqResUserService>(options => options
                    .WithPriority(Priority.UserInitiated))
                .AddManagerFor<IReqResResourceService>(),
            options => options.WithPriority(Priority.Background))
        .AddManagerFor<IHttpBinService>()
        .AddCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>(options => options
            .WithBaseAddress("https://reqres.in/api/users")
            .WithPriority(Priority.Speculative)),
    options => options.WithPriority());
```

In this quite complexe example, we can see we defined some default priorities to apply at deferent levels.

### [Requesting](#tab/tabid-requesting)

Just call your api with your priority thanks to the request options builder (extension method):

```csharp
var result = await _reqResManager.ExecuteAsync((opt, api) => api.GetUsersAsync(opt), 
    options => options.WithPriority(Priority.Background));
```

***

Note that you can mix design, register and request time priority configurations.
In case of mixed configurations, the internal duplicate strategy will be to take the closest one to the request.

Priority configuration duplicate strategy order:
- take fluent request configuration first if defined (request options)
- otherwise the request attribute decoration one (method)
- otherwise the fluent proper resgistration one (api proper options)
- otherwise the api attribute decoration one (interface)
- otherwise the fluent common resgistration one (registry common options)
- otherwise the global attribute decoration one (assembly)