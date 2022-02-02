## Registering a managed instance, the static way

Here is an example of how to register a managed instance of the CRUD api interface:
```csharp
// Apizr registration
myContainer.RegistrationMethod(() =>
    Apizr.CreateCrudFor<T, TKey, TReadAllResult, TReadAllParams>(options => options
        .WithBaseAddress("your specific T entity crud base uri"))
);
```

T must be a class.

TKey must be primitive. If you don't provide it here, it will be defined as ```int```.

TReadAllResult must inherit from ```IEnumerable<>``` or be a class.
If you don't use paged result, just don't provide any TReadAllResult here and it will be defined as ```IEnumerable<T>```.

TReadAllParams must be a class.
If you don't use a custom class holding your query parameters, just don't provide any TReadAllParams here and it will be defined as ```IDictionary<string, object>```.

You have to provide the specific entity crud base uri with the options builder (if you don't plan to use entity crud attribute).

Also, you could use the manager directly instead of registering it.

## Registering multiple managed instances, the static way

You may want to register multiple managed api interfaces within the same project.
Also, you may want to share some common configuration between apis without repeating yourself, but at the same time, you may need to set some specific ones for some of it.
This is where the ApizrRegistry comes on stage.

Here is an example of how to register a managed instance of multiple api interfaces:
```csharp
// Apizr registry
var apizrRegistry = Apizr.Create(
    registry => registry
        .AddCrudFor<T1>(
            options => options
                .WithBaseAddress("your specific T1 entity crud base uri")
        .AddCrudFor<T2, T2Key, T2ReadAllResult, T2ReadAllParams>(
            options => options
                .WithBaseAddress("your specific T2 entity crud base uri"),
    
    config => config
        .WithAkavacheCacheHandler()
);

// Container registration
apizrRegistry.Populate((type, factory) => 
    myContainer.RegistrationMethodFactory(type, factory)
);
```

Here is what we're saying in this example:
- Add a manager for T1 entity with CRUD api interface and default types into the registry
  - Set a specific address dedicated to T1's manager
- Add a manager for T2 entity with CRUD api interface and custom types into the registry
  - Set a specific address dedicated to T2's manager
- Apply common configuration to all managers by:
  - Providing a cache handler

It's an example, meaning if you don't need common and/or specific configuration, just don't provide it.
And yes you can mix classic and CRUD manager registration into the same registry.

Also, you could register the registry itslef, instead of its populated managers and then use its managers directly.

Or, you could use the managers directly from the registry instead of registering anything.

Here's how to get a manager from the registry:

```csharp
// T1 with default registered types
var t1Manager = apizrRegistry.GetCrudFor<T1>();

// T2 with custom registered types
var t2Manager = apizrRegistry.GetCrudFor<T2, T2Key, T2ReadAllResult, T2ReadAllParams>();
```

## Next steps

- [Using the manager](crud_using.md)
