# Apizr
Refit based web api client, but resilient (retry, connectivity, cache, auth, log, priority, etc...)

You'll find a [blog post series here](https://www.respawnsive.com/category/blog-en/apizr/) about Apizr.

#### V-Next

This readme is all about v3 with related source code hosted by the master branch.
We're about to release v4 during 2022 Q1 with a lot of improvements and new features, plus some fixes.
Feel free to keep an eye on source code hosted by the dev branch, especially the test project and the samples, to get an idea about this expected v4 release (Readme still about v3 but ChangeLog up to date with v4).

## Libraries

Apizr v3+ introduced some breaking changes as it relies on Refit v6+ which actually introduced breaking changes and the fact that Fusillade has been moved from core package to a dedicated one.
Please take a look at the changelog and this updated readme.

[Change Log - Mar 12, 2021](changelog.md)

|Project|NuGet|
|-------|-----|
|Apizr|[![NuGet](https://img.shields.io/nuget/v/Apizr.svg)](https://www.nuget.org/packages/Apizr/)|
|Apizr.Extensions.Microsoft.DependencyInjection|[![NuGet](https://img.shields.io/nuget/v/Apizr.Extensions.Microsoft.DependencyInjection.svg)](https://www.nuget.org/packages/Apizr.Extensions.Microsoft.DependencyInjection/)|
|Apizr.Integrations.Shiny|[![NuGet](https://img.shields.io/nuget/v/Apizr.Integrations.Shiny.svg)](https://www.nuget.org/packages/Apizr.Integrations.Shiny/)|
|Apizr.Integrations.Fusillade|[![NuGet](https://img.shields.io/nuget/v/Apizr.Integrations.Fusillade.svg)](https://www.nuget.org/packages/Apizr.Integrations.Fusillade/)|
|Apizr.Integrations.Akavache|[![NuGet](https://img.shields.io/nuget/v/Apizr.Integrations.Akavache.svg)](https://www.nuget.org/packages/Apizr.Integrations.Akavache/)|
|Apizr.Integrations.MonkeyCache|[![NuGet](https://img.shields.io/nuget/v/Apizr.Integrations.MonkeyCache.svg)](https://www.nuget.org/packages/Apizr.Integrations.MonkeyCache/)|
|Apizr.Integrations.MediatR|[![NuGet](https://img.shields.io/nuget/v/Apizr.Integrations.MediatR.svg)](https://www.nuget.org/packages/Apizr.Integrations.MediatR/)|
|Apizr.Integrations.Optional|[![NuGet](https://img.shields.io/nuget/v/Apizr.Integrations.Optional.svg)](https://www.nuget.org/packages/Apizr.Integrations.Optional/)|
|Apizr.Integrations.AutoMapper|[![NuGet](https://img.shields.io/nuget/v/Apizr.Integrations.AutoMapper.svg)](https://www.nuget.org/packages/Apizr.Integrations.AutoMapper/)|

Install the NuGet package of your choice:

   - **Apizr** package comes with the For and CrudFor static instantiation approach (which you can register in your DI container then)
   - **Apizr.Extensions.Microsoft.DependencyInjection** package extends your IServiceCollection with AddApizrFor and AddApizrCrudFor registration methods (ASP.Net Core, etc)
   - **Apizr.Integrations.Shiny** package brings ICacheHandler, ILogHandler and IConnectivityHandler method mapping implementations for [Shiny](https://github.com/shinyorg/shiny), extending your IServiceCollection with a UseApizr and UseApizrCrudFor registration methods
   - **Apizr.Integrations.Fusillade** package enables request priority management using [Fusillade](https://github.com/reactiveui/Fusillade)
   - **Apizr.Integrations.Akavache** package brings an ICacheHandler method mapping implementation for [Akavache](https://github.com/reactiveui/Akavache)
   - **Apizr.Integrations.MonkeyCache** package brings an ICacheHandler method mapping implementation for [MonkeyCache](https://github.com/jamesmontemagno/monkey-cache)
   - **Apizr.Integrations.MediatR** package enables request auto handling with mediation using [MediatR](https://github.com/jbogard/MediatR)
   - **Apizr.Integrations.Optional** package enables Optional result from mediation requests (requires MediatR integration) using [Optional.Async](https://github.com/dnikolovv/optional-async)
   - **Apizr.Integrations.AutoMapper** package enables auto mapping for mediation requests (requires MediatR integration and could work with Optional integration) using [AutoMapper](https://github.com/AutoMapper/AutoMapper)

Apizr core package make use of well known nuget packages to make the magic appear:

|Package|Features|
|-------|--------|
|[Refit](https://github.com/reactiveui/refit)|Auto-implement web api interface and deal with HttpClient|
|[Polly](https://github.com/App-vNext/Polly)|Apply some policies like Retry, CircuitBreaker, etc...|
|[HttpTracer](https://github.com/BSiLabs/HttpTracer)|Trace Http(s) request/response traffic to log it|

It also comes with some handling interfaces to let you provide your own services for:
- **Caching** with ICacheHandler, which comes with its default VoidCacheHandler (no cache), but also with:
  - AkavacheCacheHandler: [Akavache](https://github.com/reactiveui/Akavache) method mapping interface (Integration package referenced above)
  - MonkeyCacheHandler: [MonkeyCache](https://github.com/jamesmontemagno/monkey-cache) method mapping interface (Integration package referenced above)
  - ShinyCacheHandler: [Shiny](https://github.com/shinyorg/shiny) chaching method mapping interface (Integration package referenced above)
- **Logging** with ILogHandler, which comes with its default DefaultLogHandler (Console and Debug), but also with:
  - ShinyLogHandler: [Shiny](https://github.com/shinyorg/shiny) logging method mapping interface (Integration package referenced above)
- **Connectivity** with IConnectivityHandler, which comes with its default VoidConnectivityHandler (no connectivity check), but also with:
  - ShinyConnectivityHandler: [Shiny](https://github.com/shinyorg/shiny) connectivity method mapping interface (Integration package referenced above)
- **Mapping** with IMappingHandler, which comes with its default VoidMappingHandler (no mapping conversion), but also with:
  - AutoMapperMappingHandler: [AutoMapper](https://github.com/AutoMapper/AutoMapper) mapping method mapping interface (Integration package referenced above)