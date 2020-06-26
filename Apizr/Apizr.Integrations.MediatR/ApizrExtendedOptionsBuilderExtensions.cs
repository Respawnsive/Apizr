using System;
using System.Collections.Generic;
using System.Linq;
using Apizr.Mediation.Cruding;
using Apizr.Mediation.Cruding.Handling;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

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
            return WithCrudMediation(optionsBuilder, typeof(ReadQueryHandler<,,>), typeof(ReadAllQueryHandler<,,>),
                typeof(CreateCommandHandler<,,>), typeof(UpdateCommandHandler<,,>), typeof(DeleteCommandHandler<,,>));
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
            var validCrudHandlerTypes = new List<Type>();
            if (crudHandlerTypes != null)
            {
                foreach (var crudHandlerType in crudHandlerTypes.Distinct())
                {
                    if (crudHandlerType.IsOpenGeneric() && 
                        (typeof(ReadQueryHandler<,,>).IsAssignableFromGenericType(crudHandlerType) ||
                        typeof(ReadAllQueryHandler<,,>).IsAssignableFromGenericType(crudHandlerType) ||
                        typeof(CreateCommandHandler<,,>).IsAssignableFromGenericType(crudHandlerType) ||
                        typeof(UpdateCommandHandler<,,>).IsAssignableFromGenericType(crudHandlerType) ||
                        typeof(DeleteCommandHandler<,,>).IsAssignableFromGenericType(crudHandlerType)))
                    {
                        validCrudHandlerTypes.Add(crudHandlerType);
                    }
                    else
                        throw new ArgumentException(
                            $"{crudHandlerType.Name} must be open generic and inherit from {typeof(ReadQueryHandler<,,>)}, {typeof(ReadAllQueryHandler<,,>)}, {typeof(CreateCommandHandler<,,>)}, {typeof(UpdateCommandHandler<,,>)} or {typeof(DeleteCommandHandler<,,>)}",
                            nameof(crudHandlerTypes));
                } 
            }

            // Registering crud mediation request handlers
            optionsBuilder.ApizrOptions.PostRegistrationActions.Add(services =>
            {
                foreach (var crudEntity in optionsBuilder.ApizrOptions.CrudEntities)
                {
                    // Read
                    services.AddTransient(
                        typeof(IRequestHandler<,>).MakeGenericType(
                            typeof(ReadQuery<,>).MakeGenericType(crudEntity.Key, crudEntity.Value.KeyType),
                            crudEntity.Key),
                        (validCrudHandlerTypes.FirstOrDefault(t =>
                             typeof(ReadQueryHandler<,,>).IsAssignableFromGenericType(t)) ??
                         typeof(ReadQueryHandler<,,>)).MakeGenericType(crudEntity.Key, crudEntity.Value.KeyType,
                            crudEntity.Value.ReadAllResultType.MakeGenericTypeIfNeeded(crudEntity.Key)));

                    // ReadAll
                    services.AddTransient(
                        typeof(IRequestHandler<,>).MakeGenericType(
                            typeof(ReadAllQuery<>).MakeGenericType(crudEntity.Value.ReadAllResultType.MakeGenericType(crudEntity.Key)),
                            crudEntity.Value.ReadAllResultType.MakeGenericType(crudEntity.Key)),
                        (validCrudHandlerTypes.FirstOrDefault(t =>
                             typeof(ReadAllQueryHandler<,,>).IsAssignableFromGenericType(t)) ??
                         typeof(ReadAllQueryHandler<,,>)).MakeGenericType(crudEntity.Key, crudEntity.Value.KeyType, crudEntity.Value.ReadAllResultType.MakeGenericTypeIfNeeded(crudEntity.Key)));

                    // Create
                    services.AddTransient(
                        typeof(IRequestHandler<,>).MakeGenericType(
                            typeof(CreateCommand<>).MakeGenericType(crudEntity.Key),
                            crudEntity.Key),
                        (validCrudHandlerTypes.FirstOrDefault(t =>
                             typeof(CreateCommandHandler<,,>).IsAssignableFromGenericType(t)) ??
                         typeof(CreateCommandHandler<,,>)).MakeGenericType(crudEntity.Key, crudEntity.Value.KeyType, crudEntity.Value.ReadAllResultType.MakeGenericTypeIfNeeded(crudEntity.Key)));

                    // Update
                    services.AddTransient(
                        typeof(IRequestHandler<>).MakeGenericType(
                            typeof(UpdateCommand<,>).MakeGenericType(crudEntity.Value.KeyType, crudEntity.Key)),
                        (validCrudHandlerTypes.FirstOrDefault(t =>
                             typeof(UpdateCommandHandler<,,>).IsAssignableFromGenericType(t)) ??
                         typeof(UpdateCommandHandler<,,>)).MakeGenericType(crudEntity.Key, crudEntity.Value.KeyType, crudEntity.Value.ReadAllResultType.MakeGenericTypeIfNeeded(crudEntity.Key)));

                    // Delete
                    services.AddTransient(
                        typeof(IRequestHandler<>).MakeGenericType(
                            typeof(DeleteCommand<>).MakeGenericType(crudEntity.Value.KeyType)),
                        (validCrudHandlerTypes.FirstOrDefault(t =>
                             typeof(ReadQueryHandler<,,>).IsAssignableFromGenericType(t)) ??
                         typeof(ReadQueryHandler<,,>)).MakeGenericType(crudEntity.Key, crudEntity.Value.KeyType, crudEntity.Value.ReadAllResultType.MakeGenericTypeIfNeeded(crudEntity.Key)));
                }
            });

            return optionsBuilder;
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
                        if (typeof(ReadQuery<,>).IsAssignableFromGenericType(requestAndHandlerTypeCombination.Item1))
                        {
                            if (requestAndHandlerTypeCombination.Item2.IsOpenGeneric())
                            {
                                if (typeof(ReadQueryHandler<,,>).IsAssignableFromGenericType(
                                   requestAndHandlerTypeCombination.Item2))
                                {
                                    validRequestAndHandlerTypes.Add(requestAndHandlerTypeCombination.Item1,
                                        requestAndHandlerTypeCombination.Item2);
                                }
                                else
                                    throw new ArgumentException(
                                        $"{requestAndHandlerTypeCombination.Item2.Name} must inherit from {typeof(ReadQueryHandler<,,>).Name}",
                                        nameof(requestAndHandlerTypeCombination.Item2.Name));
                            }
                            else
                                throw new ArgumentException(
                                    $"{requestAndHandlerTypeCombination.Item2.Name} must be open generic",
                                    nameof(requestAndHandlerTypeCombination.Item2.Name));
                        }
                        else if (typeof(ReadAllQuery<>).IsAssignableFromGenericType(requestAndHandlerTypeCombination.Item1))
                        {
                            if (requestAndHandlerTypeCombination.Item2.IsOpenGeneric())
                            {
                                if (typeof(ReadAllQueryHandler<,,>).IsAssignableFromGenericType(
                                       requestAndHandlerTypeCombination.Item2))
                                {
                                    validRequestAndHandlerTypes.Add(requestAndHandlerTypeCombination.Item1,
                                        requestAndHandlerTypeCombination.Item2);
                                }
                                else
                                    throw new ArgumentException(
                                        $"{requestAndHandlerTypeCombination.Item2.Name} must inherit from {typeof(ReadAllQueryHandler<,,>).Name}",
                                        nameof(requestAndHandlerTypeCombination.Item2.Name));
                            }
                            else
                                throw new ArgumentException(
                                    $"{requestAndHandlerTypeCombination.Item2.Name} must be open generic",
                                    nameof(requestAndHandlerTypeCombination.Item2.Name));
                        }
                        else if (typeof(CreateCommand<>).IsAssignableFromGenericType(requestAndHandlerTypeCombination.Item1))
                        {
                            if (requestAndHandlerTypeCombination.Item2.IsOpenGeneric())
                            {
                                if (typeof(CreateCommandHandler<,,>).IsAssignableFromGenericType(
                                       requestAndHandlerTypeCombination.Item2))
                                {
                                    validRequestAndHandlerTypes.Add(requestAndHandlerTypeCombination.Item1,
                                        requestAndHandlerTypeCombination.Item2);
                                }
                                else
                                    throw new ArgumentException(
                                        $"{requestAndHandlerTypeCombination.Item2.Name} must inherit from {typeof(CreateCommandHandler<,,>).Name}",
                                        nameof(requestAndHandlerTypeCombination.Item2.Name));
                            }
                            else
                                throw new ArgumentException(
                                    $"{requestAndHandlerTypeCombination.Item2.Name} must be open generic",
                                    nameof(requestAndHandlerTypeCombination.Item2.Name));
                        }
                        else if (typeof(UpdateCommand<,>).IsAssignableFromGenericType(requestAndHandlerTypeCombination.Item1))
                        {
                            if (requestAndHandlerTypeCombination.Item2.IsOpenGeneric())
                            {
                                if (typeof(UpdateCommandHandler<,,>).IsAssignableFromGenericType(
                                       requestAndHandlerTypeCombination.Item2))
                                {
                                    validRequestAndHandlerTypes.Add(requestAndHandlerTypeCombination.Item1,
                                        requestAndHandlerTypeCombination.Item2);
                                }
                                else
                                    throw new ArgumentException(
                                        $"{requestAndHandlerTypeCombination.Item2.Name} must inherit from {typeof(UpdateCommandHandler<,,>).Name}",
                                        nameof(requestAndHandlerTypeCombination.Item2.Name));
                            }
                            else
                                throw new ArgumentException(
                                    $"{requestAndHandlerTypeCombination.Item2.Name} must be open generic",
                                    nameof(requestAndHandlerTypeCombination.Item2.Name));
                        }
                        else if (typeof(DeleteCommand<>).IsAssignableFromGenericType(requestAndHandlerTypeCombination.Item1))
                        {
                            if (requestAndHandlerTypeCombination.Item2.IsOpenGeneric())
                            { 
                                if(typeof(DeleteCommandHandler<,,>).IsAssignableFromGenericType(
                                       requestAndHandlerTypeCombination.Item2))
                                {
                                    validRequestAndHandlerTypes.Add(requestAndHandlerTypeCombination.Item1,
                                        requestAndHandlerTypeCombination.Item2);
                                }
                                else
                                    throw new ArgumentException(
                                        $"{requestAndHandlerTypeCombination.Item2.Name} must inherit from {typeof(DeleteCommandHandler<,,>).Name}",
                                        nameof(requestAndHandlerTypeCombination.Item2.Name));
                            }
                            else
                                throw new ArgumentException(
                                    $"{requestAndHandlerTypeCombination.Item2.Name} must be open generic",
                                    nameof(requestAndHandlerTypeCombination.Item2.Name));
                        }
                        else
                            throw new ArgumentException(
                                $"{requestAndHandlerTypeCombination.Item1.Name} must inherit from {typeof(ReadQuery<,>).Name}, {typeof(ReadAllQuery<>).Name}, {typeof(CreateCommand<>).Name}, {typeof(UpdateCommand<,>).Name} or {typeof(DeleteCommand<>).Name}",
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
                    // Read
                    var readCombination = validRequestAndHandlerTypes.FirstOrDefault(kvp =>
                        typeof(ReadQuery<,>).IsAssignableFromGenericType(kvp.Key));
                    services.AddTransient(
                        typeof(IRequestHandler<,>).MakeGenericType(
                            (readCombination.Key ?? typeof(ReadQuery<,>)).MakeGenericType(crudEntity.Key,
                                crudEntity.Value.KeyType), crudEntity.Key),
                        (readCombination.Value ?? typeof(ReadQueryHandler<,,>)).MakeGenericType(crudEntity.Key,
                            crudEntity.Value.KeyType,
                            crudEntity.Value.ReadAllResultType.MakeGenericTypeIfNeeded(crudEntity.Key)));

                    // ReadAll
                    var readAllCombination = validRequestAndHandlerTypes.FirstOrDefault(kvp =>
                        typeof(ReadAllQuery<>).IsAssignableFromGenericType(kvp.Key));
                    services.AddTransient(
                        typeof(IRequestHandler<,>).MakeGenericType(
                            (readAllCombination.Key ?? typeof(ReadAllQuery<>)).MakeGenericType(
                                crudEntity.Value.ReadAllResultType.MakeGenericType(crudEntity.Key)),
                            crudEntity.Value.ReadAllResultType.MakeGenericType(crudEntity.Key)),
                        (readAllCombination.Value ??
                         typeof(ReadAllQueryHandler<,,>)).MakeGenericType(crudEntity.Key, crudEntity.Value.KeyType,
                            crudEntity.Value.ReadAllResultType.MakeGenericTypeIfNeeded(crudEntity.Key)));

                    // Create
                    var createCombination = validRequestAndHandlerTypes.FirstOrDefault(kvp =>
                        typeof(CreateCommand<>).IsAssignableFromGenericType(kvp.Key));
                    services.AddTransient(
                        typeof(IRequestHandler<,>).MakeGenericType(
                            (createCombination.Key ?? typeof(CreateCommand<>)).MakeGenericType(crudEntity.Key),
                            crudEntity.Key),
                        (createCombination.Value ??
                         typeof(CreateCommandHandler<,,>)).MakeGenericType(crudEntity.Key, crudEntity.Value.KeyType,
                            crudEntity.Value.ReadAllResultType.MakeGenericTypeIfNeeded(crudEntity.Key)));

                    // Update
                    var updateCombination = validRequestAndHandlerTypes.FirstOrDefault(kvp =>
                        typeof(UpdateCommand<,>).IsAssignableFromGenericType(kvp.Key));
                    services.AddTransient(
                        typeof(IRequestHandler<>).MakeGenericType(
                            (updateCombination.Key ?? typeof(UpdateCommand<,>)).MakeGenericType(
                                crudEntity.Value.KeyType, crudEntity.Key)),
                        (updateCombination.Value ??
                         typeof(UpdateCommandHandler<,,>)).MakeGenericType(crudEntity.Key, crudEntity.Value.KeyType,
                            crudEntity.Value.ReadAllResultType.MakeGenericTypeIfNeeded(crudEntity.Key)));

                    // Delete
                    var deleteCombination = validRequestAndHandlerTypes.FirstOrDefault(kvp =>
                        typeof(DeleteCommand<>).IsAssignableFromGenericType(kvp.Key));
                    services.AddTransient(
                        typeof(IRequestHandler<>).MakeGenericType(
                            (deleteCombination.Key ?? typeof(DeleteCommand<>))
                            .MakeGenericType(crudEntity.Value.KeyType)),
                        (deleteCombination.Value ??
                         typeof(ReadQueryHandler<,,>)).MakeGenericType(crudEntity.Key, crudEntity.Value.KeyType,
                            crudEntity.Value.ReadAllResultType.MakeGenericTypeIfNeeded(crudEntity.Key)));
                }
            });

            return optionsBuilder;
        }
    }
}
