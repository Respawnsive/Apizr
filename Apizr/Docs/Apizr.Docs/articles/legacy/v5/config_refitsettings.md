## Configuring RefitSettings

You can set RefitSettings thanks to this option:

### [Static](#tab/tabid-static)

```csharp
// direct configuration
options => options.WithRefitSettings(YourOwnRefitSettings)

// OR factory configuration
options => options.WithRefitSettings(() => YourOwnRefitSettings)
```

### [Extended](#tab/tabid-extended)

```csharp
// direct configuration
options => options.WithRefitSettings(YourOwnRefitSettings)

// OR factory configuration
options => options.WithRefitSettings(serviceProvider => YourOwnRefitSettings)
```

***

>[!WARNING]
>
>**AuthorizationHeaderValue**
>
>Apizr provides its own AuthenticationHandler to manage authorization (see Configuring > Authentication). 
>There's no need to use AuthorizationHeaderValue properties.
