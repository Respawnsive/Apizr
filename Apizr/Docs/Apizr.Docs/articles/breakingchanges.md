## Breaking changes

Please find here some breaking changes while upgrading from previous versions

### 6.0

#### Apizr

- [Polly] Now **supporting the brand new Polly v8+ Resilience Strategies/Pipelines** instead of former Polly v7- Policies

    - You'll have to rewrite your policies as strategies/pipelines. 
        Here is an example of a former policy and its new equivalent strategy/pipeline:
 
        Don't write anymore:
        ```csharp 
        var policy = Policy
            .Handle<SomeExceptionType>()
            .Retry(3);
        ``` 

        Now write:
        ```csharp 
        var resiliencePipelineBuilder = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                ShouldHandle = new PredicateBuilder().Handle<SomeExceptionType>(),
                MaxRetryAttempts = 3,
                Delay = TimeSpan.Zero,
            });
        ``` 

    - You can't register your policies into a policy registry and provide it to Apizr anymore. 
        You have to register your pipeline into a pipeline registry instead and provide it to Apizr:
 
        ### [Static](#tab/tabid-static)

        Don't write anymore:
        ```csharp 
        var registry = new PolicyRegistry
        {
            { "TransientHttpError", policy }
        };  
        ...
        options => options.WithPolicyRegistry(registry)
        ``` 

        Now write:
        ```csharp 
        var resiliencePipelineRegistry = new ResiliencePipelineRegistry<string>();
        resiliencePipelineRegistry.TryAddBuilder<HttpResponseMessage>("TransientHttpError", 
            (builder, _) => builder.AddPipeline(resiliencePipelineBuilder.Build()));  
        ...
        options => options.WithResiliencePipelineRegistry(resiliencePipelineRegistry)  
        ``` 

        ### [Extended](#tab/tabid-extended)

        Don't write anymore:
        ```csharp 
        var registry = new PolicyRegistry
        {
            { "TransientHttpError", policy }
        };  
        ...
        services.AddPolicyRegistry(registry);
        ``` 

        Now write:
        ```csharp 
        services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
            builder => builder.AddPipeline(resiliencePipelineBuilder.Build()));  
        ```

        ***
        
    - You can't provide your own context instance anymore to carry some properties. 
        But you can provide your properties directly instead:
 
        Don't write anymore:
        ```csharp 
        var context = new Context {{ "TestKey1", 1 }};
        ...
        options => options.WithContext(context);
        ``` 

        Now write:
        ```csharp 
        ResiliencePropertyKey<string> testKey1 = new("TestKey1");
        ...
        options => options.WithResilienceProperty(testKey1, "testValue1")
        ``` 

    - You'll have to change your PolicyAttribute to ResiliencePipelineAttribute. 
 
        Don't write anymore:
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

        Now write:
        ```csharp 
        [assembly:ResiliencePipeline("TransientHttpError")]
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

- [Headers] Now **WithHeaders options take an enumerable parameter instead of a parameter array** so that we could provide some more optional parameters

    Don't write anymore:
    ```csharp
    // direct configuration
    options => options.AddHeaders("HeaderKey1: HeaderValue1", "HeaderKey2: HeaderValue2")

    // OR factory configuration
    options => options.AddHeaders(() => $"HeaderKey3: {YourHeaderValue3}")

    // OR extended factory configuration with the service provider instance
    options => options.AddHeaders(serviceProvider => $"HeaderKey3: {serviceProvider.GetRequiredService<IYourSettingsService>().YourHeaderValue3}")
    ```

    Now write:
    ```csharp
    // direct configuration
    options => options.AddHeaders(["HeaderKey1: HeaderValue1", "HeaderKey2: HeaderValue2"])

    // OR factory configuration
    options => options.AddHeaders(() => [$"HeaderKey3: {YourHeaderValue3}"])

    // OR extended factory configuration with the service provider instance
    options => options.AddHeaders(serviceProvider => [$"HeaderKey3: {serviceProvider.GetRequiredService<IYourSettingsService>().YourHeaderValue3}"])

    // OR extended factory configuration with your service instance
    options => options.AddHeaders<IYourSettingsService>([settings => $"HeaderKey3: {settings.YourHeaderValue3}"])
    ```
    
### 5.3

#### Apizr

- [HttpClient] Now we can **configure the HttpClient instead of providing one** (same as extended experience) with the brand new ConfigureHttpClient fluent option
 
    Don't write anymore:
    ```csharp 
    options => options.WithHttpClient((httpMessageHandler, baseUri) => 
        new YourOwnHttpClient(httpMessageHandler, false){BaseAddress = baseUri, WhateverOption = whateverValue});
    ``` 

    Now write:
    ```csharp 
    options => options.ConfigureHttpClient(httpClient => httpClient.WhateverOption = whateverValue)
    ``` 
    
### 5.0

#### Apizr

- Now **ApizrBuilder static class exposes a single public property named Current and returning its own instance to get acces to its methods**, so that it could be extended then by other packages

    Don't write anymore:
    ```csharp 
    ApizrBuilder.WhatEver();
    ``` 

    Now write:
    ```csharp 
    ApizrBuilder.Current.WhatEver();
    ``` 
    
- **Some methods have been deprecated and moved as extension methods to a dedicated namespace**, pointing to the new core ones

    Don't write anymore: 
    ```csharp 
    // Designing
    [Get("/")]
    Task<MyResult> WhatEver([Priority] int priority, [Context] Context context, CancellationToken cancellationToken);
    
    // Requesting
    myManager.ExecuteAsync((ctx, ct, api) => api.WhatEver((int)Priority.Background, ct), context, token, true, OnEx)
    ``` 

    Now write:
    ```csharp 
    // Designing
    [Get("/")]
    Task<MyResult> WhatEver([RequestOptions] IApizrRequestOptions options);
    
    // Requesting
    myResult = await myManager.ExecuteAsync((opt, api) => api.WhatEver(opt), 
        options => options.WithCacheClearing(true)
            .WithCancellation(token)
            .WithContext(context)
            .WithPriority(Priority.Background)
            .WithExCatcher(OnEx));
    ``` 

### 4.1

#### Apizr

- **Apizr static class renamed to ApizrBuilder to match its purpose** and doesn't conflict with its namespace anymore
- **ApizrBuilder's methods renamed to match their return type** so that we know what we're about to build (e.g. CreateRegistry, AddManagerFor, CreateManagerFor)
- **ApizrRegistry's methods renamed to match their return type** so that we know what we're about to get (e.g. GetManagerFor, GetCrudManagerFor, ContainsManagerFor)

#### Apizr.Extensions.Microsoft.DependencyInjection

- **Extension methods renamed to match their return type** so that we know what we're about to build (e.g. AddManagerFor, AddCrudManagerFor)
- **ApizrExtendedRegistry's methods renamed to match their return type** so that we know what we're about to get (e.g. GetManagerFor, GetCrudManagerFor, ContainsManagerFor)

#### Apizr.Integrations.MediatR

- **ApizrMediationRegistry's methods renamed to match their return type** so that we know what we're about to get (e.g. GetMediatorFor, GetCrudMediatorFor, ContainsMediatorFor)

#### Apizr.Integrations.Optional

- **ApizrOptionalMediationRegistry's methods renamed to match their return type** so that we know what we're about to get (e.g. GetOptionalMediatorFor, GetCrudOptionalMediatorFor, ContainsOptionalMediatorFor)

### 4.0

#### Apizr

- TraceAttribute has been **renamed back to LogAttribute**
- Now we can set a **LogLevel value for each Low, Medium and High severity** by attribute or fluent configuration

#### Apizr.Integrations.MediatR

- Now **Apizr.Integrations.MediatR targets .Net Standard 2.1** as MediatR v10+ does

#### Apizr.Integrations.Optional

- Now **Apizr.Integrations.Optional targets .Net Standard 2.1** as Apizr.Integrations.MediatR v4+ does

#### Apizr.Integrations.AutoMapper

- Now **Apizr.Integrations.AutoMapper targets .Net Standard 2.1** as AutoMapper v11+ does

>[!WARNING]
>
>**Apizr.Integrations.Shiny has been discontinued**
>
>This integration project has been dropped out as Shiny no longer provide built-in caching and logging feature anymore. Apizr now either relies on MS Caching extensions, Akavache or MonkeyCache for caching feature and MS Logging extensions for logging feature. You'll have to provide a connectivity handler if you want Apizr to check it.

### 3.0

#### Apizr

- **Fusillade has been moved to an integration package**. If you used to play with it, just install it from its brand new dedicated integration package and follow the new Readme instructions

### 2.0

#### Apizr

- **TraceAttribute renamed to LogItAttribute** to suits its tracing and logging both features activation
- **CacheAttribute renamed to CacheItAttribute** to keep things consistent
- **No more cache and policy attribute decorating CRUD api** by default. You can activate it fluently with the options builder.

### 1.4.0

#### Apizr.Integrations.MediatR

- ```WithCrudMediation``` renamed to ```WithMediation```

#### Apizr.Integrations.Optional

- ```WithCrudOptionalMediation``` renamed to ```WithOptionalMediation```

### 1.2.0

#### Apizr

- Apizr instantiation/registration methods names standardized to Apizr.For and Apizr.CrudFor

#### Apizr.Extensions.Microsoft.DependencyInjection

- Apizr instantiation/registration methods names standardized to services.AddApizrFor and services.AddApizrCrudFor

#### Apizr.Integrations.Shiny

- Apizr instantiation/registration methods names standardized to services.UseApizrFor and services.UseApizrCrudFor
