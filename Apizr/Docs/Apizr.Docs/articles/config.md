## Configuring

Many options could be set by attribute decoration. It allows you to use assembly scanning auto registration feature.

Much more options could be set by fluent configuration.
All fluent configuration flavors offer a contextualized options builder, depending on what you're asking and where.

The option types:
- Proper: options available at api configuration level only and applied to it exclusively (e.g. BaseAddress obviously)
- Common: options available at global configuration level only and applied to all registered apis (e.g. RefitSettings, PolicyRegistry)

There're also some Shared options available at both api (proper) and global (common) configuration level (e.g. LogLevel)

Here is what using it with a registry, the extended way, could look like:

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
            .AddFor<IReqResService>()
            .AddFor<IHttpBinService>(
                options => options
                    .WithLogging(
                        HttpTracerMode.Everything, 
                        HttpMessageParts.All, 
                        LogLevel.Trace))
            .AddCrudFor<User, int, PagedResult<User>, IDictionary<string, object>>(
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

Note that fluent configuration almost allways wins over the attribute one (if both set) and the same for proper over common.
For example, if you decorated your api interface with a Log attribute, but also set some common logging options fluently plus some proper logging options, 
the proper logging options will be applied to the api manager. But, if you decorated the api method itself instead of the interface with this Log attribute, it will win over all others :)
Actualy, the closer the option is defined from the api method, the more chance it will have to be applied over all others.

Are you still following? Don't worry! Every single option is detailed through this documentation, so let's browse it!