## Registering a managed instance, the static way :

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
    Apizr.For<IReqResService>(options => options
        .WithPolicyRegistry(registry)
        .WithAkavacheCacheHandler())
);
```

We provided a policy registry and a cache handler here as we asked for it with cache and policy attributes while designing the api interface.
Also, you could use the manager directly instead of registering it.

### Next steps

- [Using the manager](classic_using.md)