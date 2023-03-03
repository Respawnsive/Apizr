## Configuring file transfer

Apizr could extend its core features with file transfer management thanks to a dedicated integration package.

Once installed, you'll be able to:

- Register upload, download or transfer (both) managers
- Upload files with dedicated methods
- Download files with dedicated methods
- Track transfer progress with a dedicated progress handler

### Installing

Please first install one of these integration packages, depending of your needs:

|Project|Registration|Current|Upcoming|
|-------|-----|-----|-----|
|Apizr.Integrations.FileTransfer|Static|[![NuGet](https://img.shields.io/nuget/v/Apizr.Integrations.FileTransfer.svg)](https://www.nuget.org/packages/Apizr.Integrations.FileTransfer/)|[![NuGet Pre Release](https://img.shields.io/nuget/vpre/Apizr.Integrations.FileTransfer.svg)](https://www.nuget.org/packages/Apizr.Integrations.FileTransfer/)|
|Apizr.Extensions.Microsoft.FileTransfer|MS Extensions|[![NuGet](https://img.shields.io/nuget/v/Apizr.Integrations.FileTransfer.svg)](https://www.nuget.org/packages/Apizr.Integrations.FileTransfer/)|[![NuGet Pre Release](https://img.shields.io/nuget/vpre/Apizr.Integrations.FileTransfer.svg)](https://www.nuget.org/packages/Apizr.Integrations.FileTransfer/)|
|Apizr.Integrations.FileTransfer.MediatR|MS Extensions with MediatR|[![NuGet](https://img.shields.io/nuget/v/Apizr.Integrations.FileTransfer.svg)](https://www.nuget.org/packages/Apizr.Integrations.FileTransfer/)|[![NuGet Pre Release](https://img.shields.io/nuget/vpre/Apizr.Integrations.FileTransfer.svg)](https://www.nuget.org/packages/Apizr.Integrations.FileTransfer/)|
|Apizr.Integrations.FileTransfer.Optional|MS Extensions with MediatR & Optional|[![NuGet](https://img.shields.io/nuget/v/Apizr.Integrations.FileTransfer.svg)](https://www.nuget.org/packages/Apizr.Integrations.FileTransfer/)|[![NuGet Pre Release](https://img.shields.io/nuget/vpre/Apizr.Integrations.FileTransfer.svg)](https://www.nuget.org/packages/Apizr.Integrations.FileTransfer/)|

### Designing

File Transfer package comes with some built-in apis son you don't have to create it yourself.

Here is what the provided apis look like then:

#### [Upload](#tab/tabid-upload)

```csharp
public interface IUploadApi : ITransferApiBase
{
    #region ByteArrayPart

    [Multipart]
    [Post("/")]
    Task UploadAsync(ByteArrayPart byteArrayPart);

    [Multipart]
    [Post("/{path}"), QueryUriFormat(UriFormat.Unescaped)]
    Task UploadAsync(ByteArrayPart byteArrayPart, string path);

    [Multipart]
    [Post("/")]
    Task UploadAsync(ByteArrayPart byteArrayPart, 
        [RequestOptions] IApizrRequestOptions options);

    [Multipart]
    [Post("/{path}"), QueryUriFormat(UriFormat.Unescaped)]
    Task UploadAsync(ByteArrayPart byteArrayPart, 
        string path, 
        [RequestOptions] IApizrRequestOptions options);

    #endregion

    #region StreamPart

    [Multipart]
    [Post("/")]
    Task UploadAsync(StreamPart streamPart);

    [Multipart]
    [Post("/{path}"), QueryUriFormat(UriFormat.Unescaped)]
    Task UploadAsync(StreamPart streamPart, string path);

    [Multipart]
    [Post("/")]
    Task UploadAsync(StreamPart streamPart, 
        [RequestOptions] IApizrRequestOptions options);

    [Multipart]
    [Post("/{path}"), QueryUriFormat(UriFormat.Unescaped)]
    Task UploadAsync(StreamPart streamPart, 
        string path, 
        [RequestOptions] IApizrRequestOptions options);

    #endregion

    #region FileInfoPart

    [Multipart]
    [Post("/")]
    Task UploadAsync(FileInfoPart fileInfoPart);

    [Multipart]
    [Post("/{filePath}"), QueryUriFormat(UriFormat.Unescaped)]
    Task UploadAsync(FileInfoPart fileInfoPart, 
        string filePath);

    [Multipart]
    [Post("/")]
    Task UploadAsync(FileInfoPart fileInfoPart, 
        [RequestOptions] IApizrRequestOptions options);

    [Multipart]
    [Post("/{filePath}"), QueryUriFormat(UriFormat.Unescaped)]
    Task UploadAsync(FileInfoPart fileInfoPart, 
        string filePath, 
        [RequestOptions] IApizrRequestOptions options); 

    #endregion
}
```

The Upload api offers you the choice between ByteArray, Stream or FileInfo sources.
`filePath` is an unesacped uri file path optionaly provided at request time, in case you want to use the same api for different uris.

#### [Download](#tab/tabid-download)

```csharp
public interface IDownloadApi<in TDownloadParams> : 
    ITransferApiBase
{
    [Get("/{filePathOrName}"), 
     QueryUriFormat(UriFormat.Unescaped), Cache(CacheMode.None)]
    Task<HttpResponseMessage> DownloadAsync(string filePathOrName);

    [Get("/{filePathOrName}"), 
     QueryUriFormat(UriFormat.Unescaped), Cache(CacheMode.None)]
    Task<HttpResponseMessage> DownloadAsync(string filePathOrName, 
        [RequestOptions] IApizrRequestOptions options);

    [Get("/{filePathOrName}"), 
     QueryUriFormat(UriFormat.Unescaped), Cache(CacheMode.None)]
    Task<HttpResponseMessage> DownloadAsync(string filePathOrName, 
        TDownloadParams downloadParams);

    [Get("/{filePathOrName}"), 
     QueryUriFormat(UriFormat.Unescaped), Cache(CacheMode.None)]
    Task<HttpResponseMessage> DownloadAsync(string filePathOrName, 
        TDownloadParams downloadParams, 
        [RequestOptions] IApizrRequestOptions options);
}

public interface IDownloadApi : 
    IDownloadApi<IDictionary<string, object>>
{
}
```

The Download api could be used with `IDictionary<string, object>` parameter type by default thanks to `IDownloadApi` or any provided custom type thanks to `IDownloadApi<TDownloadParams>`. If you don't need it you'll definitly be able to ignore it.
`filePathOrName` is an unesacped uri file path provided at request time, so you could use the same api for different uris.

#### [Transfer](#tab/tabid-transfer)

```csharp
public interface ITransferApi<in TDownloadParams> : 
    IDownloadApi<TDownloadParams>, IUploadApi { }
    
public interface ITransferApi : 
    ITransferApi<IDictionary<string, object>>, IDownloadApi { }
```

The Transfer api inherits from both the upload and the download one, in case you want to deal with the both of it from the same api.

***

- One may have only one transfer endpoint with no dynamic path to deal with, so there's nothing more to design here.
- Other may have several transfer endpoints or some dynamic paths to deal with:
  - feeling confortable with setting it at request time, so there's nothing more to design here.
  - preferring getting a dedicated api with preconfigured base uri, so he should:
    - create his own named and blank api interface
    - make sure to inherit from one of the above apis
    - define its base uri thanks to the `WebApi` attribute decoration

### Registering

Designing your custom transfer apis or using the built-in ones directly, you still have to register you apis.
Where you could register it as we used to do it with any other apis, FileTransfer package comes with some wrapping managers helping you to get things short and simple.

>[!NOTE]
>
>**Upload/Download/Transfer**
>
> Following exemples use the `Transfer` manager but you definitly can use the `Upload` or the `Download` ones insteed. 
>

#### Registering a single manager

#### [Static](#tab/tabid-static)

```csharp
// register the built-in transfer api
var transferManager = ApizrBuilder.Current.CreateTransferManager(
    options => options.WithBaseAddress("YOUR_API_BASE_ADDRESS_HERE"));

// OR register a custom transfer api
var transferManager = ApizrBuilder.Current.CreateTransferManagerFor<ITransferSampleApi>();
```

Here you go with your `Transfer` manager instance.

#### [Extended](#tab/tabid-extended)

```csharp
// register the built-in transfer api
services.AddApizrTransferManager(
    options => options.WithBaseAddress("YOUR_API_BASE_ADDRESS_HERE"));

// OR register a custom transfer api
services.AddApizrTransferManagerFor<ITransferSampleApi>();
```

Then, get your `Transfer` manager instance by resolving/injecting `IApizrTransferManager` for the built-in one or `IApizrTransferManager<TCustomApi>` for the custom one.

#### [MediatR](#tab/tabid-mediatr)

```csharp
// register the built-in transfer api
services.AddApizrTransferManager(
    options => options.WithBaseAddress("YOUR_API_BASE_ADDRESS_HERE")
                .WithFileTransferMediation());

// OR register a custom transfer api
services.AddApizrTransferManagerFor<ITransferSampleApi>(
    options => options.WithFileTransferMediation());
```

Then, get an Apizr mediator instance by resolving/injecting `IApizrMediator` to send some transfer requests.


#### [Optional](#tab/tabid-optional)

```csharp
// register the built-in transfer api
services.AddApizrTransferManager(
    options => options.WithBaseAddress("YOUR_API_BASE_ADDRESS_HERE")
                .WithFileTransferOptionalMediation());

// OR register a custom transfer api
services.AddApizrTransferManagerFor<ITransferSampleApi>(
    options => options.WithFileTransferOptionalMediation());
```

Then, get an Apizr optional mediator instance by resolving/injecting `IApizrOptionalMediator` to send some transfer requests returning optional results.

***

#### Registering multiple managers

#### [Static](#tab/tabid-static)

```csharp
var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
    // Built-in api
    .AddTransferManager(options => options
        .WithBaseAddress("YOUR_API_BASE_ADDRESS_HERE"))
    // Custom api
    .AddTransferManagerFor<ITransferSampleApi>());
```

Then, get your `Transfer` manager instance by calling:
```csharp
// for the built-in transfer api
var transferManager = apizrRegistry.GetTransferManager();

// OR for a custom transfer api
var transferManager = apizrRegistry.GetTransferManagerFor<ITransferSampleApi>();
```

#### [Extended](#tab/tabid-extended)

```csharp
services.AddApizr(registry => registry
    // Built-in api
    .AddTransferManager(options => options
        .WithBaseAddress("YOUR_API_BASE_ADDRESS_HERE"))
    // Custom api
    .AddTransferManagerFor<ITransferSampleApi>());
```

Then, get your `Transfer` manager instance directly by resolving/injecting `IApizrTransferManager` for the built-in one or `IApizrTransferManager<TCustomApi>` for the custom one.

You otherwise can resolve/inject `IApizrExtendedRegistry` to get the regisrty instance itself and then get your `Transfer` manager instance by calling:
```csharp
// for the built-in transfer api
var transferManager = apizrRegistry.GetTransferManager();

// OR for a custom transfer api
var transferManager = apizrRegistry.GetTransferManagerFor<ITransferSampleApi>();
```

#### [MediatR](#tab/tabid-mediatr)

```csharp
services.AddApizr(registry => registry
        // Built-in api
        .AddTransferManager(options => options
            .WithBaseAddress("YOUR_API_BASE_ADDRESS_HERE"))
        // Custom api
        .AddTransferManagerFor<ITransferSampleApi>(),
    config => config.WithFileTransferMediation());
```

Then, get an Apizr mediator instance by resolving/injecting `IApizrMediator` to send some transfer requests.

For more info about MediatR intergration, see [Configuring MediatR](config_mediatr.md).

#### [Optional](#tab/tabid-optional)

```csharp
services.AddApizr(registry => registry
        // Built-in api
        .AddTransferManager(options => options
            .WithBaseAddress("YOUR_API_BASE_ADDRESS_HERE"))
        // Custom api
        .AddTransferManagerFor<ITransferSampleApi>(),
    config => config.WithFileTransferOptionalMediation());
```

Then, get an Apizr optional mediator instance by resolving/injecting `IApizrOptionalMediator` to send some transfer requests returning optional results.

For more info about Optional.Async intergration, see [Configuring Optional.Async](config_optional.md).

***

You definitly can group registrations if needed like illustrated into the [Getting started](gettingstarted_classic.md).

Note that auto registration thanks to assembly scanning is not yet available for this package.

### Requesting

#### [Static](#tab/tabid-static)

Once you get an instance of your manager, here is how to play with it:
```csharp
var transferResult = await transferManager.DownloadAsync(
    new FileInfo("YOUR_FILE_FULL_NAME_HERE"));
```

Note that if you're in that case where you need to set a custom path dynamically at request time, here is how to do that:
```csharp
var transferResult = await transferManager.DownloadAsync(new FileInfo("YOUR_FILE_FULL_NAME_HERE"), 
    options => options.WithDynamicPath("YOUR_DYNAMIC_PATH_HERE"));
```

If you registered with the registry, some provided shortcut methods could help you to write things without the need of the manager.
You can call download or upload methods directly from the registry itself.
```csharp
// for the built-in transfer api
var transferResult = await registry.DownloadAsync(
    new FileInfo("YOUR_FILE_FULL_NAME_HERE"));

// OR for a custom transfer api
var transferResult = await registry.DownloadAsync<ITransferSampleApi>(
    new FileInfo("YOUR_FILE_FULL_NAME_HERE"));
```

#### [Extended](#tab/tabid-extended)

Once you get an instance of your manager by resolving/injecting `IApizrTransferManager` for the built-in one or `IApizrTransferManager<TCustomApi>` for the custom one, here is how to play with it:
```csharp
var transferResult = await transferManager.DownloadAsync(
    new FileInfo("YOUR_FILE_FULL_NAME_HERE"));
```

Note that if you're in that case where you need to set a custom path dynamically at request time, here is how to do that:
```csharp
var transferResult = await transferManager.DownloadAsync(new FileInfo("YOUR_FILE_FULL_NAME_HERE"), 
    options => options.WithDynamicPath("YOUR_DYNAMIC_PATH_HERE"));
```

If you registered with the registry, some provided shortcut methods could help you to write things without the need of the manager.
You can call download or upload methods directly from the registry itself.
```csharp
// for the built-in transfer api
var transferResult = await registry.DownloadAsync(
    new FileInfo("YOUR_FILE_FULL_NAME_HERE"));

// OR for a custom transfer api
var transferResult = await registry.DownloadAsync<ITransferSampleApi>(
    new FileInfo("YOUR_FILE_FULL_NAME_HERE"));
```

#### [MediatR](#tab/tabid-mediatr)

Once you get an Apizr mediator instance by resolving/injecting `IApizrMediator`, here is how to play with it:
```csharp
// for the built-in transfer api
var transferResult = await apizrMediator.SendDownloadQuery(
    new FileInfo("YOUR_FILE_FULL_NAME_HERE"));

// OR for a custom transfer api
var transferResult = await apizrMediator.SendDownloadQuery<ITransferSampleApi>(
    new FileInfo("YOUR_FILE_FULL_NAME_HERE"));
```

For more info about MediatR intergration, see [Configuring MediatR](config_mediatr.md).

#### [Optional](#tab/tabid-optional)

Once you get an Apizr optional mediator instance by resolving/injecting `IApizrOptionalMediator`, here is how to play with it:
```csharp
// for the built-in transfer api
var transferOptionalResult = await apizrOptionalMediator.SendDownloadOptionalQuery(
    new FileInfo("YOUR_FILE_FULL_NAME_HERE"));

// OR for a custom transfer api
var transferOptionalResult = await apizrOptionalMediator.SendDownloadOptionalQuery<ITransferSampleApi>(
    new FileInfo("YOUR_FILE_FULL_NAME_HERE"));
```

For more info about Optional.Async intergration, see [Configuring Optional.Async](config_optional.md).

***