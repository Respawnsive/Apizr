<h2 id="classic-automatically">
Automatically registering decorated interfaces by extensions:
</h2>

Register in your Startup class like so:
```csharp
public override void ConfigureServices(IServiceCollection services)
{
    // Apizr registration
    services.AddApizrFor(options => options.WithAkavacheCacheHandler(), typeof(AnyClassFromServicesAssembly));
}
```

There are many AddApizrFor flavors for classic automatic registration, depending on what you want to do and provide.
This one is the simplest.