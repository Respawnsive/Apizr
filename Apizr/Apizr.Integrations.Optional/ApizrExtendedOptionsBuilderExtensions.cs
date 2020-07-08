using System;
using System.Collections.Generic;
using System.Linq;
using Apizr.Mediation.Cruding;
using Apizr.Mediation.Cruding.Base;
using Apizr.Mediation.Cruding.Handling;
using Apizr.Mediation.Cruding.Handling.Base;
using Apizr.Optional.Cruding;
using Apizr.Optional.Cruding.Handling;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Optional;

namespace Apizr
{
    public static class ApizrExtendedOptionsBuilderExtensions
    {
        /// <summary>
        /// Let Apizr handle crud requests execution with mediation and optional result
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <returns></returns>
        public static IApizrExtendedOptionsBuilder WithCrudOptionalMediation(this IApizrExtendedOptionsBuilder optionsBuilder)
        {
            return WithCrudOptionalMediation(optionsBuilder, default(Type));
        }

        /// <summary>
        /// Let Apizr handle crud requests execution with mediation and optional result, but with your own requests handlers
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <param name="crudHandlerTypes">Requests handlers types</param>
        /// <returns></returns>
        public static IApizrExtendedOptionsBuilder WithCrudOptionalMediation(this IApizrExtendedOptionsBuilder optionsBuilder, params Type[] crudHandlerTypes)
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
                            validCrudHandlerTypes.Add(typeof(ReadOptionalQuery<>), crudHandlerType);
                        else if (typeof(ReadQueryHandlerBase<,,,,,,>).IsAssignableFromGenericType(crudHandlerType))
                            validCrudHandlerTypes.Add(typeof(ReadOptionalQuery<,>), crudHandlerType);
                        else if (typeof(ReadAllQueryHandlerBase<,,,,,>).IsAssignableFromGenericType(crudHandlerType))
                            validCrudHandlerTypes.Add(typeof(ReadAllOptionalQuery<>), crudHandlerType);
                        else if (typeof(ReadAllQueryHandlerBase<,,,,,,>).IsAssignableFromGenericType(crudHandlerType))
                            validCrudHandlerTypes.Add(typeof(ReadAllOptionalQuery<,>), crudHandlerType);
                        else if (typeof(CreateCommandHandlerBase<,,,,,,>).IsAssignableFromGenericType(crudHandlerType))
                            validCrudHandlerTypes.Add(typeof(CreateOptionalCommand<>), crudHandlerType);
                        else if (typeof(UpdateCommandHandlerBase<,,,,>).IsAssignableFromGenericType(crudHandlerType))
                            validCrudHandlerTypes.Add(typeof(UpdateOptionalCommand<>), crudHandlerType);
                        else if (typeof(UpdateCommandHandlerBase<,,,,,,>).IsAssignableFromGenericType(crudHandlerType))
                            validCrudHandlerTypes.Add(typeof(UpdateOptionalCommand<,>), crudHandlerType);
                        else if (typeof(DeleteCommandHandlerBase<,,,,,>).IsAssignableFromGenericType(crudHandlerType))
                            validCrudHandlerTypes.Add(typeof(DeleteOptionalCommand<>), crudHandlerType);
                        else if (typeof(DeleteCommandHandlerBase<,,,,,,>).IsAssignableFromGenericType(crudHandlerType))
                            validCrudHandlerTypes.Add(typeof(DeleteOptionalCommand<,>), crudHandlerType);
                        else
                            throw new ArgumentException(
                                $"{crudHandlerType.GetFriendlyName()} must inherit from " +
                                $"{typeof(ReadQueryHandlerBase<,,,,,>).GetFriendlyName()}, {typeof(ReadQueryHandlerBase<,,,,,,>).GetFriendlyName()}, " +
                                $"{typeof(ReadAllQueryHandlerBase<,,,,,>).GetFriendlyName()}, {typeof(ReadAllQueryHandlerBase<,,,,,,>).GetFriendlyName()}, " +
                                $"{typeof(CreateCommandHandlerBase<,,,,,,>).GetFriendlyName()}, " +
                                $"{typeof(UpdateCommandHandlerBase<,,,,>).GetFriendlyName()}, {typeof(UpdateCommandHandlerBase<,,,,,,>).GetFriendlyName()},  " +
                                $"{typeof(DeleteCommandHandlerBase<,,,,,>).GetFriendlyName()} or {typeof(DeleteCommandHandlerBase<,,,,,,>).GetFriendlyName()}",
                                nameof(crudHandlerType));
                    }
                    else
                        throw new ArgumentException(
                            $"{crudHandlerType.GetFriendlyName()} must be open generic",
                            nameof(crudHandlerTypes));
                }
            }

            var crudRequestAndHandlerTypesArray = validCrudHandlerTypes.Select(kvp => (kvp.Key, kvp.Value)).ToArray();

            return WithCrudOptionalMediation(optionsBuilder, crudRequestAndHandlerTypesArray);
        }

        /// <summary>
        /// Let Apizr handle crud requests execution with mediation and optional result, but with your own requests and requests handlers
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <param name="crudRequestAndHandlerTypes">Requests and requests handlers types</param>
        /// <returns></returns>
        public static IApizrExtendedOptionsBuilder WithCrudOptionalMediation(this IApizrExtendedOptionsBuilder optionsBuilder, params (Type, Type)[] crudRequestAndHandlerTypes)
        {
            // Checking types validity
            var validRequestAndHandlerTypes = new Dictionary<Type, Type>();
            if (crudRequestAndHandlerTypes != null)
            {
                foreach (var requestAndHandlerTypeCombination in crudRequestAndHandlerTypes)
                {
                    if (validRequestAndHandlerTypes.ContainsKey(requestAndHandlerTypeCombination.Item1))
                        continue;

                    if (requestAndHandlerTypeCombination.Item1.IsOpenGeneric())
                    {
                        if (typeof(ReadOptionalQuery<>).IsAssignableFromGenericType(requestAndHandlerTypeCombination.Item1))
                        {
                            if (requestAndHandlerTypeCombination.Item2.IsOpenGeneric())
                            {
                                if (typeof(ReadOptionalQueryHandler<,,,>).IsAssignableFromGenericType(
                                   requestAndHandlerTypeCombination.Item2))
                                {
                                    validRequestAndHandlerTypes.Add(requestAndHandlerTypeCombination.Item1,
                                        requestAndHandlerTypeCombination.Item2);
                                }
                                else
                                    throw new ArgumentException(
                                        $"{requestAndHandlerTypeCombination.Item2.GetFriendlyName()} must inherit from {typeof(ReadOptionalQueryHandler<,,,>).GetFriendlyName()}");
                            }
                            else
                                throw new ArgumentException(
                                    $"{requestAndHandlerTypeCombination.Item2.GetFriendlyName()} must be open generic");
                        }
                        else if (typeof(ReadOptionalQuery<,>).IsAssignableFromGenericType(requestAndHandlerTypeCombination.Item1))
                        {
                            if (requestAndHandlerTypeCombination.Item2.IsOpenGeneric())
                            {
                                if (typeof(ReadOptionalQueryHandler<,,,,>).IsAssignableFromGenericType(
                                    requestAndHandlerTypeCombination.Item2))
                                {
                                    validRequestAndHandlerTypes.Add(requestAndHandlerTypeCombination.Item1,
                                        requestAndHandlerTypeCombination.Item2);
                                }
                                else
                                    throw new ArgumentException(
                                        $"{requestAndHandlerTypeCombination.Item2.GetFriendlyName()} must inherit from {typeof(ReadOptionalQueryHandler<,,,,>).GetFriendlyName()}");
                            }
                            else
                                throw new ArgumentException(
                                    $"{requestAndHandlerTypeCombination.Item2.GetFriendlyName()} must be open generic");
                        }
                        else if (typeof(ReadAllOptionalQuery<>).IsAssignableFromGenericType(requestAndHandlerTypeCombination.Item1))
                        {
                            if (requestAndHandlerTypeCombination.Item2.IsOpenGeneric())
                            {
                                if (typeof(ReadAllOptionalQueryHandler<,,,>).IsAssignableFromGenericType(
                                       requestAndHandlerTypeCombination.Item2))
                                {
                                    validRequestAndHandlerTypes.Add(requestAndHandlerTypeCombination.Item1,
                                        requestAndHandlerTypeCombination.Item2);
                                }
                                else
                                    throw new ArgumentException(
                                        $"{requestAndHandlerTypeCombination.Item2.GetFriendlyName()} must inherit from {typeof(ReadAllOptionalQueryHandler<,,,>).GetFriendlyName()}");
                            }
                            else
                                throw new ArgumentException(
                                    $"{requestAndHandlerTypeCombination.Item2.GetFriendlyName()} must be open generic");
                        }
                        else if (typeof(ReadAllOptionalQuery<,>).IsAssignableFromGenericType(requestAndHandlerTypeCombination.Item1))
                        {
                            if (requestAndHandlerTypeCombination.Item2.IsOpenGeneric())
                            {
                                if (typeof(ReadAllOptionalQueryHandler<,,,,>).IsAssignableFromGenericType(
                                    requestAndHandlerTypeCombination.Item2))
                                {
                                    validRequestAndHandlerTypes.Add(requestAndHandlerTypeCombination.Item1,
                                        requestAndHandlerTypeCombination.Item2);
                                }
                                else
                                    throw new ArgumentException(
                                        $"{requestAndHandlerTypeCombination.Item2.GetFriendlyName()} must inherit from {typeof(ReadAllOptionalQueryHandler<,,,,>).GetFriendlyName()}");
                            }
                            else
                                throw new ArgumentException(
                                    $"{requestAndHandlerTypeCombination.Item2.GetFriendlyName()} must be open generic");
                        }
                        else if (typeof(CreateOptionalCommand<>).IsAssignableFromGenericType(requestAndHandlerTypeCombination.Item1))
                        {
                            if (requestAndHandlerTypeCombination.Item2.IsOpenGeneric())
                            {
                                if (typeof(CreateOptionalCommandHandler<,,,,>).IsAssignableFromGenericType(
                                       requestAndHandlerTypeCombination.Item2))
                                {
                                    validRequestAndHandlerTypes.Add(requestAndHandlerTypeCombination.Item1,
                                        requestAndHandlerTypeCombination.Item2);
                                }
                                else
                                    throw new ArgumentException(
                                        $"{requestAndHandlerTypeCombination.Item2.GetFriendlyName()} must inherit from {typeof(CreateOptionalCommandHandler<,,,,>).GetFriendlyName()}");
                            }
                            else
                                throw new ArgumentException(
                                    $"{requestAndHandlerTypeCombination.Item2.GetFriendlyName()} must be open generic");
                        }
                        else if (typeof(UpdateOptionalCommand<>).IsAssignableFromGenericType(requestAndHandlerTypeCombination.Item1))
                        {
                            if (requestAndHandlerTypeCombination.Item2.IsOpenGeneric())
                            {
                                if (typeof(UpdateOptionalCommandHandler<,,,>).IsAssignableFromGenericType(
                                    requestAndHandlerTypeCombination.Item2))
                                {
                                    validRequestAndHandlerTypes.Add(requestAndHandlerTypeCombination.Item1,
                                        requestAndHandlerTypeCombination.Item2);
                                }
                                else
                                    throw new ArgumentException(
                                        $"{requestAndHandlerTypeCombination.Item2.GetFriendlyName()} must inherit from {typeof(UpdateOptionalCommandHandler<,,,>).GetFriendlyName()}");
                            }
                            else
                                throw new ArgumentException(
                                    $"{requestAndHandlerTypeCombination.Item2.GetFriendlyName()} must be open generic");
                        }
                        else if (typeof(UpdateOptionalCommand<,>).IsAssignableFromGenericType(requestAndHandlerTypeCombination.Item1))
                        {
                            if (requestAndHandlerTypeCombination.Item2.IsOpenGeneric())
                            {
                                if (typeof(UpdateOptionalCommandHandler<,,,,>).IsAssignableFromGenericType(
                                       requestAndHandlerTypeCombination.Item2))
                                {
                                    validRequestAndHandlerTypes.Add(requestAndHandlerTypeCombination.Item1,
                                        requestAndHandlerTypeCombination.Item2);
                                }
                                else
                                    throw new ArgumentException(
                                        $"{requestAndHandlerTypeCombination.Item2.GetFriendlyName()} must inherit from {typeof(UpdateOptionalCommandHandler<,,,,>).GetFriendlyName()}");
                            }
                            else
                                throw new ArgumentException(
                                    $"{requestAndHandlerTypeCombination.Item2.GetFriendlyName()} must be open generic");
                        }
                        else if (typeof(DeleteOptionalCommand<>).IsAssignableFromGenericType(requestAndHandlerTypeCombination.Item1))
                        {
                            if (requestAndHandlerTypeCombination.Item2.IsOpenGeneric())
                            {
                                if (typeof(DeleteOptionalCommandHandler<,,,>).IsAssignableFromGenericType(
                                       requestAndHandlerTypeCombination.Item2))
                                {
                                    validRequestAndHandlerTypes.Add(requestAndHandlerTypeCombination.Item1,
                                        requestAndHandlerTypeCombination.Item2);
                                }
                                else
                                    throw new ArgumentException(
                                        $"{requestAndHandlerTypeCombination.Item2.GetFriendlyName()} must inherit from {typeof(DeleteOptionalCommandHandler<,,,>).GetFriendlyName()}");
                            }
                            else
                                throw new ArgumentException(
                                    $"{requestAndHandlerTypeCombination.Item2.GetFriendlyName()} must be open generic");
                        }
                        else if (typeof(DeleteOptionalCommand<,>).IsAssignableFromGenericType(requestAndHandlerTypeCombination.Item1))
                        {
                            if (requestAndHandlerTypeCombination.Item2.IsOpenGeneric())
                            {
                                if (typeof(DeleteOptionalCommandHandler<,,,,>).IsAssignableFromGenericType(
                                    requestAndHandlerTypeCombination.Item2))
                                {
                                    validRequestAndHandlerTypes.Add(requestAndHandlerTypeCombination.Item1,
                                        requestAndHandlerTypeCombination.Item2);
                                }
                                else
                                    throw new ArgumentException(
                                        $"{requestAndHandlerTypeCombination.Item2.GetFriendlyName()} must inherit from {typeof(DeleteOptionalCommandHandler<,,,,>).GetFriendlyName()}");
                            }
                            else
                                throw new ArgumentException(
                                    $"{requestAndHandlerTypeCombination.Item2.GetFriendlyName()} must be open generic");
                        }
                        else
                            throw new ArgumentException(
                                $"{requestAndHandlerTypeCombination.Item1.GetFriendlyName()} must inherit from " +
                                $"{typeof(ReadOptionalQuery<>).GetFriendlyName()}, {typeof(ReadOptionalQuery<,>).GetFriendlyName()}, " +
                                $"{typeof(ReadAllOptionalQuery<>).GetFriendlyName()}, {typeof(ReadAllOptionalQuery<,>).GetFriendlyName()}, " +
                                $"{typeof(CreateOptionalCommand<>).GetFriendlyName()}, " +
                                $"{typeof(UpdateOptionalCommand<>).GetFriendlyName()}, {typeof(UpdateOptionalCommand<,>).GetFriendlyName()},  " +
                                $"{typeof(DeleteOptionalCommand<>).GetFriendlyName()} or {typeof(DeleteOptionalCommand<,>).GetFriendlyName()}");
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

                    // Completing missing handlers
                    var crudedEntityValidRequestAndHandlerTypes = new Dictionary<Type, Type>(validRequestAndHandlerTypes);

                    if (apiEntityAttribute.KeyType == typeof(int) &&
                        !validRequestAndHandlerTypes.Any(kvp => typeof(ReadOptionalQuery<>).IsAssignableFromGenericType(kvp.Key)))
                        crudedEntityValidRequestAndHandlerTypes.Add(typeof(ReadOptionalQuery<>), typeof(ReadOptionalQueryHandler<,,,>));

                    if (!validRequestAndHandlerTypes.Any(kvp => typeof(ReadOptionalQuery<,>).IsAssignableFromGenericType(kvp.Key)))
                        crudedEntityValidRequestAndHandlerTypes.Add(typeof(ReadOptionalQuery<,>), typeof(ReadOptionalQueryHandler<,,,,>));

                    if (apiEntityAttribute.ReadAllParamsType == typeof(IDictionary<string, object>) &&
                        !validRequestAndHandlerTypes.Any(kvp => typeof(ReadAllOptionalQuery<>).IsAssignableFromGenericType(kvp.Key)))
                        crudedEntityValidRequestAndHandlerTypes.Add(typeof(ReadAllOptionalQuery<>), typeof(ReadAllOptionalQueryHandler<,,,>));

                    if (!validRequestAndHandlerTypes.Any(kvp => typeof(ReadAllOptionalQuery<,>).IsAssignableFromGenericType(kvp.Key)))
                        crudedEntityValidRequestAndHandlerTypes.Add(typeof(ReadAllOptionalQuery<,>), typeof(ReadAllOptionalQueryHandler<,,,,>));

                    if (!validRequestAndHandlerTypes.Any(kvp => typeof(CreateOptionalCommand<>).IsAssignableFromGenericType(kvp.Key)))
                        crudedEntityValidRequestAndHandlerTypes.Add(typeof(CreateOptionalCommand<>), typeof(CreateOptionalCommandHandler<,,,,>));

                    if (apiEntityAttribute.KeyType == typeof(int) &&
                        !validRequestAndHandlerTypes.Any(kvp => typeof(UpdateOptionalCommand<>).IsAssignableFromGenericType(kvp.Key)))
                        crudedEntityValidRequestAndHandlerTypes.Add(typeof(UpdateOptionalCommand<>), typeof(UpdateOptionalCommandHandler<,,,>));

                    if (!validRequestAndHandlerTypes.Any(kvp => typeof(UpdateOptionalCommand<,>).IsAssignableFromGenericType(kvp.Key)))
                        crudedEntityValidRequestAndHandlerTypes.Add(typeof(UpdateOptionalCommand<,>), typeof(UpdateOptionalCommandHandler<,,,,>));

                    if (apiEntityAttribute.KeyType == typeof(int) &&
                        !validRequestAndHandlerTypes.Any(kvp => typeof(DeleteOptionalCommand<>).IsAssignableFromGenericType(kvp.Key)))
                        crudedEntityValidRequestAndHandlerTypes.Add(typeof(DeleteOptionalCommand<>), typeof(DeleteOptionalCommandHandler<,,,>));

                    if (!validRequestAndHandlerTypes.Any(kvp => typeof(DeleteOptionalCommand<,>).IsAssignableFromGenericType(kvp.Key)))
                        crudedEntityValidRequestAndHandlerTypes.Add(typeof(DeleteOptionalCommand<,>), typeof(DeleteOptionalCommandHandler<,,,,>));
                    
                    var requestResponseTypes = new Dictionary<Type, Type>
                    {
                        { typeof(ReadQueryBase<,>), typeof(Option<,>).MakeGenericType(modelEntityType, typeof(ApizrException<>).MakeGenericType(modelEntityType)) },
                        { typeof(ReadAllQueryBase<,>), typeof(Option<,>).MakeGenericType(modelEntityReadAllResultType, typeof(ApizrException<>).MakeGenericType(modelEntityReadAllResultType)) },
                        { typeof(CreateCommandBase<,>), typeof(Option<,>).MakeGenericType(modelEntityType, typeof(ApizrException)) },
                        { typeof(UpdateCommandBase<,,>), typeof(Option<Unit,ApizrException>) },
                        { typeof(DeleteCommandBase<,,>), typeof(Option<Unit,ApizrException>) }
                    };

                    // Registering
                    optionsBuilder.WithCrudMediation(services, apiEntityType, apiEntityAttribute, crudedEntityValidRequestAndHandlerTypes, requestResponseTypes);
                }
            });
            
            return optionsBuilder;
        }
    }
}
