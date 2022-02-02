## Defining an interface for a classic Api

We could define our web api service just like:
```csharp
[assembly:Policy("TransientHttpError")]
namespace Apizr.Sample.Api
{
    [WebApi("https://reqres.in/"), Cache, Log]
    public interface IReqResService
    {
        [Get("/api/users")]
        Task<UserList> GetUsersAsync();

        [Get("/api/users/{userId}")]
        Task<UserDetails> GetUserAsync([CacheKey] int userId);

        [Post("/api/users")]
        Task<User> CreateUser(User user);
    }
}
```

And that's all.

Every attributes here will inform Apizr on how to manage each web api request. No more boilerplate.

## Next steps

- [Register the managed instance, the static way](classic_static_registering.md)

OR

- [Register the managed definition, the manual extended way](classic_extended_manual_registering.md)

OR

- [Register any definition with management, the automatic extended way](classic_extended_auto_registering.md)