## Automatically registering decorated interfaces by extensions:

Here is an example:
```csharp
public override void ConfigureServices(IServiceCollection services)
{
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
    services.AddApizrFor(options => options.WithAkavacheCacheHandler(), typeof(AnyClassFromServicesAssembly));
}
```

We provided a policy registry and a cache handler here as we asked for it with cache and policy attributes while designing the api interface.

### Next steps

- [Using the manager](classic_using.md)