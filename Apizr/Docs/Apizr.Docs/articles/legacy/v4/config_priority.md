## Configuring Priority

Apizr could use [Fusillade](https://github.com/reactiveui/Fusillade) to offer some api priority management on calls.

To be short, Fusillade is about:

- Auto-deduplication of relevant requests
- Request Limiting
- Request Prioritization
- Speculative requests

Please refer to its [official documentation](https://github.com/reactiveui/Fusillade) if you’d like to know more about it.

### Registering

Please first install this integration package:

|Project|Current|V-Next|
|-------|-----|-----|
|Apizr.Integrations.Fusillade|[![NuGet](https://img.shields.io/nuget/v/Apizr.Integrations.Fusillade.svg)](https://www.nuget.org/packages/Apizr.Integrations.Fusillade/)|[![NuGet Pre Release](https://img.shields.io/nuget/vpre/Apizr.Integrations.Fusillade.svg)](https://www.nuget.org/packages/Apizr.Integrations.Fusillade/)|

Then you'll be able to register with this option:

```csharp
options => options.WithPriorityManagement()
```

### Defining

While defining your api interfaces using Apizr to send a request, you can add an int property param decorated with the provided Property attribute to your methods like:

```csharp
[WebApi("https://reqres.in/api")]
public interface IReqResService
{
    [Get("/users")]
    Task<UserList> GetUsersAsync([Priority] int priority);
}
```

### Using

Just call your api with your priority:

```csharp
var result = await _reqResManager.ExecuteAsync(api => api.GetUsersAsync((int)Priority.Background));
```