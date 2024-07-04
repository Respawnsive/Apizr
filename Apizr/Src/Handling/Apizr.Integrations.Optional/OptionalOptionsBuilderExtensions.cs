using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Apizr.Extending;
using Apizr.Extending.Configuring.Common;
using Apizr.Extending.Configuring.Manager;
using Apizr.Mapping;
using Apizr.Optional.Configuring.Registry;
using Apizr.Optional.Cruding;
using Apizr.Optional.Cruding.Handling;
using Apizr.Optional.Cruding.Sending;
using Apizr.Optional.Requesting;
using Apizr.Optional.Requesting.Handling;
using Apizr.Optional.Requesting.Sending;
using Apizr.Requesting;
using MediatR;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Optional;
using Refit;

[assembly: Apizr.Preserve]
namespace Apizr
{
    /// <summary>
    /// Optional with MediatR options builder extensions
    /// </summary>
    public static class OptionalOptionsBuilderExtensions
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
        public static IApizrExtendedManagerOptionsBuilder WithOptionalMediation(
            this IApizrExtendedManagerOptionsBuilder optionsBuilder)
        {
            WithMediation(optionsBuilder.ApizrOptions);

            return optionsBuilder;
        }

        private static void WithMediation(IApizrExtendedCommonOptions commonOptions)
        {
            commonOptions.PostRegistrationActions.Add((managerOptions, services) =>
            {
                #region Crud

                // Crud interfaces auto registration
                if (managerOptions.IsCrudApi)
                {
                    // Register crud optional mediator
                    services.TryAddSingleton<IApizrCrudOptionalMediator, ApizrCrudOptionalMediator>();

                    var crudApiEntityType = managerOptions.CrudApiEntityType;
                    var objectMapping = commonOptions.ObjectMappings.SelectMany(item => item.Value)
                        .FirstOrDefault(mapping =>
                            mapping.SourceEntityType == crudApiEntityType ||
                            mapping.TargetEntityType == crudApiEntityType);
                    var crudModelEntityType = objectMapping?.SourceEntityType == crudApiEntityType ? objectMapping!.TargetEntityType :
                        objectMapping?.TargetEntityType == crudApiEntityType ? objectMapping!.SourceEntityType :
                        crudApiEntityType;
                    var crudApiEntityKeyType = managerOptions.CrudApiEntityKeyType;
                    var crudApiEntityReadAllResultType = managerOptions.CrudApiReadAllResultType.MakeGenericTypeIfNeeded(crudApiEntityType);
                    var crudModelEntityReadAllResultType = managerOptions.CrudApiReadAllResultType.IsGenericTypeDefinition
                        ? managerOptions.CrudApiReadAllResultType.MakeGenericTypeIfNeeded(crudModelEntityType)
                        : managerOptions.CrudApiReadAllResultType.GetGenericTypeDefinition()
                            .MakeGenericTypeIfNeeded(crudModelEntityType);
                    var crudApiEntityReadAllParamsType = managerOptions.CrudApiReadAllParamsType;
                    var isMapped = crudApiEntityType != crudModelEntityType &&
                                   commonOptions.MappingHandlerType != typeof(VoidMappingHandler) &&
                                   commonOptions.MappingHandlerFactory?.Method.ReturnType != typeof(VoidMappingHandler);

                    #region ShortRead

                    // Read but short default version if concerned
                    if (crudApiEntityKeyType == typeof(int))
                    {
                        #region Unmapped

                        // ServiceType
                        var shortReadQueryType = typeof(ReadOptionalQuery<>).MakeGenericType(crudApiEntityType);
                        var shortReadQueryExceptionType = typeof(ApizrException<>).MakeGenericType(crudApiEntityType);
                        var shortReadQueryResponseType = typeof(Option<,>).MakeGenericType(crudApiEntityType, shortReadQueryExceptionType);
                        var shortReadQueryHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(shortReadQueryType, shortReadQueryResponseType);

                        // ImplementationType
                        var shortReadQueryHandlerImplementationType = typeof(ReadOptionalQueryHandler<,,,>).MakeGenericType(
                            crudApiEntityType,
                            crudApiEntityType,
                            crudApiEntityReadAllResultType,
                            crudApiEntityReadAllParamsType);

                        // Registration
                        services.TryAddTransient(shortReadQueryHandlerServiceType, shortReadQueryHandlerImplementationType);

                        #endregion

                        #region Mapped

                        if (isMapped)
                        {
                            // ServiceType
                            var shortReadMappedQueryType = typeof(ReadOptionalQuery<>).MakeGenericType(crudModelEntityType);
                            var shortReadMappedQueryExceptionType = typeof(ApizrException<>).MakeGenericType(crudModelEntityType);
                            var shortReadMappedQueryResponseType = typeof(Option<,>).MakeGenericType(crudModelEntityType, shortReadMappedQueryExceptionType);
                            var shortReadMappedQueryHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(shortReadMappedQueryType, shortReadMappedQueryResponseType);

                            // ImplementationType
                            var shortReadMappedQueryHandlerImplementationType = typeof(ReadOptionalQueryHandler<,,,>).MakeGenericType(
                                crudModelEntityType,
                                crudApiEntityType,
                                crudApiEntityReadAllResultType,
                                crudApiEntityReadAllParamsType);

                            // Registration
                            services.TryAddTransient(shortReadMappedQueryHandlerServiceType, shortReadMappedQueryHandlerImplementationType);
                        }

                        #endregion
                    }

                    #endregion

                    #region Read

                    #region Unmapped

                    // ServiceType
                    var readQueryType = typeof(ReadOptionalQuery<,>).MakeGenericType(crudApiEntityType, crudApiEntityKeyType);
                    var readQueryExceptionType = typeof(ApizrException<>).MakeGenericType(crudApiEntityType);
                    var readQueryResponseType = typeof(Option<,>).MakeGenericType(crudApiEntityType, readQueryExceptionType);
                    var readQueryHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(readQueryType, readQueryResponseType);

                    // ImplementationType
                    var readQueryHandlerImplementationType = typeof(ReadOptionalQueryHandler<,,,,>).MakeGenericType(
                        crudApiEntityType,
                        crudApiEntityType,
                        crudApiEntityKeyType,
                        crudApiEntityReadAllResultType,
                        crudApiEntityReadAllParamsType);

                    // Registration
                    services.TryAddTransient(readQueryHandlerServiceType, readQueryHandlerImplementationType); 

                    #endregion

                    #region Mapped

                    if (isMapped)
                    {
                        // ServiceType
                        var readMappedQueryType = typeof(ReadOptionalQuery<,>).MakeGenericType(crudModelEntityType, crudApiEntityKeyType);
                        var readMappedQueryExceptionType = typeof(ApizrException<>).MakeGenericType(crudModelEntityType);
                        var readMappedQueryResponseType = typeof(Option<,>).MakeGenericType(crudModelEntityType, readMappedQueryExceptionType);
                        var readMappedQueryHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(readMappedQueryType, readMappedQueryResponseType);

                        // ImplementationType
                        var readMappedQueryHandlerImplementationType = typeof(ReadOptionalQueryHandler<,,,,>).MakeGenericType(
                            crudModelEntityType,
                            crudApiEntityType,
                            crudApiEntityKeyType,
                            crudApiEntityReadAllResultType,
                            crudApiEntityReadAllParamsType);

                        // Registration
                        services.TryAddTransient(readMappedQueryHandlerServiceType, readMappedQueryHandlerImplementationType);
                    }

                    #endregion

                    #endregion

                    #region ShortReadAll

                    // ReadAll but short default version if concerned
                    if (crudApiEntityReadAllParamsType == typeof(IDictionary<string, object>))
                    {
                        #region Unmapped

                        // ServiceType
                        var shortReadAllQueryType = typeof(ReadAllOptionalQuery<>).MakeGenericType(crudApiEntityReadAllResultType);
                        var shortReadAllQueryExceptionType = typeof(ApizrException<>).MakeGenericType(crudApiEntityReadAllResultType);
                        var shortReadAllQueryResponseType = typeof(Option<,>).MakeGenericType(crudApiEntityReadAllResultType, shortReadAllQueryExceptionType);
                        var shortReadAllQueryHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(shortReadAllQueryType, shortReadAllQueryResponseType);

                        // ImplementationType
                        var shortReadAllQueryHandlerImplementationType = typeof(ReadAllOptionalQueryHandler<,,,>).MakeGenericType(
                            crudApiEntityType,
                            crudApiEntityKeyType,
                            crudApiEntityReadAllResultType,
                            crudApiEntityReadAllResultType);

                        // Registration
                        services.TryAddTransient(shortReadAllQueryHandlerServiceType, shortReadAllQueryHandlerImplementationType);

                        #endregion

                        #region Mapped

                        if (isMapped)
                        {
                            // ServiceType
                            var shortReadAllMappedQueryType = typeof(ReadAllOptionalQuery<>).MakeGenericType(crudModelEntityReadAllResultType);
                            var shortReadAllMappedQueryExceptionType = typeof(ApizrException<>).MakeGenericType(crudModelEntityReadAllResultType);
                            var shortReadAllMappedQueryResponseType = typeof(Option<,>).MakeGenericType(crudModelEntityReadAllResultType, shortReadAllMappedQueryExceptionType);
                            var shortReadAllMappedQueryHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(shortReadAllMappedQueryType, shortReadAllMappedQueryResponseType);

                            // ImplementationType
                            var shortReadAllMappedQueryHandlerImplementationType = typeof(ReadAllOptionalQueryHandler<,,,>).MakeGenericType(
                                crudApiEntityType,
                                crudApiEntityKeyType,
                                crudModelEntityReadAllResultType,
                                crudApiEntityReadAllResultType);

                            // Registration
                            services.TryAddTransient(shortReadAllMappedQueryHandlerServiceType, shortReadAllMappedQueryHandlerImplementationType);
                        }

                        #endregion
                    }

                    #endregion

                    #region ReadAll

                    #region Unmapped

                    // ServiceType
                    var readAllQueryType = typeof(ReadAllOptionalQuery<,>).MakeGenericType(crudApiEntityReadAllParamsType, crudApiEntityReadAllResultType);
                    var readAllQueryExceptionType = typeof(ApizrException<>).MakeGenericType(crudApiEntityReadAllResultType);
                    var readAllQueryResponseType = typeof(Option<,>).MakeGenericType(crudApiEntityReadAllResultType, readAllQueryExceptionType);
                    var readAllQueryHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(readAllQueryType, readAllQueryResponseType);

                    // ImplementationType
                    var readAllQueryHandlerImplementationType = typeof(ReadAllOptionalQueryHandler<,,,,>).MakeGenericType(
                        crudApiEntityType,
                        crudApiEntityKeyType,
                        crudApiEntityReadAllResultType,
                        crudApiEntityReadAllResultType,
                        crudApiEntityReadAllParamsType);

                    // Registration
                    services.TryAddTransient(readAllQueryHandlerServiceType, readAllQueryHandlerImplementationType);

                    #endregion

                    #region Mapped

                    if (isMapped)
                    {
                        // ServiceType
                        var readAllMappedQueryType = typeof(ReadAllOptionalQuery<,>).MakeGenericType(crudApiEntityReadAllParamsType, crudModelEntityReadAllResultType);
                        var readAllMappedQueryExceptionType = typeof(ApizrException<>).MakeGenericType(crudModelEntityReadAllResultType);
                        var readAllMappedQueryResponseType = typeof(Option<,>).MakeGenericType(crudModelEntityReadAllResultType, readAllMappedQueryExceptionType);
                        var readAllMappedQueryHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(readAllMappedQueryType, readAllMappedQueryResponseType);

                        // ImplementationType
                        var readAllMappedQueryHandlerImplementationType = typeof(ReadAllOptionalQueryHandler<,,,,>).MakeGenericType(
                            crudApiEntityType,
                            crudApiEntityKeyType,
                            crudModelEntityReadAllResultType,
                            crudApiEntityReadAllResultType,
                            crudApiEntityReadAllParamsType);

                        // Registration
                        services.TryAddTransient(readAllMappedQueryHandlerServiceType, readAllMappedQueryHandlerImplementationType);
                    }

                    #endregion

                    #endregion

                    #region Create

                    #region Unmapped

                    // ServiceType
                    var createCommandType = typeof(CreateOptionalCommand<>).MakeGenericType(crudApiEntityType);
                    var createCommandExceptionType = typeof(ApizrException);
                    var createCommandResponseType = typeof(Option<,>).MakeGenericType(crudApiEntityType, createCommandExceptionType);
                    var createCommandHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(createCommandType, createCommandResponseType);

                    // ImplementationType
                    var createCommandHandlerImplementationType = typeof(CreateOptionalCommandHandler<,,,,>).MakeGenericType(
                        crudApiEntityType,
                        crudApiEntityType,
                        crudApiEntityKeyType,
                        crudApiEntityReadAllResultType,
                        crudApiEntityReadAllParamsType);

                    // Registration
                    services.TryAddTransient(createCommandHandlerServiceType, createCommandHandlerImplementationType);

                    #endregion

                    #region Mapped

                    if (isMapped)
                    {
                        // ServiceType
                        var createMappedCommandType = typeof(CreateOptionalCommand<>).MakeGenericType(crudModelEntityType);
                        var createMappedCommandExceptionType = typeof(ApizrException);
                        var createMappedCommandResponseType = typeof(Option<,>).MakeGenericType(crudModelEntityType, createMappedCommandExceptionType);
                        var createMappedCommandHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(createMappedCommandType, createMappedCommandResponseType);

                        // ImplementationType
                        var createMappedCommandHandlerImplementationType = typeof(CreateOptionalCommandHandler<,,,,>).MakeGenericType(
                            crudModelEntityType,
                            crudApiEntityType,
                            crudApiEntityKeyType,
                            crudApiEntityReadAllResultType,
                            crudApiEntityReadAllParamsType);

                        // Registration
                        services.TryAddTransient(createMappedCommandHandlerServiceType, createMappedCommandHandlerImplementationType);
                    }

                    #endregion

                    #endregion

                    #region ShortUpdate

                    // Update but short default version if concerned
                    if (crudApiEntityKeyType == typeof(int))
                    {
                        #region Unmapped

                        // ServiceType
                        var shortUpdateCommandType = typeof(UpdateOptionalCommand<>).MakeGenericType(crudApiEntityType);
                        var shortUpdateCommandResponseType = typeof(Option<Unit, ApizrException>);
                        var shortUpdateCommandHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(shortUpdateCommandType, shortUpdateCommandResponseType);

                        // ImplementationType
                        var shortUpdateCommandHandlerImplementationType = typeof(UpdateOptionalCommandHandler<,,,>).MakeGenericType(
                            crudApiEntityType,
                            crudApiEntityType,
                            crudApiEntityReadAllResultType,
                            crudApiEntityReadAllParamsType);

                        // Registration
                        services.TryAddTransient(shortUpdateCommandHandlerServiceType, shortUpdateCommandHandlerImplementationType);

                        #endregion

                        #region Mapped

                        if (isMapped)
                        {
                            // ServiceType
                            var shortUpdateMappedCommandType = typeof(UpdateOptionalCommand<>).MakeGenericType(crudModelEntityType);
                            var shortUpdateMappedCommandResponseType = typeof(Option<Unit, ApizrException>);
                            var shortUpdateMappedCommandHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(shortUpdateMappedCommandType, shortUpdateMappedCommandResponseType);

                            // ImplementationType
                            var shortUpdateMappedCommandHandlerImplementationType = typeof(UpdateOptionalCommandHandler<,,,>).MakeGenericType(
                                crudModelEntityType,
                                crudApiEntityType,
                                crudApiEntityReadAllResultType,
                                crudApiEntityReadAllParamsType);

                            // Registration
                            services.TryAddTransient(shortUpdateMappedCommandHandlerServiceType, shortUpdateMappedCommandHandlerImplementationType);
                        }

                        #endregion
                    }

                    #endregion

                    #region Update

                    #region Unmapped

                    // ServiceType
                    var updateCommandType = typeof(UpdateOptionalCommand<,>).MakeGenericType(crudApiEntityKeyType, crudApiEntityType);
                    var updateCommandResponseType = typeof(Option<Unit, ApizrException>);
                    var updateCommandHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(updateCommandType, updateCommandResponseType);

                    // ImplementationType
                    var updateCommandHandlerImplementationType = typeof(UpdateOptionalCommandHandler<,,,,>).MakeGenericType(
                        crudApiEntityType,
                        crudApiEntityType,
                        crudApiEntityKeyType,
                        crudApiEntityReadAllResultType,
                        crudApiEntityReadAllParamsType);

                    // Registration
                    services.TryAddTransient(updateCommandHandlerServiceType, updateCommandHandlerImplementationType);

                    #endregion

                    #region Mapped

                    if (isMapped)
                    {
                        // ServiceType
                        var updateMappedCommandType = typeof(UpdateOptionalCommand<,>).MakeGenericType(crudApiEntityKeyType, crudModelEntityType);
                        var updateMappedCommandResponseType = typeof(Option<Unit, ApizrException>);
                        var updateMappedCommandHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(updateMappedCommandType, updateMappedCommandResponseType);

                        // ImplementationType
                        var updateMappedCommandHandlerImplementationType = typeof(UpdateOptionalCommandHandler<,,,,>).MakeGenericType(
                            crudModelEntityType,
                            crudApiEntityType,
                            crudApiEntityKeyType,
                            crudApiEntityReadAllResultType,
                            crudApiEntityReadAllParamsType);

                        // Registration
                        services.TryAddTransient(updateMappedCommandHandlerServiceType, updateMappedCommandHandlerImplementationType);
                    }

                    #endregion

                    #endregion

                    #region ShortDelete

                    // Delete but short default version if concerned
                    if (crudApiEntityKeyType == typeof(int))
                    {
                        #region Unmapped

                        // ServiceType
                        var shortDeleteCommandType = typeof(DeleteOptionalCommand<>).MakeGenericType(crudApiEntityType);
                        var shortDeleteCommandResponseType = typeof(Option<Unit, ApizrException>);
                        var shortDeleteCommandHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(shortDeleteCommandType, shortDeleteCommandResponseType);

                        // ImplementationType
                        var shortDeleteCommandHandlerImplementationType = typeof(DeleteOptionalCommandHandler<,,,>).MakeGenericType(
                            crudApiEntityType,
                            crudApiEntityType,
                            crudApiEntityReadAllResultType,
                            crudApiEntityReadAllParamsType);

                        // Registration
                        services.TryAddTransient(shortDeleteCommandHandlerServiceType, shortDeleteCommandHandlerImplementationType);

                        #endregion

                        #region Mapped

                        if (isMapped)
                        {
                            // ServiceType
                            var shortDeleteMappedCommandType = typeof(DeleteOptionalCommand<>).MakeGenericType(crudModelEntityType);
                            var shortDeleteMappedCommandResponseType = typeof(Option<Unit, ApizrException>);
                            var shortDeleteMappedCommandHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(shortDeleteMappedCommandType, shortDeleteMappedCommandResponseType);

                            // ImplementationType
                            var shortDeleteMappedCommandHandlerImplementationType = typeof(DeleteOptionalCommandHandler<,,,>).MakeGenericType(
                                crudModelEntityType,
                                crudApiEntityType,
                                crudApiEntityReadAllResultType,
                                crudApiEntityReadAllParamsType);

                            // Registration
                            services.TryAddTransient(shortDeleteMappedCommandHandlerServiceType, shortDeleteMappedCommandHandlerImplementationType);
                        }

                        #endregion
                    }

                    #endregion

                    #region Delete

                    #region Unmapped

                    // ServiceType
                    var deleteCommandType = typeof(DeleteOptionalCommand<,>).MakeGenericType(crudApiEntityType, crudApiEntityKeyType);
                    var deleteCommandResponseType = typeof(Option<Unit, ApizrException>);
                    var deleteCommandHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(deleteCommandType, deleteCommandResponseType);

                    // ImplementationType
                    var deleteCommandHandlerImplementationType = typeof(DeleteOptionalCommandHandler<,,,,>).MakeGenericType(
                        crudApiEntityType,
                        crudApiEntityType,
                        crudApiEntityKeyType,
                        crudApiEntityReadAllResultType,
                        crudApiEntityReadAllParamsType);

                    // Registration
                    services.TryAddTransient(deleteCommandHandlerServiceType, deleteCommandHandlerImplementationType);

                    #endregion

                    #region Mapped

                    if (isMapped)
                    {
                        // ServiceType
                        var deleteMappedCommandType = typeof(DeleteOptionalCommand<,>).MakeGenericType(crudModelEntityType, crudApiEntityKeyType);
                        var deleteMappedCommandResponseType = typeof(Option<Unit, ApizrException>);
                        var deleteMappedCommandHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(deleteMappedCommandType, deleteMappedCommandResponseType);

                        // ImplementationType
                        var deleteMappedCommandHandlerImplementationType = typeof(DeleteOptionalCommandHandler<,,,,>).MakeGenericType(
                            crudModelEntityType,
                            crudApiEntityType,
                            crudApiEntityKeyType,
                            crudApiEntityReadAllResultType,
                            crudApiEntityReadAllParamsType);

                        // Registration
                        services.TryAddTransient(deleteMappedCommandHandlerServiceType, deleteMappedCommandHandlerImplementationType);
                    }

                    #endregion

                    #endregion

                    #region Typed

                    // Typed crud optional mediator
                    var typedCrudOptionalMediatorServiceType = typeof(IApizrCrudOptionalMediator<,,,>).MakeGenericType(crudApiEntityType,
                        crudApiEntityKeyType,
                        crudApiEntityReadAllResultType,
                        crudApiEntityReadAllParamsType);
                    var typedCrudOptionalMediatorImplementationType = typeof(ApizrCrudOptionalMediator<,,,>).MakeGenericType(crudApiEntityType,
                        crudApiEntityKeyType,
                        crudApiEntityReadAllResultType,
                        crudApiEntityReadAllParamsType);

                    // Register typed crud optional mediator
                    services.TryAddTransient(typedCrudOptionalMediatorServiceType, typedCrudOptionalMediatorImplementationType);

                    // Get or create and register an optional mediation registry
                    if (!commonOptions.PostRegistries.TryGetValue(typeof(IApizrOptionalMediationConcurrentRegistry), out var commonRegistry))
                    {
                        var optionalMediationRegistry = new ApizrOptionalMediationRegistry();
                        commonRegistry = optionalMediationRegistry;
                        commonOptions.PostRegistries.Add(typeof(IApizrOptionalMediationConcurrentRegistry), commonRegistry);
                        services.TryAddSingleton(serviceProvider => optionalMediationRegistry.GetInstance(serviceProvider));
                    }

                    // Add or update the optional mediator service into the registry
                    commonRegistry.AddOrUpdateManager(typedCrudOptionalMediatorServiceType);

                    #endregion
                }

                #endregion

                #region Classic

                // Register optional mediator
                services.TryAddSingleton<IApizrOptionalMediator, ApizrOptionalMediator>();

                // Classic interfaces auto registration
                // Request handlers registration
                foreach (var methodInfo in MediationOptionsBuilderExtensions.GetMethods(managerOptions.WebApiType))
                {
                    var returnType = methodInfo.ReturnType;

                    #region Result

                    if (returnType.IsGenericType &&
                                        (methodInfo.ReturnType.GetGenericTypeDefinition() == typeof(Task<>)
                                         || methodInfo.ReturnType.GetGenericTypeDefinition() == typeof(IObservable<>)))
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
                        var executeRequestType = typeof(ExecuteOptionalResultRequest<,>).MakeGenericType(managerOptions.WebApiType, apiResponseType);
                        var executeRequestExceptionType = typeof(ApizrException<>).MakeGenericType(apiResponseType);
                        var executeRequestResponseType = typeof(Option<,>).MakeGenericType(apiResponseType, executeRequestExceptionType);
                        var executeRequestHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(executeRequestType, executeRequestResponseType);

                        // ImplementationType
                        var executeRequestHandlerImplementationType = typeof(ExecuteOptionalResultRequestHandler<,>).MakeGenericType(managerOptions.WebApiType, apiResponseType);

                        // Registration
                        services.TryAddTransient(executeRequestHandlerServiceType, executeRequestHandlerImplementationType);

                        #endregion

                        #region Mapped

                        // Mapped object
                        var mappedTypes = commonOptions.ObjectMappings.SelectMany(item => item.Value).ToList();
                        var modelResponseType =
                            methodInfo.GetCustomAttribute<MappedWithAttribute>()?.TargetEntityType ??
                            mappedTypes
                                .FirstOrDefault(attribute => attribute.SourceEntityType == apiResponseType)?.TargetEntityType ??
                            mappedTypes
                                .FirstOrDefault(attribute => attribute.TargetEntityType == apiResponseType)?.SourceEntityType;
                        if (modelResponseType != null)
                        {
                            // Mapped object
                            var mappedParameterInfo = methodInfo.GetParameters().FirstOrDefault(p =>
                                p.ParameterType.IsClass && !p.ParameterType.IsAbstract &&
                                p.ParameterType.GetCustomAttribute<MappedWithAttribute>() != null);
                            if (mappedParameterInfo == null) // ExecuteOptionalResultRequest<TWebApi, TModelData, TApiData>
                            {
                                // ServiceType
                                var executeMappedRequestType = typeof(ExecuteOptionalResultRequest<,,>).MakeGenericType(managerOptions.WebApiType, modelResponseType, apiResponseType);
                                var executeMappedRequestExceptionType = typeof(ApizrException<>).MakeGenericType(modelResponseType);
                                var executeMappedRequestResponseType = typeof(Option<,>).MakeGenericType(modelResponseType, executeMappedRequestExceptionType);
                                var executeMappedRequestHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(executeMappedRequestType, executeMappedRequestResponseType);

                                // ImplementationType
                                var executeMappedRequestHandlerImplementationType = typeof(ExecuteOptionalResultRequestHandler<,,>).MakeGenericType(managerOptions.WebApiType, modelResponseType, apiResponseType);

                                // Registration
                                services.TryAddTransient(executeMappedRequestHandlerServiceType, executeMappedRequestHandlerImplementationType);
                            }
                            else // ExecuteOptionalResultRequest<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>
                            {
                                // Mapped request
                                var modelRequestType = mappedParameterInfo.ParameterType.GetCustomAttribute<MappedWithAttribute>().TargetEntityType;
                                var apiRequestType = mappedParameterInfo.ParameterType;

                                // ServiceType
                                var executeMappedRequestType = typeof(ExecuteOptionalResultRequest<,,,,>).MakeGenericType(managerOptions.WebApiType, modelResponseType, apiResponseType, apiRequestType, modelRequestType);
                                var executeMappedRequestExceptionType = typeof(ApizrException<>).MakeGenericType(modelResponseType);
                                var executeMappedRequestResponseType = typeof(Option<,>).MakeGenericType(modelResponseType, executeMappedRequestExceptionType);
                                var executeMappedRequestHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(executeMappedRequestType, executeMappedRequestResponseType);

                                // ImplementationType
                                var executeMappedRequestHandlerImplementationType = typeof(ExecuteOptionalResultRequestHandler<,,,,>).MakeGenericType(managerOptions.WebApiType, modelResponseType, apiResponseType, apiRequestType, modelRequestType);

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
                        var executeRequestType = typeof(ExecuteOptionalUnitRequest<>).MakeGenericType(managerOptions.WebApiType);
                        var executeRequestResponseType = typeof(Option<Unit, ApizrException>);
                        var executeRequestHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(executeRequestType, executeRequestResponseType);

                        // ImplementationType
                        var executeRequestHandlerImplementationType = typeof(ExecuteOptionalUnitRequestHandler<>).MakeGenericType(managerOptions.WebApiType);

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
                            var modelEntityType = mappedParameterInfo.ParameterType.GetCustomAttribute<MappedWithAttribute>().TargetEntityType;
                            var apiEntityType = mappedParameterInfo.ParameterType;

                            // ServiceType
                            var executeMappedRequestType = typeof(ExecuteOptionalUnitRequest<,,>).MakeGenericType(managerOptions.WebApiType, modelEntityType, apiEntityType);
                            var executeMappedRequestResponseType = typeof(Option<Unit, ApizrException>);
                            var executeMappedRequestHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(executeMappedRequestType, executeMappedRequestResponseType);

                            // ImplementationType
                            var executeMappedRequestHandlerImplementationType = typeof(ExecuteOptionalUnitRequestHandler<,,>).MakeGenericType(managerOptions.WebApiType, modelEntityType, apiEntityType);

                            // Registration
                            services.TryAddTransient(executeMappedRequestHandlerServiceType, executeMappedRequestHandlerImplementationType);
                        }

                        #endregion
                    }

                    #endregion
                }

                #region Typed

                // Typed optional mediator
                var typedOptionalMediatorServiceType = typeof(IApizrOptionalMediator<>).MakeGenericType(managerOptions.WebApiType);
                var typedOptionalMediatorImplementationType = typeof(ApizrOptionalMediator<>).MakeGenericType(managerOptions.WebApiType);

                // Register typed optional mediator
                services.TryAddSingleton(typedOptionalMediatorServiceType, typedOptionalMediatorImplementationType);

                // Get or create and register an optional mediation registry
                if (!commonOptions.PostRegistries.TryGetValue(typeof(IApizrOptionalMediationConcurrentRegistry), out var registry))
                {
                    var optionalMediationRegistry = new ApizrOptionalMediationRegistry();
                    registry = optionalMediationRegistry;
                    commonOptions.PostRegistries.Add(typeof(IApizrOptionalMediationConcurrentRegistry), registry);
                    services.TryAddSingleton(serviceProvider => optionalMediationRegistry.GetInstance(serviceProvider));
                }

                // Add or update the optional mediator service into the registry
                registry.AddOrUpdateManager(typedOptionalMediatorServiceType);

                #endregion

                #endregion
            });
        }
    }
}
