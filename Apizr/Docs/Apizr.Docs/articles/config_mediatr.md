## Configuring MediatR

Apizr offers an integration with [MediatR](https://github.com/jbogard/MediatR), following the [Mediator pattern](https://en.wikipedia.org/wiki/Mediator_pattern), available only with the extended approach.
Mediator pattern ensures to keep all the thing as loosely coupled as we can between our ViewModel/ViewControler and our Data Access Layer. As everything should be loosely coupled between Views and ViewModels (MVVM) or ViewControlers (MVC) thanks to data binding, MediatR offers you to keep it all loosely coupled between your VM/VC and your DAL too.
Please read the [official documentation](https://github.com/jbogard/MediatR/wiki) to know more about MediatR.
The main benefit in using it with Apizr is to offer you a very simple and unified way to send your request, no matter from where or about what.
Simple and unified because instead of injecting/resolving each api interface you need to get your data, you just have to use the IMediator interface, everywhere, every time.

### Registering

Please first install this integration package:

|Project|Current|Upcoming|
|-------|-----|-----|
|Apizr.Integrations.MediatR|[![NuGet](https://img.shields.io/nuget/v/Apizr.Integrations.MediatR.svg)](https://www.nuget.org/packages/Apizr.Integrations.MediatR/)|[![NuGet Pre Release](https://img.shields.io/nuget/vpre/Apizr.Integrations.MediatR.svg)](https://www.nuget.org/packages/Apizr.Integrations.MediatR/)|

Then you'll be able to register with this option:

```csharp
options => options.WithMediation()
```

And don't forget to register MediatR itself as usual:
```csharp
services.AddMediatR(YOUR_REQUESTHANDLER_ASSEMBLIES);
```

Where `YOUR_REQUESTHANDLER_ASSEMBLIES` should be the assemblies containing your custom request handlers, if you get some (Apizr MediatR request handlers will be auto registered).

### Using

>[!NOTE]
>
>**Sending the safe way**
>
>We are sometime talking about Safe request meaning that Refit will handle exceptions and return an `IApiResponse` to Apizr and then Apizr will return it as an `IApizrResponse` without throwing. Please read the exception handling doc to get more info.

#### [`IMediator`](#tab/tabid-imediator)

When registered, you don't have to inject/resolve anything else than `IMediator`, in order to play with your api services (both classic and crud). 
Everything you need to do then, is sending your request by calling:
```csharp
var result = await _mediator.Send(YOUR_REQUEST_HERE);
```

Where `YOUR_REQUEST_HERE` could be:

Classic API:
 - With no result:
   - `ExecuteUnitRequest<TWebApi>`: execute any method from `TWebApi`
   - `ExecuteSafeUnitRequest<TWebApi>`: execute any method from `TWebApi`, the safe way with `IApizrResponse` handling
   - `ExecuteUnitRequest<TWebApi, TModelData, TApiData>`: execute any method from `TWebApi` with `TModelData` mapped with `TApiData`
   - `ExecuteSafeUnitRequest<TWebApi, TModelData, TApiData>`: execute any method from `TWebApi` with `TModelData` mapped with `TApiData`, the safe way with `IApizrResponse` handling
 - With result:
   - `ExecuteResultRequest<TWebApi, TApiData>`: execute any method from `TWebApi` with a `TApiData` request/result data
   - `ExecuteSafeResultRequest<TWebApi, TApiData>`: execute any method from `TWebApi` with a `TApiData` request/result data, the safe way with `IApizrResponse<TApiData>` handling
   - `ExecuteResultRequest<TWebApi, TModelData, TApiData>`: execute any method from `TWebApi` with `TModelData` request/result data mapped with `TApiData`
   - `ExecuteSafeResultRequest<TWebApi, TModelData, TApiData>`: execute any method from `TWebApi` with `TModelData` request/result data mapped with `TApiData`, the safe way with `IApizrResponse<TModelData>` handling
   - `ExecuteResultRequest<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>`: execute any method from `TWebApi`, sending `TApiRequestData` mapped from `TModelRequestData`, then returning `TModelResultData` mapped from `TApiResultData`
   - `ExecuteSafeResultRequest<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>`: execute any method from `TWebApi`, sending `TApiRequestData` mapped from `TModelRequestData`, then returning `TModelResultData` mapped from `TApiResultData`, the safe way with `IApizrResponse<TModelResultData>` handling

CRUD API:
 - Read:
   - `ReadQuery<TResultData>`: get the `TResultData` entity matching an int key
   - `SafeReadQuery<TResultData>`: get the `TResultData` entity matching an int key, the safe way with `IApizrResponse<TApiData>` handling
   - `ReadQuery<TResultData, TKey>`: get the `TResultData` entity matching a `TKey` 
   - `SafeReadQuery<TResultData, TKey>`: get the `TResultData` entity matching a `TKey` , the safe way with `IApizrResponse<TApiData>` handling
 - ReadAll:
   - `ReadAllQuery<TReadAllResult>`: get `TReadAllResult` with `IDictionary<string, object>` optional query parameters
   - `SafeReadAllQuery<TReadAllResult>`: get `TReadAllResult` with `IDictionary<string, object>` optional query parameters, the safe way with `IApizrResponse<TReadAllResult>` handling
   - `ReadAllQuery<TReadAllParams, TReadAllResult>`: get `TReadAllResult` with `TReadAllParams` optional query parameters
   - `SafeReadAllQuery<TReadAllParams, TReadAllResult>`: get `TReadAllResult` with `TReadAllParams` optional query parameters, the safe way with `IApizrResponse<TReadAllResult>` handling
 - Create:
   - `CreateCommand<TModelData>`: create a `TModelData` entity
   - `SafeCreateCommand<TModelData>`: create a `TModelData` entity, the safe way with `IApizrResponse<TModelData>` handling
 - Update:
   - `UpdateCommand<TRequestData>`: update the `TRequestData` entity matching an int key
   - `SafeUpdateCommand<TRequestData>`: update the `TRequestData` entity matching an int key, the safe way with `IApizrResponse` handling
   - `UpdateCommand<TKey, TRequestData>`: update the `TRequestData` entity matching a `TKey`
   - `SafeUpdateCommand<TKey, TRequestData>`: update the `TRequestData` entity matching a `TKey`, the safe way with `IApizrResponse` handling
 - Delete:
   - `DeleteCommand<T>`: delete the `T` entity matching an int key
   - `SafeDeleteCommand<T>`: delete the `T` entity matching an int key, the safe way with `IApizrResponse` handling
   - `DeleteCommand<T, TKey>`: delete the `T` entity matching a `TKey`
   - `SafeDeleteCommand<T, TKey>`: delete the `T` entity matching a `TKey`, the safe way with `IApizrResponse` handling

#### [`IApizrMediator`](#tab/tabid-iapizrmediator)

Writting things shorter, instead of injecting/resolving `IMediator`, you could do it with `IApizrMediator` or `IApizrCrudMediator`. 
Everything you need to do then, is sending your request by calling something like:
```csharp
// Classic
var result = await _apizrMediator.SendFor<TWebApi>(YOUR_API_METHOD_HERE);

// OR CRUD
var result = await _apizrCrudMediator.SendReadAllQuery<TReadAllResult>();
```

Classic mediator methods:
 - No result:
   - `SendFor<TWebApi>`: execute any method from `TWebApi`
   - `SendFor<TWebApi, TModelData, TApiData>`: execute any method from `TWebApi` with `TModelData` mapped with `TApiData`
 - With result:
   - `SendFor<TWebApi, TApiData>`: execute any method from `TWebApi` with a `TApiData` request/result data
   - `SendFor<TWebApi, TModelData, TApiData>`: execute any method from `TWebApi` with `TModelData` request/result data mapped with `TApiData`
   - `SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>`: execute any method from `TWebApi`, sending `TApiRequestData` mapped from `TModelRequestData`, then returning `TModelResultData` mapped from `TApiResultData`

>[!TIP]
>
>**Sending the safe way**
>
>Note that if your api returns an `IApiResponse<T>`, Apizr mediator will send a safe request so you can handle an `IApizrResponse<T>` back from it.

CRUD mediator methods:
 - Read:
   - `SendReadQuery<TApiEntity, TApiEntityKey>`: get the `TApiEntity` matching a `TApiEntityKey`
   - `SendSafeReadQuery<TApiEntity, TApiEntityKey>`: get the `TApiEntity` matching a `TApiEntityKey`, the safe way with `IApizrResponse<TApiEntity>` handling
   - `SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>`: get the `TModelEntity` mapped from `TApiEntity` and matching a `TApiEntityKey`
   - `SendSafeReadQuery<TModelEntity, TApiEntity, TApiEntityKey>`: get the `TModelEntity` mapped from `TApiEntity` and matching a `TApiEntityKey`, the safe way with `IApizrResponse<TModelEntity>` handling
 - ReadAll:
   - `SendReadAllQuery<TReadAllResult>`: get `TReadAllResult` with `IDictionary<string, object>` optional query parameters
   - `SendSafeReadAllQuery<TReadAllResult>`: get `TReadAllResult` with `IDictionary<string, object>` optional query parameters, the safe way with `IApizrResponse<TReadAllResult>` handling
   - `SendReadAllQuery<TModelReadAllResult, TApiReadAllResult>`: get `TModelReadAllResult` mapped from `TApiReadAllResult`
   - `SendSafeReadAllQuery<TModelReadAllResult, TApiReadAllResult>`: get `TModelReadAllResult` mapped from `TApiReadAllResult`, the safe way with `IApizrResponse<TModelReadAllResult>` handling
   - `SendReadAllQuery<TReadAllResult, TReadAllParams>`: get `TReadAllResult` with `TReadAllParams` optional query parameters
   - `SendSafeReadAllQuery<TReadAllResult, TReadAllParams>`: get `TReadAllResult` with `TReadAllParams` optional query parameters, the safe way with `IApizrResponse<TReadAllResult>` handling
   - `SendReadAllQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>`: get `TModelReadAllResult` mapped from `TApiReadAllResult` with `TReadAllParams` optional query parameters
   - `SendSafeReadAllQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>`: get `TModelReadAllResult` mapped from `TApiReadAllResult` with `TReadAllParams` optional query parameters, the safe way with `IApizrResponse<TModelReadAllResult>` handling
 - Create:
   - `SendCreateCommand<TApiEntity>`: create a `TApiEntity`
   - `SendSafeCreateCommand<TApiEntity>`: create a `TApiEntity`, the safe way with `IApizrResponse<TApiEntity>` handling
   - `SendCreateCommand<TModelEntity, TApiEntity>`: create a `TApiEntity` mapped from `TModelEntity`
   - `SendSafeCreateCommand<TModelEntity, TApiEntity>`: create a `TApiEntity` mapped from `TModelEntity`, the safe way with `IApizrResponse<TModelEntity>` handling
 - Update:
   - `SendUpdateCommand<TApiEntity, TApiEntityKey>`: update the `TApiEntity` entity matching a `TApiEntityKey`
   - `SendSafeUpdateCommand<TApiEntity, TApiEntityKey>`: update the `TApiEntity` entity matching a `TApiEntityKey`, the safe way with `IApizrResponse` handling
   - `SendUpdateCommand<TModelEntity, TApiEntity, TApiEntityKey>`: update the `TApiEntity` mapped from `TModelEntity` and matching a `TApiEntityKey`
   - `SendSafeUpdateCommand<TModelEntity, TApiEntity, TApiEntityKey>`: update the `TApiEntity` mapped from `TModelEntity` and matching a `TApiEntityKey`, the safe way with `IApizrResponse` handling
 - Delete:
   - `SendDeleteCommand<TApiEntity, TApiEntityKey>`: delete the `TApiEntity` matching a `TApiEntityKey`
   - `SendSafeDeleteCommand<TApiEntity, TApiEntityKey>`: delete the `TApiEntity` matching a `TApiEntityKey`, the safe way with `IApizrResponse` handling

#### [`IApizrMediator<TWebApi>`](#tab/tabid-iapizrmediator-twebapi)

Writting things shorter than ever, instead of injecting/resolving `IMediator`, `IApizrMediator` or `IApizrCrudMediator`, you could do it with `IApizrMediator<TWebApi>` or `IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>`. 

>[!TIP]
>
>**Apizr Registry**
>
>Note that if you were using the registry feature while configuring, you'll be able to inject/resolve `IApizrMediationRegistry` instead and then just get an `IApizrMediator<TWebApi>` or `IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>` thanks to its `Get` methods.

Everything you need to do then, is sending your request by calling something like:
```csharp
// Classic
var result = await _apizrMediator.SendFor(YOUR_API_METHOD_HERE);

// OR CRUD
var result = await _apizrCrudMediator.SendReadAllQuery();
```

Classic typed mediator methods:
- No result:
  - `SendFor`: execute any method from `TWebApi`
  - `SendFor<TModelData, TApiData>`: execute any method from `TWebApi` with `TModelData` mapped with `TApiData`
- With result:
  - `SendFor<TApiData>`: execute any method from `TWebApi` with a `TApiData` request/result data
  - `SendFor<TModelData, TApiData>`: execute any method from `TWebApi` with `TModelData` request/result data mapped with `TApiData`
  - `SendFor<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>`: execute any method from `TWebApi`, sending `TApiRequestData` mapped from `TModelRequestData`, then returning `TModelResultData` mapped from `TApiResultData`

>[!TIP]
>
>**Sending the safe way**
>
>Note that if your api returns an `IApiResponse<T>`, Apizr mediator will send a safe request so you can handle an `IApizrResponse<T>` back from it.

CRUD typed mediator methods:
- Read:
  - `SendReadQuery`: get the `TApiEntity` matching a `TApiEntityKey`
  - `SendSafeReadQuery`: get the `TApiEntity` matching a `TApiEntityKey`, the safe way with `IApizrResponse<TApiEntity>` handling
  - `SendReadQuery<TModelEntity>`: get the `TModelEntity` mapped from `TApiEntity` and matching a `TApiEntityKey`
  - `SendSafeReadQuery<TModelEntity>`: get the `TModelEntity` mapped from `TApiEntity` and matching a `TApiEntityKey`, the safe way with `IApizrResponse<TModelEntity>` handling
- ReadAll:
  - `SendReadAllQuery`: get `TReadAllResult` with `TReadAllParams` optional query parameters
  - `SendSafeReadAllQuery`: get `TReadAllResult` with `TReadAllParams` optional query parameters, the safe way with `IApizrResponse<TReadAllResult>` handling
  - `SendReadAllQuery<TModelReadAllResult>`: get `TModelReadAllResult` mapped from `TApiReadAllResult` with `TReadAllParams` optional query parameters
  - `SendSafeReadAllQuery<TModelReadAllResult>`: get `TModelReadAllResult` mapped from `TApiReadAllResult` with `TReadAllParams` optional query parameters, the safe way with `IApizrResponse<TModelReadAllResult>` handling
- Create:
  - `SendCreateCommand`: create a `TApiEntity`
  - `SendSafeCreateCommand`: create a `TApiEntity`, the safe way with `IApizrResponse<TApiEntity>` handling
  - `SendCreateCommand<TModelEntity>`: create a `TApiEntity` mapped from `TModelEntity`
  - `SendSafeCreateCommand<TModelEntity>`: create a `TApiEntity` mapped from `TModelEntity`, the safe way with `IApizrResponse<TModelEntity>` handling
- Update:
  - `SendUpdateCommand`: update the `TApiEntity` entity matching a `TApiEntityKey`
  - `SendSafeUpdateCommand`: update the `TApiEntity` entity matching a `TApiEntityKey`, the safe way with `IApizrResponse` handling
  - `SendUpdateCommand<TModelEntity>`: update the `TApiEntity` mapped from `TModelEntity` and matching a `TApiEntityKey`
  - `SendSafeUpdateCommand<TModelEntity>`: update the `TApiEntity` mapped from `TModelEntity` and matching a `TApiEntityKey`, the safe way with `IApizrResponse` handling
- Delete:
  - `SendDeleteCommand`: delete the `TApiEntity` matching a `TApiEntityKey`
  - `SendSafeDeleteCommand`: delete the `TApiEntity` matching a `TApiEntityKey`, the safe way with `IApizrResponse` handling

***