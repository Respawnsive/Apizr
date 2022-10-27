using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Mediation.Cruding;
using Apizr.Mediation.Cruding.Sending;
using Polly;

namespace Apizr.Mediation.Extending
{
    public static class ApizrTypedCrudMediatorExtensions
    {
        #region Create

        #region SendCreateCommand

        /// <summary>
        /// Send a <see cref="CreateCommand{TModelData}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="onException">Handle exception</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TApiEntity> SendCreateCommand<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TApiEntity entity, Action<Exception> onException) where TApiEntity : class =>
            mediator.SendCreateCommand(entity, options => options.Catch(onException));

        /// <summary>
        /// Send a <see cref="CreateCommand{TApiEntity}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="context">The Polly context</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TApiEntity> SendCreateCommand<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TApiEntity entity, Context context, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendCreateCommand(entity,
                options => options.WithContext(context)
                    .Catch(onException));

        /// <summary>
        /// Send a <see cref="CreateCommand{TApiEntity}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TApiEntity> SendCreateCommand<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TApiEntity entity, CancellationToken cancellationToken, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendCreateCommand(entity,
                options => options.WithCancellation(cancellationToken)
                    .Catch(onException));

        /// <summary>
        /// Send a <see cref="CreateCommand{TApiEntity}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TApiEntity> SendCreateCommand<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TApiEntity entity, Context context, CancellationToken cancellationToken,
            Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendCreateCommand(entity,
                options => options.WithContext(context)
                    .WithCancellation(cancellationToken)
                    .Catch(onException));

        #endregion

        #region SendCreateCommand<TModelEntity>

        /// <summary>
        /// Send a mapped <see cref="CreateCommand{TModelEntity}"/> to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="onException">Handle exception</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelEntity> SendCreateCommand<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TModelEntity entity, Action<Exception> onException) where TApiEntity : class =>
            mediator.SendCreateCommand<TModelEntity>(entity,
                options => options.Catch(onException));

        /// <summary>
        /// Send a mapped <see cref="CreateCommand{TModelEntity}"/> to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="context">The Polly context</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelEntity> SendCreateCommand<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TModelEntity entity, Context context, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendCreateCommand<TModelEntity>(entity,
                options => options.WithContext(context)
                    .Catch(onException));

        /// <summary>
        /// Send a mapped <see cref="CreateCommand{TModelEntity}"/> to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelEntity> SendCreateCommand<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TModelEntity entity, CancellationToken cancellationToken, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendCreateCommand<TModelEntity>(entity,
                options => options.WithCancellation(cancellationToken)
                    .Catch(onException));

        /// <summary>
        /// Send a mapped <see cref="CreateCommand{TModelEntity}"/> to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelEntity> SendCreateCommand<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TModelEntity entity, Context context, CancellationToken cancellationToken,
            Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendCreateCommand<TModelEntity>(entity,
                options => options.WithContext(context)
                    .WithCancellation(cancellationToken)
                    .Catch(onException));

        #endregion

        #endregion

        #region ReadAll

        #region SendReadAllQuery

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="onException">Handle exception and return cached result</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TReadAllResult> SendReadAllQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            Action<Exception> onException) where TApiEntity : class =>
            mediator.SendReadAllQuery(options => options.Catch(onException));

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TReadAllResult> SendReadAllQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            bool clearCache, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendReadAllQuery(options => options.ClearCache(clearCache)
                .Catch(onException));

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TReadAllResult> SendReadAllQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            Context context, bool clearCache = false, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendReadAllQuery(options => options.WithContext(context)
                .ClearCache(clearCache)
                .Catch(onException));

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TReadAllResult> SendReadAllQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendReadAllQuery(options => options.WithCancellation(cancellationToken)
                .ClearCache(clearCache)
                .Catch(onException));

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TReadAllResult> SendReadAllQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            int priority, bool clearCache = false, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendReadAllQuery(priority, options => options.ClearCache(clearCache)
                .Catch(onException));

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TReadAllResult> SendReadAllQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            int priority, Context context, bool clearCache = false, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendReadAllQuery(priority, options => options.WithContext(context)
                .ClearCache(clearCache)
                .Catch(onException));

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TReadAllResult> SendReadAllQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            int priority, CancellationToken cancellationToken, bool clearCache = false,
            Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendReadAllQuery(priority, options => options
                .WithCancellation(cancellationToken)
                .ClearCache(clearCache)
                .Catch(onException));

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TReadAllResult> SendReadAllQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator, Context context, CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendReadAllQuery(options => options.WithContext(context)
                .WithCancellation(cancellationToken)
                .ClearCache(clearCache)
                .Catch(onException));

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TReadAllResult> SendReadAllQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            int priority, Context context, CancellationToken cancellationToken, bool clearCache = false,
            Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendReadAllQuery(priority, options => options.WithContext(context)
                .WithCancellation(cancellationToken)
                .ClearCache(clearCache)
                .Catch(onException));

        #endregion

        #region SendReadAllQuery<TModelReadAllResult>

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelReadAllResult}"/> to Apizr using MediatR returning mapped result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="onException">Handle exception and return cached result</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
            this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator, Action<Exception> onException) where TApiEntity : class =>
            mediator.SendReadAllQuery<TModelReadAllResult>(options =>
                options.Catch(onException));

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelReadAllResult}"/> to Apizr using MediatR returning mapped result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
            this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator, bool clearCache, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendReadAllQuery<TModelReadAllResult>(options => options
                .ClearCache(clearCache)
                .Catch(onException));

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelReadAllResult}"/> to Apizr using MediatR returning mapped result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
            this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator, Context context, bool clearCache = false,
            Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendReadAllQuery<TModelReadAllResult>(options => options.WithContext(context)
                .ClearCache(clearCache)
                .Catch(onException));

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelReadAllResult}"/> to Apizr using MediatR returning mapped result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
            this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator, CancellationToken cancellationToken, bool clearCache = false,
            Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendReadAllQuery<TModelReadAllResult>(options => options
                .WithCancellation(cancellationToken)
                .ClearCache(clearCache)
                .Catch(onException));

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelReadAllResult}"/> to Apizr using MediatR with priority returning mapped result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
            this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator, int priority, bool clearCache = false,
            Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendReadAllQuery<TModelReadAllResult>(priority, options => options
                .ClearCache(clearCache)
                .Catch(onException));

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelReadAllResult}"/> to Apizr using MediatR with priority returning mapped result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
            this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator, int priority,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendReadAllQuery<TModelReadAllResult>(priority, options => options
                .WithCancellation(cancellationToken)
                .ClearCache(clearCache)
                .Catch(onException));

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelReadAllResult}"/> to Apizr using MediatR with priority returning mapped result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
            this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator, int priority, Context context, bool clearCache = false,
            Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendReadAllQuery<TModelReadAllResult>(priority, options => options
                .WithContext(context)
                .ClearCache(clearCache)
                .Catch(onException));

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelReadAllResult}"/> to Apizr using MediatR with priority returning mapped result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
            this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator, Context context,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendReadAllQuery<TModelReadAllResult>(options => options.WithContext(context)
                .WithCancellation(cancellationToken)
                .ClearCache(clearCache)
                .Catch(onException));

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelReadAllResult}"/> to Apizr using MediatR with priority returning mapped result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
            this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator, int priority, Context context,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendReadAllQuery<TModelReadAllResult>(priority, options => options
                .WithContext(context)
                .WithCancellation(cancellationToken)
                .ClearCache(clearCache)
                .Catch(onException));

        #endregion

        #region SendReadAllQuery(TReadAllParams)

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="onException">Handle exception and return cached result</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TReadAllResult> SendReadAllQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
            this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator, TReadAllParams readAllParams, Action<Exception> onException) where TApiEntity : class =>
            mediator.SendReadAllQuery(readAllParams, options => options
                .Catch(onException));

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TReadAllResult> SendReadAllQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
            this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator, TReadAllParams readAllParams, bool clearCache,
            Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendReadAllQuery(readAllParams, options => options
                .ClearCache(clearCache)
                .Catch(onException));

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TReadAllResult> SendReadAllQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
            this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator, TReadAllParams readAllParams, Context context, bool clearCache = false,
            Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendReadAllQuery(readAllParams, options => options
                .WithContext(context)
                .ClearCache(clearCache)
                .Catch(onException));

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TReadAllResult> SendReadAllQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
            this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator, TReadAllParams readAllParams,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendReadAllQuery(readAllParams, options => options
                .WithCancellation(cancellationToken)
                .ClearCache(clearCache)
                .Catch(onException));

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TReadAllResult> SendReadAllQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
            this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator, TReadAllParams readAllParams,
            int priority, bool clearCache = false, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendReadAllQuery(readAllParams, priority, options => options
                .ClearCache(clearCache)
                .Catch(onException));

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TReadAllResult> SendReadAllQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
            this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator, TReadAllParams readAllParams,
            int priority, Context context, bool clearCache = false, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendReadAllQuery(readAllParams, priority, options => options
                .WithContext(context)
                .ClearCache(clearCache)
                .Catch(onException));

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TReadAllResult> SendReadAllQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
            this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator, TReadAllParams readAllParams,
            int priority,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendReadAllQuery(readAllParams, priority, options => options
                .WithCancellation(cancellationToken)
                .ClearCache(clearCache)
                .Catch(onException));

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TReadAllResult> SendReadAllQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
            this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator, TReadAllParams readAllParams, Context context,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendReadAllQuery(readAllParams, options => options
                .WithContext(context)
                .WithCancellation(cancellationToken)
                .ClearCache(clearCache)
                .Catch(onException));

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TReadAllResult> SendReadAllQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
            this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator, TReadAllParams readAllParams,
            int priority, Context context,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendReadAllQuery(readAllParams, priority, options => options
                .WithContext(context)
                .WithCancellation(cancellationToken)
                .ClearCache(clearCache)
                .Catch(onException));

        #endregion

        #region SendReadAllQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator, TReadAllParams)

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelReadAllResult}"/> to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="onException">Handle exception and return cached result</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelReadAllResult>
            SendReadAllQuery<TModelReadAllResult, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
                TReadAllParams readAllParams, Action<Exception> onException) where TApiEntity : class =>
            mediator.SendReadAllQuery<TModelReadAllResult>(readAllParams, options =>
                options
                    .Catch(onException));

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelReadAllResult}"/> to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelReadAllResult>
            SendReadAllQuery<TModelReadAllResult, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
                TReadAllParams readAllParams, bool clearCache, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendReadAllQuery<TModelReadAllResult>(readAllParams, options =>
                options
                    .ClearCache(clearCache)
                    .Catch(onException));

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelReadAllResult}"/> to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelReadAllResult>
            SendReadAllQuery<TModelReadAllResult, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
                TReadAllParams readAllParams, Context context, bool clearCache = false,
                Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendReadAllQuery<TModelReadAllResult>(readAllParams, options =>
                options
                    .WithContext(context)
                    .ClearCache(clearCache)
                    .Catch(onException));

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelReadAllResult}"/> to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelReadAllResult>
            SendReadAllQuery<TModelReadAllResult, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
                TReadAllParams readAllParams,
                CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendReadAllQuery<TModelReadAllResult>(readAllParams, options =>
                options
                    .WithCancellation(cancellationToken)
                    .ClearCache(clearCache)
                    .Catch(onException));

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelReadAllResult}"/> to Apizr using MediatR with priority returning a mapped result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelReadAllResult>
            SendReadAllQuery<TModelReadAllResult, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
                TReadAllParams readAllParams,
                int priority, bool clearCache = false, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendReadAllQuery<TModelReadAllResult>(readAllParams, priority,
                options => options
                    .ClearCache(clearCache)
                    .Catch(onException));

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelReadAllResult}"/> to Apizr using MediatR with priority returning a mapped result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelReadAllResult>
            SendReadAllQuery<TModelReadAllResult, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
                TReadAllParams readAllParams,
                int priority, Context context, bool clearCache = false, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendReadAllQuery<TModelReadAllResult>(readAllParams, priority,
                options => options
                    .WithContext(context)
                    .ClearCache(clearCache)
                    .Catch(onException));

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelReadAllResult}"/> to Apizr using MediatR with priority returning a mapped result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelReadAllResult>
            SendReadAllQuery<TModelReadAllResult, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
                TReadAllParams readAllParams,
                int priority,
                CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendReadAllQuery<TModelReadAllResult>(readAllParams, priority,
                options => options
                    .WithCancellation(cancellationToken)
                    .ClearCache(clearCache)
                    .Catch(onException));

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelReadAllResult}"/> to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelReadAllResult>
            SendReadAllQuery<TModelReadAllResult, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
                TReadAllParams readAllParams, Context context,
                CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendReadAllQuery<TModelReadAllResult>(readAllParams, options =>
                options
                    .WithContext(context)
                    .WithCancellation(cancellationToken)
                    .ClearCache(clearCache)
                    .Catch(onException));

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelReadAllResult}"/> to Apizr using MediatR with priority returning a mapped result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelReadAllResult>
            SendReadAllQuery<TModelReadAllResult, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
                TReadAllParams readAllParams,
                int priority, Context context,
                CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendReadAllQuery<TModelReadAllResult>(readAllParams, priority,
                options => options
                    .WithContext(context)
                    .WithCancellation(cancellationToken)
                    .ClearCache(clearCache)
                    .Catch(onException));

        #endregion

        #endregion

        #region Read

        #region SendReadQuery

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="onException">Handle exception and return cached result</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TApiEntity> SendReadQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TApiEntityKey key, Action<Exception> onException) where TApiEntity : class =>
            mediator.SendReadQuery(key, options => options
                .Catch(onException));

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TApiEntity> SendReadQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TApiEntityKey key, bool clearCache, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendReadQuery(key, options => options
                .ClearCache(clearCache)
                .Catch(onException));

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TApiEntity> SendReadQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TApiEntityKey key, Context context, bool clearCache = false, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendReadQuery(key, options => options
                .WithContext(context)
                .ClearCache(clearCache)
                .Catch(onException));

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TApiEntity> SendReadQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TApiEntityKey key,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendReadQuery(key, options => options
                .WithCancellation(cancellationToken)
                .ClearCache(clearCache)
                .Catch(onException));

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR with priority
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TApiEntity> SendReadQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TApiEntityKey key,
            int priority, bool clearCache = false, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendReadQuery(key, priority,
                options => options
                    .ClearCache(clearCache)
                    .Catch(onException));

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TApiEntity> SendReadQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TApiEntityKey key, Context context,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendReadQuery(key, options => options
                .WithContext(context)
                .WithCancellation(cancellationToken)
                .ClearCache(clearCache)
                .Catch(onException));

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR with priority
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TApiEntity> SendReadQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TApiEntityKey key,
            int priority, Context context, bool clearCache = false, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendReadQuery(key, priority,
                options => options
                    .WithContext(context)
                    .ClearCache(clearCache)
                    .Catch(onException));

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR with priority
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TApiEntity> SendReadQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TApiEntityKey key,
            int priority,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendReadQuery(key, priority,
                options => options
                    .WithCancellation(cancellationToken)
                    .ClearCache(clearCache)
                    .Catch(onException));

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR with priority
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TApiEntity> SendReadQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TApiEntityKey key,
            int priority, Context context,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendReadQuery(key, priority,
                options => options
                    .WithContext(context)
                    .WithCancellation(cancellationToken)
                    .ClearCache(clearCache)
                    .Catch(onException));

        #endregion

        #region SendReadQuery<TModelEntity>

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="onException">Handle exception and return cached result</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelEntity> SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
            this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator, TApiEntityKey key, Action<Exception> onException) where TApiEntity : class =>
            mediator.SendReadQuery<TModelEntity>(key,
                options => options
                    .Catch(onException));

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelEntity> SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
            this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator, TApiEntityKey key, bool clearCache,
            Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendReadQuery<TModelEntity>(key,
                options => options
                    .ClearCache(clearCache)
                    .Catch(onException));

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelEntity> SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
            this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator, TApiEntityKey key, Context context, bool clearCache = false,
            Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendReadQuery<TModelEntity>(key,
                options => options
                    .WithContext(context)
                    .ClearCache(clearCache)
                    .Catch(onException));

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelEntity> SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator, TApiEntityKey key,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendReadQuery<TModelEntity>(key,
                options => options
                    .WithCancellation(cancellationToken)
                    .ClearCache(clearCache)
                    .Catch(onException));

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR with priority returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelEntity> SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
            this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator, TApiEntityKey key,
            int priority, bool clearCache = false, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendReadQuery<TModelEntity>(key, priority,
                options => options
                    .ClearCache(clearCache)
                    .Catch(onException));

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelEntity> SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
            this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator, TApiEntityKey key, Context context,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendReadQuery<TModelEntity>(key,
                options => options
                    .WithContext(context)
                    .WithCancellation(cancellationToken)
                    .ClearCache(clearCache)
                    .Catch(onException));

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR with priority returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelEntity> SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
            this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator, TApiEntityKey key,
            int priority, Context context, bool clearCache = false, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendReadQuery<TModelEntity>(key, priority,
                options => options
                    .WithContext(context)
                    .ClearCache(clearCache)
                    .Catch(onException));

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR with priority returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelEntity> SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
            this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator, TApiEntityKey key,
            int priority,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendReadQuery<TModelEntity>(key, priority,
                options => options
                    .WithCancellation(cancellationToken)
                    .ClearCache(clearCache)
                    .Catch(onException));

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR with priority returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelEntity> SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
            this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator, TApiEntityKey key,
            int priority, Context context,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendReadQuery<TModelEntity>(key, priority,
                options => options
                    .WithContext(context)
                    .WithCancellation(cancellationToken)
                    .ClearCache(clearCache)
                    .Catch(onException));

        #endregion

        #endregion

        #region Update

        #region SendUpdateCommand

        /// <summary>
        /// Send a <see cref="UpdateCommand{TApiEntityKey, TApiEntity}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="onException">Handle exception</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task SendUpdateCommand<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TApiEntityKey key,
            TApiEntity entity, Action<Exception> onException) where TApiEntity : class =>
            mediator.SendUpdateCommand(key, entity,
                options => options
                    .Catch(onException));

        /// <summary>
        /// Send a <see cref="UpdateCommand{TApiEntityKey, TApiEntity}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="context">The Polly context</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task SendUpdateCommand<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TApiEntityKey key,
            TApiEntity entity, Context context, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendUpdateCommand(key, entity,
                options => options
                    .WithContext(context)
                    .Catch(onException));

        /// <summary>
        /// Send a <see cref="UpdateCommand{TApiEntityKey, TApiEntity}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task SendUpdateCommand<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TApiEntityKey key,
            TApiEntity entity,
            CancellationToken cancellationToken, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendUpdateCommand(key, entity,
                options => options
                    .WithCancellation(cancellationToken)
                    .Catch(onException));

        /// <summary>
        /// Send a <see cref="UpdateCommand{TApiEntityKey, TApiEntity}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task SendUpdateCommand<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TApiEntityKey key,
            TApiEntity entity, Context context,
            CancellationToken cancellationToken, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendUpdateCommand(key, entity,
                options => options
                    .WithContext(context)
                    .WithCancellation(cancellationToken)
                    .Catch(onException));

        #endregion

        #region SendUpdateCommand<TModelEntity>

        /// <summary>
        /// Send a mapped <see cref="UpdateCommand{TApiEntityKey, TModelEntity}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="onException">Handle exception</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task SendUpdateCommand<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TApiEntityKey key,
            TModelEntity entity, Action<Exception> onException) where TApiEntity : class =>
            mediator.SendUpdateCommand<TModelEntity>(key, entity,
                options => options
                    .Catch(onException));

        /// <summary>
        /// Send a mapped <see cref="UpdateCommand{TApiEntityKey, TModelEntity}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="context">The Polly context</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task SendUpdateCommand<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TApiEntityKey key,
            TModelEntity entity, Context context, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendUpdateCommand<TModelEntity>(key, entity,
                options => options
                    .WithContext(context)
                    .Catch(onException));

        /// <summary>
        /// Send a mapped <see cref="UpdateCommand{TApiEntityKey, TModelEntity}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task SendUpdateCommand<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TApiEntityKey key,
            TModelEntity entity,
            CancellationToken cancellationToken, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendUpdateCommand<TModelEntity>(key, entity,
                options => options
                    .WithCancellation(cancellationToken)
                    .Catch(onException));

        /// <summary>
        /// Send a mapped <see cref="UpdateCommand{TApiEntityKey, TModelEntity}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task SendUpdateCommand<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TApiEntityKey key,
            TModelEntity entity, Context context,
            CancellationToken cancellationToken, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendUpdateCommand<TModelEntity>(key, entity,
                options => options
                    .WithContext(context)
                    .WithCancellation(cancellationToken)
                    .Catch(onException));

        #endregion

        #endregion

        #region Delete

        /// <summary>
        /// Send a <see cref="DeleteCommand{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="onException">Handle exception</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task SendDeleteCommand<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TApiEntityKey key, Action<Exception> onException) where TApiEntity : class =>
            mediator.SendDeleteCommand(key,
                options => options
                    .Catch(onException));

        /// <summary>
        /// Send a <see cref="DeleteCommand{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="context">The Polly context</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task SendDeleteCommand<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TApiEntityKey key, Context context, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendDeleteCommand(key,
                options => options
                    .WithContext(context)
                    .Catch(onException));

        /// <summary>
        /// Send a <see cref="DeleteCommand{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task SendDeleteCommand<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TApiEntityKey key,
            CancellationToken cancellationToken, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendDeleteCommand(key,
                options => options
                    .WithCancellation(cancellationToken)
                    .Catch(onException));

        /// <summary>
        /// Send a <see cref="DeleteCommand{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task SendDeleteCommand<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(this IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TApiEntityKey key, Context context,
            CancellationToken cancellationToken, Action<Exception> onException = null) where TApiEntity : class =>
            mediator.SendDeleteCommand(key,
                options => options
                    .WithContext(context)
                    .WithCancellation(cancellationToken)
                    .Catch(onException));

        #endregion
    }
}
