## Configuring HttpClientBuilder

With the extended approach only, you can configure HttpClientBuilder thanks to this option:

```csharp
options => options.ConfigureHttpClientBuilder(httpClientBuilder => httpClientBuilder.WhateverOption())
```

>[!WARNING]**HttpClientBuilder**
>
>Apizr makes use of HttpClientBuilder so keep in mind that you may override some of its features depending of what you're trying to do with HttpClientBuilder. Use with caution.