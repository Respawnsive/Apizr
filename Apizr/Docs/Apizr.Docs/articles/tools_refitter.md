## Refitter

Refitter ([Documentation](https://refitter.github.io/) | [GitHub](https://github.com/christianhelle/refitter)) is a tool for generating a C# REST API Client using the [Refit](https://github.com/reactiveui/refit) library. 
Refitter can generate the Refit interface and contracts from OpenAPI specifications. 
Refitter (v1.2+) could also format the generated Refit interface to be managed by [Apizr](https://www.apizr.net) (v6+) and generate some registration helpers too.
It comes in 2 forms:
- A [.NET CLI Tool](https://refitter.github.io/articles/cli-tool.md) distributed via [nuget.org](http://www.nuget.org/packages/refitter) that outputs a single C# file on disk
- A [C# Source Generator](https://refitter.github.io/articles/source-generator.md) via the [Refitter.SourceGenerator](http://www.nuget.org/packages/refitter.sourcegenerator) package that generates code on compile time based on a [.refitter](https://refitter.github.io/articles/refitter-file-format.md) within the project directory.

### Installing the package

Choose which generating approach suites to your needs by installing either:

|Project|Current|Upcoming|
|-------|-----|-----|
|Refitter|[![NuGet](https://img.shields.io/nuget/v/refitter.svg)](https://www.nuget.org/packages/refitter/)|[![NuGet Pre Release](https://img.shields.io/nuget/vpre/refitter.svg)](https://www.nuget.org/packages/refitter/)|
|Refitter.SourceGenerator|[![NuGet](https://img.shields.io/nuget/v/refitter.sourcegenerator.svg)](https://www.nuget.org/packages/refitter.sourcegenerator/)|[![NuGet Pre Release](https://img.shields.io/nuget/vpre/refitter.sourcegenerator.svg)](https://www.nuget.org/packages/refitter.sourcegenerator/)|

### Generating the interfaces

Refitter (v1.2+) supports generating Apizr formatted Refit interfaces that can be managed then by Apizr (v6+).

You can enable Apizr formatted Refit interface generation either:
- With the `--use-apizr` command line argument
- By setting the `apizrSettings` section in the `.refitter` settings file

Note that `--use-apizr` uses default Apizr settings with `withRequestOptions` set to `true` as recommended, while the `.refitter` settings file allows you to configure it deeper.

In both cases, it will format the generated Refit interfaces to be Apizr ready by:
- Adding a final IApizrRequestOptions options parameter to all generated methods (if `withRequestOptions` is set to `true`)
- Providing cancellation tokens by Apizr request options instead of a dedicated parameter (if `withRequestOptions` is set to `true`)
- Using method overloads instead of optional parameters (note that setting `useDynamicQuerystringParameters` to true improve overloading experience)

From here, you're definitly free to use the formatted interface with Apizr by registering, configuring and using it following the Apizr documentation. But Refitter (v1.2+) can go further by generating some helpers to make the configuration easier.

### Generating the helpers

Refitter (v1.2+) supports generating Apizr (v6+) bootstrapping code that allows the user to conveniently configure all generated Apizr formatted Refit interfaces by calling a single method.
It could be either an extension method to `IServiceCollection` if DependencyInjectionSettings are set, or a static builder method if not.

### [Extended](#tab/tabid-extended)

To enable Apizr registration code generation for `IServiceCollection`, you need at least to set the `withRegistrationHelper` property to `true` and configure the `DependencyInjectionSettings` section in the `.refitter` settings file.
This is what the `.refitter` settings file may look like, depending on you configuration:

```json
{
  "openApiPath": "https://petstore3.swagger.io/api/v3/openapi.yaml",
  "namespace": "Petstore",
  "useDynamicQuerystringParameters": true,
  "dependencyInjectionSettings": {
    "baseUrl": "https://petstore3.swagger.io/api/v3",
    "httpMessageHandlers": [ "MyDelegatingHandler" ],
    "transientErrorHandler": "HttpResilience",
    "maxRetryCount": 3,
    "firstBackoffRetryInSeconds": 0.5
  },
  "apizrSettings": {
    "withRequestOptions": true, // Recommended to include an Apizr request options parameter to Refit interface methods
    "withRegistrationHelper": true, // Mandatory to actually generate the Apizr registration extended method
    "withCacheProvider": "InMemory", // Optional, default is None
    "withPriority": true, // Optional, default is false
    "withMediation": true, // Optional, default is false
    "withOptionalMediation": true, // Optional, default is false
    "withMappingProvider": "AutoMapper", // Optional, default is None
    "withFileTransfer": true // Optional, default is false
  }
}
```

which will generate an extension method to `IServiceCollection` called `ConfigurePetstoreApiApizrManager()`. The generated extension method depends on [`Apizr.Extensions.Microsoft.DependencyInjection`](https://www.nuget.org/packages/Apizr.Extensions.Microsoft.DependencyInjection) library and looks like this:

```cs
public static IServiceCollection ConfigurePetstoreApiApizrManager(
    this IServiceCollection services,
    Action<IApizrExtendedManagerOptionsBuilder>? optionsBuilder = null)
{
    optionsBuilder ??= _ => { }; // Default empty options if null
    optionsBuilder += options => options
        .WithBaseAddress("https://petstore3.swagger.io/api/v3", ApizrDuplicateStrategy.Ignore)
        .WithDelegatingHandler<MyDelegatingHandler>()
        .ConfigureHttpClientBuilder(builder => builder
            .AddStandardResilienceHandler(config =>
            {
                config.Retry = new HttpRetryStrategyOptions
                {
                    UseJitter = true,
                    MaxRetryAttempts = 3,
                    Delay = TimeSpan.FromSeconds(0.5)
                };
            }))
        .WithInMemoryCacheHandler()
        .WithAutoMapperMappingHandler()
        .WithPriority()
        .WithOptionalMediation()
        .WithFileTransferOptionalMediation();
                 
    return services.AddApizrManagerFor<IPetstoreApi>(optionsBuilder);
}
```

This comes in handy especially when generating multiple interfaces, by tag or endpoint. For example, the following `.refitter` settings file

```json
{
  "openApiPath": "https://petstore3.swagger.io/api/v3/openapi.yaml",
  "namespace": "Petstore",
  "useDynamicQuerystringParameters": true,
  "multipleInterfaces": "ByTag",
  "naming": {    
    "useOpenApiTitle": false,
    "interfaceName": "Petstore"
  },
  "dependencyInjectionSettings": {
    "baseUrl": "https://petstore3.swagger.io/api/v3",
    "httpMessageHandlers": [ "MyDelegatingHandler" ],
    "transientErrorHandler": "HttpResilience",
    "maxRetryCount": 3,
    "firstBackoffRetryInSeconds": 0.5
  },
  "apizrSettings": {
    "withRequestOptions": true, // Recommended to include an Apizr request options parameter to Refit interface methods
    "withRegistrationHelper": true, // Mandatory to actually generate the Apizr registration extended method
    "withCacheProvider": "InMemory", // Optional, default is None
    "withPriority": true, // Optional, default is false
    "withMediation": true, // Optional, default is false
    "withOptionalMediation": true, // Optional, default is false
    "withMappingProvider": "AutoMapper", // Optional, default is None
    "withFileTransfer": true // Optional, default is false
  }
}
```

Will generate a single `ConfigurePetstoreApizrManagers()` extension method that may contain dependency injection configuration code for multiple interfaces like this

```csharp
public static IServiceCollection ConfigurePetstoreApizrManagers(
    this IServiceCollection services,
    Action<IApizrExtendedCommonOptionsBuilder>? optionsBuilder = null)
{
    optionsBuilder ??= _ => { }; // Default empty options if null
    optionsBuilder += options => options
        .WithBaseAddress("https://petstore3.swagger.io/api/v3", ApizrDuplicateStrategy.Ignore)
        .WithDelegatingHandler<MyDelegatingHandler>()
        .ConfigureHttpClientBuilder(builder => builder
            .AddStandardResilienceHandler(config =>
            {
                config.Retry = new HttpRetryStrategyOptions
                {
                    UseJitter = true,
                    MaxRetryAttempts = 3,
                    Delay = TimeSpan.FromSeconds(0.5)
                };
            }))
        .WithInMemoryCacheHandler()
        .WithAutoMapperMappingHandler()
        .WithPriority()
        .WithOptionalMediation()
        .WithFileTransferOptionalMediation();
            
    return services.AddApizr(
        registry => registry
            .AddManagerFor<IPetApi>()
            .AddManagerFor<IStoreApi>()
            .AddManagerFor<IUserApi>(),
        optionsBuilder);

}
```

Here, `IPetApi`, `IStoreApi` and `IUserApi` are the generated interfaces which share the same common configuration defined from the `.refitter` file.

### [Static](#tab/tabid-static)

To enable Apizr static builder code generation, you need at least to set the `withRegistrationHelper` property to `true` and leave the `DependencyInjectionSettings` section to null in the `.refitter` settings file.
This is what the `.refitter` settings file may look like, depending on you configuration:

```json
{
  "openApiPath": "../OpenAPI/v3.0/petstore.json",
  "namespace": "Petstore",
  "useDynamicQuerystringParameters": true,
  "apizrSettings": {
    "withRequestOptions": true, // Recommended to include an Apizr request options parameter to Refit interface methods
    "withRegistrationHelper": true, // Mandatory to actually generate the Apizr registration extended method
    "withCacheProvider": "Akavache", // Optional, default is None
    "withPriority": true, // Optional, default is false
    "withMappingProvider": "AutoMapper", // Optional, default is None
    "withFileTransfer": true // Optional, default is false
  }
}
```

which will generate a static builder method called `BuildPetstore30ApizrManager()`. The generated builder method depends on [`Apizr`](https://www.nuget.org/packages/Apizr) library and looks like this:

```cs
public static IApizrManager<ISwaggerPetstoreOpenAPI30> BuildPetstore30ApizrManager(Action<IApizrManagerOptionsBuilder> optionsBuilder)
{
    optionsBuilder ??= _ => { }; // Default empty options if null
    optionsBuilder += options => options
        .WithAkavacheCacheHandler()
        .WithAutoMapperMappingHandler(new MapperConfiguration(config => { /* YOUR_MAPPINGS_HERE */ }))
        .WithPriority();
            
    return ApizrBuilder.Current.CreateManagerFor<ISwaggerPetstoreOpenAPI30>(optionsBuilder);  
}
```

This comes in handy especially when generating multiple interfaces, by tag or endpoint. For example, the following `.refitter` settings file

```json
{
  "openApiPath": "../OpenAPI/v3.0/petstore.json",
  "namespace": "Petstore",
  "multipleInterfaces": "ByTag",
  "naming": {    
    "useOpenApiTitle": false,
    "interfaceName": "Petstore"
  },
  "dependencyInjectionSettings": {
    "baseUrl": "https://petstore3.swagger.io/api/v3",
    "httpMessageHandlers": [ "MyDelegatingHandler" ],
    "transientErrorHandler": "HttpResilience",
    "maxRetryCount": 3,
    "firstBackoffRetryInSeconds": 0.5
  },
  "apizrSettings": {
    "withRequestOptions": true, // Recommended to include an Apizr request options parameter to Refit interface methods
    "withRegistrationHelper": true, // Mandatory to actually generate the Apizr registration extended method
    "withCacheProvider": "InMemory", // Optional, default is None
    "withPriority": true, // Optional, default is false
    "withMediation": true, // Optional, default is false
    "withOptionalMediation": true, // Optional, default is false
    "withMappingProvider": "AutoMapper", // Optional, default is None
    "withFileTransfer": true // Optional, default is false
  }
}
```

Will generate a single `BuildPetstoreApizrManagers()` builder method that may contain configuration code for multiple interfaces like this

```csharp
public static IApizrRegistry BuildPetstoreApizrManagers(Action<IApizrCommonOptionsBuilder> optionsBuilder)
{
    optionsBuilder ??= _ => { }; // Default empty options if null
    optionsBuilder += options => options
        .WithAkavacheCacheHandler()
        .WithAutoMapperMappingHandler(new MapperConfiguration(config => { /* YOUR_MAPPINGS_HERE */ }))
        .WithPriority();
            
    return ApizrBuilder.Current.CreateRegistry(
        registry => registry
            .AddManagerFor<IPetApi>()
            .AddManagerFor<IStoreApi>()
            .AddManagerFor<IUserApi>(),
        optionsBuilder);
}
```

Here, `IPetApi`, `IStoreApi` and `IUserApi` are the generated interfaces which share the same common configuration defined from the `.refitter` file.

***

You now just have to call the generated helper method to get all the thing ready to use.

### Customizing the configuration

You may want to adjust apis configuration, for example, to add a custom header to requests. This can be done using the `Action<TApizrOptionsBuilder>` parameter while calling the generated method.

Here is the recommended way to customize the configuration using IConfiguration, as it lets you do it from the top assembly common level down to the api specific method one:

### [Extended](#tab/tabid-extended)
```csharp
services.ConfigurePetstoreApizrManager(options => options
	.WithConfiguration(Your_Configuration));
```

### [Static](#tab/tabid-static)
```csharp
ApizrRegistrationHelper.BuildPetstoreApizrManager(options => options
	.WithConfiguration(Your_Configuration));
```
***

To know how to make it fit your needs, please refer to the current Apizr documentation.

### Using the managers

Once you called the generated method, you will get an `IApizrManager<T>` instance that you can use to make requests to the API. Here's an example of how to use it:

```csharp
var result = await petstoreManager.ExecuteAsync((api, opt) => api.GetPetById(1, opt), 
    options => options // Whatever final request options you want to apply
        .WithPriority(Priority.Background)
        .WithHeaders(["HeaderKey1: HeaderValue1"])
        .WithRequestTimeout("00:00:10")
        .WithCancellation(cts.Token));
```