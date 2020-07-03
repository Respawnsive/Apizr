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