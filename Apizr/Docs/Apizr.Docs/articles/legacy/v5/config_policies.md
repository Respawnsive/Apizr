## Configuring Policies

Apizr comes with a `Policy` attribute to apply some policies on apis, handled by [Polly](https://github.com/App-vNext/Polly).

You’ll find also policy attributes dedicated to CRUD apis like `CreatePolicy`, `ReadPolicy` and so on…

Polly will help you to manage some retry scenarios but can do more. Please refer to its [official documentation](https://github.com/App-vNext/Polly) if you’d like to know more about it.

### Registering

Here is how to define a policy, adding it to a policy registry.

```csharp
var registry = new PolicyRegistry
{
    {
        "TransientHttpError", HttpPolicyExtensions.HandleTransientHttpError().WaitAndRetryAsync(new[]
        {
            TimeSpan.FromSeconds(1),
            TimeSpan.FromSeconds(5),
            TimeSpan.FromSeconds(10)
        }, LoggedPolicies.OnLoggedRetry).WithPolicyKey("TransientHttpError")
    }
};
```

`TransiantHttpError` policy is actually provided by Polly itself, so we jsut call its `HttpPolicyExtensions.HandleTransientHttpError()` method.

We're also giving here an `OnLoggedRetry` method provided by Apizr, so we coud get some logging outputs when Polly comes in the party in case of handled failures.

`PolicyRegistry` is where you register all your named policies to be used by Polly thanks to attribute decoration, TransiantHttpError is just an example.

Now we have to register our policy registry:

### [Static](#tab/tabid-static)

You'll be able to register your policy registry with this option:

```csharp
// direct configuration
options => options.WithPolicyRegistry(registry)

// OR factory configuration
options => options.WithPolicyRegistry(() => registry)
```

### [Extended](#tab/tabid-extended)

There's nothing specific to do with Apizr about Polly when using the extended approach.

Just don't forget to register it like you usualy do:

```csharp
services.AddPolicyRegistry(registry);
```

***

### Defining

Now we can use it thanks to attribute decoration:

```csharp
[assembly:Policy("TransientHttpError")]
namespace Apizr.Sample
{
    [WebApi("https://reqres.in/api")]
    public interface IReqResService
    {
        [Get("/users")]
        Task<UserList> GetUsersAsync();
    }
}
```

Here we are using it at assembly level, telling Apizr to apply `TransiantHttpError` policy to all apis.

You can mix levels and mix policies as all will be wrapped.

### Using

Apizr will automatically tell Polly to handle request of any decorated api method.