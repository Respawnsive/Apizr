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

namespace Apizr
{
    public static class ApizrExtendedOptionsBuilderExtensions
    {
        private static readonly IDictionary<Type, Type> RequestResponseTypes = new Dictionary<Type, Type>
        {
            { typeof(ReadQueryBase<,>), default }
        };


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
                        if (typeof(ReadQueryHandlerBase<,,,,>).IsAssignableFromGenericType(crudHandlerType))
                            validCrudHandlerTypes.Add(typeof(ReadQuery<>), crudHandlerType);
                        else if (typeof(ReadQueryHandlerBase<,,,,,>).IsAssignableFromGenericType(crudHandlerType))
                            validCrudHandlerTypes.Add(typeof(ReadQuery<,>), crudHandlerType);
                        else if (typeof(ReadAllQueryHandlerBase<,,,,>).IsAssignableFromGenericType(crudHandlerType))
                            validCrudHandlerTypes.Add(typeof(ReadAllQuery<>), crudHandlerType);
                        else if (typeof(ReadAllQueryHandlerBase<,,,,,>).IsAssignableFromGenericType(crudHandlerType))
                            validCrudHandlerTypes.Add(typeof(ReadAllQuery<,>), crudHandlerType);
                        else if (typeof(CreateCommandHandlerBase<,,,,,>).IsAssignableFromGenericType(crudHandlerType))
                            validCrudHandlerTypes.Add(typeof(CreateCommand<>), crudHandlerType);
                        else if (typeof(UpdateCommandHandlerBase<,,,>).IsAssignableFromGenericType(crudHandlerType))
                            validCrudHandlerTypes.Add(typeof(UpdateCommand<>), crudHandlerType);
                        else if (typeof(UpdateCommandHandlerBase<,,,,,>).IsAssignableFromGenericType(crudHandlerType))
                            validCrudHandlerTypes.Add(typeof(UpdateCommand<,>), crudHandlerType);
                        else if (typeof(DeleteCommandHandlerBase<,,,,>).IsAssignableFromGenericType(crudHandlerType))
                            validCrudHandlerTypes.Add(typeof(DeleteCommand<>), crudHandlerType);
                        else if (typeof(DeleteCommandHandlerBase<,,,,,>).IsAssignableFromGenericType(crudHandlerType))
                            validCrudHandlerTypes.Add(typeof(DeleteCommand<,>), crudHandlerType);
                        else
                            throw new ArgumentException(
                                $"{crudHandlerType.Name} must inherit from " +
                                $"{typeof(ReadQueryHandlerBase<,,,,>).Name}, {typeof(ReadQueryHandlerBase<,,,,,>)}, " +
                                $"{typeof(ReadAllQueryHandlerBase<,,,,>).Name}, {typeof(ReadAllQueryHandlerBase<,,,,,>)}, " +
                                $"{typeof(CreateCommandHandlerBase<,,,,,>).Name}, " +
                                $"{typeof(UpdateCommandHandlerBase<,,,>).Name}, {typeof(UpdateCommandHandlerBase<,,,,,>)},  " +
                                $"{typeof(DeleteCommandHandlerBase<,,,,>).Name} or {typeof(DeleteCommandHandlerBase<,,,,,>)}",
                                nameof(crudHandlerType.Name));
                    }
                    else
                        throw new ArgumentException(
                            $"{crudHandlerType.Name} must be open generic",
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
                                if (typeof(ReadQueryHandlerBase<,,,,>).IsAssignableFromGenericType(
                                    requestAndHandlerTypeCombination.Item2))
                                {
                                    validRequestAndHandlerTypes.Add(requestAndHandlerTypeCombination.Item1,
                                        requestAndHandlerTypeCombination.Item2);
                                }
                                else
                                    throw new ArgumentException(
                                        $"{requestAndHandlerTypeCombination.Item2.Name} must inherit from {typeof(ReadQueryHandlerBase<,,,,>).Name} to handle {typeof(ReadQueryBase<>).Name}",
                                        nameof(requestAndHandlerTypeCombination.Item2.Name));
                            }
                            else
                                throw new ArgumentException(
                                    $"{requestAndHandlerTypeCombination.Item2.Name} must be open generic",
                                    nameof(requestAndHandlerTypeCombination.Item2.Name));
                        }
                        else if (typeof(ReadQueryBase<,>).IsAssignableFromGenericType(requestAndHandlerTypeCombination.Item1))
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
                                        $"{requestAndHandlerTypeCombination.Item2.Name} must inherit from {typeof(ReadQueryHandlerBase<,,,,,>).Name} to handle {typeof(ReadQueryBase<,>).Name}",
                                        nameof(requestAndHandlerTypeCombination.Item2.Name));
                            }
                            else
                                throw new ArgumentException(
                                    $"{requestAndHandlerTypeCombination.Item2.Name} must be open generic",
                                    nameof(requestAndHandlerTypeCombination.Item2.Name));
                        }
                        else if (typeof(ReadAllQueryBase<>).IsAssignableFromGenericType(requestAndHandlerTypeCombination.Item1))
                        {
                            if (requestAndHandlerTypeCombination.Item2.IsOpenGeneric())
                            {
                                if (typeof(ReadAllQueryHandlerBase<,,,,>).IsAssignableFromGenericType(
                                       requestAndHandlerTypeCombination.Item2))
                                {
                                    validRequestAndHandlerTypes.Add(requestAndHandlerTypeCombination.Item1,
                                        requestAndHandlerTypeCombination.Item2);
                                }
                                else
                                    throw new ArgumentException(
                                        $"{requestAndHandlerTypeCombination.Item2.Name} must inherit from {typeof(ReadAllQueryHandlerBase<,,,,>).Name} to handle {typeof(ReadAllQueryBase<>).Name}",
                                        nameof(requestAndHandlerTypeCombination.Item2.Name));
                            }
                            else
                                throw new ArgumentException(
                                    $"{requestAndHandlerTypeCombination.Item2.Name} must be open generic",
                                    nameof(requestAndHandlerTypeCombination.Item2.Name));
                        }
                        else if (typeof(ReadAllQueryBase<,>).IsAssignableFromGenericType(requestAndHandlerTypeCombination.Item1))
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
                                        $"{requestAndHandlerTypeCombination.Item2.Name} must inherit from {typeof(ReadAllQueryHandlerBase<,,,,,>).Name} to handle {typeof(ReadAllQueryBase<,>).Name}",
                                        nameof(requestAndHandlerTypeCombination.Item2.Name));
                            }
                            else
                                throw new ArgumentException(
                                    $"{requestAndHandlerTypeCombination.Item2.Name} must be open generic",
                                    nameof(requestAndHandlerTypeCombination.Item2.Name));
                        }
                        else if (typeof(CreateCommandBase<,>).IsAssignableFromGenericType(requestAndHandlerTypeCombination.Item1))
                        {
                            if (requestAndHandlerTypeCombination.Item2.IsOpenGeneric())
                            {
                                if (typeof(CreateCommandHandlerBase<,,,,,>).IsAssignableFromGenericType(
                                       requestAndHandlerTypeCombination.Item2))
                                {
                                    validRequestAndHandlerTypes.Add(requestAndHandlerTypeCombination.Item1,
                                        requestAndHandlerTypeCombination.Item2);
                                }
                                else
                                    throw new ArgumentException(
                                        $"{requestAndHandlerTypeCombination.Item2.Name} must inherit from {typeof(CreateCommandHandlerBase<,,,,,>).Name} to handle {typeof(CreateCommandBase<,>).Name}",
                                        nameof(requestAndHandlerTypeCombination.Item2.Name));
                            }
                            else
                                throw new ArgumentException(
                                    $"{requestAndHandlerTypeCombination.Item2.Name} must be open generic",
                                    nameof(requestAndHandlerTypeCombination.Item2.Name));
                        }
                        else if (typeof(UpdateCommandBase<>).IsAssignableFromGenericType(requestAndHandlerTypeCombination.Item1))
                        {
                            if (requestAndHandlerTypeCombination.Item2.IsOpenGeneric())
                            {
                                if (typeof(UpdateCommandHandlerBase<,,,>).IsAssignableFromGenericType(
                                    requestAndHandlerTypeCombination.Item2))
                                {
                                    validRequestAndHandlerTypes.Add(requestAndHandlerTypeCombination.Item1,
                                        requestAndHandlerTypeCombination.Item2);
                                }
                                else
                                    throw new ArgumentException(
                                        $"{requestAndHandlerTypeCombination.Item2.Name} must inherit from {typeof(UpdateCommandHandlerBase<,,,>).Name}",
                                        nameof(requestAndHandlerTypeCombination.Item2.Name));
                            }
                            else
                                throw new ArgumentException(
                                    $"{requestAndHandlerTypeCombination.Item2.Name} must be open generic",
                                    nameof(requestAndHandlerTypeCombination.Item2.Name));
                        }
                        else if (typeof(UpdateCommandBase<,>).IsAssignableFromGenericType(requestAndHandlerTypeCombination.Item1))
                        {
                            if (requestAndHandlerTypeCombination.Item2.IsOpenGeneric())
                            {
                                if (typeof(UpdateCommandHandlerBase<,,,,,>).IsAssignableFromGenericType(
                                    requestAndHandlerTypeCombination.Item2))
                                {
                                    validRequestAndHandlerTypes.Add(requestAndHandlerTypeCombination.Item1,
                                        requestAndHandlerTypeCombination.Item2);
                                }
                                else
                                    throw new ArgumentException(
                                        $"{requestAndHandlerTypeCombination.Item2.Name} must inherit from {typeof(UpdateCommandHandlerBase<,,,,,>).Name}",
                                        nameof(requestAndHandlerTypeCombination.Item2.Name));
                            }
                            else
                                throw new ArgumentException(
                                    $"{requestAndHandlerTypeCombination.Item2.Name} must be open generic",
                                    nameof(requestAndHandlerTypeCombination.Item2.Name));
                        }
                        else if (typeof(DeleteCommandBase<>).IsAssignableFromGenericType(requestAndHandlerTypeCombination.Item1))
                        {
                            if (requestAndHandlerTypeCombination.Item2.IsOpenGeneric())
                            {
                                if (typeof(DeleteCommandHandlerBase<,,,,>).IsAssignableFromGenericType(
                                    requestAndHandlerTypeCombination.Item2))
                                {
                                    validRequestAndHandlerTypes.Add(requestAndHandlerTypeCombination.Item1,
                                        requestAndHandlerTypeCombination.Item2);
                                }
                                else
                                    throw new ArgumentException(
                                        $"{requestAndHandlerTypeCombination.Item2.Name} must inherit from {typeof(DeleteCommandHandlerBase<,,,,>).Name}",
                                        nameof(requestAndHandlerTypeCombination.Item2.Name));
                            }
                            else
                                throw new ArgumentException(
                                    $"{requestAndHandlerTypeCombination.Item2.Name} must be open generic",
                                    nameof(requestAndHandlerTypeCombination.Item2.Name));
                        }
                        else if (typeof(DeleteCommandBase<,>).IsAssignableFromGenericType(requestAndHandlerTypeCombination.Item1))
                        {
                            if (requestAndHandlerTypeCombination.Item2.IsOpenGeneric())
                            { 
                                if(typeof(DeleteCommandHandlerBase<,,,,,>).IsAssignableFromGenericType(
                                       requestAndHandlerTypeCombination.Item2))
                                {
                                    validRequestAndHandlerTypes.Add(requestAndHandlerTypeCombination.Item1,
                                        requestAndHandlerTypeCombination.Item2);
                                }
                                else
                                    throw new ArgumentException(
                                        $"{requestAndHandlerTypeCombination.Item2.Name} must inherit from {typeof(DeleteCommandHandlerBase<,,,,,>).Name}",
                                        nameof(requestAndHandlerTypeCombination.Item2.Name));
                            }
                            else
                                throw new ArgumentException(
                                    $"{requestAndHandlerTypeCombination.Item2.Name} must be open generic",
                                    nameof(requestAndHandlerTypeCombination.Item2.Name));
                        }
                        else
                            throw new ArgumentException(
                                $"{requestAndHandlerTypeCombination.Item1.Name} must inherit from " +
                                $"{typeof(ReadQueryBase<>).Name}, {typeof(ReadQueryBase<,>)}, " +
                                $"{typeof(ReadAllQueryBase<>).Name}, {typeof(ReadAllQueryBase<,>)}, " +
                                $"{typeof(CreateCommandBase<,>).Name}, " +
                                $"{typeof(UpdateCommandBase<,>).Name}, {typeof(UpdateCommandBase<,,>)},  " +
                                $"{typeof(DeleteCommandBase<>).Name} or {typeof(DeleteCommandBase<,>)}",
                                nameof(requestAndHandlerTypeCombination.Item1.Name));
                    }
                    else
                        throw new ArgumentException(
                            $"{requestAndHandlerTypeCombination.Item1.Name} must be open generic",
                            nameof(requestAndHandlerTypeCombination.Item1.Name));
                }
            }

            // Registering crud mediation request handlers
            optionsBuilder.ApizrOptions.PostRegistrationActions.Add(services =>
            {
                foreach (var crudEntity in optionsBuilder.ApizrOptions.CrudEntities)
                {
                    var crudedEntityType = crudEntity.Key;
                    var crudEntityAttribute = crudEntity.Value;

                    var requestResponseTypes = new Dictionary<Type, Type>
                    {
                        { typeof(ReadQueryBase<,>), crudedEntityType },
                        { typeof(ReadAllQueryBase<,>), crudEntityAttribute.ReadAllResultType.MakeGenericType(crudedEntityType) },
                        { typeof(CreateCommandBase<,>), crudedEntityType },
                        { typeof(UpdateCommandBase<,,>), typeof(Unit) },
                        { typeof(DeleteCommandBase<,,>), typeof(Unit) }
                    };

                    WithCrudMediation(optionsBuilder, services, crudedEntityType, crudEntityAttribute, validRequestAndHandlerTypes, requestResponseTypes);
                }
            });

            return optionsBuilder;
        }

        public static IApizrExtendedOptionsBuilder WithCrudMediation(this IApizrExtendedOptionsBuilder optionsBuilder,
            IServiceCollection services, Type crudedEntityType, CrudEntityAttribute crudEntityAttribute,  IDictionary<Type, Type> validRequestAndHandlerTypes, IDictionary<Type, Type> requestResponseTypes)
        {
            // Read but simplified default version if concerned
            if (crudEntityAttribute.KeyType == typeof(int))
            {
                var simplifiedReadCombination = validRequestAndHandlerTypes.FirstOrDefault(kvp =>
                    typeof(ReadQueryBase<>).IsAssignableFromGenericType(kvp.Key));
                services.AddTransient(
                    typeof(IRequestHandler<,>).MakeGenericType(
                        (simplifiedReadCombination.Key ?? typeof(ReadQuery<>)).MakeGenericType(crudedEntityType),
                        requestResponseTypes.First(kvp => kvp.Key == typeof(ReadQueryBase<,>)).Value),
                    (simplifiedReadCombination.Value ?? typeof(ReadQueryHandler<,,>)).MakeGenericType(
                        crudedEntityType,
                        crudEntityAttribute.ReadAllResultType.MakeGenericTypeIfNeeded(crudedEntityType),
                        crudEntityAttribute.ReadAllParamsType));
            }

            // Read
            var readCombination = validRequestAndHandlerTypes.FirstOrDefault(kvp =>
                typeof(ReadQueryBase<,>).IsAssignableFromGenericType(kvp.Key));
            services.AddTransient(
                typeof(IRequestHandler<,>).MakeGenericType(
                    (readCombination.Key ?? typeof(ReadQuery<,>)).MakeGenericType(crudedEntityType,
                        crudEntityAttribute.KeyType),
                    requestResponseTypes.First(kvp => kvp.Key == typeof(ReadQueryBase<,>)).Value),
                (readCombination.Value ?? typeof(ReadQueryHandler<,,,>)).MakeGenericType(crudedEntityType,
                    crudEntityAttribute.KeyType,
                    crudEntityAttribute.ReadAllResultType.MakeGenericTypeIfNeeded(crudedEntityType),
                    crudEntityAttribute.ReadAllParamsType));

            // ReadAll but simplified default version if concerned
            if (crudEntityAttribute.ReadAllParamsType == typeof(IDictionary<string, object>))
            {
                var simplifiedReadAllCombination = validRequestAndHandlerTypes.FirstOrDefault(kvp =>
                    typeof(ReadAllQueryBase<>).IsAssignableFromGenericType(kvp.Key));
                services.AddTransient(
                    typeof(IRequestHandler<,>).MakeGenericType(
                        (simplifiedReadAllCombination.Key ?? typeof(ReadAllQuery<>)).MakeGenericType(
                            crudEntityAttribute.ReadAllResultType.MakeGenericType(crudedEntityType)),
                        requestResponseTypes.First(kvp => kvp.Key == typeof(ReadAllQueryBase<,>)).Value),
                    (simplifiedReadAllCombination.Value ??
                     typeof(ReadAllQueryHandler<,,>)).MakeGenericType(crudedEntityType, crudEntityAttribute.KeyType,
                        crudEntityAttribute.ReadAllResultType.MakeGenericTypeIfNeeded(crudedEntityType)));
            }

            // ReadAll
            var readAllCombination = validRequestAndHandlerTypes.FirstOrDefault(kvp =>
            typeof(ReadAllQueryBase<,>).IsAssignableFromGenericType(kvp.Key));
            services.AddTransient(
                typeof(IRequestHandler<,>).MakeGenericType(
                    (readAllCombination.Key ?? typeof(ReadAllQuery<,>)).MakeGenericType(
                        crudEntityAttribute.ReadAllParamsType,
                        crudEntityAttribute.ReadAllResultType.MakeGenericType(crudedEntityType)),
                    requestResponseTypes.First(kvp => kvp.Key == typeof(ReadAllQueryBase<,>)).Value),
                (readAllCombination.Value ??
                 typeof(ReadAllQueryHandler<,,,>)).MakeGenericType(crudedEntityType, crudEntityAttribute.KeyType,
                    crudEntityAttribute.ReadAllResultType.MakeGenericTypeIfNeeded(crudedEntityType),
                    crudEntityAttribute.ReadAllParamsType));

            // Create
            var createCombination = validRequestAndHandlerTypes.FirstOrDefault(kvp =>
                typeof(CreateCommandBase<,>).IsAssignableFromGenericType(kvp.Key));
            services.AddTransient(
                typeof(IRequestHandler<,>).MakeGenericType(
                    (createCombination.Key ?? typeof(CreateCommand<>)).MakeGenericType(crudedEntityType),
                    requestResponseTypes.First(kvp => kvp.Key == typeof(CreateCommandBase<,>)).Value),
                (createCombination.Value ??
                 typeof(CreateCommandHandler<,,,>)).MakeGenericType(crudedEntityType, crudEntityAttribute.KeyType,
                    crudEntityAttribute.ReadAllResultType.MakeGenericTypeIfNeeded(crudedEntityType),
                    crudEntityAttribute.ReadAllParamsType));

            // Update but simplified default version if concerned
            if (crudEntityAttribute.KeyType == typeof(int))
            {
                var simplifiedUpdateCombination = validRequestAndHandlerTypes.FirstOrDefault(kvp =>
                                typeof(UpdateCommandBase<>).IsAssignableFromGenericType(kvp.Key));
                services.AddTransient(
                    typeof(IRequestHandler<,>).MakeGenericType(
                        (simplifiedUpdateCombination.Key ?? typeof(UpdateCommand<>)).MakeGenericType(
                            crudedEntityType),
                        requestResponseTypes.First(kvp => kvp.Key == typeof(UpdateCommandBase<,,>)).Value),
                    (simplifiedUpdateCombination.Value ??
                     typeof(UpdateCommandHandler<,,>)).MakeGenericType(crudedEntityType,
                        crudEntityAttribute.ReadAllResultType.MakeGenericTypeIfNeeded(crudedEntityType),
                        crudEntityAttribute.ReadAllParamsType));
            }

            // Update
            var updateCombination = validRequestAndHandlerTypes.FirstOrDefault(kvp =>
                typeof(UpdateCommandBase<,>).IsAssignableFromGenericType(kvp.Key));
            services.AddTransient(
                typeof(IRequestHandler<,>).MakeGenericType(
                    (updateCombination.Key ?? typeof(UpdateCommand<,>)).MakeGenericType(
                        crudEntityAttribute.KeyType, crudedEntityType),
                    requestResponseTypes.First(kvp => kvp.Key == typeof(UpdateCommandBase<,,>)).Value),
                (updateCombination.Value ??
                 typeof(UpdateCommandHandler<,,,>)).MakeGenericType(crudedEntityType, crudEntityAttribute.KeyType,
                    crudEntityAttribute.ReadAllResultType.MakeGenericTypeIfNeeded(crudedEntityType),
                    crudEntityAttribute.ReadAllParamsType));

            // Delete but simplified default version if concerned
            if (crudEntityAttribute.KeyType == typeof(int))
            {
                var simplifiedDeleteCombination = validRequestAndHandlerTypes.FirstOrDefault(kvp =>
                                typeof(DeleteCommandBase<>).IsAssignableFromGenericType(kvp.Key));
                services.AddTransient(
                    typeof(IRequestHandler<,>).MakeGenericType(
                        (simplifiedDeleteCombination.Key ?? typeof(DeleteCommand<>)).MakeGenericType(crudedEntityType),
                        requestResponseTypes.First(kvp => kvp.Key == typeof(DeleteCommandBase<,,>)).Value),
                    (simplifiedDeleteCombination.Key ??
                     typeof(DeleteCommandHandler<,,>)).MakeGenericType(crudedEntityType,
                        crudEntityAttribute.ReadAllResultType.MakeGenericTypeIfNeeded(crudedEntityType),
                        crudEntityAttribute.ReadAllParamsType));
            }

            // Delete
            var deleteCombination = validRequestAndHandlerTypes.FirstOrDefault(kvp =>
                typeof(DeleteCommandBase<,>).IsAssignableFromGenericType(kvp.Key));
            services.AddTransient(
                typeof(IRequestHandler<,>).MakeGenericType(
                    (deleteCombination.Key ?? typeof(DeleteCommand<,>))
                    .MakeGenericType(crudedEntityType, crudEntityAttribute.KeyType),
                    requestResponseTypes.First(kvp => kvp.Key == typeof(DeleteCommandBase<,,>)).Value),
                (deleteCombination.Value ??
                 typeof(DeleteCommandHandler<,,,>)).MakeGenericType(crudedEntityType, crudEntityAttribute.KeyType,
                    crudEntityAttribute.ReadAllResultType.MakeGenericTypeIfNeeded(crudedEntityType),
                    crudEntityAttribute.ReadAllParamsType));

            return optionsBuilder;
        }
    }
}
