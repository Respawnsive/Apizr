## Configuring DelegatingHandlers

You can add DelegatingHandlers thanks to this option:

### [Static](#tab/tabid-static)

```csharp
// direct configuration
options => options.WithDelegatingHandler(YourDelegatingHandler)

// OR factory configuration with the logger instance
options => options.WithDelegatingHandler(logger => YourDelegatingHandler)

// OR factory configuration with the logger and options instances
options => options.WithDelegatingHandler((logger, options) => YourDelegatingHandler)
```

### [Extended](#tab/tabid-extended)

```csharp
// direct configuration
options => options.WithDelegatingHandler(YourDelegatingHandler)

// Or type configuration (has to be registered in the service collection)
options => options.WithDelegatingHandler<YourDelegatingHandler>()

// OR factory configuration with the service provider instance
options => options.WithDelegatingHandler(serviceProvider => YourDelegatingHandler)

// OR factory configuration with the service provider and options instances
options => options.WithDelegatingHandler((serviceProvider, options) => YourDelegatingHandler)
```

***

You may want to adjust duplicate strategy while registering a DelegatingHandler. You can do it by providing your own strategy thanks to the optional parameter (default: Add).


>[!WARNING]
>
>**Inner DelegatingHandler**
>
>Do not try to manage delegating handlers hierarchy by yourself, providing any inner handlers. Instead, just add your handlers thanks to the WithDelegatingHandler option the order you want and Apizr will do it for you.