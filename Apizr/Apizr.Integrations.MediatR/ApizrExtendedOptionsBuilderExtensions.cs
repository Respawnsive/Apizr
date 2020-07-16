using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Apizr.Mediation.Cruding;
using Apizr.Mediation.Cruding.Base;
using Apizr.Mediation.Cruding.Handling;
using Apizr.Mediation.Cruding.Handling.Base;
using Apizr.Mediation.Requesting;
using Apizr.Mediation.Requesting.Handling;
using Apizr.Requesting;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Refit;

namespace Apizr
{
    public static class ApizrExtendedOptionsBuilderExtensions
    {
        public static IApizrExtendedOptionsBuilder WithMediation(this IApizrExtendedOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ApizrOptions.PostRegistrationActions.Add(services =>
            {
                #region Crud

                // Crud entities auto registration
                foreach (var crudEntity in optionsBuilder.ApizrOptions.CrudEntities)
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
                }

                #endregion

                #region Classic

                // Classic interfaces auto registration
                foreach (var webApi in optionsBuilder.ApizrOptions.WebApis)
                {
                    foreach (var methodInfo in webApi.Key.GetMethods())
                    {
                        var returnType = methodInfo.ReturnType;
                        if (returnType.IsGenericType &&
                            (methodInfo.ReturnType.GetGenericTypeDefinition() != typeof(Task<>)
                             || methodInfo.ReturnType.GetGenericTypeDefinition() != typeof(IObservable<>)))
                        {
                            var returnResultType = returnType.GetGenericArguments()[0];
                            if (returnResultType.IsGenericType &&
                                (returnResultType.GetGenericTypeDefinition() == typeof(ApiResponse<>)
                                 || returnResultType.GetGenericTypeDefinition() == typeof(IApiResponse<>)))
                            {
                                returnResultType = returnResultType.GetGenericArguments()[0];
                            }
                            else if (returnResultType == typeof(IApiResponse))
                            {
                                returnResultType = typeof(HttpContent);
                            }

                            // ServiceType
                            var executeRequestType = typeof(ExecuteRequest<,>).MakeGenericType(optionsBuilder.ApizrOptions.WebApiType, returnResultType);
                            var executeRequestHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(executeRequestType, returnResultType);

                            // ImplementationType
                            var executeRequestHandlerImplementationType = typeof(ExecuteRequestHandler<,>).MakeGenericType(optionsBuilder.ApizrOptions.WebApiType, returnResultType);

                            // Registration
                            services.TryAddTransient(executeRequestHandlerServiceType, executeRequestHandlerImplementationType);
                        }
                        else if (returnType == typeof(Task))
                        {
                            // ServiceType
                            var executeRequestType = typeof(ExecuteRequest<>).MakeGenericType(optionsBuilder.ApizrOptions.WebApiType);
                            var executeRequestResponseType = typeof(Unit);
                            var executeRequestHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(executeRequestType, executeRequestResponseType);

                            // ImplementationType
                            var executeRequestHandlerImplementationType = typeof(ExecuteRequestHandler<>).MakeGenericType(optionsBuilder.ApizrOptions.WebApiType);

                            // Registration
                            services.TryAddTransient(executeRequestHandlerServiceType, executeRequestHandlerImplementationType);
                        }
                    }
                } 

                #endregion
            });

            return optionsBuilder;
        }

        public static IApizrExtendedOptionsBuilder WithCrudMediation(this IApizrExtendedOptionsBuilder optionsBuilder,
            IServiceCollection services, Type apiEntityType, CrudEntityAttribute apiEntityAttribute,  IDictionary<Type, Type> validRequestAndHandlerTypes, IDictionary<Type, Type> requestResponseTypes)
        {
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
                var shortReadCombination = validRequestAndHandlerTypes.FirstOrDefault(kvp =>
                    typeof(ReadQueryBase<>).IsAssignableFromGenericType(kvp.Key));

                // ServiceType
                var shortGenericReadQueryType = shortReadCombination.Key ?? typeof(ReadQuery<>);
                var shortReadQueryType = shortGenericReadQueryType.MakeGenericType(modelEntityType);
                var shortReadQueryResponseType = requestResponseTypes.First(kvp => kvp.Key == typeof(ReadQueryBase<,>)).Value;
                var shortReadQueryHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(shortReadQueryType, shortReadQueryResponseType);

                // ImplementationType
                var shortGenericReadQueryHandlerType = shortReadCombination.Value ?? typeof(ReadQueryHandler<,,,>);
                var shortReadQueryHandlerImplementationType = shortGenericReadQueryHandlerType.MakeGenericType(
                    modelEntityType,
                    apiEntityType,
                    apiEntityReadAllResultType,
                    apiEntityReadAllParamsType);

                // Registration
                services.TryAddTransient(shortReadQueryHandlerServiceType, shortReadQueryHandlerImplementationType);
            }

            #endregion

            #region Read

            // Read
            var readCombination = validRequestAndHandlerTypes.FirstOrDefault(kvp =>
                typeof(ReadQueryBase<,>).IsAssignableFromGenericType(kvp.Key) && !typeof(ReadQueryBase<>).IsAssignableFromGenericType(kvp.Key));

            // ServiceType
            var genericReadQueryType = readCombination.Key ?? typeof(ReadQuery<,>);
            var readQueryType = genericReadQueryType.MakeGenericType(modelEntityType, apiEntityKeyType);
            var readQueryResponseType = requestResponseTypes.First(kvp => kvp.Key == typeof(ReadQueryBase<,>)).Value;
            var readQueryHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(readQueryType, readQueryResponseType);

            // ImplementationType
            var genericReadQueryHandlerType = readCombination.Value ?? typeof(ReadQueryHandler<,,,,>);
            var readQueryHandlerImplementationType = genericReadQueryHandlerType.MakeGenericType(
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
                var shortReadAllCombination = validRequestAndHandlerTypes.FirstOrDefault(kvp =>
                    typeof(ReadAllQueryBase<>).IsAssignableFromGenericType(kvp.Key));

                // ServiceType
                var shortGenericReadAllQueryType = shortReadAllCombination.Key ?? typeof(ReadAllQuery<>);
                var shortReadAllQueryType = shortGenericReadAllQueryType.MakeGenericType(modelEntityReadAllResultType);
                var shortReadAllQueryResponseType = requestResponseTypes.First(kvp => kvp.Key == typeof(ReadAllQueryBase<,>)).Value;
                var shortReadAllQueryHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(shortReadAllQueryType, shortReadAllQueryResponseType);

                // ImplementationType
                var shortGenericReadAllQueryHandlerType = shortReadAllCombination.Value ?? typeof(ReadAllQueryHandler<,,,>);
                var shortReadAllQueryHandlerImplementationType = shortGenericReadAllQueryHandlerType.MakeGenericType(
                    apiEntityType,
                    apiEntityKeyType,
                    modelEntityReadAllResultType,
                    apiEntityReadAllResultType);

                // Registration
                services.TryAddTransient(shortReadAllQueryHandlerServiceType, shortReadAllQueryHandlerImplementationType);
            }

            #endregion

            #region ReadAll

            // ReadAll
            var readAllCombination = validRequestAndHandlerTypes.FirstOrDefault(kvp =>
                typeof(ReadAllQueryBase<,>).IsAssignableFromGenericType(kvp.Key) && !typeof(ReadAllQueryBase<>).IsAssignableFromGenericType(kvp.Key));

            // ServiceType
            var genericReadAllQueryType = readAllCombination.Key ?? typeof(ReadAllQuery<,>);
            var readAllQueryType = genericReadAllQueryType.MakeGenericType(apiEntityReadAllParamsType, modelEntityReadAllResultType);
            var readAllQueryResponseType = requestResponseTypes.First(kvp => kvp.Key == typeof(ReadAllQueryBase<,>)).Value;
            var readAllQueryHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(readAllQueryType, readAllQueryResponseType);

            // ImplementationType
            var genericReadAllQueryHandlerType = readAllCombination.Value ??typeof(ReadAllQueryHandler<,,,,>);
            var readAllQueryHandlerImplementationType = genericReadAllQueryHandlerType.MakeGenericType(
                apiEntityType,
                apiEntityKeyType,
                modelEntityReadAllResultType,
                apiEntityReadAllResultType,
                apiEntityReadAllParamsType);

            // Registration
            services.TryAddTransient(readAllQueryHandlerServiceType, readAllQueryHandlerImplementationType);

            #endregion

            #region Create

            // Create
            var createCombination = validRequestAndHandlerTypes.FirstOrDefault(kvp =>
                typeof(CreateCommandBase<,>).IsAssignableFromGenericType(kvp.Key));

            // ServiceType
            var genericCreateCommandType = createCombination.Key ?? typeof(CreateCommand<>);
            var createCommandType = genericCreateCommandType.MakeGenericType(modelEntityType);
            var createCommandResponseType = requestResponseTypes.First(kvp => kvp.Key == typeof(CreateCommandBase<,>)).Value;
            var createCommandHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(createCommandType, createCommandResponseType);

            // ImplementationType
            var genericCreateCommandHandlerType = createCombination.Value ?? typeof(CreateCommandHandler<,,,,>);
            var createCommandHandlerImplementationType = genericCreateCommandHandlerType.MakeGenericType(
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
                var shortUpdateCombination = validRequestAndHandlerTypes.FirstOrDefault(kvp =>
                    typeof(UpdateCommandBase<,>).IsAssignableFromGenericType(kvp.Key));

                // ServiceType
                var shortGenericUpdateCommandType = shortUpdateCombination.Key ?? typeof(UpdateCommand<>);
                var shortUpdateCommandType = shortGenericUpdateCommandType.MakeGenericType(modelEntityType);
                var shortUpdateCommandResponseType = requestResponseTypes.First(kvp => kvp.Key == typeof(UpdateCommandBase<,,>)).Value;
                var shortUpdateCommandHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(shortUpdateCommandType, shortUpdateCommandResponseType);

                // ImplementationType
                var shortGenericUpdateCommandHandlerType = shortUpdateCombination.Value ?? typeof(UpdateCommandHandler<,,,>);
                var shortUpdateCommandHandlerImplementationType = shortGenericUpdateCommandHandlerType.MakeGenericType(
                    modelEntityType,
                    apiEntityType,
                    apiEntityReadAllResultType,
                    apiEntityReadAllParamsType);

                // Registration
                services.TryAddTransient(shortUpdateCommandHandlerServiceType, shortUpdateCommandHandlerImplementationType);
            }

            #endregion

            #region Update

            // Update
            var updateCombination = validRequestAndHandlerTypes.FirstOrDefault(kvp =>
                typeof(UpdateCommandBase<,,>).IsAssignableFromGenericType(kvp.Key) && !typeof(UpdateCommandBase<,>).IsAssignableFromGenericType(kvp.Key));

            // ServiceType
            var genericUpdateCommandType = updateCombination.Key ?? typeof(UpdateCommand<,>);
            var updateCommandType = genericUpdateCommandType.MakeGenericType(apiEntityKeyType, modelEntityType);
            var updateCommandResponseType = requestResponseTypes.First(kvp => kvp.Key == typeof(UpdateCommandBase<,,>)).Value;
            var updateCommandHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(updateCommandType, updateCommandResponseType);

            // ImplementationType
            var genericUpdateCommandHandlerType = updateCombination.Value ?? typeof(UpdateCommandHandler<,,,,>);
            var updateCommandHandlerImplementationType = genericUpdateCommandHandlerType.MakeGenericType(
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
                var shortDeleteCombination = validRequestAndHandlerTypes.FirstOrDefault(kvp =>
                    typeof(DeleteCommandBase<,>).IsAssignableFromGenericType(kvp.Key));

                // ServiceType
                var shortGenericDeleteCommandType = shortDeleteCombination.Key ?? typeof(DeleteCommand<>);
                var shortDeleteCommandType = shortGenericDeleteCommandType.MakeGenericType(modelEntityType);
                var shortDeleteCommandResponseType = requestResponseTypes.First(kvp => kvp.Key == typeof(DeleteCommandBase<,,>)).Value;
                var shortDeleteCommandHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(shortDeleteCommandType, shortDeleteCommandResponseType);

                // ImplementationType
                var shortGenericDeleteCommandHandlerType = shortDeleteCombination.Value ?? typeof(DeleteCommandHandler<,,,>);
                var shortDeleteCommandHandlerImplementationType = shortGenericDeleteCommandHandlerType.MakeGenericType(
                    modelEntityType,
                    apiEntityType,
                    apiEntityReadAllResultType,
                    apiEntityReadAllParamsType);

                // Registration
                services.TryAddTransient(shortDeleteCommandHandlerServiceType, shortDeleteCommandHandlerImplementationType);
            }

            #endregion

            #region Delete

            // Delete
            var deleteCombination = validRequestAndHandlerTypes.FirstOrDefault(kvp =>
                typeof(DeleteCommandBase<,,>).IsAssignableFromGenericType(kvp.Key) && !typeof(DeleteCommandBase<,>).IsAssignableFromGenericType(kvp.Key));

            // ServiceType
            var genericDeleteCommandType = deleteCombination.Key ?? typeof(DeleteCommand<,>);
            var deleteCommandType = genericDeleteCommandType.MakeGenericType(modelEntityType, apiEntityKeyType);
            var deleteCommandResponseType = requestResponseTypes.First(kvp => kvp.Key == typeof(DeleteCommandBase<,,>)).Value;
            var deleteCommandHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(deleteCommandType, deleteCommandResponseType);

            // ImplementationType
            var genericDeleteCommandHandlerType = deleteCombination.Value ?? typeof(DeleteCommandHandler<,,,,>);
            var deleteCommandHandlerImplementationType = genericDeleteCommandHandlerType.MakeGenericType(
                modelEntityType,
                apiEntityType, 
                apiEntityKeyType,
                apiEntityReadAllResultType,
                apiEntityReadAllParamsType);

            // Registration
            services.TryAddTransient(deleteCommandHandlerServiceType, deleteCommandHandlerImplementationType);

            #endregion

            return optionsBuilder;
        }
    }
}
