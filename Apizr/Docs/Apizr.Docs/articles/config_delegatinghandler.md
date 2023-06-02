## Configuring DelegatingHandlers

You can add DelegatingHandlers thanks to this option:

### [Static](#tab/tabid-static)

```csharp
// direct configuration
options => options.AddDelegatingHandler(YourDelegatingHandler)

// OR factory configuration with the logger instance
options => options.AddDelegatingHandler(logger => YourDelegatingHandler)

// OR factory configuration with the logger and options instances
options => options.AddDelegatingHandler((logger, options) => YourDelegatingHandler)
```

### [Extended](#tab/tabid-extended)

```csharp
// direct configuration
options => options.AddDelegatingHandler(YourDelegatingHandler)

// OR factory configuration with the service provider instance
options => options.AddDelegatingHandler(serviceProvider => YourDelegatingHandler)

// OR factory configuration with the service provider and options instances
options => options.AddDelegatingHandler((serviceProvider, options) => YourDelegatingHandler)
```

***

>[!WARNING]
>
>**Inner DelegatingHandler**
>
>Do not try to manage delegating handlers hierarchy by yourself, providing any inner handlers. Instead, just add your handlers thanks to the AddDelegatingHandler option the order you want and Apizr will do it for you.