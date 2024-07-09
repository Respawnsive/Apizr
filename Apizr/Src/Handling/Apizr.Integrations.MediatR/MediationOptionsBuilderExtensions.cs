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
using Apizr.Mediation.Configuring.Registry;
using Apizr.Mediation.Cruding;
using Apizr.Mediation.Cruding.Handling;
using Apizr.Mediation.Cruding.Sending;
using Apizr.Mediation.Requesting;
using Apizr.Mediation.Requesting.Handling;
using Apizr.Mediation.Requesting.Sending;
using Apizr.Requesting;
using MediatR;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Refit;

[assembly: Apizr.Preserve]
namespace Apizr
{
    /// <summary>
    /// MediatR options builder extensions
    /// </summary>
    public static class MediationOptionsBuilderExtensions
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
        public static IApizrExtendedManagerOptionsBuilder WithMediation(this IApizrExtendedManagerOptionsBuilder optionsBuilder)
        {
            WithMediation(optionsBuilder.ApizrOptions);

            return optionsBuilder;
        }

        private static void WithMediation(IApizrExtendedCommonOptions commonOptions)
        {
            commonOptions.PostRegistrationActions.Add((managerOptions, services) =>
            {
                var mappedTypes = commonOptions.ObjectMappings.SelectMany(item => item.Value).ToList();
                var isMappingHandlerRegistered = commonOptions.MappingHandlerType != typeof(VoidMappingHandler) &&
                                       commonOptions.MappingHandlerFactory?.Method.ReturnType != typeof(VoidMappingHandler);

                #region Crud

                // Crud interfaces auto registration
                if (managerOptions.IsCrudApi)
                {
                    // Register crud mediator
                    services.TryAddSingleton<IApizrCrudMediator, ApizrCrudMediator>();
                    
                    var crudApiEntityType = managerOptions.CrudApiEntityType;
                    var objectMapping = mappedTypes.FirstOrDefault(mapping =>
                            mapping.FirstEntityType == crudApiEntityType ||
                            mapping.SecondEntityType == crudApiEntityType);
                    var crudModelEntityType = objectMapping?.FirstEntityType == crudApiEntityType ? objectMapping!.SecondEntityType :
                        objectMapping?.SecondEntityType == crudApiEntityType ? objectMapping!.FirstEntityType :
                        crudApiEntityType;
                    var crudApiEntityKeyType = managerOptions.CrudApiEntityKeyType;
                    var crudApiEntityReadAllResultType = managerOptions.CrudApiReadAllResultType.MakeGenericTypeIfNeeded(crudApiEntityType);
                    var crudModelEntityReadAllResultType = managerOptions.CrudApiReadAllResultType.IsGenericTypeDefinition
                        ? managerOptions.CrudApiReadAllResultType.MakeGenericTypeIfNeeded(crudModelEntityType)
                        : managerOptions.CrudApiReadAllResultType.GetGenericTypeDefinition().MakeGenericTypeIfNeeded(crudModelEntityType);
                    var crudApiEntityReadAllParamsType = managerOptions.CrudApiReadAllParamsType;
                    var handleMapping = isMappingHandlerRegistered && crudApiEntityType != crudModelEntityType;

                    #region ShortRead

                    // Read but short default version if concerned
                    if (crudApiEntityKeyType == typeof(int))
                    {
                        #region Unmapped

                        // ServiceType
                        var shortReadQueryType = typeof(ReadQuery<>).MakeGenericType(crudApiEntityType);
                        var shortReadQueryHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(shortReadQueryType, crudApiEntityType);

                        // ImplementationType
                        var shortReadQueryHandlerImplementationType = typeof(ReadQueryHandler<,,,>).MakeGenericType(
                            crudApiEntityType,
                            crudApiEntityType,
                            crudApiEntityReadAllResultType,
                            crudApiEntityReadAllParamsType);

                        // Registration
                        services.TryAddTransient(shortReadQueryHandlerServiceType, shortReadQueryHandlerImplementationType);

                        // -- Safe --
                        // ServiceType
                        var shortSafeReadQueryType = typeof(SafeReadQuery<>).MakeGenericType(crudApiEntityType);
                        var shortSafeReadQueryResponseType = typeof(IApizrResponse<>).MakeGenericType(crudApiEntityType);
                        var shortSafeReadQueryHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(shortSafeReadQueryType, shortSafeReadQueryResponseType);

                        // ImplementationType
                        var shortSafeReadQueryHandlerImplementationType = typeof(SafeReadQueryHandler<,,,>).MakeGenericType(
                            crudApiEntityType,
                            crudApiEntityType,
                            crudApiEntityReadAllResultType,
                            crudApiEntityReadAllParamsType);

                        // Registration
                        services.TryAddTransient(shortSafeReadQueryHandlerServiceType, shortSafeReadQueryHandlerImplementationType);

                        #endregion

                        #region Mapped

                        if (handleMapping)
                        {
                            // ServiceType
                            var shortReadMappedQueryType = typeof(ReadQuery<>).MakeGenericType(crudModelEntityType);
                            var shortReadMappedQueryHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(shortReadMappedQueryType, crudModelEntityType);

                            // ImplementationType
                            var shortReadMappedQueryHandlerImplementationType = typeof(ReadQueryHandler<,,,>).MakeGenericType(
                                crudModelEntityType,
                                crudApiEntityType,
                                crudApiEntityReadAllResultType,
                                crudApiEntityReadAllParamsType);

                            // Registration
                            services.TryAddTransient(shortReadMappedQueryHandlerServiceType, shortReadMappedQueryHandlerImplementationType);

                            // -- Safe --
                            // ServiceType
                            var shortSafeReadMappedQueryType = typeof(SafeReadQuery<>).MakeGenericType(crudModelEntityType);
                            var shortSafeReadMappedQueryResponseType = typeof(IApizrResponse<>).MakeGenericType(crudModelEntityType);
                            var shortSafeReadMappedQueryHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(shortSafeReadMappedQueryType, shortSafeReadMappedQueryResponseType);

                            // ImplementationType
                            var shortSafeReadMappedQueryHandlerImplementationType = typeof(SafeReadQueryHandler<,,,>).MakeGenericType(
                                crudModelEntityType,
                                crudApiEntityType,
                                crudApiEntityReadAllResultType,
                                crudApiEntityReadAllParamsType);

                            // Registration
                            services.TryAddTransient(shortSafeReadMappedQueryHandlerServiceType, shortSafeReadMappedQueryHandlerImplementationType);
                        } 

                        #endregion
                    }

                    #endregion

                    #region Read

                    #region Unmapped

                    // ServiceType
                    var readQueryType = typeof(ReadQuery<,>).MakeGenericType(crudApiEntityType, crudApiEntityKeyType);
                    var readQueryHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(readQueryType, crudApiEntityType);

                    // ImplementationType
                    var readQueryHandlerImplementationType = typeof(ReadQueryHandler<,,,,>).MakeGenericType(
                        crudApiEntityType,
                        crudApiEntityType,
                        crudApiEntityKeyType,
                        crudApiEntityReadAllResultType,
                        crudApiEntityReadAllParamsType);

                    // Registration
                    services.TryAddTransient(readQueryHandlerServiceType, readQueryHandlerImplementationType);

                    // -- Safe --
                    // ServiceType
                    var safeReadQueryType = typeof(SafeReadQuery<,>).MakeGenericType(crudApiEntityType, crudApiEntityKeyType);
                    var safeReadQueryResponseType = typeof(IApizrResponse<>).MakeGenericType(crudApiEntityType);
                    var safeReadQueryHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(safeReadQueryType, safeReadQueryResponseType);

                    // ImplementationType
                    var safeReadQueryHandlerImplementationType = typeof(SafeReadQueryHandler<,,,,>).MakeGenericType(
                        crudApiEntityType,
                        crudApiEntityType,
                        crudApiEntityKeyType,
                        crudApiEntityReadAllResultType,
                        crudApiEntityReadAllParamsType);

                    // Registration
                    services.TryAddTransient(safeReadQueryHandlerServiceType, safeReadQueryHandlerImplementationType);

                    #endregion

                    #region Mapped

                    if (handleMapping)
                    {
                        // ServiceType
                        var readMappedQueryType = typeof(ReadQuery<,>).MakeGenericType(crudModelEntityType, crudApiEntityKeyType);
                        var readMappedQueryHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(readMappedQueryType, crudModelEntityType);

                        // ImplementationType
                        var readMappedQueryHandlerImplementationType = typeof(ReadQueryHandler<,,,,>).MakeGenericType(
                            crudModelEntityType,
                            crudApiEntityType,
                            crudApiEntityKeyType,
                            crudApiEntityReadAllResultType,
                            crudApiEntityReadAllParamsType);

                        // Registration
                        services.TryAddTransient(readMappedQueryHandlerServiceType, readMappedQueryHandlerImplementationType);

                        // -- Safe --
                        // ServiceType
                        var safeReadMappedQueryType = typeof(SafeReadQuery<,>).MakeGenericType(crudModelEntityType, crudApiEntityKeyType);
                        var safeReadMappedQueryResponseType = typeof(IApizrResponse<>).MakeGenericType(crudModelEntityType);
                        var safeReadMappedQueryHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(safeReadMappedQueryType, safeReadMappedQueryResponseType);

                        // ImplementationType
                        var safeReadMappedQueryHandlerImplementationType = typeof(SafeReadQueryHandler<,,,,>).MakeGenericType(
                            crudModelEntityType,
                            crudApiEntityType,
                            crudApiEntityKeyType,
                            crudApiEntityReadAllResultType,
                            crudApiEntityReadAllParamsType);

                        // Registration
                        services.TryAddTransient(safeReadMappedQueryHandlerServiceType, safeReadMappedQueryHandlerImplementationType);  
                    }

                    #endregion

                    #endregion

                    #region ShortReadAll

                    // ReadAll but short default version if concerned
                    if (crudApiEntityReadAllParamsType == typeof(IDictionary<string, object>))
                    {
                        #region Unmapped

                        // ServiceType
                        var shortReadAllQueryType = typeof(ReadAllQuery<>).MakeGenericType(crudApiEntityReadAllResultType);
                        var shortReadAllQueryHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(shortReadAllQueryType, crudApiEntityReadAllResultType);

                        // ImplementationType
                        var shortReadAllQueryHandlerImplementationType = typeof(ReadAllQueryHandler<,,,>).MakeGenericType(
                            crudApiEntityType,
                            crudApiEntityKeyType,
                            crudApiEntityReadAllResultType,
                            crudApiEntityReadAllResultType);

                        // Registration
                        services.TryAddTransient(shortReadAllQueryHandlerServiceType, shortReadAllQueryHandlerImplementationType);

                        // -- Safe --
                        // ServiceType
                        var shortSafeReadAllQueryType = typeof(SafeReadAllQuery<>).MakeGenericType(crudApiEntityReadAllResultType);
                        var shortSafeReadAllQueryResponseType = typeof(IApizrResponse<>).MakeGenericType(crudApiEntityReadAllResultType);
                        var shortSafeReadAllQueryHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(shortSafeReadAllQueryType, shortSafeReadAllQueryResponseType);

                        // ImplementationType
                        var shortSafeReadAllQueryHandlerImplementationType = typeof(SafeReadAllQueryHandler<,,,>).MakeGenericType(
                            crudApiEntityType,
                            crudApiEntityKeyType,
                            crudApiEntityReadAllResultType,
                            crudApiEntityReadAllResultType);

                        // Registration
                        services.TryAddTransient(shortSafeReadAllQueryHandlerServiceType, shortSafeReadAllQueryHandlerImplementationType); 

                        #endregion

                        #region Mapped

                        if (handleMapping)
                        {
                            // ServiceType
                            var shortReadAllMappedQueryType = typeof(ReadAllQuery<>).MakeGenericType(crudModelEntityReadAllResultType);
                            var shortReadAllMappedQueryHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(shortReadAllMappedQueryType, crudModelEntityReadAllResultType);

                            // ImplementationType
                            var shortReadAllMappedQueryHandlerImplementationType = typeof(ReadAllQueryHandler<,,,>).MakeGenericType(
                                crudApiEntityType,
                                crudApiEntityKeyType,
                                crudModelEntityReadAllResultType,
                                crudApiEntityReadAllResultType);

                            // Registration
                            services.TryAddTransient(shortReadAllMappedQueryHandlerServiceType, shortReadAllMappedQueryHandlerImplementationType);

                            // -- Safe --
                            // ServiceType
                            var shortSafeReadAllMappedQueryType = typeof(SafeReadAllQuery<>).MakeGenericType(crudModelEntityReadAllResultType);
                            var shortSafeReadAllMappedQueryResponseType = typeof(IApizrResponse<>).MakeGenericType(crudModelEntityReadAllResultType);
                            var shortSafeReadAllMappedQueryHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(shortSafeReadAllMappedQueryType, shortSafeReadAllMappedQueryResponseType);

                            // ImplementationType
                            var shortSafeReadAllMappedQueryHandlerImplementationType = typeof(SafeReadAllQueryHandler<,,,>).MakeGenericType(
                                crudApiEntityType,
                                crudApiEntityKeyType,
                                crudModelEntityReadAllResultType,
                                crudApiEntityReadAllResultType);

                            // Registration
                            services.TryAddTransient(shortSafeReadAllMappedQueryHandlerServiceType, shortSafeReadAllMappedQueryHandlerImplementationType);
                        } 

                        #endregion
                    }

                    #endregion

                    #region ReadAll

                    #region Unmapped

                    // ServiceType
                    var readAllQueryType = typeof(ReadAllQuery<,>).MakeGenericType(crudApiEntityReadAllParamsType, crudApiEntityReadAllResultType);
                    var readAllQueryHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(readAllQueryType, crudApiEntityReadAllResultType);

                    // ImplementationType
                    var readAllQueryHandlerImplementationType = typeof(ReadAllQueryHandler<,,,,>).MakeGenericType(
                        crudApiEntityType,
                        crudApiEntityKeyType,
                        crudApiEntityReadAllResultType,
                        crudApiEntityReadAllResultType,
                        crudApiEntityReadAllParamsType);

                    // Registration
                    services.TryAddTransient(readAllQueryHandlerServiceType, readAllQueryHandlerImplementationType);

                    // -- Safe --
                    // ServiceType
                    var safeReadAllQueryType = typeof(SafeReadAllQuery<,>).MakeGenericType(crudApiEntityReadAllParamsType, crudApiEntityReadAllResultType);
                    var safeReadAllQueryResponseType = typeof(IApizrResponse<>).MakeGenericType(crudApiEntityReadAllResultType);
                    var safeReadAllQueryHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(safeReadAllQueryType, safeReadAllQueryResponseType);

                    // ImplementationType
                    var safeReadAllQueryHandlerImplementationType = typeof(SafeReadAllQueryHandler<,,,,>).MakeGenericType(
                        crudApiEntityType,
                        crudApiEntityKeyType,
                        crudApiEntityReadAllResultType,
                        crudApiEntityReadAllResultType,
                        crudApiEntityReadAllParamsType);

                    // Registration
                    services.TryAddTransient(safeReadAllQueryHandlerServiceType, safeReadAllQueryHandlerImplementationType); 

                    #endregion

                    #region Mapped

                    if (handleMapping)
                    {
                        // ServiceType
                        var readAllMappedQueryType = typeof(ReadAllQuery<,>).MakeGenericType(crudApiEntityReadAllParamsType, crudModelEntityReadAllResultType);
                        var readAllMappedQueryHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(readAllMappedQueryType, crudModelEntityReadAllResultType);

                        // ImplementationType
                        var readAllMappedQueryHandlerImplementationType = typeof(ReadAllQueryHandler<,,,,>).MakeGenericType(
                            crudApiEntityType,
                            crudApiEntityKeyType,
                            crudModelEntityReadAllResultType,
                            crudApiEntityReadAllResultType,
                            crudApiEntityReadAllParamsType);

                        // Registration
                        services.TryAddTransient(readAllMappedQueryHandlerServiceType, readAllMappedQueryHandlerImplementationType);

                        // -- Safe --
                        // ServiceType
                        var safeReadAllMappedQueryType = typeof(SafeReadAllQuery<,>).MakeGenericType(crudApiEntityReadAllParamsType, crudModelEntityReadAllResultType);
                        var safeReadAllMappedQueryResponseType = typeof(IApizrResponse<>).MakeGenericType(crudModelEntityReadAllResultType);
                        var safeReadAllMappedQueryHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(safeReadAllMappedQueryType, safeReadAllMappedQueryResponseType);

                        // ImplementationType
                        var safeReadAllMappedQueryHandlerImplementationType = typeof(SafeReadAllQueryHandler<,,,,>).MakeGenericType(
                            crudApiEntityType,
                            crudApiEntityKeyType,
                            crudModelEntityReadAllResultType,
                            crudApiEntityReadAllResultType,
                            crudApiEntityReadAllParamsType);

                        // Registration
                        services.TryAddTransient(safeReadAllMappedQueryHandlerServiceType, safeReadAllMappedQueryHandlerImplementationType);  
                    }

                    #endregion

                    #endregion

                    #region Create

                    #region Unmapped

                    // ServiceType
                    var createCommandType = typeof(CreateCommand<>).MakeGenericType(crudApiEntityType);
                    var createCommandHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(createCommandType, crudApiEntityType);

                    // ImplementationType
                    var createCommandHandlerImplementationType = typeof(CreateCommandHandler<,,,,>).MakeGenericType(
                        crudApiEntityType,
                        crudApiEntityType,
                        crudApiEntityKeyType,
                        crudApiEntityReadAllResultType,
                        crudApiEntityReadAllParamsType);

                    // Registration
                    services.TryAddTransient(createCommandHandlerServiceType, createCommandHandlerImplementationType);

                    // -- Safe --
                    // ServiceType
                    var safeCreateCommandType = typeof(SafeCreateCommand<>).MakeGenericType(crudApiEntityType);
                    var safeCreateCommandResponseType = typeof(IApizrResponse<>).MakeGenericType(crudApiEntityType);
                    var safeCreateCommandHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(safeCreateCommandType, safeCreateCommandResponseType);

                    // ImplementationType
                    var safeCreateCommandHandlerImplementationType = typeof(SafeCreateCommandHandler<,,,,>).MakeGenericType(
                        crudApiEntityType,
                        crudApiEntityType,
                        crudApiEntityKeyType,
                        crudApiEntityReadAllResultType,
                        crudApiEntityReadAllParamsType);

                    // Registration
                    services.TryAddTransient(safeCreateCommandHandlerServiceType, safeCreateCommandHandlerImplementationType); 

                    #endregion

                    #region Mapped

                    if (handleMapping)
                    {
                        // ServiceType
                        var createMappedCommandType = typeof(CreateCommand<>).MakeGenericType(crudModelEntityType);
                        var createMappedCommandHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(createMappedCommandType, crudModelEntityType);

                        // ImplementationType
                        var createMappedCommandHandlerImplementationType = typeof(CreateCommandHandler<,,,,>).MakeGenericType(
                            crudModelEntityType,
                            crudApiEntityType,
                            crudApiEntityKeyType,
                            crudApiEntityReadAllResultType,
                            crudApiEntityReadAllParamsType);

                        // Registration
                        services.TryAddTransient(createMappedCommandHandlerServiceType, createMappedCommandHandlerImplementationType);

                        // -- Safe --
                        // ServiceType
                        var safeCreateMappedCommandType = typeof(SafeCreateCommand<>).MakeGenericType(crudModelEntityType);
                        var safeCreateMappedCommandResponseType = typeof(IApizrResponse<>).MakeGenericType(crudModelEntityType);
                        var safeCreateMappedCommandHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(safeCreateMappedCommandType, safeCreateMappedCommandResponseType);

                        // ImplementationType
                        var safeCreateMappedCommandHandlerImplementationType = typeof(SafeCreateCommandHandler<,,,,>).MakeGenericType(
                            crudModelEntityType,
                            crudApiEntityType,
                            crudApiEntityKeyType,
                            crudApiEntityReadAllResultType,
                            crudApiEntityReadAllParamsType);

                        // Registration
                        services.TryAddTransient(safeCreateMappedCommandHandlerServiceType, safeCreateMappedCommandHandlerImplementationType);
                    }

                    #endregion

                    #endregion

                    #region ShortUpdate

                    // Update but short default version if concerned
                    if (crudApiEntityKeyType == typeof(int))
                    {
                        #region Unmapped

                        // ServiceType
                        var shortUpdateCommandType = typeof(UpdateCommand<>).MakeGenericType(crudApiEntityType);
                        var shortUpdateCommandResponseType = typeof(Unit);
                        var shortUpdateCommandHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(shortUpdateCommandType, shortUpdateCommandResponseType);

                        // ImplementationType
                        var shortUpdateCommandHandlerImplementationType = typeof(UpdateCommandHandler<,,,>).MakeGenericType(
                            crudApiEntityType,
                            crudApiEntityType,
                            crudApiEntityReadAllResultType,
                            crudApiEntityReadAllParamsType);

                        // Registration
                        services.TryAddTransient(shortUpdateCommandHandlerServiceType, shortUpdateCommandHandlerImplementationType);

                        // -- Safe --
                        // ServiceType
                        var shortSafeUpdateCommandType = typeof(SafeUpdateCommand<>).MakeGenericType(crudApiEntityType);
                        var shortSafeUpdateCommandResponseType = typeof(IApizrResponse);
                        var shortSafeUpdateCommandHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(shortSafeUpdateCommandType, shortSafeUpdateCommandResponseType);

                        // ImplementationType
                        var shortSafeUpdateCommandHandlerImplementationType = typeof(SafeUpdateCommandHandler<,,,>).MakeGenericType(
                            crudApiEntityType,
                            crudApiEntityType,
                            crudApiEntityReadAllResultType,
                            crudApiEntityReadAllParamsType);

                        // Registration
                        services.TryAddTransient(shortSafeUpdateCommandHandlerServiceType, shortSafeUpdateCommandHandlerImplementationType); 

                        #endregion

                        #region Mapped

                        if (handleMapping)
                        {
                            // ServiceType
                            var shortUpdateMappedCommandType = typeof(UpdateCommand<>).MakeGenericType(crudModelEntityType);
                            var shortUpdateMappedCommandResponseType = typeof(Unit);
                            var shortUpdateMappedCommandHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(shortUpdateMappedCommandType, shortUpdateMappedCommandResponseType);

                            // ImplementationType
                            var shortUpdateMappedCommandHandlerImplementationType = typeof(UpdateCommandHandler<,,,>).MakeGenericType(
                                crudModelEntityType,
                                crudApiEntityType,
                                crudApiEntityReadAllResultType,
                                crudApiEntityReadAllParamsType);

                            // Registration
                            services.TryAddTransient(shortUpdateMappedCommandHandlerServiceType, shortUpdateMappedCommandHandlerImplementationType);

                            // -- Safe --
                            // ServiceType
                            var shortSafeUpdateMappedCommandType = typeof(SafeUpdateCommand<>).MakeGenericType(crudModelEntityType);
                            var shortSafeUpdateMappedCommandResponseType = typeof(IApizrResponse);
                            var shortSafeUpdateMappedCommandHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(shortSafeUpdateMappedCommandType, shortSafeUpdateMappedCommandResponseType);

                            // ImplementationType
                            var shortSafeUpdateMappedCommandHandlerImplementationType = typeof(SafeUpdateCommandHandler<,,,>).MakeGenericType(
                                crudModelEntityType,
                                crudApiEntityType,
                                crudApiEntityReadAllResultType,
                                crudApiEntityReadAllParamsType);

                            // Registration
                            services.TryAddTransient(shortSafeUpdateMappedCommandHandlerServiceType, shortSafeUpdateMappedCommandHandlerImplementationType);
                        }

                        #endregion
                    }

                    #endregion

                    #region Update

                    #region Unmapped

                    // ServiceType
                    var updateCommandType = typeof(UpdateCommand<,>).MakeGenericType(crudApiEntityKeyType, crudApiEntityType);
                    var updateCommandResponseType = typeof(Unit);
                    var updateCommandHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(updateCommandType, updateCommandResponseType);

                    // ImplementationType
                    var updateCommandHandlerImplementationType = typeof(UpdateCommandHandler<,,,,>).MakeGenericType(
                        crudApiEntityType,
                        crudApiEntityType,
                        crudApiEntityKeyType,
                        crudApiEntityReadAllResultType,
                        crudApiEntityReadAllParamsType);

                    // Registration
                    services.TryAddTransient(updateCommandHandlerServiceType, updateCommandHandlerImplementationType);

                    // -- Safe --
                    // ServiceType
                    var safeUpdateCommandType = typeof(SafeUpdateCommand<,>).MakeGenericType(crudApiEntityKeyType, crudApiEntityType);
                    var safeUpdateCommandResponseType = typeof(IApizrResponse);
                    var safeUpdateCommandHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(safeUpdateCommandType, safeUpdateCommandResponseType);

                    // ImplementationType
                    var safeUpdateCommandHandlerImplementationType = typeof(SafeUpdateCommandHandler<,,,,>).MakeGenericType(
                        crudApiEntityType,
                        crudApiEntityType,
                        crudApiEntityKeyType,
                        crudApiEntityReadAllResultType,
                        crudApiEntityReadAllParamsType);

                    // Registration
                    services.TryAddTransient(safeUpdateCommandHandlerServiceType, safeUpdateCommandHandlerImplementationType);

                    #endregion

                    #region Mapped

                    // ServiceType
                    var updateMappedCommandType = typeof(UpdateCommand<,>).MakeGenericType(crudApiEntityKeyType, crudModelEntityType);
                    var updateMappedCommandResponseType = typeof(Unit);
                    var updateMappedCommandHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(updateMappedCommandType, updateMappedCommandResponseType);

                    // ImplementationType
                    var updateMappedCommandHandlerImplementationType = typeof(UpdateCommandHandler<,,,,>).MakeGenericType(
                        crudModelEntityType,
                        crudApiEntityType,
                        crudApiEntityKeyType,
                        crudApiEntityReadAllResultType,
                        crudApiEntityReadAllParamsType);

                    // Registration
                    services.TryAddTransient(updateMappedCommandHandlerServiceType, updateMappedCommandHandlerImplementationType);

                    // -- Safe --
                    // ServiceType
                    var safeUpdateMappedCommandType = typeof(SafeUpdateCommand<,>).MakeGenericType(crudApiEntityKeyType, crudModelEntityType);
                    var safeUpdateMappedCommandResponseType = typeof(IApizrResponse);
                    var safeUpdateMappedCommandHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(safeUpdateMappedCommandType, safeUpdateMappedCommandResponseType);

                    // ImplementationType
                    var safeUpdateMappedCommandHandlerImplementationType = typeof(SafeUpdateCommandHandler<,,,,>).MakeGenericType(
                        crudModelEntityType,
                        crudApiEntityType,
                        crudApiEntityKeyType,
                        crudApiEntityReadAllResultType,
                        crudApiEntityReadAllParamsType);

                    // Registration
                    services.TryAddTransient(safeUpdateMappedCommandHandlerServiceType, safeUpdateMappedCommandHandlerImplementationType); 

                    #endregion

                    #endregion

                    #region ShortDelete

                    // Delete but short default version if concerned
                    if (crudApiEntityKeyType == typeof(int))
                    {
                        #region Unmapped

                        // ServiceType
                        var shortDeleteCommandType = typeof(DeleteCommand<>).MakeGenericType(crudApiEntityType);
                        var shortDeleteCommandResponseType = typeof(Unit);
                        var shortDeleteCommandHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(shortDeleteCommandType, shortDeleteCommandResponseType);

                        // ImplementationType
                        var shortDeleteCommandHandlerImplementationType = typeof(DeleteCommandHandler<,,,>).MakeGenericType(
                            crudApiEntityType,
                            crudApiEntityType,
                            crudApiEntityReadAllResultType,
                            crudApiEntityReadAllParamsType);

                        // Registration
                        services.TryAddTransient(shortDeleteCommandHandlerServiceType, shortDeleteCommandHandlerImplementationType);

                        // -- Safe --
                        // ServiceType
                        var shortSafeDeleteCommandType = typeof(SafeDeleteCommand<>).MakeGenericType(crudApiEntityType);
                        var shortSafeDeleteCommandResponseType = typeof(IApizrResponse);
                        var shortSafeDeleteCommandHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(shortSafeDeleteCommandType, shortSafeDeleteCommandResponseType);

                        // ImplementationType
                        var shortSafeDeleteCommandHandlerImplementationType = typeof(SafeDeleteCommandHandler<,,,>).MakeGenericType(
                            crudApiEntityType,
                            crudApiEntityType,
                            crudApiEntityReadAllResultType,
                            crudApiEntityReadAllParamsType);

                        // Registration
                        services.TryAddTransient(shortSafeDeleteCommandHandlerServiceType, shortSafeDeleteCommandHandlerImplementationType);

                        #endregion

                        #region Mapped

                        if (handleMapping)
                        {
                            // ServiceType
                            var shortDeleteMappedCommandType = typeof(DeleteCommand<>).MakeGenericType(crudModelEntityType);
                            var shortDeleteMappedCommandResponseType = typeof(Unit);
                            var shortDeleteMappedCommandHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(shortDeleteMappedCommandType, shortDeleteMappedCommandResponseType);

                            // ImplementationType
                            var shortDeleteMappedCommandHandlerImplementationType = typeof(DeleteCommandHandler<,,,>).MakeGenericType(
                                crudModelEntityType,
                                crudApiEntityType,
                                crudApiEntityReadAllResultType,
                                crudApiEntityReadAllParamsType);

                            // Registration
                            services.TryAddTransient(shortDeleteMappedCommandHandlerServiceType, shortDeleteMappedCommandHandlerImplementationType);

                            // -- Safe --
                            // ServiceType
                            var shortSafeDeleteMappedCommandType = typeof(SafeDeleteCommand<>).MakeGenericType(crudModelEntityType);
                            var shortSafeDeleteMappedCommandResponseType = typeof(IApizrResponse);
                            var shortSafeDeleteMappedCommandHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(shortSafeDeleteMappedCommandType, shortSafeDeleteMappedCommandResponseType);

                            // ImplementationType
                            var shortSafeDeleteMappedCommandHandlerImplementationType = typeof(SafeDeleteCommandHandler<,,,>).MakeGenericType(
                                crudModelEntityType,
                                crudApiEntityType,
                                crudApiEntityReadAllResultType,
                                crudApiEntityReadAllParamsType);

                            // Registration
                            services.TryAddTransient(shortSafeDeleteMappedCommandHandlerServiceType, shortSafeDeleteMappedCommandHandlerImplementationType);
                        }

                        #endregion
                    }

                    #endregion

                    #region Delete

                    #region Unmapped

                    // ServiceType
                    var deleteCommandType = typeof(DeleteCommand<,>).MakeGenericType(crudApiEntityType, crudApiEntityKeyType);
                    var deleteCommandResponseType = typeof(Unit);
                    var deleteCommandHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(deleteCommandType, deleteCommandResponseType);

                    // ImplementationType
                    var deleteCommandHandlerImplementationType = typeof(DeleteCommandHandler<,,,,>).MakeGenericType(
                        crudApiEntityType,
                        crudApiEntityType,
                        crudApiEntityKeyType,
                        crudApiEntityReadAllResultType,
                        crudApiEntityReadAllParamsType);

                    // Registration
                    services.TryAddTransient(deleteCommandHandlerServiceType, deleteCommandHandlerImplementationType);

                    // -- Safe --
                    // ServiceType
                    var safeDeleteCommandType = typeof(SafeDeleteCommand<,>).MakeGenericType(crudApiEntityType, crudApiEntityKeyType);
                    var safeDeleteCommandResponseType = typeof(IApizrResponse);
                    var safeDeleteCommandHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(safeDeleteCommandType, safeDeleteCommandResponseType);

                    // ImplementationType
                    var safeDeleteCommandHandlerImplementationType = typeof(SafeDeleteCommandHandler<,,,,>).MakeGenericType(
                        crudApiEntityType,
                        crudApiEntityType,
                        crudApiEntityKeyType,
                        crudApiEntityReadAllResultType,
                        crudApiEntityReadAllParamsType);

                    // Registration
                    services.TryAddTransient(safeDeleteCommandHandlerServiceType, safeDeleteCommandHandlerImplementationType);

                    #endregion

                    #region Mapped

                    if (handleMapping)
                    {
                        // ServiceType
                        var deleteMappedCommandType = typeof(DeleteCommand<,>).MakeGenericType(crudModelEntityType, crudApiEntityKeyType);
                        var deleteMappedCommandResponseType = typeof(Unit);
                        var deleteMappedCommandHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(deleteMappedCommandType, deleteMappedCommandResponseType);

                        // ImplementationType
                        var deleteMappedCommandHandlerImplementationType = typeof(DeleteCommandHandler<,,,,>).MakeGenericType(
                            crudModelEntityType,
                            crudApiEntityType,
                            crudApiEntityKeyType,
                            crudApiEntityReadAllResultType,
                            crudApiEntityReadAllParamsType);

                        // Registration
                        services.TryAddTransient(deleteMappedCommandHandlerServiceType, deleteMappedCommandHandlerImplementationType);

                        // -- Safe --
                        // ServiceType
                        var safeDeleteMappedCommandType = typeof(SafeDeleteCommand<,>).MakeGenericType(crudModelEntityType, crudApiEntityKeyType);
                        var safeDeleteMappedCommandResponseType = typeof(IApizrResponse);
                        var safeDeleteMappedCommandHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(safeDeleteMappedCommandType, safeDeleteMappedCommandResponseType);

                        // ImplementationType
                        var safeDeleteMappedCommandHandlerImplementationType = typeof(SafeDeleteCommandHandler<,,,,>).MakeGenericType(
                            crudModelEntityType,
                            crudApiEntityType,
                            crudApiEntityKeyType,
                            crudApiEntityReadAllResultType,
                            crudApiEntityReadAllParamsType);

                        // Registration
                        services.TryAddTransient(safeDeleteMappedCommandHandlerServiceType, safeDeleteMappedCommandHandlerImplementationType);
                    }

                    #endregion

                    #endregion

                    #region Typed

                    // Typed crud mediator
                    var typedCrudMediatorServiceType = typeof(IApizrCrudMediator<,,,>).MakeGenericType(crudApiEntityType,
                        crudApiEntityKeyType,
                        crudApiEntityReadAllResultType,
                        crudApiEntityReadAllParamsType);
                    var typedCrudMediatorImplementationType = typeof(ApizrCrudMediator<,,,>).MakeGenericType(crudApiEntityType,
                        crudApiEntityKeyType,
                        crudApiEntityReadAllResultType,
                        crudApiEntityReadAllParamsType);

                    // Register typed crud mediator
                    services.TryAddTransient(typedCrudMediatorServiceType, typedCrudMediatorImplementationType);

                    // Get or create and register a mediation registry
                    if (!commonOptions.PostRegistries.TryGetValue(typeof(IApizrMediationConcurrentRegistry), out var commonRegistry))
                    {
                        var mediationRegistry = new ApizrMediationRegistry();
                        commonRegistry = mediationRegistry;
                        commonOptions.PostRegistries.Add(typeof(IApizrMediationConcurrentRegistry), commonRegistry);
                        services.TryAddSingleton(serviceProvider => mediationRegistry.GetInstance(serviceProvider));
                    }

                    // Add or update the mediator service into the registry
                    commonRegistry.AddOrUpdateManager(typedCrudMediatorServiceType);

                    #endregion
                }

                #endregion

                #region Classic

                // Register mediator
                services.TryAddSingleton<IApizrMediator, ApizrMediator>();
                
                // Classic interfaces auto registration
                // Request handlers registration
                foreach (var methodInfo in GetMethods(managerOptions.WebApiType))
                {
                    var returnType = methodInfo.ReturnType;

                    #region Result

                    if (returnType.IsGenericType && 
                        returnType != typeof(Task<IApiResponse>) &&
                        (returnType.GetGenericTypeDefinition() == typeof(Task<>)
                         || returnType.GetGenericTypeDefinition() == typeof(IObservable<>)))
                    {
                        var apiResponseType = returnType.GetGenericArguments()[0];

                        #region Unmapped

                        // ExecuteResultRequest<TWebApi, TApiData>
                        if (apiResponseType.IsGenericType &&
                            (apiResponseType.GetGenericTypeDefinition() == typeof(ApiResponse<>)
                             || apiResponseType.GetGenericTypeDefinition() == typeof(IApiResponse<>)))
                        {
                            apiResponseType = apiResponseType.GetGenericArguments()[0];
                        }

                        // ServiceType
                        var executeRequestType = typeof(ExecuteResultRequest<,>).MakeGenericType(managerOptions.WebApiType, apiResponseType);
                        var executeRequestHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(executeRequestType, apiResponseType);

                        // ImplementationType
                        var executeRequestHandlerImplementationType = typeof(ExecuteResultRequestHandler<,>).MakeGenericType(managerOptions.WebApiType, apiResponseType);

                        // Registration
                        services.TryAddTransient(executeRequestHandlerServiceType, executeRequestHandlerImplementationType);

                        // -- Safe --
                        // ServiceType
                        var safeExecuteRequestType = typeof(ExecuteSafeResultRequest<,>).MakeGenericType(managerOptions.WebApiType, apiResponseType); 
                        var safeExecuteResponseType = typeof(IApizrResponse<>).MakeGenericType(apiResponseType);
                        var safeExecuteRequestHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(safeExecuteRequestType, safeExecuteResponseType);

                        // ImplementationType
                        var safeExecuteRequestHandlerImplementationType = typeof(ExecuteSafeResultRequestHandler<,>).MakeGenericType(managerOptions.WebApiType, apiResponseType);

                        // Registration
                        services.TryAddTransient(safeExecuteRequestHandlerServiceType, safeExecuteRequestHandlerImplementationType);

                        #endregion

                        #region Mapped

                        if (isMappingHandlerRegistered)
                        {
                            // Mapped response
                            var modelResponseType = methodInfo.GetCustomAttribute<MappedWithAttribute>()?.SecondEntityType ??
                                mappedTypes.FirstOrDefault(attribute => attribute.FirstEntityType == apiResponseType)?.SecondEntityType ??
                                mappedTypes.FirstOrDefault(attribute => attribute.SecondEntityType == apiResponseType)?.FirstEntityType;
                            if (modelResponseType != null)
                            {
                                // ExecuteResultRequest<TWebApi, TModelData, TApiData>
                                // ServiceType
                                var executeMappedResultRequestType = typeof(ExecuteResultRequest<,,>).MakeGenericType(managerOptions.WebApiType, modelResponseType, apiResponseType);
                                var executeMappedResultRequestHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(executeMappedResultRequestType, modelResponseType);

                                // ImplementationType
                                var executeMappedResultRequestHandlerImplementationType = typeof(ExecuteResultRequestHandler<,,>).MakeGenericType(managerOptions.WebApiType, modelResponseType, apiResponseType);

                                // Registration
                                services.TryAddTransient(executeMappedResultRequestHandlerServiceType, executeMappedResultRequestHandlerImplementationType);

                                // -- Safe --
                                // ServiceType
                                var safeExecuteMappedResultRequestType = typeof(ExecuteSafeResultRequest<,,>).MakeGenericType(managerOptions.WebApiType, modelResponseType, apiResponseType);
                                var safeExecuteMappedResultResponseType = typeof(IApizrResponse<>).MakeGenericType(modelResponseType);
                                var safeExecuteMappedResultRequestHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(safeExecuteMappedResultRequestType, safeExecuteMappedResultResponseType);

                                // ImplementationType
                                var safeExecuteMappedResultRequestHandlerImplementationType = typeof(ExecuteSafeResultRequestHandler<,,>).MakeGenericType(managerOptions.WebApiType, modelResponseType, apiResponseType);

                                // Registration
                                services.TryAddTransient(safeExecuteMappedResultRequestHandlerServiceType, safeExecuteMappedResultRequestHandlerImplementationType);

                                // Mapped request
                                var requestMappedWithAttribute = methodInfo.GetParameters()
                                    .Where(parameterInfo => parameterInfo.ParameterType is
                                        { IsClass: true, IsAbstract: false })
                                    .Select(parameterInfo => new MappedWithAttribute(parameterInfo.ParameterType,
                                        parameterInfo.GetCustomAttribute<MappedWithAttribute>()?.SecondEntityType ??
                                        mappedTypes.FirstOrDefault(attribute => attribute.FirstEntityType == parameterInfo.ParameterType)?.SecondEntityType ??
                                        mappedTypes.FirstOrDefault(attribute => attribute.SecondEntityType == parameterInfo.ParameterType)?.FirstEntityType))
                                    .FirstOrDefault(item => item.SecondEntityType != null);

                                if (requestMappedWithAttribute != null) // ExecuteResultRequest<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>
                                {
                                    // Mapped request
                                    var apiRequestType = requestMappedWithAttribute.FirstEntityType;
                                    var modelRequestType = requestMappedWithAttribute.SecondEntityType;

                                    // ServiceType
                                    var executeMappedRequestType = typeof(ExecuteResultRequest<,,,,>).MakeGenericType(managerOptions.WebApiType, modelResponseType, apiResponseType, apiRequestType, modelRequestType);
                                    var executeMappedRequestHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(executeMappedRequestType, modelResponseType);

                                    // ImplementationType
                                    var executeMappedRequestHandlerImplementationType = typeof(ExecuteResultRequestHandler<,,,,>).MakeGenericType(managerOptions.WebApiType, modelResponseType, apiResponseType, apiRequestType, modelRequestType);

                                    // Registration
                                    services.TryAddTransient(executeMappedRequestHandlerServiceType, executeMappedRequestHandlerImplementationType);

                                    // -- Safe --
                                    // ServiceType
                                    var safeExecuteMappedRequestType = typeof(ExecuteSafeResultRequest<,,,,>).MakeGenericType(managerOptions.WebApiType, modelResponseType, apiResponseType, apiRequestType, modelRequestType);
                                    var safeExecuteMappedResponseType = typeof(IApizrResponse<>).MakeGenericType(modelResponseType);
                                    var safeExecuteMappedRequestHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(safeExecuteMappedRequestType, safeExecuteMappedResponseType);

                                    // ImplementationType
                                    var safeExecuteMappedRequestHandlerImplementationType = typeof(ExecuteSafeResultRequestHandler<,,,,>).MakeGenericType(managerOptions.WebApiType, modelResponseType, apiResponseType, apiRequestType, modelRequestType);

                                    // Registration
                                    services.TryAddTransient(safeExecuteMappedRequestHandlerServiceType, safeExecuteMappedRequestHandlerImplementationType);
                                }
                            }
                        }

                        #endregion
                    }

                    #endregion

                    #region Unit

                    else if (returnType == typeof(Task<IApiResponse>))
                    {
                        // -- Safe --

                        #region Unmapped

                        // ServiceType
                        var safeExecuteRequestType = typeof(ExecuteSafeUnitRequest<>).MakeGenericType(managerOptions.WebApiType);
                        var safeExecuteRequestResponseType = typeof(IApizrResponse);
                        var safeExecuteRequestHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(safeExecuteRequestType, safeExecuteRequestResponseType);

                        // ImplementationType
                        var safeExecuteRequestHandlerImplementationType = typeof(ExecuteSafeUnitRequestHandler<>).MakeGenericType(managerOptions.WebApiType);

                        // Registration
                        services.TryAddTransient(safeExecuteRequestHandlerServiceType, safeExecuteRequestHandlerImplementationType);

                        #endregion

                        #region Mapped

                        if (isMappingHandlerRegistered)
                        {
                            // Mapped request
                            var requestMappedWithAttribute = methodInfo.GetParameters()
                                .Where(parameterInfo => parameterInfo.ParameterType is
                                    { IsClass: true, IsAbstract: false })
                                .Select(parameterInfo => new MappedWithAttribute(parameterInfo.ParameterType,
                                    parameterInfo.GetCustomAttribute<MappedWithAttribute>()?.SecondEntityType ??
                                    mappedTypes.FirstOrDefault(attribute => attribute.FirstEntityType == parameterInfo.ParameterType)?.SecondEntityType ??
                                    mappedTypes.FirstOrDefault(attribute => attribute.SecondEntityType == parameterInfo.ParameterType)?.FirstEntityType))
                                .FirstOrDefault(item => item.SecondEntityType != null);

                            if (requestMappedWithAttribute != null)
                            {
                                var apiRequestType = requestMappedWithAttribute.FirstEntityType;
                                var modelRequestType = requestMappedWithAttribute.SecondEntityType;

                                // ServiceType
                                var safeExecuteMappedRequestType = typeof(ExecuteSafeUnitRequest<,,>).MakeGenericType(managerOptions.WebApiType, modelRequestType, apiRequestType);
                                var safeExecuteMappedRequestResponseType = typeof(IApizrResponse);
                                var safeExecuteMappedRequestHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(safeExecuteMappedRequestType, safeExecuteMappedRequestResponseType);

                                // ImplementationType
                                var safeExecuteMappedRequestHandlerImplementationType = typeof(ExecuteSafeUnitRequestHandler<,,>).MakeGenericType(managerOptions.WebApiType, modelRequestType, apiRequestType);

                                // Registration
                                services.TryAddTransient(safeExecuteMappedRequestHandlerServiceType, safeExecuteMappedRequestHandlerImplementationType);
                            }
                        }

                        #endregion
                    }

                    else if (returnType == typeof(Task))
                    {
                        #region Unmapped

                        // ServiceType
                        var executeRequestType = typeof(ExecuteUnitRequest<>).MakeGenericType(managerOptions.WebApiType);
                        var executeRequestResponseType = typeof(Unit);
                        var executeRequestHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(executeRequestType, executeRequestResponseType);

                        // ImplementationType
                        var executeRequestHandlerImplementationType = typeof(ExecuteUnitRequestHandler<>).MakeGenericType(managerOptions.WebApiType);

                        // Registration
                        services.TryAddTransient(executeRequestHandlerServiceType, executeRequestHandlerImplementationType);

                        #endregion

                        #region Mapped

                        if (isMappingHandlerRegistered)
                        {
                            // Mapped request
                            var requestMappedWithAttribute = methodInfo.GetParameters()
                                .Where(parameterInfo => parameterInfo.ParameterType is
                                    { IsClass: true, IsAbstract: false })
                                .Select(parameterInfo => new MappedWithAttribute(parameterInfo.ParameterType,
                                    parameterInfo.GetCustomAttribute<MappedWithAttribute>()?.SecondEntityType ??
                                    mappedTypes.FirstOrDefault(attribute => attribute.FirstEntityType == parameterInfo.ParameterType)?.SecondEntityType ??
                                    mappedTypes.FirstOrDefault(attribute => attribute.SecondEntityType == parameterInfo.ParameterType)?.FirstEntityType))
                                .FirstOrDefault(item => item.SecondEntityType != null);

                            if (requestMappedWithAttribute != null)
                            {
                                var apiRequestType = requestMappedWithAttribute.FirstEntityType;
                                var modelRequestType = requestMappedWithAttribute.SecondEntityType;

                                // ServiceType
                                var executeMappedRequestType = typeof(ExecuteUnitRequest<,,>).MakeGenericType(managerOptions.WebApiType, modelRequestType, apiRequestType);
                                var executeMappedRequestHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(executeMappedRequestType, typeof(Unit));

                                // ImplementationType
                                var executeMappedRequestHandlerImplementationType = typeof(ExecuteUnitRequestHandler<,,>).MakeGenericType(managerOptions.WebApiType, modelRequestType, apiRequestType);

                                // Registration
                                services.TryAddTransient(executeMappedRequestHandlerServiceType, executeMappedRequestHandlerImplementationType);
                            }
                        }

                        #endregion
                    }

                    #endregion
                }

                #region Typed

                // Typed mediator
                var typedMediatorServiceType = typeof(IApizrMediator<>).MakeGenericType(managerOptions.WebApiType);
                var typedMediatorImplementationType = typeof(ApizrMediator<>).MakeGenericType(managerOptions.WebApiType);

                // Register typed mediator
                services.TryAddSingleton(typedMediatorServiceType, typedMediatorImplementationType);

                // Get or create and register a mediation registry
                if (!commonOptions.PostRegistries.TryGetValue(typeof(IApizrMediationConcurrentRegistry), out var registry))
                {
                    var mediationRegistry = new ApizrMediationRegistry();
                    registry = mediationRegistry;
                    commonOptions.PostRegistries.Add(typeof(IApizrMediationConcurrentRegistry), registry);
                    services.TryAddSingleton(serviceProvider => mediationRegistry.GetInstance(serviceProvider));
                }

                // Add or update the mediator service into the registry
                registry.AddOrUpdateManager(typedMediatorServiceType);

                #endregion

                #endregion
            });
        }
        
        #region Internal

        internal static IList<MethodInfo> GetMethods(Type webApiType)
        {
            var methods = webApiType.GetMethods().ToList();

            foreach (var parentInterface in webApiType.GetInterfaces())
            {
                var parentMethods = parentInterface.GetMethods();
                if (parentMethods.Any())
                    methods.AddRange(parentMethods);
            }

            return methods;
        }

        #endregion
    }
}
