## Automatically registering managers for decorated CRUD entities by extensions:

You need to have access to your entity model classes for this option.

Decorate your crud entities like so (but with your own settings):
```csharp
[CrudEntity("https://mybaseuri.com/api/myentity", typeof(int), typeof(PagedResult<>), typeof(ReadAllUsersParams))]
public class MyEntity
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    ...
}
```

Thanks to this attribute:
- (Mandatory) We have to provide the specific entity crud base uri (no more fluent declaration)
- (Optional) We can set TKey type to any primitive type (default to int)
- (Optional) We can set TReadAllResult to any class or must inherit from ```IEnumerable<>``` (default to ```IEnumerable<T>```)
- (Optional) We can set TReadAllParams to any class (default to ```IDictionary<string, object>```)

Then, here is a registration example:
```csharp
public override void ConfigureServices(IServiceCollection services)
{
    // Apizr registration
    services.AddApizrCrudFor(ASSEMBLIES_CONTAINING_ENTITIES);
}
```

Apizr will scan assemblies to auto register managers for decorated entities.

### Next steps

- [Using the manager](crud_using.md)