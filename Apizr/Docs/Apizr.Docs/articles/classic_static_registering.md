## Registering a managed instance, the static way

Here is an example of how to register a managed instance of an api interface:
```csharp
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

// Apizr registration
myContainer.RegistrationMethodFactory(() => 
    Apizr.CreateFor<IReqResService>(options => options
        .WithPolicyRegistry(registry)
        .WithAkavacheCacheHandler())
);
```

We provided a policy registry and a cache handler here as we asked for it with cache and policy attributes while designing the api interface.
Also, you could use the manager directly instead of registering it.

## Registering multiple managed instances, the static way

You may want to register multiple managed api interfaces within the same project.
Also, you may want to share some common configuration between apis without repeating yourself, but at the same time, you may need to set some specific ones for some of it.
This is where the ApizrRegistry comes on stage.

Here is an example of how to register a managed instance of multiple api interfaces:
```csharp
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

// Apizr registry
var apizrRegistry = Apizr.Create(
    registry => registry
        .AddFor<IReqResService>()
        .AddFor<IHttpBinService>(
            options => options
                .WithLogging(
                    HttpTracerMode.Everything, 
                    HttpMessageParts.All, 
                    LogLevel.Trace)),
    
    config => config
        .WithPolicyRegistry(registry)
        .WithAkavacheCacheHandler()
        .WithLogging(
            HttpTracerMode.ExceptionsOnly, 
            HttpMessageParts.ResponseAll, 
            LogLevel.Error)
);

// Container registration
apizrRegistry.Populate((type, factory) => 
    myContainer.RegistrationMethodFactory(type, factory)
);
```

Here is what we're saying in this example:
- Add a manager for IReqResService api interface into the registry
- Add a manager for IHttpBinService api interface into the registry
  - Set some specific logging settings dedicated to IHttpBinService's manager
- Apply common configuration to all managers by:
  - Providing a policy registry
  - Providing a cache handler
  - Providing some logging settings (won't apply to IHttpBinService's manager as we set some specific ones)

It's an example, meaning if you don't need common and/or specific configuration, just don't provide it.
And yes you can mix classic and CRUD manager registration into the same registry.

Also, you could register the registry itslef, instead of its populated managers and then use its managers directly.

Or, you could use the managers directly from the registry instead of registering anything.

Here's how to get a manager from the registry:

```csharp
var reqResManager = apizrRegistry.GetFor<IReqResService>();

var httpBinManager = apizrRegistry.GetFor<IHttpBinService>();
```

## Next steps

- [Using the manager](classic_using.md)