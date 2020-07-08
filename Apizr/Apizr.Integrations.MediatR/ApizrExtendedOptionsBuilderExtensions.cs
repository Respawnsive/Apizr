using System;
using System.Collections.Generic;
using System.Linq;
using Apizr.Mediation.Cruding;
using Apizr.Mediation.Cruding.Base;
using Apizr.Mediation.Cruding.Handling;
using Apizr.Mediation.Cruding.Handling.Base;
using Apizr.Requesting;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Apizr
{
    public static class ApizrExtendedOptionsBuilderExtensions
    {
        /// <summary>
        /// Let Apizr handle crud requests execution with mediation
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <returns></returns>
        public static IApizrExtendedOptionsBuilder WithCrudMediation(this IApizrExtendedOptionsBuilder optionsBuilder)
        {
            return WithCrudMediation(optionsBuilder, default(Type[]));
        }

        /// <summary>
        /// Let Apizr handle crud requests execution with mediation, but with your own requests handlers
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <param name="crudHandlerTypes">Requests handlers types</param>
        /// <returns></returns>
        public static IApizrExtendedOptionsBuilder WithCrudMediation(this IApizrExtendedOptionsBuilder optionsBuilder, params Type[] crudHandlerTypes)
        {
            // Checking types validity
            var validCrudHandlerTypes = new Dictionary<Type, Type>();
            if (crudHandlerTypes != default(Type[]))
            {
                foreach (var crudHandlerType in crudHandlerTypes.Where(t => t != default).Distinct())
                {
                    if (crudHandlerType.IsOpenGeneric())
                    {
                        if (typeof(ReadQueryHandlerBase<,,,,,>).IsAssignableFromGenericType(crudHandlerType))
                            validCrudHandlerTypes.Add(typeof(ReadQuery<>), crudHandlerType);
                        else if (typeof(ReadQueryHandlerBase<,,,,,,>).IsAssignableFromGenericType(crudHandlerType))
                            validCrudHandlerTypes.Add(typeof(ReadQuery<,>), crudHandlerType);
                        else if (typeof(ReadAllQueryHandlerBase<,,,,,>).IsAssignableFromGenericType(crudHandlerType))
                            validCrudHandlerTypes.Add(typeof(ReadAllQuery<>), crudHandlerType);
                        else if (typeof(ReadAllQueryHandlerBase<,,,,,,>).IsAssignableFromGenericType(crudHandlerType))
                            validCrudHandlerTypes.Add(typeof(ReadAllQuery<,>), crudHandlerType);
                        else if (typeof(CreateCommandHandlerBase<,,,,,,>).IsAssignableFromGenericType(crudHandlerType))
                            validCrudHandlerTypes.Add(typeof(CreateCommand<>), crudHandlerType);
                        else if (typeof(UpdateCommandHandlerBase<,,,,>).IsAssignableFromGenericType(crudHandlerType))
                            validCrudHandlerTypes.Add(typeof(UpdateCommand<>), crudHandlerType);
                        else if (typeof(UpdateCommandHandlerBase<,,,,,,>).IsAssignableFromGenericType(crudHandlerType))
                            validCrudHandlerTypes.Add(typeof(UpdateCommand<,>), crudHandlerType);
                        else if (typeof(DeleteCommandHandlerBase<,,,,,>).IsAssignableFromGenericType(crudHandlerType))
                            validCrudHandlerTypes.Add(typeof(DeleteCommand<>), crudHandlerType);
                        else if (typeof(DeleteCommandHandlerBase<,,,,,,>).IsAssignableFromGenericType(crudHandlerType))
                            validCrudHandlerTypes.Add(typeof(DeleteCommand<,>), crudHandlerType);
                        else
                            throw new ArgumentException(
                                $"{crudHandlerType.GetFriendlyName()} must inherit from " +
                                $"{typeof(ReadQueryHandlerBase<,,,,,>).GetFriendlyName()}, {typeof(ReadQueryHandlerBase<,,,,,,>)}, " +
                                $"{typeof(ReadAllQueryHandlerBase<,,,,,>).GetFriendlyName()}, {typeof(ReadAllQueryHandlerBase<,,,,,,>)}, " +
                                $"{typeof(CreateCommandHandlerBase<,,,,,,>).GetFriendlyName()}, " +
                                $"{typeof(UpdateCommandHandlerBase<,,,,>).GetFriendlyName()}, {typeof(UpdateCommandHandlerBase<,,,,,,>)},  " +
                                $"{typeof(DeleteCommandHandlerBase<,,,,,>).GetFriendlyName()} or {typeof(DeleteCommandHandlerBase<,,,,,,>)}",
                                nameof(crudHandlerType));
                    }
                    else
                        throw new ArgumentException(
                            $"{crudHandlerType.GetFriendlyName()} must be open generic",
                            nameof(crudHandlerTypes));
                } 
            }

            var crudRequestAndHandlerTypes = validCrudHandlerTypes.Select(kvp => (kvp.Key, kvp.Value)).ToArray();

            return WithCrudMediation(optionsBuilder,  crudRequestAndHandlerTypes);
        }

        /// <summary>
        /// Let Apizr handle crud requests execution with mediation, but with your own requests and requests handlers
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <param name="crudRequestAndHandlerTypes">Requests and requests handlers types</param>
        /// <returns></returns>
        public static IApizrExtendedOptionsBuilder WithCrudMediation(this IApizrExtendedOptionsBuilder optionsBuilder, params (Type, Type)[] crudRequestAndHandlerTypes)
        {
            // Checking types validity
            var validRequestAndHandlerTypes = new Dictionary<Type, Type>();
            if (crudRequestAndHandlerTypes != null)
            {
                foreach (var requestAndHandlerTypeCombination in crudRequestAndHandlerTypes)
                {
                    if(validRequestAndHandlerTypes.ContainsKey(requestAndHandlerTypeCombination.Item1))
                        continue;

                    if (requestAndHandlerTypeCombination.Item1.IsOpenGeneric())
                    {
                        if (typeof(ReadQueryBase<>).IsAssignableFromGenericType(requestAndHandlerTypeCombination.Item1))
                        {
                            if (requestAndHandlerTypeCombination.Item2.IsOpenGeneric())
                            {
                                if (typeof(ReadQueryHandlerBase<,,,,,>).IsAssignableFromGenericType(
                                    requestAndHandlerTypeCombination.Item2))
                                {
                                    validRequestAndHandlerTypes.Add(requestAndHandlerTypeCombination.Item1,
                                        requestAndHandlerTypeCombination.Item2);
                                }
                                else
                                    throw new ArgumentException(
                                        $"{requestAndHandlerTypeCombination.Item2.GetFriendlyName()} must inherit from {typeof(ReadQueryHandlerBase<,,,,,>).GetFriendlyName()} to handle {typeof(ReadQueryBase<>).GetFriendlyName()}");
                            }
                            else
                                throw new ArgumentException(
                                    $"{requestAndHandlerTypeCombination.Item2.GetFriendlyName()} must be open generic");
                        }
                        else if (typeof(ReadQueryBase<,>).IsAssignableFromGenericType(requestAndHandlerTypeCombination.Item1))
                        {
                            if (requestAndHandlerTypeCombination.Item2.IsOpenGeneric())
                            {
                                if (typeof(ReadQueryHandlerBase<,,,,,,>).IsAssignableFromGenericType(
                                    requestAndHandlerTypeCombination.Item2))
                                {
                                    validRequestAndHandlerTypes.Add(requestAndHandlerTypeCombination.Item1,
                                        requestAndHandlerTypeCombination.Item2);
                                }
                                else
                                    throw new ArgumentException(
                                        $"{requestAndHandlerTypeCombination.Item2.GetFriendlyName()} must inherit from {typeof(ReadQueryHandlerBase<,,,,,,>).GetFriendlyName()} to handle {typeof(ReadQueryBase<,>).GetFriendlyName()}");
                            }
                            else
                                throw new ArgumentException(
                                    $"{requestAndHandlerTypeCombination.Item2.GetFriendlyName()} must be open generic");
                        }
                        else if (typeof(ReadAllQueryBase<>).IsAssignableFromGenericType(requestAndHandlerTypeCombination.Item1))
                        {
                            if (requestAndHandlerTypeCombination.Item2.IsOpenGeneric())
                            {
                                if (typeof(ReadAllQueryHandlerBase<,,,,,>).IsAssignableFromGenericType(
                                       requestAndHandlerTypeCombination.Item2))
                                {
                                    validRequestAndHandlerTypes.Add(requestAndHandlerTypeCombination.Item1,
                                        requestAndHandlerTypeCombination.Item2);
                                }
                                else
                                    throw new ArgumentException(
                                        $"{requestAndHandlerTypeCombination.Item2.GetFriendlyName()} must inherit from {typeof(ReadAllQueryHandlerBase<,,,,,>).GetFriendlyName()} to handle {typeof(ReadAllQueryBase<>).GetFriendlyName()}");
                            }
                            else
                                throw new ArgumentException(
                                    $"{requestAndHandlerTypeCombination.Item2.GetFriendlyName()} must be open generic");
                        }
                        else if (typeof(ReadAllQueryBase<,>).IsAssignableFromGenericType(requestAndHandlerTypeCombination.Item1))
                        {
                            if (requestAndHandlerTypeCombination.Item2.IsOpenGeneric())
                            {
                                if (typeof(ReadAllQueryHandlerBase<,,,,,,>).IsAssignableFromGenericType(
                                    requestAndHandlerTypeCombination.Item2))
                                {
                                    validRequestAndHandlerTypes.Add(requestAndHandlerTypeCombination.Item1,
                                        requestAndHandlerTypeCombination.Item2);
                                }
                                else
                                    throw new ArgumentException(
                                        $"{requestAndHandlerTypeCombination.Item2.GetFriendlyName()} must inherit from {typeof(ReadAllQueryHandlerBase<,,,,,,>).GetFriendlyName()} to handle {typeof(ReadAllQueryBase<,>).GetFriendlyName()}");
                            }
                            else
                                throw new ArgumentException(
                                    $"{requestAndHandlerTypeCombination.Item2.GetFriendlyName()} must be open generic");
                        }
                        else if (typeof(CreateCommandBase<,>).IsAssignableFromGenericType(requestAndHandlerTypeCombination.Item1))
                        {
                            if (requestAndHandlerTypeCombination.Item2.IsOpenGeneric())
                            {
                                if (typeof(CreateCommandHandlerBase<,,,,,,>).IsAssignableFromGenericType(
                                       requestAndHandlerTypeCombination.Item2))
                                {
                                    validRequestAndHandlerTypes.Add(requestAndHandlerTypeCombination.Item1,
                                        requestAndHandlerTypeCombination.Item2);
                                }
                                else
                                    throw new ArgumentException(
                                        $"{requestAndHandlerTypeCombination.Item2.GetFriendlyName()} must inherit from {typeof(CreateCommandHandlerBase<,,,,,,>).GetFriendlyName()} to handle {typeof(CreateCommandBase<,>).GetFriendlyName()}");
                            }
                            else
                                throw new ArgumentException(
                                    $"{requestAndHandlerTypeCombination.Item2.GetFriendlyName()} must be open generic");
                        }
                        else if (typeof(UpdateCommandBase<>).IsAssignableFromGenericType(requestAndHandlerTypeCombination.Item1))
                        {
                            if (requestAndHandlerTypeCombination.Item2.IsOpenGeneric())
                            {
                                if (typeof(UpdateCommandHandlerBase<,,,,>).IsAssignableFromGenericType(
                                    requestAndHandlerTypeCombination.Item2))
                                {
                                    validRequestAndHandlerTypes.Add(requestAndHandlerTypeCombination.Item1,
                                        requestAndHandlerTypeCombination.Item2);
                                }
                                else
                                    throw new ArgumentException(
                                        $"{requestAndHandlerTypeCombination.Item2.GetFriendlyName()} must inherit from {typeof(UpdateCommandHandlerBase<,,,,>).GetFriendlyName()}");
                            }
                            else
                                throw new ArgumentException(
                                    $"{requestAndHandlerTypeCombination.Item2.GetFriendlyName()} must be open generic");
                        }
                        else if (typeof(UpdateCommandBase<,>).IsAssignableFromGenericType(requestAndHandlerTypeCombination.Item1))
                        {
                            if (requestAndHandlerTypeCombination.Item2.IsOpenGeneric())
                            {
                                if (typeof(UpdateCommandHandlerBase<,,,,,,>).IsAssignableFromGenericType(
                                    requestAndHandlerTypeCombination.Item2))
                                {
                                    validRequestAndHandlerTypes.Add(requestAndHandlerTypeCombination.Item1,
                                        requestAndHandlerTypeCombination.Item2);
                                }
                                else
                                    throw new ArgumentException(
                                        $"{requestAndHandlerTypeCombination.Item2.GetFriendlyName()} must inherit from {typeof(UpdateCommandHandlerBase<,,,,,,>).GetFriendlyName()}");
                            }
                            else
                                throw new ArgumentException(
                                    $"{requestAndHandlerTypeCombination.Item2.GetFriendlyName()} must be open generic");
                        }
                        else if (typeof(DeleteCommandBase<>).IsAssignableFromGenericType(requestAndHandlerTypeCombination.Item1))
                        {
                            if (requestAndHandlerTypeCombination.Item2.IsOpenGeneric())
                            {
                                if (typeof(DeleteCommandHandlerBase<,,,,,>).IsAssignableFromGenericType(
                                    requestAndHandlerTypeCombination.Item2))
                                {
                                    validRequestAndHandlerTypes.Add(requestAndHandlerTypeCombination.Item1,
                                        requestAndHandlerTypeCombination.Item2);
                                }
                                else
                                    throw new ArgumentException(
                                        $"{requestAndHandlerTypeCombination.Item2.GetFriendlyName()} must inherit from {typeof(DeleteCommandHandlerBase<,,,,,>).GetFriendlyName()}");
                            }
                            else
                                throw new ArgumentException(
                                    $"{requestAndHandlerTypeCombination.Item2.GetFriendlyName()} must be open generic");
                        }
                        else if (typeof(DeleteCommandBase<,>).IsAssignableFromGenericType(requestAndHandlerTypeCombination.Item1))
                        {
                            if (requestAndHandlerTypeCombination.Item2.IsOpenGeneric())
                            { 
                                if(typeof(DeleteCommandHandlerBase<,,,,,,>).IsAssignableFromGenericType(
                                       requestAndHandlerTypeCombination.Item2))
                                {
                                    validRequestAndHandlerTypes.Add(requestAndHandlerTypeCombination.Item1,
                                        requestAndHandlerTypeCombination.Item2);
                                }
                                else
                                    throw new ArgumentException(
                                        $"{requestAndHandlerTypeCombination.Item2.GetFriendlyName()} must inherit from {typeof(DeleteCommandHandlerBase<,,,,,,>).GetFriendlyName()}");
                            }
                            else
                                throw new ArgumentException(
                                    $"{requestAndHandlerTypeCombination.Item2.GetFriendlyName()} must be open generic");
                        }
                        else
                            throw new ArgumentException(
                                $"{requestAndHandlerTypeCombination.Item1.GetFriendlyName()} must inherit from " +
                                $"{typeof(ReadQueryBase<>).GetFriendlyName()}, {typeof(ReadQueryBase<,>).GetFriendlyName()}, " +
                                $"{typeof(ReadAllQueryBase<>).GetFriendlyName()}, {typeof(ReadAllQueryBase<,>).GetFriendlyName()}, " +
                                $"{typeof(CreateCommandBase<,>).GetFriendlyName()}, " +
                                $"{typeof(UpdateCommandBase<,>).GetFriendlyName()}, {typeof(UpdateCommandBase<,,>).GetFriendlyName()},  " +
                                $"{typeof(DeleteCommandBase<>).GetFriendlyName()} or {typeof(DeleteCommandBase<,>).GetFriendlyName()}");
                    }
                    else
                        throw new ArgumentException(
                            $"{requestAndHandlerTypeCombination.Item1.GetFriendlyName()} must be open generic");
                }
            }

            // Registering crud mediation request handlers
            optionsBuilder.ApizrOptions.PostRegistrationActions.Add(services =>
            {
                foreach (var crudEntity in optionsBuilder.ApizrOptions.CrudEntities)
                {
                    var apiEntityAttribute = crudEntity.Value;
                    var apiEntityType = crudEntity.Key;
                    var modelEntityType = apiEntityAttribute.MappedEntityType;
                    var modelEntityReadAllResultType = apiEntityAttribute.ReadAllResultType.IsGenericTypeDefinition
                        ? apiEntityAttribute.ReadAllResultType.MakeGenericTypeIfNeeded(modelEntityType)
                        : apiEntityAttribute.ReadAllResultType.GetGenericTypeDefinition()
                            .MakeGenericTypeIfNeeded(modelEntityType);

                    var requestResponseTypes = new Dictionary<Type, Type>
                    {
                        { typeof(ReadQueryBase<,>), modelEntityType },
                        { typeof(ReadAllQueryBase<,>), modelEntityReadAllResultType },
                        { typeof(CreateCommandBase<,>), modelEntityType },
                        { typeof(UpdateCommandBase<,,>), typeof(Unit) },
                        { typeof(DeleteCommandBase<,,>), typeof(Unit) }
                    };

                    WithCrudMediation(optionsBuilder, services, apiEntityType, apiEntityAttribute, validRequestAndHandlerTypes, requestResponseTypes);
                }
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
