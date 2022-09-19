## Breaking changes

Please find here some breaking changes while upgrading from previous versions

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
