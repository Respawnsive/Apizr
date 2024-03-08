## Configuring data mapping

You may need to map data between some API types and Model types, known as DTO design pattern.
Apizr could handle it for you by providing an `IMappingHandler` interface implementation to it.
Fortunately, there are some integration Nuget packages to do so.
Of course, you can implement your own integration, but here we'll talk about the provided ones.

Please first install this integration package of your choice:

|Project|Current|Upcoming|
|-------|-----|-----|
|Apizr.Integrations.AutoMapper|[![NuGet](https://img.shields.io/nuget/v/Apizr.Integrations.AutoMapper.svg)](https://www.nuget.org/packages/Apizr.Integrations.AutoMapper/)|[![NuGet Pre Release](https://img.shields.io/nuget/vpre/Apizr.Integrations.AutoMapper.svg)](https://www.nuget.org/packages/Apizr.Integrations.AutoMapper/)|
|Apizr.Integrations.Mapster|[![NuGet](https://img.shields.io/nuget/v/Apizr.Integrations.Mapster.svg)](https://www.nuget.org/packages/Apizr.Integrations.Mapster/)|[![NuGet Pre Release](https://img.shields.io/nuget/vpre/Apizr.Integrations.Mapster.svg)](https://www.nuget.org/packages/Apizr.Integrations.Mapster/)|

Where:
   - **Apizr.Integrations.AutoMapper** package brings an `IMappingHandler` implementation for [AutoMapper](https://github.com/AutoMapper/AutoMapper)
   - **Apizr.Integrations.Mapster** package brings an `IMappingHandler` implementation for [Mapster](https://github.com/MapsterMapper/Mapster)

### Defining

#### AutoMapper

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

#### Mapster

No need to write your own DTO classes. Mapster provides Mapster.Tool to help you generating models. And if you would like to have explicit mapping, Mapster also generates mapper class for you.

````csharp
[AdaptTo("[name]Dto"), GenerateMapper]
public class Student {
    ...
}
````

Then Mapster will generate:

````csharp
public class StudentDto {
    ...
}
public static class StudentMapper {
    public static StudentDto AdaptToDto(this Student poco) { ... }
    public static StudentDto AdaptTo(this Student poco, StudentDto dto) { ... }
    public static Expression<Func<Student, StudentDto>> ProjectToDto => ...
}
````

But you can also write your own mapping configuration, like for example:
```csharp
TypeAdapterConfig<TSource, TDestination>
    .NewConfig()
    .Ignore(dest => dest.Age)
    .Map(dest => dest.FullName,
        src => string.Format("{0} {1}", src.FirstName, src.LastName));
```

#### Advanced

>[!WARNING]
>
>**Data Mapping with MediatR and/or Optional**
>
>If you plan to use MediatR and/or Optional integrations, one more defining step has to be done.

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

#### AutoMapper

##### [Static](#tab/tabid-static)

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

##### [Extended](#tab/tabid-extended)

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

#### Mapster

##### [Static](#tab/tabid-static)

Register with one of the following options:

```csharp
// direct short configuration
options => options.WithMapsterMappingHandler(new Mapper())

// OR direct configuration
options => options.WithMappingHandler(new MapsterMappingHandler(new Mapper()))

// OR factory configuration
options => options.WithMappingHandler(() => new MapsterMappingHandler(new Mapper()))
```

##### [Extended](#tab/tabid-extended)

First register Mapster as you used to do:
```csharp
var config = new TypeAdapterConfig();
// Or
// var config = TypeAdapterConfig.GlobalSettings;
services.AddSingleton(config);
services.AddScoped<IMapper, ServiceMapper>();
```

Then you'll be able to register with this option:

```csharp
// direct short configuration
options => options.WithMapsterMappingHandler()

// OR closed type configuration
options => options.WithMappingHandler<MapsterMappingHandler>()

// OR parameter type configuration
options => options.WithMappingHandler(typeof(MapsterMappingHandler))
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