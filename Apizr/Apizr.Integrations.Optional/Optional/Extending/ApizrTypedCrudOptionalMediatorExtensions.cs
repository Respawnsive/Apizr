using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Mediation.Cruding;
using Apizr.Optional.Cruding;
using Apizr.Optional.Cruding.Sending;
using MediatR;
using Optional;
using Polly;

namespace Apizr.Optional.Extending
{
    public static class ApizrTypedCrudOptionalMediatorExtensions
    {
        #region Create

        #region SendCreateOptionalCommand

        /// <summary>
        /// Send a <see cref="CreateOptionalCommand{TModelData}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TApiEntity, ApizrException>> SendCreateOptionalCommand<TApiEntity, TApiEntityKey,
            TReadAllResult, TReadAllParams>(
            this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TApiEntity entity, Context context) where TApiEntity : class =>
            mediator.SendCreateOptionalCommand(entity,
                options => options.WithContext(context));

        /// <summary>
        /// Send a <see cref="CreateOptionalCommand{TApiEntity}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TApiEntity, ApizrException>> SendCreateOptionalCommand<TApiEntity, TApiEntityKey,
            TReadAllResult, TReadAllParams>(
            this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TApiEntity entity,
            CancellationToken cancellationToken) where TApiEntity : class =>
            mediator.SendCreateOptionalCommand(entity,
                options => options.WithCancellation(cancellationToken));

        /// <summary>
        /// Send a <see cref="CreateOptionalCommand{TApiEntity}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TApiEntity, ApizrException>> SendCreateOptionalCommand<TApiEntity, TApiEntityKey,
            TReadAllResult, TReadAllParams>(
            this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TApiEntity entity, Context context,
            CancellationToken cancellationToken) where TApiEntity : class =>
            mediator.SendCreateOptionalCommand(entity,
                options => options.WithContext(context)
                    .WithCancellation(cancellationToken));

        #endregion

        #region SendCreateOptionalCommand<TModelEntity>

        /// <summary>
        /// Send a mapped <see cref="CreateCommand{TModelData}"/> to Apizr using MediatR and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelEntity, ApizrException>> SendCreateOptionalCommand<TModelEntity, TApiEntity,
            TApiEntityKey, TReadAllResult, TReadAllParams>(
            this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TModelEntity entity, Context context) where TApiEntity : class =>
            mediator.SendCreateOptionalCommand<TModelEntity>(entity,
                options => options.WithContext(context));

        /// <summary>
        /// Send a mapped <see cref="CreateCommand{TModelEntity}"/> to Apizr using MediatR and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelEntity, ApizrException>> SendCreateOptionalCommand<TModelEntity, TApiEntity,
            TApiEntityKey, TReadAllResult, TReadAllParams>(
            this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TModelEntity entity,
            CancellationToken cancellationToken) where TApiEntity : class =>
            mediator.SendCreateOptionalCommand<TModelEntity>(entity,
                options => options.WithCancellation(cancellationToken));

        /// <summary>
        /// Send a mapped <see cref="CreateCommand{TModelEntity}"/> to Apizr using MediatR and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelEntity, ApizrException>> SendCreateOptionalCommand<TModelEntity, TApiEntity,
            TApiEntityKey, TReadAllResult, TReadAllParams>(
            this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TModelEntity entity, Context context,
            CancellationToken cancellationToken) where TApiEntity : class =>
            mediator.SendCreateOptionalCommand<TModelEntity>(entity,
                options => options.WithContext(context)
                    .WithCancellation(cancellationToken));

        #endregion

        #endregion

        #region ReadAll

        #region SendReadAllOptionalQuery

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TReadAllResult, ApizrException<TReadAllResult>>>
            SendReadAllOptionalQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
                this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
                Context context,
                bool clearCache = false) where TApiEntity : class =>
            mediator.SendReadAllOptionalQuery(options => options.WithContext(context)
                .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TReadAllResult, ApizrException<TReadAllResult>>>
            SendReadAllOptionalQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
                this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
                CancellationToken cancellationToken, bool clearCache = false) where TApiEntity : class =>
            mediator.SendReadAllOptionalQuery(options => options
                .WithCancellation(cancellationToken)
                .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TReadAllResult, ApizrException<TReadAllResult>>>
            SendReadAllOptionalQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
                this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
                int priority,
                bool clearCache = false) where TApiEntity : class =>
            mediator.SendReadAllOptionalQuery(priority,
                options => options.ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TReadAllResult, ApizrException<TReadAllResult>>>
            SendReadAllOptionalQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator, int priority,
                Context context, bool clearCache = false) where TApiEntity : class =>
            mediator.SendReadAllOptionalQuery(priority, options => options.WithContext(context)
                .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TReadAllResult, ApizrException<TReadAllResult>>>
            SendReadAllOptionalQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
                this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
                int priority,
                CancellationToken cancellationToken, bool clearCache = false) where TApiEntity : class =>
            mediator.SendReadAllOptionalQuery(priority, options => options
                .WithCancellation(cancellationToken)
                .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TReadAllResult, ApizrException<TReadAllResult>>>
            SendReadAllOptionalQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
                this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
                Context context,
                CancellationToken cancellationToken, bool clearCache = false) where TApiEntity : class =>
            mediator.SendReadAllOptionalQuery(options => options.WithContext(context)
                .WithCancellation(cancellationToken)
                .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TReadAllResult, ApizrException<TReadAllResult>>>
            SendReadAllOptionalQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
                this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
                int priority,
                Context context,
                CancellationToken cancellationToken, bool clearCache = false) where TApiEntity : class =>
            mediator.SendReadAllOptionalQuery(priority, options => options.WithContext(context)
                .WithCancellation(cancellationToken)
                .ClearCache(clearCache));

        #endregion

        #region SendReadAllOptionalQuery<TModelReadAllResult>

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR and returning mapped optional result
        /// </summary>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
                this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
                bool clearCache = false) where TApiEntity : class =>
            mediator.SendReadAllOptionalQuery<TModelReadAllResult>(options => options
                .ClearCache(clearCache));

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR and returning mapped optional result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
                this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
                Context context, bool clearCache = false) where TApiEntity : class =>
            mediator.SendReadAllOptionalQuery<TModelReadAllResult>(options => options
                .WithContext(context)
                .ClearCache(clearCache));

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR and returning mapped optional result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
                this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
                CancellationToken cancellationToken, bool clearCache = false) where TApiEntity : class =>
            mediator.SendReadAllOptionalQuery<TModelReadAllResult>(options => options
                .WithCancellation(cancellationToken)
                .ClearCache(clearCache));

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR with priority and returning mapped optional result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
                this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
                int priority, bool clearCache = false) where TApiEntity : class =>
            mediator.SendReadAllOptionalQuery<TModelReadAllResult>(priority, options => options
                .ClearCache(clearCache));

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR with priority and returning mapped optional result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
                this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
                int priority, CancellationToken cancellationToken, bool clearCache = false) where TApiEntity : class =>
            mediator.SendReadAllOptionalQuery<TModelReadAllResult>(priority, options => options
                .WithCancellation(cancellationToken)
                .ClearCache(clearCache));

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR with priority and returning mapped optional result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
                this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
                int priority, Context context, bool clearCache = false) where TApiEntity : class =>
            mediator.SendReadAllOptionalQuery<TModelReadAllResult>(priority, options => options
                .WithContext(context)
                .ClearCache(clearCache));

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR and returning mapped optional result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
                this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
                Context context,
                CancellationToken cancellationToken, bool clearCache = false) where TApiEntity : class =>
            mediator.SendReadAllOptionalQuery<TModelReadAllResult>(options => options
                .WithContext(context)
                .WithCancellation(cancellationToken)
                .ClearCache(clearCache));

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR with priority and returning mapped optional result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
                this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
                int priority, Context context,
                CancellationToken cancellationToken, bool clearCache = false) where TApiEntity : class =>
            mediator.SendReadAllOptionalQuery<TModelReadAllResult>(priority, options => options
                .WithContext(context)
                .WithCancellation(cancellationToken)
                .ClearCache(clearCache));

        #endregion

        #region SendReadAllOptionalQuery(TReadAllParams)

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TReadAllResult, ApizrException<TReadAllResult>>>
            SendReadAllOptionalQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
                this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
                TReadAllParams readAllParams, bool clearCache = false) where TApiEntity : class =>
            mediator.SendReadAllOptionalQuery(readAllParams, options =>
                options
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TReadAllResult, ApizrException<TReadAllResult>>>
            SendReadAllOptionalQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
                this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
                TReadAllParams readAllParams, Context context, bool clearCache = false) where TApiEntity : class =>
            mediator.SendReadAllOptionalQuery(readAllParams, options =>
                options
                    .WithContext(context)
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TReadAllResult, ApizrException<TReadAllResult>>>
            SendReadAllOptionalQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
                this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
                TReadAllParams readAllParams,
                CancellationToken cancellationToken, bool clearCache = false) where TApiEntity : class =>
            mediator.SendReadAllOptionalQuery(readAllParams, options =>
                options
                    .WithCancellation(cancellationToken)
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TReadAllResult, ApizrException<TReadAllResult>>>
            SendReadAllOptionalQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
                this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
                TReadAllParams readAllParams,
                int priority, bool clearCache = false) where TApiEntity : class =>
            mediator.SendReadAllOptionalQuery(readAllParams, priority, options =>
                options
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TReadAllResult, ApizrException<TReadAllResult>>>
            SendReadAllOptionalQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
                this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
                TReadAllParams readAllParams,
                int priority, Context context, bool clearCache = false) where TApiEntity : class =>
            mediator.SendReadAllOptionalQuery(readAllParams, priority, options =>
                options
                    .WithContext(context)
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TReadAllResult, ApizrException<TReadAllResult>>>
            SendReadAllOptionalQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
                this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
                TReadAllParams readAllParams,
                int priority,
                CancellationToken cancellationToken, bool clearCache = false) where TApiEntity : class =>
            mediator.SendReadAllOptionalQuery(readAllParams, priority, options =>
                options
                    .WithCancellation(cancellationToken)
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TReadAllResult, ApizrException<TReadAllResult>>>
            SendReadAllOptionalQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
                this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
                TReadAllParams readAllParams, Context context,
                CancellationToken cancellationToken, bool clearCache = false) where TApiEntity : class =>
            mediator.SendReadAllOptionalQuery(readAllParams, options =>
                options
                    .WithContext(context)
                    .WithCancellation(cancellationToken)
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TReadAllResult, ApizrException<TReadAllResult>>>
            SendReadAllOptionalQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
                this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
                TReadAllParams readAllParams,
                int priority, Context context,
                CancellationToken cancellationToken, bool clearCache = false) where TApiEntity : class =>
            mediator.SendReadAllOptionalQuery(readAllParams, priority, options =>
                options
                    .WithContext(context)
                    .WithCancellation(cancellationToken)
                    .ClearCache(clearCache));

        #endregion

        #region SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator, TReadAllParams)

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
                this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
                TReadAllParams readAllParams, bool clearCache = false) where TApiEntity : class =>
            mediator.SendReadAllOptionalQuery<TModelReadAllResult>(readAllParams,
                options => options
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
                this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
                TReadAllParams readAllParams, Context context,
                bool clearCache = false) where TApiEntity : class =>
            mediator.SendReadAllOptionalQuery<TModelReadAllResult>(readAllParams,
                options => options
                    .WithContext(context)
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
                this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
                TReadAllParams readAllParams,
                CancellationToken cancellationToken, bool clearCache = false) where TApiEntity : class =>
            mediator.SendReadAllOptionalQuery<TModelReadAllResult>(readAllParams,
                options => options
                    .WithCancellation(cancellationToken)
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR with priority and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
                this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
                TReadAllParams readAllParams,
                int priority, bool clearCache = false) where TApiEntity : class =>
            mediator.SendReadAllOptionalQuery<TModelReadAllResult>(readAllParams,
                priority,
                options => options
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR with priority and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
                this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
                TReadAllParams readAllParams,
                int priority, Context context, bool clearCache = false) where TApiEntity : class =>
            mediator.SendReadAllOptionalQuery<TModelReadAllResult>(readAllParams,
                priority,
                options => options
                    .WithContext(context)
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR with priority and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
                this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
                TReadAllParams readAllParams,
                int priority,
                CancellationToken cancellationToken, bool clearCache = false) where TApiEntity : class =>
            mediator.SendReadAllOptionalQuery<TModelReadAllResult>(readAllParams,
                priority,
                options => options
                    .WithCancellation(cancellationToken)
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
                this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
                TReadAllParams readAllParams, Context context,
                CancellationToken cancellationToken, bool clearCache = false) where TApiEntity : class =>
            mediator.SendReadAllOptionalQuery<TModelReadAllResult>(readAllParams,
                options => options
                    .WithContext(context)
                    .WithCancellation(cancellationToken)
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR with priority and returning a mapped optional result
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
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
                this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
                TReadAllParams readAllParams,
                int priority, Context context,
                CancellationToken cancellationToken, bool clearCache = false) where TApiEntity : class =>
            mediator.SendReadAllOptionalQuery<TModelReadAllResult>(readAllParams,
                priority,
                options => options
                    .WithContext(context)
                    .WithCancellation(cancellationToken)
                    .ClearCache(clearCache));

        #endregion

        #endregion

        #region Read

        #region SendReadOptionalQuery

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TApiEntity, ApizrException<TApiEntity>>>
            SendReadOptionalQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
                this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
                TApiEntityKey key, bool clearCache = false) where TApiEntity : class =>
            mediator.SendReadOptionalQuery(key,
                options => options
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TApiEntity, ApizrException<TApiEntity>>>
            SendReadOptionalQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
                this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
                TApiEntityKey key, Context context, bool clearCache = false) where TApiEntity : class =>
            mediator.SendReadOptionalQuery(key,
                options => options
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TApiEntity, ApizrException<TApiEntity>>>
            SendReadOptionalQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
                this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
                TApiEntityKey key,
                CancellationToken cancellationToken, bool clearCache = false) where TApiEntity : class =>
            mediator.SendReadOptionalQuery(key,
                options => options
                    .WithCancellation(cancellationToken)
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR with priority and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TApiEntity, ApizrException<TApiEntity>>>
            SendReadOptionalQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
                this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
                TApiEntityKey key,
                int priority, bool clearCache = false) where TApiEntity : class =>
            mediator.SendReadOptionalQuery(key, priority,
                options => options
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TApiEntity, ApizrException<TApiEntity>>>
            SendReadOptionalQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
                this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
                TApiEntityKey key, Context context,
                CancellationToken cancellationToken, bool clearCache = false) where TApiEntity : class =>
            mediator.SendReadOptionalQuery(key,
                options => options
                    .WithCancellation(cancellationToken)
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR with priority and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TApiEntity, ApizrException<TApiEntity>>>
            SendReadOptionalQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
                this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
                TApiEntityKey key,
                int priority, Context context, bool clearCache = false) where TApiEntity : class =>
            mediator.SendReadOptionalQuery(key, priority,
                options => options
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR with priority and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TApiEntity, ApizrException<TApiEntity>>>
            SendReadOptionalQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
                this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
                TApiEntityKey key,
                int priority,
                CancellationToken cancellationToken, bool clearCache = false) where TApiEntity : class =>
            mediator.SendReadOptionalQuery(key, priority,
                options => options
                    .WithCancellation(cancellationToken)
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR with priority and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TApiEntity, ApizrException<TApiEntity>>>
            SendReadOptionalQuery<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
                this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
                TApiEntityKey key,
                int priority, Context context,
                CancellationToken cancellationToken, bool clearCache = false) where TApiEntity : class =>
            mediator.SendReadOptionalQuery(key, priority,
                options => options
                    .WithContext(context)
                    .WithCancellation(cancellationToken)
                    .ClearCache(clearCache));

        #endregion

        #region SendReadOptionalQuery<TModelEntity>

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelEntity, ApizrException<TModelEntity>>>
            SendReadOptionalQuery<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
                this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
                TApiEntityKey key, bool clearCache = false) where TApiEntity : class =>
            mediator.SendReadOptionalQuery<TModelEntity>(key,
                options => options
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelEntity, ApizrException<TModelEntity>>>
            SendReadOptionalQuery<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
                this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
                TApiEntityKey key, Context context, bool clearCache = false) where TApiEntity : class =>
            mediator.SendReadOptionalQuery<TModelEntity>(key,
                options => options
                    .WithContext(context)
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity,
            TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
            this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TApiEntityKey key,
            CancellationToken cancellationToken, bool clearCache = false) where TApiEntity : class =>
            mediator.SendReadOptionalQuery<TModelEntity>(key,
                options => options
                    .WithCancellation(cancellationToken)
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR with priority and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity,
            TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
            this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TApiEntityKey key,
            int priority, bool clearCache = false) where TApiEntity : class =>
            mediator.SendReadOptionalQuery<TModelEntity>(key, priority,
                options => options
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity,
            TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
            this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TApiEntityKey key, Context context,
            CancellationToken cancellationToken, bool clearCache = false) where TApiEntity : class =>
            mediator.SendReadOptionalQuery<TModelEntity>(key,
                options => options
                    .WithContext(context)
                    .WithCancellation(cancellationToken)
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR with priority and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity,
            TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
            this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TApiEntityKey key,
            int priority, Context context, bool clearCache = false) where TApiEntity : class =>
            mediator.SendReadOptionalQuery<TModelEntity>(key, priority,
                options => options
                    .WithContext(context)
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR with priority and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity,
            TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
            this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TApiEntityKey key,
            int priority,
            CancellationToken cancellationToken, bool clearCache = false) where TApiEntity : class =>
            mediator.SendReadOptionalQuery<TModelEntity>(key, priority,
                options => options
                    .WithCancellation(cancellationToken)
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR with priority and returning a mapped optional result
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
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity,
            TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
            this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TApiEntityKey key,
            int priority, Context context,
            CancellationToken cancellationToken, bool clearCache = false) where TApiEntity : class =>
            mediator.SendReadOptionalQuery<TModelEntity>(key, priority,
                options => options
                    .WithContext(context)
                    .WithCancellation(cancellationToken)
                    .ClearCache(clearCache));

        #endregion

        #endregion

        #region Update

        #region SendUpdateOptionalCommand

        /// <summary>
        /// Send a <see cref="UpdateCommand{TApiEntityKey, TApiEntity}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand<TApiEntity, TApiEntityKey,
            TReadAllResult, TReadAllParams>(
            this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TApiEntityKey key, TApiEntity entity, Context context) where TApiEntity : class =>
            mediator.SendUpdateOptionalCommand(key, entity,
                options => options
                    .WithContext(context));

        /// <summary>
        /// Send a <see cref="UpdateCommand{TApiEntityKey, TApiEntity}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand<TApiEntity, TApiEntityKey,
            TReadAllResult, TReadAllParams>(
            this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TApiEntityKey key, TApiEntity entity,
            CancellationToken cancellationToken) where TApiEntity : class =>
            mediator.SendUpdateOptionalCommand(key, entity,
                options => options
                    .WithCancellation(cancellationToken));

        /// <summary>
        /// Send a <see cref="UpdateCommand{TApiEntityKey, TApiEntity}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand<TApiEntity, TApiEntityKey,
            TReadAllResult, TReadAllParams>(
            this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TApiEntityKey key, TApiEntity entity, Context context,
            CancellationToken cancellationToken) where TApiEntity : class =>
            mediator.SendUpdateOptionalCommand(key, entity,
                options => options
                    .WithContext(context)
                    .WithCancellation(cancellationToken));

        #endregion

        #region SendUpdateOptionalCommand<TModelEntity>

        /// <summary>
        /// Send a mapped <see cref="UpdateCommand{TApiEntityKey, TModelEntity}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand<TModelEntity, TApiEntity,
            TApiEntityKey, TReadAllResult, TReadAllParams>(
            this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TApiEntityKey key,
            TModelEntity entity, Context context) where TApiEntity : class =>
            mediator.SendUpdateOptionalCommand<TModelEntity>(key, entity,
                options => options
                    .WithContext(context));

        /// <summary>
        /// Send a mapped <see cref="UpdateCommand{TApiEntityKey, TModelEntity}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand<TModelEntity, TApiEntity,
            TApiEntityKey, TReadAllResult, TReadAllParams>(
            this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TApiEntityKey key,
            TModelEntity entity, CancellationToken cancellationToken) where TApiEntity : class =>
            mediator.SendUpdateOptionalCommand<TModelEntity>(key, entity,
                options => options
                    .WithCancellation(cancellationToken));

        /// <summary>
        /// Send a mapped <see cref="UpdateCommand{TApiEntityKey, TModelEntity}"/> to Apizr using MediatR and returning an optional result
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
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand<TModelEntity, TApiEntity,
            TApiEntityKey, TReadAllResult, TReadAllParams>(
            this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TApiEntityKey key,
            TModelEntity entity, Context context,
            CancellationToken cancellationToken) where TApiEntity : class =>
            mediator.SendUpdateOptionalCommand<TModelEntity>(key, entity,
                options => options
                    .WithContext(context)
                    .WithCancellation(cancellationToken));

        #endregion

        #endregion

        #region Delete

        /// <summary>
        /// Send a <see cref="DeleteCommand{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<Unit, ApizrException>> SendDeleteOptionalCommand<TApiEntity, TApiEntityKey,
            TReadAllResult, TReadAllParams>(
            this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TApiEntityKey key, Context context) where TApiEntity : class =>
            mediator.SendDeleteOptionalCommand(key,
                options => options
                    .WithContext(context));

        /// <summary>
        /// Send a <see cref="DeleteCommand{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<Unit, ApizrException>> SendDeleteOptionalCommand<TApiEntity, TApiEntityKey,
            TReadAllResult, TReadAllParams>(
            this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TApiEntityKey key,
            CancellationToken cancellationToken) where TApiEntity : class =>
            mediator.SendDeleteOptionalCommand(key,
                options => options
                    .WithCancellation(cancellationToken));

        /// <summary>
        /// Send a <see cref="DeleteCommand{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<Unit, ApizrException>> SendDeleteOptionalCommand<TApiEntity, TApiEntityKey,
            TReadAllResult, TReadAllParams>(
            this IApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> mediator,
            TApiEntityKey key, Context context,
            CancellationToken cancellationToken) where TApiEntity : class =>
            mediator.SendDeleteOptionalCommand(key,
                options => options
                    .WithContext(context)
                    .WithCancellation(cancellationToken));

        #endregion
    }
}
