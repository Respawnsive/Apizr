using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Apizr.Extending;
using Apizr.Extending.Configuring;
using Apizr.Extending.Configuring.Common;
using Apizr.Mapping;
using Apizr.Optional.Configuring.Registry;
using Apizr.Optional.Cruding;
using Apizr.Optional.Cruding.Handling;
using Apizr.Optional.Cruding.Sending;
using Apizr.Optional.Requesting;
using Apizr.Optional.Requesting.Handling;
using Apizr.Optional.Requesting.Sending;
using MediatR;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Optional;
using Refit;

[assembly: Apizr.Preserve]
namespace Apizr
{
    public static class ApizrExtendedOptionsBuilderExtensions
    {
        /// <summary>
        /// Let Apizr handle requests execution with some mediation and optional result
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <returns></returns>
        public static IApizrExtendedCommonOptionsBuilder WithOptionalMediation(this IApizrExtendedCommonOptionsBuilder optionsBuilder)
        {
            WithMediation(optionsBuilder.ApizrOptions);

            return optionsBuilder;
        }

        /// <summary>
        /// Let Apizr handle requests execution with some mediation and optional result
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <returns></returns>
        public static IApizrExtendedOptionsBuilder WithOptionalMediation(
            this IApizrExtendedOptionsBuilder optionsBuilder)
        {
            WithMediation(optionsBuilder.ApizrOptions);

            return optionsBuilder;
        }

        private static void WithMediation(IApizrExtendedCommonOptions apizrOptions)
        {
            apizrOptions.PostRegistrationActions.Add((webApiType, services) =>
            {
                #region Crud

                // Register crud optional mediator
                services.TryAddSingleton<IApizrCrudOptionalMediator, ApizrCrudOptionalMediator>();

                // Crud entities auto registration
                foreach (var crudEntity in apizrOptions.CrudEntities)
                {
                    var apiEntityAttribute = crudEntity.Value;
                    var apiEntityType = crudEntity.Key;
                    var modelEntityType = apiEntityAttribute.MappedEntityType;
                    var apiEntityKeyType = apiEntityAttribute.KeyType;
                    var apiEntityReadAllResultType = apiEntityAttribute.ReadAllResultType.MakeGenericTypeIfNeeded(apiEntityType);
                    var modelEntityReadAllResultType = apiEntityAttribute.ReadAllResultType.IsGenericTypeDefinition
                        ? apiEntityAttribute.ReadAllResultType.MakeGenericTypeIfNeeded(modelEntityType)
                        : apiEntityAttribute.ReadAllResultType.GetGenericTypeDefinition()
                            .MakeGenericTypeIfNeeded(modelEntityType);
                    var apiEntityReadAllParamsType = apiEntityAttribute.ReadAllParamsType;

                    #region ShortRead

                    // Read but short default version if concerned
                    if (apiEntityKeyType == typeof(int))
                    {
                        // ServiceType
                        var shortReadQueryType = typeof(ReadOptionalQuery<>).MakeGenericType(modelEntityType);
                        var shortReadQueryExceptionType = typeof(ApizrException<>).MakeGenericType(modelEntityType);
                        var shortReadQueryResponseType = typeof(Option<,>).MakeGenericType(modelEntityType, shortReadQueryExceptionType);
                        var shortReadQueryHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(shortReadQueryType, shortReadQueryResponseType);

                        // ImplementationType
                        var shortReadQueryHandlerImplementationType = typeof(ReadOptionalQueryHandler<,,,>).MakeGenericType(
                            modelEntityType,
                            apiEntityType,
                            apiEntityReadAllResultType,
                            apiEntityReadAllParamsType);

                        // Registration
                        services.TryAddTransient(shortReadQueryHandlerServiceType, shortReadQueryHandlerImplementationType);
                    }

                    #endregion

                    #region Read

                    // ServiceType
                    var readQueryType = typeof(ReadOptionalQuery<,>).MakeGenericType(modelEntityType, apiEntityKeyType);
                    var readQueryExceptionType = typeof(ApizrException<>).MakeGenericType(modelEntityType);
                    var readQueryResponseType = typeof(Option<,>).MakeGenericType(modelEntityType, readQueryExceptionType);
                    var readQueryHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(readQueryType, readQueryResponseType);

                    // ImplementationType
                    var readQueryHandlerImplementationType = typeof(ReadOptionalQueryHandler<,,,,>).MakeGenericType(
                        modelEntityType,
                        apiEntityType,
                        apiEntityKeyType,
                        apiEntityReadAllResultType,
                        apiEntityReadAllParamsType);

                    // Registration
                    services.TryAddTransient(readQueryHandlerServiceType, readQueryHandlerImplementationType);

                    #endregion

                    #region ShortReadAll

                    // ReadAll but short default version if concerned
                    if (apiEntityReadAllParamsType == typeof(IDictionary<string, object>))
                    {
                        // ServiceType
                        var shortReadAllQueryType = typeof(ReadAllOptionalQuery<>).MakeGenericType(modelEntityReadAllResultType);
                        var shortReadAllQueryExceptionType = typeof(ApizrException<>).MakeGenericType(modelEntityReadAllResultType);
                        var shortReadAllQueryResponseType = typeof(Option<,>).MakeGenericType(modelEntityReadAllResultType, shortReadAllQueryExceptionType);
                        var shortReadAllQueryHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(shortReadAllQueryType, shortReadAllQueryResponseType);

                        // ImplementationType
                        var shortReadAllQueryHandlerImplementationType = typeof(ReadAllOptionalQueryHandler<,,,>).MakeGenericType(
                            apiEntityType,
                            apiEntityKeyType,
                            modelEntityReadAllResultType,
                            apiEntityReadAllResultType);

                        // Registration
                        services.TryAddTransient(shortReadAllQueryHandlerServiceType, shortReadAllQueryHandlerImplementationType);
                    }

                    #endregion

                    #region ReadAll

                    // ServiceType
                    var readAllQueryType = typeof(ReadAllOptionalQuery<,>).MakeGenericType(apiEntityReadAllParamsType, modelEntityReadAllResultType);
                    var readAllQueryExceptionType = typeof(ApizrException<>).MakeGenericType(modelEntityReadAllResultType);
                    var readAllQueryResponseType = typeof(Option<,>).MakeGenericType(modelEntityReadAllResultType, readAllQueryExceptionType);
                    var readAllQueryHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(readAllQueryType, readAllQueryResponseType);

                    // ImplementationType
                    var readAllQueryHandlerImplementationType = typeof(ReadAllOptionalQueryHandler<,,,,>).MakeGenericType(
                        apiEntityType,
                        apiEntityKeyType,
                        modelEntityReadAllResultType,
                        apiEntityReadAllResultType,
                        apiEntityReadAllParamsType);

                    // Registration
                    services.TryAddTransient(readAllQueryHandlerServiceType, readAllQueryHandlerImplementationType);

                    #endregion

                    #region Create

                    // ServiceType
                    var createCommandType = typeof(CreateOptionalCommand<>).MakeGenericType(modelEntityType);
                    var createCommandExceptionType = typeof(ApizrException);
                    var createCommandResponseType = typeof(Option<,>).MakeGenericType(modelEntityType, createCommandExceptionType);
                    var createCommandHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(createCommandType, createCommandResponseType);

                    // ImplementationType
                    var createCommandHandlerImplementationType = typeof(CreateOptionalCommandHandler<,,,,>).MakeGenericType(
                        modelEntityType,
                        apiEntityType,
                        apiEntityKeyType,
                        apiEntityReadAllResultType,
                        apiEntityReadAllParamsType);

                    // Registration
                    services.TryAddTransient(createCommandHandlerServiceType, createCommandHandlerImplementationType);

                    #endregion

                    #region ShortUpdate

                    // Update but short default version if concerned
                    if (apiEntityKeyType == typeof(int))
                    {
                        // ServiceType
                        var shortUpdateCommandType = typeof(UpdateOptionalCommand<>).MakeGenericType(modelEntityType);
                        var shortUpdateCommandResponseType = typeof(Option<Unit, ApizrException>);
                        var shortUpdateCommandHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(shortUpdateCommandType, shortUpdateCommandResponseType);

                        // ImplementationType
                        var shortUpdateCommandHandlerImplementationType = typeof(UpdateOptionalCommandHandler<,,,>).MakeGenericType(
                            modelEntityType,
                            apiEntityType,
                            apiEntityReadAllResultType,
                            apiEntityReadAllParamsType);

                        // Registration
                        services.TryAddTransient(shortUpdateCommandHandlerServiceType, shortUpdateCommandHandlerImplementationType);
                    }

                    #endregion

                    #region Update

                    // ServiceType
                    var updateCommandType = typeof(UpdateOptionalCommand<,>).MakeGenericType(apiEntityKeyType, modelEntityType);
                    var updateCommandResponseType = typeof(Option<Unit, ApizrException>);
                    var updateCommandHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(updateCommandType, updateCommandResponseType);

                    // ImplementationType
                    var updateCommandHandlerImplementationType = typeof(UpdateOptionalCommandHandler<,,,,>).MakeGenericType(
                        modelEntityType,
                        apiEntityType,
                        apiEntityKeyType,
                        apiEntityReadAllResultType,
                        apiEntityReadAllParamsType);

                    // Registration
                    services.TryAddTransient(updateCommandHandlerServiceType, updateCommandHandlerImplementationType);

                    #endregion

                    #region ShortDelete

                    // Delete but short default version if concerned
                    if (apiEntityKeyType == typeof(int))
                    {
                        // ServiceType
                        var shortDeleteCommandType = typeof(DeleteOptionalCommand<>).MakeGenericType(modelEntityType);
                        var shortDeleteCommandResponseType = typeof(Option<Unit, ApizrException>);
                        var shortDeleteCommandHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(shortDeleteCommandType, shortDeleteCommandResponseType);

                        // ImplementationType
                        var shortDeleteCommandHandlerImplementationType = typeof(DeleteOptionalCommandHandler<,,,>).MakeGenericType(
                            modelEntityType,
                            apiEntityType,
                            apiEntityReadAllResultType,
                            apiEntityReadAllParamsType);

                        // Registration
                        services.TryAddTransient(shortDeleteCommandHandlerServiceType, shortDeleteCommandHandlerImplementationType);
                    }

                    #endregion

                    #region Delete

                    // ServiceType
                    var deleteCommandType = typeof(DeleteOptionalCommand<,>).MakeGenericType(modelEntityType, apiEntityKeyType);
                    var deleteCommandResponseType = typeof(Option<Unit, ApizrException>);
                    var deleteCommandHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(deleteCommandType, deleteCommandResponseType);

                    // ImplementationType
                    var deleteCommandHandlerImplementationType = typeof(DeleteOptionalCommandHandler<,,,,>).MakeGenericType(
                        modelEntityType,
                        apiEntityType,
                        apiEntityKeyType,
                        apiEntityReadAllResultType,
                        apiEntityReadAllParamsType);

                    // Registration
                    services.TryAddTransient(deleteCommandHandlerServiceType, deleteCommandHandlerImplementationType);

                    #endregion

                    #region Typed

                    // Typed crud optional mediator
                    var typedCrudOptionalMediatorServiceType = typeof(IApizrCrudOptionalMediator<,,,>).MakeGenericType(apiEntityType,
                        apiEntityKeyType,
                        apiEntityReadAllResultType,
                        apiEntityReadAllParamsType);
                    var typedCrudOptionalMediatorImplementationType = typeof(ApizrCrudOptionalMediator<,,,>).MakeGenericType(apiEntityType,
                        apiEntityKeyType,
                        apiEntityReadAllResultType,
                        apiEntityReadAllParamsType);

                    // Register typed crud optional mediator
                    services.TryAddTransient(typedCrudOptionalMediatorServiceType, typedCrudOptionalMediatorImplementationType);

                    // Get or create and register an optional mediation registry
                    if (!apizrOptions.PostRegistries.TryGetValue(typeof(IApizrOptionalMediationConcurrentRegistry), out var registry))
                    {
                        var optionalMediationRegistry = new ApizrOptionalMediationRegistry();
                        registry = optionalMediationRegistry;
                        apizrOptions.PostRegistries.Add(typeof(IApizrOptionalMediationConcurrentRegistry), registry);
                        services.TryAddSingleton(serviceProvider => optionalMediationRegistry.GetInstance(serviceProvider));
                    }

                    // Add or update the optional mediator service into the registry
                    registry.AddOrUpdateFor(typedCrudOptionalMediatorServiceType, typedCrudOptionalMediatorServiceType);

                    #endregion
                }

                #endregion

                #region Classic

                // Register optional mediator
                services.TryAddSingleton<IApizrOptionalMediator, ApizrOptionalMediator>();

                // Classic interfaces auto registration
                foreach (var webApi in apizrOptions.WebApis)
                {
                    foreach (var methodInfo in webApi.Key.GetMethods())
                    {
                        var returnType = methodInfo.ReturnType;

                        #region Result

                        if (returnType.IsGenericType &&
                                            (methodInfo.ReturnType.GetGenericTypeDefinition() != typeof(Task<>)
                                             || methodInfo.ReturnType.GetGenericTypeDefinition() != typeof(IObservable<>)))
                        {
                            var apiResponseType = returnType.GetGenericArguments()[0];
                            
                            #region Unmapped

                            if (apiResponseType.IsGenericType &&
                                                    (apiResponseType.GetGenericTypeDefinition() == typeof(ApiResponse<>)
                                                     || apiResponseType.GetGenericTypeDefinition() == typeof(IApiResponse<>)))
                            {
                                apiResponseType = apiResponseType.GetGenericArguments()[0];
                            }
                            else if (apiResponseType == typeof(IApiResponse))
                            {
                                apiResponseType = typeof(HttpContent);
                            }

                            // ServiceType
                            var executeRequestType = typeof(ExecuteOptionalResultRequest<,>).MakeGenericType(webApiType, apiResponseType);
                            var executeRequestExceptionType = typeof(ApizrException<>).MakeGenericType(apiResponseType);
                            var executeRequestResponseType = typeof(Option<,>).MakeGenericType(apiResponseType, executeRequestExceptionType);
                            var executeRequestHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(executeRequestType, executeRequestResponseType);

                            // ImplementationType
                            var executeRequestHandlerImplementationType = typeof(ExecuteOptionalResultRequestHandler<,>).MakeGenericType(webApiType, apiResponseType);

                            // Registration
                            services.TryAddTransient(executeRequestHandlerServiceType, executeRequestHandlerImplementationType);

                            #endregion

                            #region Mapped

                            // Mapped object
                            var modelResponseType =
                                methodInfo.GetCustomAttribute<MappedWithAttribute>()?.MappedWithType ??
                                apizrOptions.ObjectMappings
                                    .FirstOrDefault(kvp => kvp.Key == apiResponseType).Value?.MappedWithType ??
                                apizrOptions.ObjectMappings
                                    .FirstOrDefault(kvp => kvp.Value?.MappedWithType == apiResponseType).Key;
                            if (modelResponseType != null)
                            {
                                // Mapped object
                                var mappedParameterInfo = methodInfo.GetParameters().FirstOrDefault(p =>
                                    p.ParameterType.IsClass && !p.ParameterType.IsAbstract &&
                                    p.ParameterType.GetCustomAttribute<MappedWithAttribute>() != null);
                                if (mappedParameterInfo == null) // ExecuteOptionalResultRequest<TWebApi, TModelData, TApiData>
                                {
                                    // ServiceType
                                    var executeMappedRequestType = typeof(ExecuteOptionalResultRequest<,,>).MakeGenericType(webApiType, modelResponseType, apiResponseType);
                                    var executeMappedRequestExceptionType = typeof(ApizrException<>).MakeGenericType(modelResponseType);
                                    var executeMappedRequestResponseType = typeof(Option<,>).MakeGenericType(modelResponseType, executeMappedRequestExceptionType);
                                    var executeMappedRequestHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(executeMappedRequestType, executeMappedRequestResponseType);

                                    // ImplementationType
                                    var executeMappedRequestHandlerImplementationType = typeof(ExecuteOptionalResultRequestHandler<,,>).MakeGenericType(webApiType, modelResponseType, apiResponseType);

                                    // Registration
                                    services.TryAddTransient(executeMappedRequestHandlerServiceType, executeMappedRequestHandlerImplementationType);
                                }
                                else // ExecuteOptionalResultRequest<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>
                                {
                                    // Mapped request
                                    var modelRequestType = mappedParameterInfo.ParameterType.GetCustomAttribute<MappedWithAttribute>().MappedWithType;
                                    var apiRequestType = mappedParameterInfo.ParameterType;

                                    // ServiceType
                                    var executeMappedRequestType = typeof(ExecuteOptionalResultRequest<,,,,>).MakeGenericType(webApiType, modelResponseType, apiResponseType, apiRequestType, modelRequestType);
                                    var executeMappedRequestExceptionType = typeof(ApizrException<>).MakeGenericType(modelResponseType);
                                    var executeMappedRequestResponseType = typeof(Option<,>).MakeGenericType(modelResponseType, executeMappedRequestExceptionType);
                                    var executeMappedRequestHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(executeMappedRequestType, executeMappedRequestResponseType);

                                    // ImplementationType
                                    var executeMappedRequestHandlerImplementationType = typeof(ExecuteOptionalResultRequestHandler<,,,,>).MakeGenericType(webApiType, modelResponseType, apiResponseType, apiRequestType, modelRequestType);

                                    // Registration
                                    services.TryAddTransient(executeMappedRequestHandlerServiceType, executeMappedRequestHandlerImplementationType);
                                }
                            } 

                            #endregion
                        }

                        #endregion

                        #region Unit

                        else if (returnType == typeof(Task))
                        {
                            #region Unmapped

                            // ServiceType
                            var executeRequestType = typeof(ExecuteOptionalUnitRequest<>).MakeGenericType(webApiType);
                            var executeRequestResponseType = typeof(Option<Unit, ApizrException>);
                            var executeRequestHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(executeRequestType, executeRequestResponseType);

                            // ImplementationType
                            var executeRequestHandlerImplementationType = typeof(ExecuteOptionalUnitRequestHandler<>).MakeGenericType(webApiType);

                            // Registration
                            services.TryAddTransient(executeRequestHandlerServiceType, executeRequestHandlerImplementationType);

                            #endregion
                            
                            #region Mapped

                            // Mapped object
                            var mappedParameterInfo = methodInfo.GetParameters().FirstOrDefault(p =>
                                p.ParameterType.IsClass && !p.ParameterType.IsAbstract &&
                                p.ParameterType.GetCustomAttribute<MappedWithAttribute>() != null);
                            if (mappedParameterInfo != null)
                            {
                                var modelEntityType = mappedParameterInfo.ParameterType.GetCustomAttribute<MappedWithAttribute>().MappedWithType;
                                var apiEntityType = mappedParameterInfo.ParameterType;

                                // ServiceType
                                var executeMappedRequestType = typeof(ExecuteOptionalUnitRequest<,,>).MakeGenericType(webApiType, modelEntityType, apiEntityType);
                                var executeMappedRequestResponseType = typeof(Option<Unit, ApizrException>);
                                var executeMappedRequestHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(executeMappedRequestType, executeMappedRequestResponseType);

                                // ImplementationType
                                var executeMappedRequestHandlerImplementationType = typeof(ExecuteOptionalUnitRequestHandler<,,>).MakeGenericType(webApiType, modelEntityType, apiEntityType);

                                // Registration
                                services.TryAddTransient(executeMappedRequestHandlerServiceType, executeMappedRequestHandlerImplementationType);
                            }

                            #endregion
                        }

                        #endregion
                    }

                    #region Typed

                    // Typed optional mediator
                    var typedOptionalMediatorServiceType = typeof(IApizrOptionalMediator<>).MakeGenericType(webApi.Key);
                    var typedOptionalMediatorImplementationType = typeof(ApizrOptionalMediator<>).MakeGenericType(webApi.Key);

                    // Register typed optional mediator
                    services.TryAddTransient(typedOptionalMediatorServiceType, typedOptionalMediatorImplementationType);

                    // Get or create and register an optional mediation registry
                    if (!apizrOptions.PostRegistries.TryGetValue(typeof(IApizrOptionalMediationConcurrentRegistry), out var registry))
                    {
                        var optionalMediationRegistry = new ApizrOptionalMediationRegistry();
                        registry = optionalMediationRegistry;
                        apizrOptions.PostRegistries.Add(typeof(IApizrOptionalMediationConcurrentRegistry), registry);
                        services.TryAddSingleton(serviceProvider => optionalMediationRegistry.GetInstance(serviceProvider));
                    }

                    // Add or update the optional mediator service into the registry
                    registry.AddOrUpdateFor(typedOptionalMediatorServiceType, typedOptionalMediatorServiceType);

                    #endregion
                }

                #endregion
            });
        }
    }
}
