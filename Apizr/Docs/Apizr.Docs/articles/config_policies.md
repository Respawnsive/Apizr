## Configuring Policies

Apizr comes with a `Policy` attribute to apply some policies on apis, handled by [Polly](https://github.com/App-vNext/Polly).

You’ll find also policy attributes dedicated to CRUD apis like `CreatePolicy`, `ReadPolicy` and so on…

Polly will help you to manage some retry scenarios but can do more. Please refer to its [official documentation](https://github.com/App-vNext/Polly) if you’d like to know more about it.

### Defining

Here is a simple example of using it:
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

Here we are using it at assembly level, telling Apizr to apply `TransiantHttpError` policy on every apis.

Then we define what actually is the `TransiantHttpError` policy:

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

We have to register our policy with the same name used by the Policy attribute decorating our apis.

`TransiantHttpError` policy is actually provided by Polly itself, so we jsut call its `HttpPolicyExtensions.HandleTransientHttpError()` method.

I’m also giving here an `OnLoggedRetry` method provided by Apizr, so I coud get some logging outputs when Polly comes in the party in case of handled failures.

`PolicyRegistry` is where you register all your named policies to be used by Polly thanks to attribute decoration, TransiantHttpError is just an example.

### Registering

### [Static](#tab/tabid-static)

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