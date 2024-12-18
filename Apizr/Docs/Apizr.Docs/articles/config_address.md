## Watching

Please find this dedicated tutorial video on YouTube about how to adjust basic options with Apizr:

> [!Video https://www.youtube.com/embed/Kbk0f_aV84k]

## Configuring base address

You can configure base address and base path either by attribute decoration or by fluent configuration.
Fluent configuration allows you to load options automatically from settings (see [Settings](config_settings.md)), or set options manually.
You can mix the configuration providing a base path by attribute and a base address/URI fluently.

### [Attribute](#tab/tabid-attribute)

### `BaseAddress` attribute

You can set api interface or CRUD entity base address or path thanks to the `BaseAddress` attribute. It let you set the base address at design time and then register your api fluently without having to set it.
You can do it like so:

```csharp
[BaseAddress("YOUR_API_INTERFACE_BASE_ADDRESS_OR_PATH")]
public interface IYourApiInterface
{
    // Your api interface methods
}

// OR the same for CRUD api
[BaseAddress("YOUR_CRUD_ENTITY_API_BASE_ADDRESS_OR_PATH")]
public record YourCrudEntity
{
    // Your CRUD entity properties
}
```

Note that if you provided only a path, you still have to set the base address/URI fluently at registration time so that Apizr could merge it all together.

### `AutoRegister` attribute

You can set api interface or CRUD entity base address or path thanks to the `AutoRegister` attribute. It let you set the base address at design time and then register apis by assembly scanning at register time.
You can do it like so:

```csharp
[AutoRegister("YOUR_API_INTERFACE_BASE_ADDRESS_OR_PATH")]
public interface IYourApiInterface
{
    // Your api interface methods
}

// OR the same for CRUD api
[AutoRegister("YOUR_CRUD_ENTITY_API_BASE_ADDRESS_OR_PATH")]
public record YourCrudEntity
{
    // Your CRUD entity properties
}
```

Note that if you provided only a path, you still have to set the base address/URI fluently at registration time so that Apizr could merge it all together.

`AutoRegister` attribute comes with more options so you should read more about it from the Getting Started doc articles.

### [Fluent](#tab/tabid-fluent)

#### Automatically

Base address and base path could both be set automatically by providing an `IConfiguration` instance to Apizr like so:
```csharp
options => options.WithConfiguration(context.Configuration)
```

We can set it at common level (shared by all apis) or specific level (dedicated to a named one).

Please heads to the [Settings](config_settings.md))  doc article to see how to configure the base address or base path automatically from settings.

#### Manually

Configuring the base address or base path fluently with manual option allows you to set it dynamically.

You can set the base address or a base path thanks to these options:

```csharp
// Address
options => options.WithBaseAddress(YourSettings.YourBaseAddress))

// Path
options => options.WithBasePath(YourSettings.YourBasePath))
```

The extended world offers another option with a factory providing a service provider instance:

```csharp
// Address
options => options.WithBaseAddress(serviceProvider => serviceProvider.GetRequiredService<IYourSettingsService>().YourBaseAddress))

// Path
options => options.WithBasePath(serviceProvider => serviceProvider.GetRequiredService<IYourSettingsService>().YourBasePath))
```

In both cases, you can mix the configurations like providing a base path by attribute and a base address/URI fluently.

```WithBaseAddress``` and ```WithBasePath``` options are available at both common and specific configuration level, meanning that you can share a base address with several api interfaces and/or set a specific one for some others.

***