## Configuring logger

Apizr v4+ relies on Microsoft.Extensions.Logging, which relies on any compatible logger of your choice. 
Apizr comes with a quite simple built-in Debug logger by default, but you'd better provide your own obviously.

You can configure logger only by fluent configuration.

### [Static](#tab/tabid-static)

You can set logger configuration thanks to this option:

```csharp
options => options.WithLoggerFactory(LoggerFactory.Create(loggingBuilder =>
{
    loggingBuilder.AddConsole();
    loggingBuilder.AddDebug();
}))
```

`LoggerFactory.Create` method is provided by Microsoft.Extensions.Logging which lets you add any compatible logger.

### [Extended](#tab/tabid-extended)

There's nothing specific to do with Apizr about logger when using the extended approach.

Just don't forget to configure it like you usualy do:

```csharp
loggingBuilder.AddConsole()
```

wherever in your app you get access to `ILoggingBuilder`.

***