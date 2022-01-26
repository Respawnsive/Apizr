<h2 id="crud-manually">
Manually registering by extensions:
</h2>

In your Startup class, add the following:
```csharp
public override void ConfigureServices(IServiceCollection services)
{
    // Apizr registration
    services.AddApizrCrudFor<T, TKey, TReadAllResult, TReadAllParams>(optionsBuilder => optionsBuilder
        .WithBaseAddress("your specific T entity crud base uri")
        .WithAkavacheCacheHandler());
}
```

Again, T must be a class.

TKey must be primitive. If you don't provide it here, it will be defined as ```int```.

TReadAllResult must inherit from ```IEnumerable<>``` or be a class.
If you don't use paged result, just don't provide any TReadAllResult here and it will be defined as ```IEnumerable<T>```.

TReadAllParams must be a class.
If you don't use a custom class holding your query parameters, just don't provide any TReadAllParams here and it will be defined as ```IDictionary<string, object>```.

You have to provide the specific entity crud base uri with the options builder.

There are many AddApizrCrudFor flavors for crud manual registration, depending on what you want to do and provide.
One of it is the simple ```services.AddApizrCrudFor<T>()```, which as you can expect, define TKey as ```int```, TReadAllResult as ```IEnumerable<T>``` and TReadAllParams as ```IDictionary<string, object>```.
