<h2 id="crud-static-approach">
Static instance registration:
</h2>

Somewhere where you can add services to your container, add the following:
```csharp
// Apizr registration
myContainer.SomeInstanceRegistrationMethod(
Apizr.CreateCrudFor<T, TKey, TReadAllResult, TReadAllParams>(options => options
    .WithBaseAddress("your specific T entity crud base uri")
    .WithAkavacheCacheHandler())
);
```

T must be a class.

TKey must be primitive. If you don't provide it here, it will be defined as ```int```.

TReadAllResult must inherit from ```IEnumerable<>``` or be a class.
If you don't use paged result, just don't provide any TReadAllResult here and it will be defined as ```IEnumerable<T>```.

TReadAllParams must be a class.
If you don't use a custom class holding your query parameters, just don't provide any TReadAllParams here and it will be defined as ```IDictionary<string, object>```.

You have to provide the specific entity crud base uri with the options builder.

There are many CreateCrudFor flavors, depending on what you want to do and provide.
One of it is the simple ```Apizr.CreateCrudFor<T>()```, which as you can expect, define TKey as ```int```, TReadAllResult as ```IEnumerable<T>``` and TReadAllParams as ```IDictionary<string, object>```.
