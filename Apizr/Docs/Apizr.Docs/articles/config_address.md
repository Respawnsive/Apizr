## Configuring base address

You can configure base address either by attribute decoration or by fluent configuration.

### [Attribute](#tab/tabid-attribute)

Configuring the base address with attribute allows you to use assembly scanning auto registration feature.

### Classic api

You can set api interface base address thanks to the WebApi attribute like so:

```csharp
[WebApi("https://YOUR_API_INTERFACE_BASE_ADDRESS/")]
public interface IYourApiInterface
{
    // Your api interface methods
}
```

Optional parameters:
- ```isAutoRegistrable``` (default: true) tells Apizr to ignore this specific interface while scanning assemblies for auto registration. 
It could be usefull when you ask for auto registration but want to do it fluently some specific interfaces.

### CRUD api

You can set CRUD entity api base address thanks to the CrudEntity attribute like so:

```csharp
[CrudEntity("https://YOUR_CRUD_ENTITY_API_BASE_ADDRESS")]
public class YourCrudEntity
{
    // Your CRUD entity properties
}
```

Optional parameters:
- ```keyType```: entity key type (default: null = typeof(int))
- ```readAllResultType```: ReadAll query result type  (default: null = typeof(IEnumerable{}))
- ```readAllParamsType```: ReadAll query parameters type  (default: null = typeof(IDictionary{string, object}))
- ```modelEntityType```: Model entity type mapped with this api entity type (default: null = decorated api entity type)


### [Fluent](#tab/tabid-fluent)

Configuring the base address fluently allows you to set it dynamically (e.g. based on settings)

You can set the base address thanks to this option:

```csharp
options => options.WithBaseAddress(YourStaticSettings.YourBaseAddress))
```

The extended world offers another option with a factory providing a service provider instance.
It could help you to resolve the setting at runtime:

```csharp
options => options.WithBaseAddress(serviceProvider => serviceProvider.GetRequiredService<IYourSettingsService>().YourBaseAddress))
```

***