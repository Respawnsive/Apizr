## Defining an interface for a CRUD Api

As we'll use the built-in yet defined ICrudApi, there's no more definition to do.

Here is what the provided interface looks like then:
```csharp
public interface ICrudApi<T, in TKey, TReadAllResult, in TReadAllParams> where T : class
{
    #region Create

    [Post("")]
    Task<T> Create([Body] T payload);

    [Post("")]
    Task<T> Create([Body] T payload, [Context] Context context);

    [Post("")]
    Task<T> Create([Body] T payload, CancellationToken cancellationToken);

    [Post("")]
    Task<T> Create([Body] T payload, [Context] Context context, CancellationToken cancellationToken);

    #endregion

    #region ReadAll

    [Get("")]
    Task<TReadAllResult> ReadAll();

    [Get("")]
    Task<TReadAllResult> ReadAll([CacheKey] TReadAllParams readAllParams);

    [Get("")]
    Task<TReadAllResult> ReadAll([Property(Constants.PriorityKey)] int priority);

    [Get("")]
    Task<TReadAllResult> ReadAll([Context] Context context);

    [Get("")]
    Task<TReadAllResult> ReadAll(CancellationToken cancellationToken);

    [Get("")]
    Task<TReadAllResult> ReadAll([CacheKey] TReadAllParams readAllParams, [Property(Constants.PriorityKey)] int priority);

    [Get("")]
    Task<TReadAllResult> ReadAll([CacheKey] TReadAllParams readAllParams, [Context] Context context);

    [Get("")]
    Task<TReadAllResult> ReadAll([CacheKey] TReadAllParams readAllParams, CancellationToken cancellationToken);

    [Get("")]
    Task<TReadAllResult> ReadAll([Property(Constants.PriorityKey)] int priority, [Context] Context context);

    [Get("")]
    Task<TReadAllResult> ReadAll([Property(Constants.PriorityKey)] int priority, CancellationToken cancellationToken);

    [Get("")]
    Task<TReadAllResult> ReadAll([Context] Context context, CancellationToken cancellationToken);

    [Get("")]
    Task<TReadAllResult> ReadAll([CacheKey] TReadAllParams readAllParams, [Property(Constants.PriorityKey)] int priority, [Context] Context context);

    [Get("")]
    Task<TReadAllResult> ReadAll([CacheKey] TReadAllParams readAllParams, [Property(Constants.PriorityKey)] int priority, CancellationToken cancellationToken);

    [Get("")]
    Task<TReadAllResult> ReadAll([CacheKey] TReadAllParams readAllParams, [Context] Context context, CancellationToken cancellationToken);

    [Get("")]
    Task<TReadAllResult> ReadAll([CacheKey] TReadAllParams readAllParams, [Property(Constants.PriorityKey)] int priority, [Context] Context context, CancellationToken cancellationToken);

    #endregion

    #region Read

    [Get("/{key}")]
    Task<T> Read([CacheKey] TKey key);

    [Get("/{key}")]
    Task<T> Read([CacheKey] TKey key, [Property(Constants.PriorityKey)] int priority);

    [Get("/{key}")]
    Task<T> Read([CacheKey] TKey key, [Context] Context context);

    [Get("/{key}")]
    Task<T> Read([CacheKey] TKey key, CancellationToken cancellationToken);

    [Get("/{key}")]
    Task<T> Read([CacheKey] TKey key, [Property(Constants.PriorityKey)] int priority, [Context] Context context);

    [Get("/{key}")]
    Task<T> Read([CacheKey] TKey key, [Property(Constants.PriorityKey)] int priority, CancellationToken cancellationToken);

    [Get("/{key}")]
    Task<T> Read([CacheKey] TKey key, [Property(Constants.PriorityKey)] int priority, [Context] Context context, CancellationToken cancellationToken);

    #endregion

    #region Update

    [Put("/{key}")]
    Task Update(TKey key, [Body] T payload);

    [Put("/{key}")]
    Task Update(TKey key, [Body] T payload, [Context] Context context);

    [Put("/{key}")]
    Task Update(TKey key, [Body] T payload, CancellationToken cancellationToken);

    [Put("/{key}")]
    Task Update(TKey key, [Body] T payload, [Context] Context context, CancellationToken cancellationToken);

    #endregion

    #region Delete

    [Delete("/{key}")]
    Task Delete(TKey key);

    [Delete("/{key}")]
    Task Delete(TKey key, [Context] Context context);

    [Delete("/{key}")]
    Task Delete(TKey key, CancellationToken cancellationToken);

    [Delete("/{key}")]
    Task Delete(TKey key, [Context] Context context, CancellationToken cancellationToken); 

    #endregion
}
```

We can see that it comes with many parameter combinations, but it won't do anything until you ask Apizr to. 
Caching, Logging, Policing, Prioritizing... everything is activable fluently with the options builder.

About generic types:
- T and TKey (optional - default: ```int```) meanings are obvious
- TReadAllResult (optional - default: ```IEnumerable<T>```) is there to handle cases where ReadAll doesn't return an ```IEnumerable<T>``` or derived, but a paged result with some statistics
- TReadAllParams (optional - default: ```IDictionary<string, object>```) is there to handle cases where you don't want to provide an ```IDictionary<string, object>``` for a ReadAll reaquest, but a custom class

But again, nothing to do around here.

## Next steps

- [Register the managed instance, the static way](crud_static_registering.md)

OR

- [Register the managed definition, the manual extended way](crud_extended_manual_registering.md)

OR

- [Register any definition with management, the automatic extended way](crud_extended_auto_registering.md)