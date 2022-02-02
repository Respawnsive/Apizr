## Registering a managed instance, the static way:

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

### Next steps

- [Using the manager](crud_using.md)
