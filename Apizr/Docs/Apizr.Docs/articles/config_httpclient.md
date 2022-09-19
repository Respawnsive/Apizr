## Configuring HttpClient

### [Static](#tab/tabid-static)

You can provide your own HttpClient thanks to this option:

```csharp
options => options.WithHttpClient((httpMessageHandler, baseUri) => new YourOwnHttpClient(httpMessageHandler, false){BaseAddress = baseUri});
```

### [Extended](#tab/tabid-extended)

You can configure HttpClient by its Builder thanks to this option:

```csharp
options => options.ConfigureHttpClientBuilder(httpClientBuilder => httpClientBuilder.WhateverOption())
```

***

>[!WARNING]
>
>**HttpClient**
>
>Apizr makes use of HttpClient so keep in mind that you may override some of its features depending of what you're trying to do with it. Use with caution.