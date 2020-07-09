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