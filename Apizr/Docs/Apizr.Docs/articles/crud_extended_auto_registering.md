<h2 id="crud-automatically">
Automatically:
</h2>

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
- (Mandatory) We have to provide the specific entity crud base uri
- (Optional) We can set TKey type to any primitive type (default to int)
- (Optional) We can set TReadAllResult to any class or must inherit from ```IEnumerable<>``` (default to ```IEnumerable<T>```)
- (Optional) We can set TReadAllParams to any class (default to ```IDictionary<string, object>```)

Then, register in your Startup class like so:
```csharp
public override void ConfigureServices(IServiceCollection services)
{
    // Apizr registration
    services.AddApizrCrudFor(typeof(MyEntity));
}
```

There are many AddApizrCrudFor flavors for crud automatic registration, depending on what you want to do and provide.
This is the simplest.