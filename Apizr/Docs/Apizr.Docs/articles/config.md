## Configuring

Many options could be set by attribute decoration and much more by fluent contextualized configuration, depending on what you're asking and where.

You can configure the way your api request will be managed by Apizr at 3 different stages:
- Design time, by attribute decoration, when you first create your api interface
- Register time, by fluent options, when you actually register your api interfaces
- Request time, by fluent options, when you finally send the request to the api

At Design time, everything is set by attribute like we used to with Refit, decorating at different levels like assembly, interface or method.

At Register time, you'll get the possibility to share some options or not with several api interfaces registrations or not.

At Request time, you'll get your last chance to adjust configuration before the request to be sent.

As you can mix stages and levels while configuring, here is the configuration pipeline:

- **1 (Design):** The **assembly attribute configuration** level set a configuration to **all api interfaces** contained into the assembly.
- **2 (Register):** The **fluent common configuration** option takes over the previous one and set a configuration to **all registered api interfaces**.
- **3 (Design):** The **interface attribute configuration** level takes over all the previous ones and set a configuration to **a specific api interface**.
- **4 (Register):** The **fluent proper or manager configuration** option takes over all the previous ones and set a configuration to **the registered api interface**.
- **5 (Design):** The **method attribute configuration** level takes over all the previous ones and set a configuration to **a specific api interface method**.
- **6 (Request):** The **fluent request configuration** option takes over all the previous ones and set a configuration to **the called api interface method**.


Let's take a quite complexe and dummy but exhaustive timeout configuration example to illustrate that pipeline.

First, the design:
```csharp
[assembly:Timeout("00:02:00")]
namespace Apizr.Sample
{
    [WebApi("https://reqres.in/api")]
    [Timeout("00:01:30")]
    public interface IReqResService
    {
        [Get("/users")]
        [Timeout("00:01:00")]
        Task<UserList> GetUsersAsync([RequestOptions] IApizrRequestOptions options);
    }
}
```

Then, the registration:

```csharp
public override void ConfigureServices(IServiceCollection services)
{
    // Apizr registration
    services.AddApizr(
        registry => registry
            .AddManagerFor<IReqResService>(properOptions => 
                properOptions.WithTimeout(new TimeSpan(0,1,15)))
            .AddManagerFor<IHttpBinService>()),
    
        commonOptions => commonOptions
            .WithTimeout(new TimeSpan(0,1,45))
    );
}
```

Finally, the request:
```csharp
// reqResManager here is a resolved instance of IApizrManager<IReqResService>>
var users = await reqResManager.ExecuteAsync((opt, api) => api.GetUsersAsync(opt), options => 
	options.WithTimeout(new TimeSpan(0,0,45)));
```

Now, guess when the request will time out?

Here is how Apizr will take its decision about that:
- It first detects we set a global timeout of 00:02:00 (assembly attribute decoration)
- Then it detects we registered another global timeout of 00:01:45 (fluent common options)
- Then it detects we set an api timeout of 00:01:30 (interface attribute decoration)
- Then it detects we registered another api timeout of 00:01:15 (fluent proper options)
- Then it detects we set a request timeout of 00:01:00 (method attribute decoration)
- Then it detects we registered another request timeout of 00:00:45 (fluent request options)

And the winner is allways the closest one to the request call, so here 00:00:45.

Now you get the picture about the configuration pipeline, let's take a more meanful example.

Here is what configuring with a registry, the extended way, could look like:

```csharp
public override void ConfigureServices(IServiceCollection services)
{
    // Some policies
    var registry = new PolicyRegistry
    {
        {
            "TransientHttpError", HttpPolicyExtensions.HandleTransientHttpError().WaitAndRetryAsync(new[]
            {
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(10)
            })
        }
    };
    services.AddPolicyRegistry(registry);

    // Apizr registration
    services.AddApizr(
        registry => registry
            .AddManagerFor<IReqResService>()
            .AddManagerFor<IHttpBinService>(
                options => options
                    .WithLogging(
                        HttpTracerMode.Everything, 
                        HttpMessageParts.All, 
                        LogLevel.Trace))
            .AddCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>(
                options => options
                    .WithBaseAddress("https://reqres.in/api/users"))),
    
        config => config
            .WithPolicyRegistry(registry)
            .WithAkavacheCacheHandler()
            .WithLogging(
                HttpTracerMode.ExceptionsOnly, 
                HttpMessageParts.ResponseAll, 
                LogLevel.Error)
    );
}
```

And here is what we're saying in this example:
- Add a manager for IReqResService api interface into the registry, to register it into the container
- Add a manager for IHttpBinService api interface into the registry, to register it into the container
  - Apply proper logging options dedicated to IHttpBinService's manager
- Add a manager for User entity with CRUD api interface and custom types into the registry, to register it into the container
  - Apply proper address option dedicated to User's manager
- Apply common options to all managers by:
  - Providing a policy registry
  - Providing a cache handler
  - Providing some logging settings (won't apply to IHttpBinService's manager as we set some specific ones)

Are you still following? Don't worry! Every single option is detailed through this documentation, so let's browse it!