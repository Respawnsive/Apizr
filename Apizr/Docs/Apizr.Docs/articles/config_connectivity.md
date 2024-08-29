## Configuring Connectivity

Apizr can check network connectivity for you, right before sending any request.

It will throw an ApizrException with an IOException as InnerException in case of network failure, which you can handle globally by showing a snack bar info or whatever.

This way, your viewmodels are kept light and clear of it.

### With boolean factory

You may want to provide just a simple boolean value to check connectivity.

Here is the right option:

### [Extended](#tab/tabid-extended)

```csharp
// Boolean factory
options => options.WithConnectivityHandler(serviceProvider => YourConnectivityBoolean)

// Boolean expression factory
options => options.WithConnectivityHandler<IYourRegisteredConnectivityService>(service => service.YourConnectivityBoolean)
```

### [Static](#tab/tabid-static)

```csharp
options => options.WithConnectivityHandler(() => YourConnectivityBoolean)
```

***

### With Connectivity Handler

You could also implement the IConnectivityHandler interface:

```csharp
public class YourConnectivityHandler : IConnectivityHandler
{
    public bool IsConnected()
    {
        // Check connectivity here
    }
}
```

Then just register it with this option:

### [Extended](#tab/tabid-extended)

```csharp
// direct configuration
options => options.WithConnectivityHandler(YourConnectivityHandler)

// OR factory configuration
options => options.WithConnectivityHandler(serviceProvider => YourConnectivityHandler)

// OR closed generic configuration
options => options.WithConnectivityHandler<YourConnectivityHandler>()

// OR type configuration
options => options.WithConnectivityHandler(typeof(YourConnectivityHandler))
```

### [Static](#tab/tabid-static)

```csharp
// direct configuration
options => options.WithConnectivityHandler(YourConnectivityHandler)

// OR factory configuration
options => options.WithConnectivityHandler(() => YourConnectivityHandler)
```

***