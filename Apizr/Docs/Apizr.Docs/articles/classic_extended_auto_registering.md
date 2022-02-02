## Registering decorated interfaces automatically, the extended way

Here is an example:
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
    services.AddApizrFor(options => options.WithAkavacheCacheHandler(), ASSEMBLIES_CONTAINING_INTERFACES);
}
```

Apizr will scan assemblies to auto register managers for decorated api interfaces.

We registered a policy registry and provided a cache handler here as we asked for it with cache and policy attributes while designing the api interface.

## Next steps

- [Using the manager](classic_using.md)