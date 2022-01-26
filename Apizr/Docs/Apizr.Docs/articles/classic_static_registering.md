<h2 id="classic-static-approach">
Static instance registration:
</h2>

Somewhere where you can add services to your container, add the following:
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
myContainer.SomeInstanceRegistrationMethod(
Apizr.CreateFor<IReqResService>(options => options
    .WithPolicyRegistry(registry)
    .WithAkavacheCacheHandler())
);
```

I provided a policy registry and a cache handler here as I asked for it with cache and policy attributes in my web api example.