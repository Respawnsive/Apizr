## Configuring base address

You can configure base address either by attribute decoration or by fluent configuration.
You can mix the configuration providing a base path by attribute and a base address/URI fluently.

### [Attribute](#tab/tabid-attribute)

Configuring the base address by attribute allows you to use assembly scanning auto registration feature.

### Classic api

You can set api interface base address or path thanks to the WebApi attribute like so:

```csharp
[WebApi("YOUR_API_INTERFACE_BASE_ADDRESS_OR_PATH/")]
public interface IYourApiInterface
{
    // Your api interface methods
}
```

If you provided only a path, you must set the base address/URI fluently so that Apizr could merge it all together.

Optional parameters:
- ```isAutoRegistrable``` (default: true) tells Apizr to include or not this specific interface while scanning assemblies for auto registration. 
It could be usefull when you ask for auto registration but want to do it fluently some specific interfaces.

### CRUD api

You can set CRUD entity api base address thanks to the CrudEntity attribute like so:

```csharp
[CrudEntity("YOUR_CRUD_ENTITY_API_BASE_ADDRESS_OR_PATH_")]
public class YourCrudEntity
{
    // Your CRUD entity properties
}
```

If you provided only a path, you must set the base address/URI fluently so that Apizr could merge it all together.

Optional parameters:
- ```keyType```: entity key type (default: null = typeof(int))
- ```readAllResultType```: ReadAll query result type  (default: null = typeof(IEnumerable{}))
- ```readAllParamsType```: ReadAll query parameters type  (default: null = typeof(IDictionary{string, object}))
- ```modelEntityType```: Model entity type mapped with this api entity type (default: null = decorated api entity type)


### [Fluent](#tab/tabid-fluent)

Configuring the base address fluently allows you to set it dynamically (e.g. based on settings)

You can set the base address or a base path thanks to these options:

```csharp
// Address
options => options.WithBaseAddress(YourSettings.YourBaseAddress))

// Path
options => options.WithBasePath(YourSettings.YourBasePath))
```

The extended world offers another option with a factory providing a service provider instance.
It could help you to resolve the setting at runtime:

```csharp
// Address
options => options.WithBaseAddress(serviceProvider => serviceProvider.GetRequiredService<IYourSettingsService>().YourBaseAddress))

// Path
options => options.WithBasePath(serviceProvider => serviceProvider.GetRequiredService<IYourSettingsService>().YourBasePath))
```

In both cases, you can mix the configurations like providing a base path by attribute and a base address/URI fluently.

```WithBaseAddress``` and ```WithBasePath``` options are available at both common and specific configuration level, meanning that you can share a base address with several api interfaces and/or set a specific one for some others.

***