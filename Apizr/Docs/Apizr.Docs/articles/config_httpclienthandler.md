## Configuring HttpClientHandler

You can provide your own HttpClientHandler thanks to this option:

### [Extended](#tab/tabid-extended)

```csharp
// direct configuration
options => options.WithHttpClientHandler(YourOwnHttpClientHandler)

// OR factory configuration
options => options.WithHttpClientHandler(serviceProvider => YourOwnHttpClientHandler)
```

### [Static](#tab/tabid-static)

```csharp
// direct configuration
options => options.WithHttpClientHandler(YourOwnHttpClientHandler)

// OR factory configuration
options => options.WithHttpClientHandler(() => YourOwnHttpClientHandler)
```

***