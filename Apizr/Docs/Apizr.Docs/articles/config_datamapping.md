## Configuring data mapping

You may need to map data between some API types and Model types, known as DTO design pattern.
Apizr could handle it for you by providing an `IMappingHandler` interface implementation to it.
Fortunately, there's an integration Nuget package called Apizr.Integration.AutoMapper to integrate... AutoMapper obviously.
Of course, you can implement your own integration, but here we'll talk about the provided AutoMapper one.

Please first install this integration package:

|Project|Current|Upcoming|
|-------|-----|-----|
|Apizr.Integrations.AutoMapper|[![NuGet](https://img.shields.io/nuget/v/Apizr.Integrations.AutoMapper.svg)](https://www.nuget.org/packages/Apizr.Integrations.AutoMapper/)|[![NuGet Pre Release](https://img.shields.io/nuget/vpre/Apizr.Integrations.AutoMapper.svg)](https://www.nuget.org/packages/Apizr.Integrations.AutoMapper/)|


### Defining

As usually with AutoMapper, define your mapping profiles, like for example:
```csharp
public class UserMinUserProfile : Profile
{
    public UserMinUserProfile()
    {
        CreateMap<User, MinUser>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FirstName));
        CreateMap<MinUser, User>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Name));
    }
}
```

>[!WARNING]
>
>**Data Mapping with MediatR and/or Optional**
>
>If you plan to use MediatR and/or Optional integrations, one more defining step need to be done.

Only for those of you planning to use data mapping with **MediatR** and/or **Optional**, Apizr provide a `MappedWith` attribute telling it to map api object with model object.
You’ll find another `MappedCrudEntity` attribute dedicated to CRUD apis, coming with auto-registration capabilities, in case of access restricted to only local client model.
We could get a model class mapped to an api one like:

```csharp
[MappedWith(typeof(User))]
public class MinUser
{
    public int Id { get; set; }
    public string Name { get; set; }
}
```

### Registering

#### [Static](#tab/tabid-static)

First create a `MapperConfiguration` with your profiles:

```csharp
var mapperConfig = new MapperConfiguration(config =>
{
    config.AddProfile<UserMinUserProfile>();
    config.AddProfile<WhateverProfile>();
});
```

Then you'll be able to register with this option:

```csharp
// direct short configuration
options => options.WithAutoMapperMappingHandler(mapperConfig)

// OR direct configuration
options => options.WithMappingHandler(new AutoMapperMappingHandler(mapperConfig.CreateMapper()))

// OR factory configuration
options => options.WithMappingHandler(() => new AutoMapperMappingHandler(mapperConfig.CreateMapper()))
```

#### [Extended](#tab/tabid-extended)

First register AutoMapper as you used to do:
```csharp
services.AddAutoMapper(ASSEMBLIES_CONTAINING_PROFILES);
```

Then you'll be able to register with this option:

```csharp
// direct short configuration
options => options.WithAutoMapperMappingHandler()

// OR direct configuration
options => options.WithMappingHandler(new AutoMapperMappingHandler(YOUR_MAPPER_CONFIG))

// OR factory configuration
options => options.WithMappingHandler(() => new AutoMapperMappingHandler(YOUR_MAPPER_CONFIG))

// OR factory configuration with the service provider instance
options => options.WithMappingHandler(serviceProvider => new AutoMapperMappingHandler(YOUR_MAPPER_CONFIG))

// OR closed type configuration
options => options.WithMappingHandler<AutoMapperMappingHandler>()

// OR parameter type configuration
options => options.WithMappingHandler(typeof(AutoMapperMappingHandler))
```

***

### Using

You can tell Apizr to map data just by providing types when executing a request.

Something like:
```csharp
var result = await reqResManager.ExecuteAsync<MinUser, User>((api, user) => 
    api.CreateUser(user, CancellationToken.None), minUser);
```

Here we give a MinUser typed object to Apizr, which will be mapped to User type just before sending it.
Then Apizr will map the User typed result back to MinUser type just before returning it.

There are much more overloads so you can map objects the way you need. 
The same while using **MediatR** and/or **Optional**.