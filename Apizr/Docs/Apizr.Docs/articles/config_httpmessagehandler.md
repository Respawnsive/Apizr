﻿## Configuring HttpMessageHandler

You can add an HttpMessageHandler thanks to this option:

### [Extended](#tab/tabid-extended)

```csharp
// direct configuration
options => options.WithHttpMessageHandler(YourHttpMessageHandler)

// Or type configuration (has to be registered in the service collection)
options => options.WithHttpMessageHandler<YourHttpMessageHandler>()

// OR factory configuration with the service provider instance
options => options.WithHttpMessageHandler(serviceProvider => YourHttpMessageHandler)

// OR factory configuration with the service provider and options instances
options => options.WithHttpMessageHandler((serviceProvider, options) => YourHttpMessageHandler)
```

### [Static](#tab/tabid-static)

```csharp
// direct configuration
options => options.WithHttpMessageHandler(YourHttpMessageHandler)

// OR factory configuration with the logger and options instances
options => options.WithHttpMessageHandler(options => YourHttpMessageHandler)
```

***