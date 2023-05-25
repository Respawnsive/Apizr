﻿# Apizr

Refit based web api client management, but resilient (retry, connectivity, cache, auth, log, priority, etc...)

[![Read - Documentation](https://img.shields.io/badge/read-documentation-blue?style=for-the-badge)](articles/index.md "Go to project documentation") [![Browse - Source](https://img.shields.io/badge/browse-source_code-lightgrey?style=for-the-badge)](https://github.com/Respawnsive/Apizr "Go to project repository")

## History

The Apizr project was motivated by this [2015 famous blog post](https://github.com/RobGibbens/ResilientServices/blob/master/post/post.md) about resilient networking.

Its main focus was to address at least everything explained into this article, meanning:

- Easy access to restful services
- Work offline with cache management
- Handle errors with retry pattern and global catching
- Handle request priority
- Check connectivity
- Fast development time
- Easy maintenance
- Reuse existing libraries

But also, some more core features like:

- Trace http traffic
- Handle authentication

And more integration/extension independent optional features like:

- Choose cache, log and connectivity providers
- Register it as an MS DI extension
- Integrate with Shiny
- Map model with DTO
- Use Mediator pattern
- Use Optional pattern

The list is not exhaustive, there’s more, but what we wanted was playing with all of it with as less code as we could, not worrying about plumbing things and being sure everything is wired and handled by design or almost.

Inspired by [Refit.Insane.PowerPack](https://github.com/thefex/Refit.Insane.PowerPack), we wanted to make it simple to use, mixing attribute decorations and fluent configuration.

Also, we built this lib to make it work with any .Net Standard 2.0 compliant platform, so we could use it seamlessly from any kind of app, with or without DI goodness.

## Libraries

[Change Log](https://respawnsive.github.io/Apizr/changelog.html)

|Project|Current|Upcoming|
|-------|-----|-----|
|Apizr|[![NuGet](https://img.shields.io/nuget/v/Apizr.svg)](https://www.nuget.org/packages/Apizr/)|[![NuGet Pre Release](https://img.shields.io/nuget/vpre/Apizr.svg)](https://www.nuget.org/packages/Apizr/)|
|Apizr.Extensions.Microsoft.DependencyInjection|[![NuGet](https://img.shields.io/nuget/v/Apizr.Extensions.Microsoft.DependencyInjection.svg)](https://www.nuget.org/packages/Apizr.Extensions.Microsoft.DependencyInjection/)|[![NuGet Pre Release](https://img.shields.io/nuget/vpre/Apizr.Extensions.Microsoft.DependencyInjection.svg)](https://www.nuget.org/packages/Apizr.Extensions.Microsoft.DependencyInjection/)|
|Apizr.Extensions.Microsoft.Caching|[![NuGet](https://img.shields.io/nuget/v/Apizr.Extensions.Microsoft.Caching.svg)](https://www.nuget.org/packages/Apizr.Extensions.Microsoft.Caching/)|[![NuGet Pre Release](https://img.shields.io/nuget/vpre/Apizr.Extensions.Microsoft.Caching.svg)](https://www.nuget.org/packages/Apizr.Extensions.Microsoft.Caching/)|
|Apizr.Integrations.Akavache|[![NuGet](https://img.shields.io/nuget/v/Apizr.Integrations.Akavache.svg)](https://www.nuget.org/packages/Apizr.Integrations.Akavache/)|[![NuGet Pre Release](https://img.shields.io/nuget/vpre/Apizr.Integrations.Akavache.svg)](https://www.nuget.org/packages/Apizr.Integrations.Akavache/)|
|Apizr.Integrations.MonkeyCache|[![NuGet](https://img.shields.io/nuget/v/Apizr.Integrations.MonkeyCache.svg)](https://www.nuget.org/packages/Apizr.Integrations.MonkeyCache/)|[![NuGet Pre Release](https://img.shields.io/nuget/vpre/Apizr.Integrations.MonkeyCache.svg)](https://www.nuget.org/packages/Apizr.Integrations.MonkeyCache/)|
|Apizr.Integrations.Fusillade|[![NuGet](https://img.shields.io/nuget/v/Apizr.Integrations.Fusillade.svg)](https://www.nuget.org/packages/Apizr.Integrations.Fusillade/)|[![NuGet Pre Release](https://img.shields.io/nuget/vpre/Apizr.Integrations.Fusillade.svg)](https://www.nuget.org/packages/Apizr.Integrations.Fusillade/)|
|Apizr.Integrations.MediatR|[![NuGet](https://img.shields.io/nuget/v/Apizr.Integrations.MediatR.svg)](https://www.nuget.org/packages/Apizr.Integrations.MediatR/)|[![NuGet Pre Release](https://img.shields.io/nuget/vpre/Apizr.Integrations.MediatR.svg)](https://www.nuget.org/packages/Apizr.Integrations.MediatR/)|
|Apizr.Integrations.Optional|[![NuGet](https://img.shields.io/nuget/v/Apizr.Integrations.Optional.svg)](https://www.nuget.org/packages/Apizr.Integrations.Optional/)|[![NuGet Pre Release](https://img.shields.io/nuget/vpre/Apizr.Integrations.Optional.svg)](https://www.nuget.org/packages/Apizr.Integrations.Optional/)|
|Apizr.Integrations.AutoMapper|[![NuGet](https://img.shields.io/nuget/v/Apizr.Integrations.AutoMapper.svg)](https://www.nuget.org/packages/Apizr.Integrations.AutoMapper/)|[![NuGet Pre Release](https://img.shields.io/nuget/vpre/Apizr.Integrations.AutoMapper.svg)](https://www.nuget.org/packages/Apizr.Integrations.AutoMapper/)|

Install the NuGet package of your choice:

   - **Apizr** package comes with the For and CrudFor static instantiation approach (which you can register in your DI container then)
   - **Apizr.Extensions.Microsoft.DependencyInjection** package extends your IServiceCollection with AddApizr, AddApizrFor and AddApizrCrudFor registration methods
   - **Apizr.Extensions.Microsoft.Caching** package brings an ICacheHandler method mapping implementation for [MS Extensions Caching](https://www.nuget.org/packages/Microsoft.Extensions.Caching.Abstractions)
   - **Apizr.Integrations.Akavache** package brings an ICacheHandler method mapping implementation for [Akavache](https://github.com/reactiveui/Akavache)
   - **Apizr.Integrations.MonkeyCache** package brings an ICacheHandler method mapping implementation for [MonkeyCache](https://github.com/jamesmontemagno/monkey-cache)
   - **Apizr.Integrations.Fusillade** package enables request priority management using [Fusillade](https://github.com/reactiveui/Fusillade)
   - **Apizr.Integrations.MediatR** package enables request auto handling with mediation using [MediatR](https://github.com/jbogard/MediatR)
   - **Apizr.Integrations.Optional** package enables Optional result from mediation requests (requires MediatR integration) using [Optional.Async](https://github.com/dnikolovv/optional-async)
   - **Apizr.Integrations.AutoMapper** package enables auto mapping for mediation requests (requires MediatR integration and could work with Optional integration) using [AutoMapper](https://github.com/AutoMapper/AutoMapper)

Apizr core package make use of well known nuget packages to make the magic appear:

|Package|Features|
|-------|--------|
|[Refit](https://github.com/reactiveui/refit)|Auto-implement web api interface and deal with HttpClient|
|[Polly](https://github.com/App-vNext/Polly)|Apply some policies like Retry, CircuitBreaker, etc...|
|[Microsoft.Extensions.Logging.Abstractions](https://github.com/BSiLabs/HttpTracer)|Delegate logging layer to MS Extensions Logging|

It also comes with some handling interfaces to let you provide your own services for:
- **Caching** with ICacheHandler, which comes with its default VoidCacheHandler (no cache), but also with:
  - InMemoryCacheHandler & DistributedCacheHandler: [MS Extensions Caching](https://www.nuget.org/packages/Microsoft.Extensions.Caching.Abstractions) methods mapping interface (Integration package referenced above), meaning you can provide any compatible caching engine
  - AkavacheCacheHandler: [Akavache](https://github.com/reactiveui/Akavache) methods mapping interface (Integration package referenced above)
  - MonkeyCacheHandler: [MonkeyCache](https://github.com/jamesmontemagno/monkey-cache) methods mapping interface (Integration package referenced above)
- **Logging** As Apizr relies on official [MS ILogger interface](https://www.nuget.org/packages/Microsoft.Extensions.Logging.Abstractions), you may want to provide any compatible logging engine (built-in DebugLogger activated by default)
- **Connectivity** with IConnectivityHandler, which comes with its default VoidConnectivityHandler (no connectivity check)
- **Mapping** with IMappingHandler, which comes with its default VoidMappingHandler (no mapping conversion), but also with:
  - AutoMapperMappingHandler: [AutoMapper](https://github.com/AutoMapper/AutoMapper) mapping methods mapping interface (Integration package referenced above)