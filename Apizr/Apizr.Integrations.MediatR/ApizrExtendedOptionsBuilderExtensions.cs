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
using Apizr.Mediation.Configuring.Registry;
using Apizr.Mediation.Cruding;
using Apizr.Mediation.Cruding.Handling;
using Apizr.Mediation.Cruding.Sending;
using Apizr.Mediation.Requesting;
using Apizr.Mediation.Requesting.Handling;
using Apizr.Mediation.Requesting.Sending;
using MediatR;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Refit;

[assembly: Apizr.Preserve]
namespace Apizr
{
    public static class ApizrExtendedOptionsBuilderExtensions
    {
        /// <summary>
        /// Let Apizr handle requests execution with some mediation
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <returns></returns>
        public static IApizrExtendedCommonOptionsBuilder WithMediation(this IApizrExtendedCommonOptionsBuilder optionsBuilder)
        {
            WithMediation(optionsBuilder.ApizrOptions);

            return optionsBuilder;
        }

        /// <summary>
        /// Let Apizr handle requests execution with some mediation
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <returns></returns>
        public static IApizrExtendedOptionsBuilder WithMediation(this IApizrExtendedOptionsBuilder optionsBuilder)
        {
            WithMediation(optionsBuilder.ApizrOptions);

            return optionsBuilder;
        }

        private static void WithMediation(IApizrExtendedCommonOptions apizrOptions)
        {
            apizrOptions.PostRegistrationActions.Add((webApiType, services) =>
            {
                #region Crud

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
                        var shortReadQueryType = typeof(ReadQuery<>).MakeGenericType(modelEntityType);
                        var shortReadQueryHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(shortReadQueryType, modelEntityType);

                        // ImplementationType
                        var shortReadQueryHandlerImplementationType = typeof(ReadQueryHandler<,,,>).MakeGenericType(
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
                    var readQueryType = typeof(ReadQuery<,>).MakeGenericType(modelEntityType, apiEntityKeyType);
                    var readQueryHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(readQueryType, modelEntityType);

                    // ImplementationType
                    var readQueryHandlerImplementationType = typeof(ReadQueryHandler<,,,,>).MakeGenericType(
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
                        var shortReadAllQueryType = typeof(ReadAllQuery<>).MakeGenericType(modelEntityReadAllResultType);
                        var shortReadAllQueryHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(shortReadAllQueryType, modelEntityReadAllResultType);

                        // ImplementationType
                        var shortReadAllQueryHandlerImplementationType = typeof(ReadAllQueryHandler<,,,>).MakeGenericType(
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
                    var readAllQueryType = typeof(ReadAllQuery<,>).MakeGenericType(apiEntityReadAllParamsType, modelEntityReadAllResultType);
                    var readAllQueryHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(readAllQueryType, modelEntityReadAllResultType);

                    // ImplementationType
                    var readAllQueryHandlerImplementationType = typeof(ReadAllQueryHandler<,,,,>).MakeGenericType(
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
                    var createCommandType = typeof(CreateCommand<>).MakeGenericType(modelEntityType);
                    var createCommandHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(createCommandType, modelEntityType);

                    // ImplementationType
                    var createCommandHandlerImplementationType = typeof(CreateCommandHandler<,,,,>).MakeGenericType(
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
                        var shortUpdateCommandType = typeof(UpdateCommand<>).MakeGenericType(modelEntityType);
                        var shortUpdateCommandResponseType = typeof(Unit);
                        var shortUpdateCommandHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(shortUpdateCommandType, shortUpdateCommandResponseType);

                        // ImplementationType
                        var shortUpdateCommandHandlerImplementationType = typeof(UpdateCommandHandler<,,,>).MakeGenericType(
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
                    var updateCommandType = typeof(UpdateCommand<,>).MakeGenericType(apiEntityKeyType, modelEntityType);
                    var updateCommandResponseType = typeof(Unit);
                    var updateCommandHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(updateCommandType, updateCommandResponseType);

                    // ImplementationType
                    var updateCommandHandlerImplementationType = typeof(UpdateCommandHandler<,,,,>).MakeGenericType(
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
                        var shortDeleteCommandType = typeof(DeleteCommand<>).MakeGenericType(modelEntityType);
                        var shortDeleteCommandResponseType = typeof(Unit);
                        var shortDeleteCommandHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(shortDeleteCommandType, shortDeleteCommandResponseType);

                        // ImplementationType
                        var shortDeleteCommandHandlerImplementationType = typeof(DeleteCommandHandler<,,,>).MakeGenericType(
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
                    var deleteCommandType = typeof(DeleteCommand<,>).MakeGenericType(modelEntityType, apiEntityKeyType);
                    var deleteCommandResponseType = typeof(Unit);
                    var deleteCommandHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(deleteCommandType, deleteCommandResponseType);

                    // ImplementationType
                    var deleteCommandHandlerImplementationType = typeof(DeleteCommandHandler<,,,,>).MakeGenericType(
                        modelEntityType,
                        apiEntityType,
                        apiEntityKeyType,
                        apiEntityReadAllResultType,
                        apiEntityReadAllParamsType);

                    // Registration
                    services.TryAddTransient(deleteCommandHandlerServiceType, deleteCommandHandlerImplementationType);

                    #endregion

                    #region Typed

                    // Typed crud mediator
                    var typedCrudMediatorServiceType = typeof(IApizrCrudMediator<,,,>).MakeGenericType(apiEntityType,
                        apiEntityKeyType,
                        apiEntityReadAllResultType,
                        apiEntityReadAllParamsType);
                    var typedCrudMediatorImplementationType = typeof(ApizrCrudMediator<,,,>).MakeGenericType(apiEntityType,
                        apiEntityKeyType,
                        apiEntityReadAllResultType,
                        apiEntityReadAllParamsType);

                    services.TryAddTransient(typedCrudMediatorServiceType, typedCrudMediatorImplementationType);

                    // Get or create and register a mediation registry
                    if (!apizrOptions.PostRegistries.TryGetValue(typeof(IApizrMediationConcurrentRegistry), out var registry))
                    {
                        var mediationRegistry = new ApizrMediationRegistry();
                        registry = mediationRegistry;
                        apizrOptions.PostRegistries.Add(typeof(IApizrMediationConcurrentRegistry), registry);
                        services.TryAddSingleton(serviceProvider => mediationRegistry.GetInstance(serviceProvider));
                    }

                    // Add or update the mediator service into the registry
                    registry.AddOrUpdateFor(typedCrudMediatorServiceType, typedCrudMediatorImplementationType);

                    #endregion
                }

                #endregion

                #region Classic

                // Classic interfaces auto registration
                foreach (var webApi in apizrOptions.WebApis)
                {
                    foreach (var methodInfo in webApi.Key.GetMethods())
                    {
                        var returnType = methodInfo.ReturnType;
                        if (returnType.IsGenericType &&
                            (methodInfo.ReturnType.GetGenericTypeDefinition() != typeof(Task<>)
                             || methodInfo.ReturnType.GetGenericTypeDefinition() != typeof(IObservable<>)))
                        {
                            var apiResponseType = returnType.GetGenericArguments()[0];
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
                            var executeRequestType = typeof(ExecuteRequest<,>).MakeGenericType(webApiType, apiResponseType);
                            var executeRequestHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(executeRequestType, apiResponseType);

                            // ImplementationType
                            var executeRequestHandlerImplementationType = typeof(ExecuteRequestHandler<,>).MakeGenericType(webApiType, apiResponseType);

                            // Registration
                            services.TryAddTransient(executeRequestHandlerServiceType, executeRequestHandlerImplementationType);

                            // Mapped object
                            var modelResponseType =
                                methodInfo.GetCustomAttribute<MappedWithAttribute>()?.MappedWithType ??
                                apizrOptions.ObjectMappings
                                    .FirstOrDefault(kvp => kvp.Key == apiResponseType).Value?.MappedWithType ??
                                apizrOptions.ObjectMappings
                                    .FirstOrDefault(kvp => kvp.Value?.MappedWithType == apiResponseType).Key;
                            if (modelResponseType != null)
                            {
                                // ServiceType
                                var executeMappedRequestType = typeof(ExecuteRequest<,,>).MakeGenericType(webApiType, modelResponseType, apiResponseType);
                                var executeMappedRequestHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(executeMappedRequestType, modelResponseType);

                                // ImplementationType
                                var executeMappedRequestHandlerImplementationType = typeof(ExecuteRequestHandler<,,>).MakeGenericType(webApiType, modelResponseType, apiResponseType);

                                // Registration
                                services.TryAddTransient(executeMappedRequestHandlerServiceType, executeMappedRequestHandlerImplementationType);
                            }
                        }
                        else if (returnType == typeof(Task))
                        {
                            // ServiceType
                            var executeRequestType = typeof(ExecuteRequest<>).MakeGenericType(webApiType);
                            var executeRequestResponseType = typeof(Unit);
                            var executeRequestHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(executeRequestType, executeRequestResponseType);

                            // ImplementationType
                            var executeRequestHandlerImplementationType = typeof(ExecuteRequestHandler<>).MakeGenericType(webApiType);

                            // Registration
                            services.TryAddTransient(executeRequestHandlerServiceType, executeRequestHandlerImplementationType);
                        }
                    }

                    #region Typed

                    // Typed mediator
                    var typedMediatorServiceType = typeof(IApizrMediator<>).MakeGenericType(webApi.Key);
                    var typedMediatorImplementationType = typeof(ApizrMediator<>).MakeGenericType(webApi.Key);

                    services.TryAddTransient(typedMediatorServiceType, typedMediatorImplementationType);

                    // Get or create and register a mediation registry
                    if (!apizrOptions.PostRegistries.TryGetValue(typeof(IApizrMediationConcurrentRegistry), out var registry))
                    {
                        var mediationRegistry = new ApizrMediationRegistry();
                        registry = mediationRegistry;
                        apizrOptions.PostRegistries.Add(typeof(IApizrMediationConcurrentRegistry), registry);
                        services.TryAddSingleton(serviceProvider => mediationRegistry.GetInstance(serviceProvider));
                    }

                    // Add or update the mediator service into the registry
                    registry.AddOrUpdateFor(typedMediatorServiceType, typedMediatorImplementationType);

                    #endregion
                }

                #endregion
            });
        }
    }
}
