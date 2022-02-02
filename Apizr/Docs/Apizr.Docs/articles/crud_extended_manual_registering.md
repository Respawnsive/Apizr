## Manually registering the managed CRUD api interface by extensions:

Here is an example:
```csharp
public override void ConfigureServices(IServiceCollection services)
{
    // Apizr registration
    services.AddApizrCrudFor<T, TKey, TReadAllResult, TReadAllParams>(options => options
        .WithBaseAddress("your specific T entity crud base uri"));
}
```

Again, T must be a class.

TKey must be primitive. If you don't provide it here, it will be defined as ```int```.

TReadAllResult must inherit from ```IEnumerable<>``` or be a class.
If you don't use paged result, just don't provide any TReadAllResult here and it will be defined as ```IEnumerable<T>```.

TReadAllParams must be a class.
If you don't use a custom class holding your query parameters, just don't provide any TReadAllParams here and it will be defined as ```IDictionary<string, object>```.

You have to provide the specific entity crud base uri with the options builder (if you don't plan to use entity crud attribute).

### Next steps

- [Using the manager](crud_using.md)