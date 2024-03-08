## Configuring HttpClient

You can configure HttpClient by its Builder thanks to this option:

### [Static](#tab/tabid-static)

```csharp
options => options.ConfigureHttpClient(httpClient => httpClient.WhateverOption = whateverValue)
```

### [Extended](#tab/tabid-extended)

```csharp
options => options.ConfigureHttpClientBuilder(httpClientBuilder => httpClientBuilder.WhateverOption())
```

***

>[!WARNING]
>
>**HttpClient**
>
>Apizr makes use of its own HttpClient with its own primary handler, so keep in mind that you may override some of its features depending of what you're trying to do with it. Use with caution.