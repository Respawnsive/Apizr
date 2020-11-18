1.9
---

### Apizr

- [New] Handling complex type as CacheKey
- [New] Now we can set Apizr log level within TraceAttribute to manage execution tracing verbosity

### Apizr.Integrations.MediatR

- [Change] Mediation's ICommand interface renamed to IMediationCommand, avoiding conflict with System.Windows.Input.ICommand

1.8.1
---

### Apizr

- [Fix] Parsing life span representation as TimeSpan from CacheAttribute

1.8
---

### Apizr.Integrations.Optional

- [New] Introducing CatchAsync optional extension method to return result from fetch or cache, no matter of execption handled on the other side by an action callback to inform the user

1.7
---

### Apizr

- [New] Now we can toggle Fusillade priority management activation
- [New] Now we can provide a base uri factory (e.g. depending on config)

### Apizr.Extensions.Microsoft.DependencyInjection

- [New] Now we can toggle Fusillade priority management activation
- [New] Now we can provide a base uri factory (e.g. depending on DI resovled settings)

1.6
---

- [Fix] Preserve attribute added

### Apizr

- [New] Now we can provide a custom HttpClientHandler instance

### Apizr.Extensions.Microsoft.DependencyInjection

- [New] Now we can provide a custom HttpClientHandler instance

1.5
---

### Apizr

- [Fix] Now the manager waits for task with no result to handle exceptions properly

### Apizr.Integrations.MediatR

- [New] Introducing typed mediator and typed crud mediator for shorter request
- [Fix] Now MediatR handlers are registered correctly when asked from a manual registration context
- [Fix] Mapping null object now works correctly
- [Fix] Now MediatR handlers wait for its handling task to handle exceptions properly

### Apizr.Integrations.Optional

- [New] Introducing typed optional mediator and typed crud optional mediator for shorter request
- [New] Introducing OnResultAsync optional extension method to make all the thing shorter than ever
- [Fix] Now Optional handlers are registered correctly when asked from a manual registration context
- [Fix] Optional request handlers now handle exceptions as expected
- [Fix] Now Optional handlers wait for its handling task to handle exceptions properly

1.4.2
---

### Apizr.Integrations.MediatR

- [Fix] Now nuget package as library both reference MediatR.Extensions.Microsoft.DependencyInjection nuget package for assembly version compatibility

1.4.1
---

### Apizr.Extensions.Microsoft.DependencyInjection

- [Fix] Now Apizr works with DryIoc and Unity containers, returning a single UserInitiated instance, while waiting for external issues beeing fixed

### Apizr.Integrations.MediatR

- [Workaround] Doc updated to work with MediatR alongside DryIoc or Unity container, while waiting for external issues beeing fixed
- [Fix] No more ```WithCrudMediation``` method available but only ```WithMediation```

1.4.0
---

### Apizr.Extensions.Microsoft.DependencyInjection

- [New] We can now auto register both crud and classic api interfaces

### Apizr.Integrations.MediatR

- [New] We can now use mediation with both crud and classic api interfaces
- [New] We can now use execution priority with both crud and classic api mediation
- [BreakingChange] ```WithCrudMediation``` renamed to ```WithMediation```

### Apizr.Integrations.Optional

- [New] We can now use optional mediation with both crud and classic api interfaces
- [New] We can now use execution priority with both crud and classic api optional mediation
- [BreakingChange] ```WithCrudOptionalMediation``` renamed to ```WithOptionalMediation```

### Apizr.Integrations.Shiny

- [New] Shiny integration now offers all the same registration extensions methods

### Apizr.Integrations.AutoMapper

- [New] We can now use auto mapping with both crud and classic api mediation and optional mediation 

1.3.0
---

### Apizr

- [New] We can now define mapped model entity type from the ```CrudEntityAttribute``` above api entities for automatic crud registration

### Apizr.Extensions.Microsoft.DependencyInjection

- [New] We can now provide an IMappingHandler implementation to the options builder for auto mapping
- [New] We can now decorate model entities with ```MappedCrudEntityAttribute``` to define mapped crud settings for automatic crud registration
- [New] We can now associate api and model entities with ```MappedEntity<TModelEntity, TApiEntity>``` during manual crud registration

### Apizr.Integrations.MediatR

- [Fix] Cacheable ReadQuery now use the key value when defining cache key
- [Fix] Auto handling now works as expected with manual crud registration

### Apizr.Integrations.Optional

- [Fix] Cacheable ReadOptionalQuery now use the key value when defining cache key

### Apizr.Integrations.AutoMapper

- [New] Brand new integration with AutoMapper, to let Apizr handle crud entity mapping during mediation handling

1.2.0
---

### Apizr

- [BreakingChange] Apizr instantiation/registration methods names standardized to Apizr.For and Apizr.CrudFor
- [New] Introducing ICrudApi service to manage standard CRUD api calls built-in

### Apizr.Extensions.Microsoft.DependencyInjection

- [BreakingChange] Apizr instantiation/registration methods names standardized to services.AddApizrFor and services.AddApizrCrudFor
- [New] Enabling ICrudApi auto registration feature with CrudEntityAttribute and assembly scanning

### Apizr.Integrations.Shiny

- [BreakingChange] Apizr instantiation/registration methods names standardized to services.UseApizrFor and services.UseApizrCrudFor
- [New] Enabling ICrudApi auto registration feature with CrudEntityAttribute and assembly scanning

### Apizr.Integrations.MediatR

- [New] Brand new integration with MediatR, to let Apizr handle crud requests execution with mediation

### Apizr.Integrations.Optional

- [New] Brand new integration with Optional, to let Apizr handle crud requests execution with mediation and optional result

1.1.0
---

### Apizr

- [New] Aibility to manage generic web apis by setting base address with the options builder

### Apizr.Extensions.Microsoft.DependencyInjection

- [New] Same as Apizr

### Apizr.Integrations.Shiny

- [New] Same as Apizr

1.0.0
---
Initial Release for
- Apizr
- Apizr.Extensions.Microsoft.DependencyInjection
- Apizr.Integrations.Akavache
- Apizr.Integrations.MonkeyCache
- Apizr.Integrations.Shiny