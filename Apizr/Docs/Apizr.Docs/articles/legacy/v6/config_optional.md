## Configuring Optional.Async

>[!WARNING]
> **Deprecated**
> 
> Both Apizr.Integrations.Optional and Apizr.Integrations.FileTransfer.Optional packages have been deprecated as Optional.Async reference package is no longer maintained.

Apizr offers an integration with [Optional.Async](https://github.com/dnikolovv/optional-async), following the [Optional pattern](https://github.com/nlkl/Optional), available only with the extended approach with MediatR integration activated.
Optional.Async offers a strongly typed alternative to null values that lets you:
- Avoid those pesky null-reference exceptions
- Signal intent and model your data more explicitly
- Cut down on manual null checks and focus on your domain
- It allows you to chain `Task<Option<T>>` and `Task<Option<T, TException>>` without having to use await

### Registering

Please first install this integration package:

|Project|Current|Upcoming|
|-------|-----|-----|
|Apizr.Integrations.Optional|[![NuGet](https://img.shields.io/nuget/v/Apizr.Integrations.Optional.svg)](https://www.nuget.org/packages/Apizr.Integrations.Optional/)|[![NuGet Pre Release](https://img.shields.io/nuget/vpre/Apizr.Integrations.Optional.svg)](https://www.nuget.org/packages/Apizr.Integrations.Optional/)|

Then you'll be able to register with this option:

```csharp
options => options.WithOptionalMediation()
```

And don't forget to register MediatR itself as usual:
```csharp
services.AddMediatR(YOUR_REQUESTHANDLER_ASSEMBLIES);
```

Where `YOUR_REQUESTHANDLER_ASSEMBLIES` should be the assemblies containing your custom request handlers, if you get some (Apizr MediatR request handlers will be auto registered).

### Using

#### [`IMediator`](#tab/tabid-imediator)

When registered, you don't have to inject/resolve anything else than `IMediator`, in order to play with your api services (both classic and crud). 
Everything you need to do then, is sending your request by calling:
```csharp
var optionalResult = await _mediator.Send(YOUR_REQUEST_HERE);
```

Where `YOUR_REQUEST_HERE` could be:

Classic API:
 - With no result:
   - `ExecuteOptionalUnitRequest<TWebApi>`: execute any method from `TWebApi`
   - `ExecuteOptionalUnitRequest<TWebApi, TModelData, TApiData>`: execute any method from `TWebApi` with `TModelData` mapped with `TApiData`
 - With result:
   - `ExecuteOptionalResultRequest<TWebApi, TApiData>`: execute any method from `TWebApi` with a `TApiData` request/result data
   - `ExecuteOptionalResultRequest<TWebApi, TModelData, TApiData>`: execute any method from `TWebApi` with `TModelData` request/result data mapped with `TApiData`
   - `ExecuteOptionalResultRequest<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>`: execute any method from `TWebApi`, sending `TApiRequestData` mapped from `TModelRequestData`, then returning `TModelResultData` mapped from `TApiResultData`

CRUD API:
 - Read:
   - `ReadOptionalQuery<TResultData>`: get the `TResultData` entity matching an int key
   - `ReadOptionalQuery<TResultData, TKey>`: get the `TResultData` entity matching a `TKey` 
 - ReadAll:
   - `ReadAllOptionalQuery<TReadAllResult>`: get `TReadAllResult` with `IDictionary<string, object>` optional query parameters
   - `ReadAllOptionalQuery<TReadAllParams, TReadAllResult>`: get `TReadAllResult` with `TReadAllParams` optional query parameters
 - Create:
   - `CreateOptionalCommand<TModelData>`: create a `TModelData` entity
 - Update:
   - `UpdateOptionalCommand<TRequestData>`: update the `TRequestData` entity matching an int key
   - `UpdateOptionalCommand<TKey, TRequestData>`: update the `TRequestData` entity matching a `TKey`
 - Delete:
   - `DeleteOptionalCommand<T>`: delete the `T` entity matching an int key
   - `DeleteOptionalCommand<T, TKey>`: delete the `T` entity matching a `TKey`

#### [`IApizrOptionalMediator`](#tab/tabid-iapizroptionalmediator)

Writting things shorter, instead of injecting/resolving `IMediator`, you could do it with `IApizrMediator` or `IApizrCrudMediator`. 
Everything you need to do then, is sending your request by calling something like:
```csharp
// Classic
var optionalResult = await _apizrOptionalMediator.SendFor<TWebApi>(YOUR_API_METHOD_HERE);

// OR CRUD
var optionalResult = await _apizrOptionalCrudMediator.SendReadAllQuery<TReadAllResult>();
```

Classic Optional mediator methods:
 - No result:
   - `SendFor<TWebApi>`: execute any method from `TWebApi`
   - `SendFor<TWebApi, TModelData, TApiData>`: execute any method from `TWebApi` with `TModelData` mapped with `TApiData`
 - With result:
   - `SendFor<TWebApi, TApiData>`: execute any method from `TWebApi` with a `TApiData` request/result data
   - `SendFor<TWebApi, TModelData, TApiData>`: execute any method from `TWebApi` with `TModelData` request/result data mapped with `TApiData`
   - `SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>`: execute any method from `TWebApi`, sending `TApiRequestData` mapped from `TModelRequestData`, then returning `TModelResultData` mapped from `TApiResultData`

CRUD Optional mediator methods:
 - Read:
   - `SendReadQuery<TApiEntity, TApiEntityKey>`: get the `TApiEntity` matching a `TApiEntityKey`
   - `SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>`: get the `TModelEntity` mapped from `TApiEntity` and matching a `TApiEntityKey`
 - ReadAll:
   - `SendReadAllQuery<TReadAllResult>`: get `TReadAllResult` with `IDictionary<string, object>` optional query parameters
   - `SendReadAllQuery<TModelReadAllResult, TApiReadAllResult>`: get `TModelReadAllResult` mapped from `TApiReadAllResult`
   - `SendReadAllQuery<TReadAllResult, TReadAllParams>`: get `TReadAllResult` with `TReadAllParams` optional query parameters
   - `SendReadAllQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>`: get `TModelReadAllResult` mapped from `TApiReadAllResult` with `TReadAllParams` optional query parameters
 - Create:
   - `SendCreateCommand<TApiEntity>`: create a `TApiEntity`
   - `SendCreateCommand<TModelEntity, TApiEntity>`: create a `TApiEntity` mapped from `TModelEntity`
 - Update:
   - `SendUpdateCommand<TApiEntity, TApiEntityKey>`: update the `TApiEntity` entity matching a `TApiEntityKey`
   - `SendUpdateCommand<TModelEntity, TApiEntity, TApiEntityKey>`: update the `TApiEntity` mapped from `TModelEntity` and matching a `TApiEntityKey`
 - Delete:
   - `SendDeleteCommand<TApiEntity, TApiEntityKey>`: delete the `TApiEntity` matching a `TApiEntityKey`

#### [`IApizrOptionalMediator<TWebApi>`](#tab/tabid-iapizrptionalmediator-twebapi)

Writting things shorter than ever, instead of injecting/resolving `IMediator`, `IApizrOptionalMediator` or `IApizrCrudOptionalMediator`, you could do it with `IApizrOptionalMediator<TWebApi>` or `IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>`. 

>[!TIP]
>
>**Apizr Registry**
>
>Note that if you were using the registry feature while configuring, you'll be able to inject/resolve `IApizrOptionalMediationRegistry` instead and then just get an `IApizrOptionalMediator<TWebApi>` or `IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>` thanks to its `GetFor` and `GetCrudFor` methods.

Everything you need to do then, is sending your request by calling something like:
```csharp
// Classic
var optionalResult = await _apizrOptionalMediator.SendFor(YOUR_API_METHOD_HERE);

// OR CRUD
var optionalResult = await _apizrCrudOptionalMediator.SendReadAllQuery();
```

Classic Optional typed mediator methods:
- No result:
  - `SendFor`: execute any method from `TWebApi`
  - `SendFor<TModelData, TApiData>`: execute any method from `TWebApi` with `TModelData` mapped with `TApiData`
- With result:
  - `SendFor<TApiData>`: execute any method from `TWebApi` with a `TApiData` request/result data
  - `SendFor<TModelData, TApiData>`: execute any method from `TWebApi` with `TModelData` request/result data mapped with `TApiData`
  - `SendFor<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>`: execute any method from `TWebApi`, sending `TApiRequestData` mapped from `TModelRequestData`, then returning `TModelResultData` mapped from `TApiResultData`

CRUD typed mediator methods:
- Read:
  - `SendReadQuery`: get the `TApiEntity` matching a `TApiEntityKey`
  - `SendReadQuery<TModelEntity>`: get the `TModelEntity` mapped from `TApiEntity` and matching a `TApiEntityKey`
- ReadAll:
  - `SendReadAllQuery`: get `TReadAllResult` with `TReadAllParams` optional query parameters
  - `SendReadAllQuery<TModelReadAllResult>`: get `TModelReadAllResult` mapped from `TApiReadAllResult` with `TReadAllParams` optional query parameters
- Create:
  - `SendCreateCommand`: create a `TApiEntity`
  - `SendCreateCommand<TModelEntity>`: create a `TApiEntity` mapped from `TModelEntity`
- Update:
  - `SendUpdateCommand`: update the `TApiEntity` entity matching a `TApiEntityKey`
  - `SendUpdateCommand<TModelEntity>`: update the `TApiEntity` mapped from `TModelEntity` and matching a `TApiEntityKey`
- Delete:
  - `SendDeleteCommand`: delete the `TApiEntity` matching a `TApiEntityKey`

***

You should finaly end with something like:
```csharp
optionalResult.Match(result =>
{
    // Oh yeah, you get a result!
}, e =>
{
    // Oh no, something went wrong!
});
```