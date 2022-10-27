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
    public static class ApizrCrudOptionalMediatorExtensions
    {
        #region Create

        #region SendCreateOptionalCommand

        /// <summary>
        /// Send a <see cref="CreateOptionalCommand{TApiEntity}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TApiEntity, ApizrException>> SendCreateOptionalCommand<TApiEntity>(
            this IApizrCrudOptionalMediator mediator, TApiEntity entity, Context context) =>
            mediator.SendCreateOptionalCommand<TApiEntity>(entity,
                options => options.WithContext(context));

        /// <summary>
        /// Send a <see cref="CreateOptionalCommand{TApiEntity}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TApiEntity, ApizrException>> SendCreateOptionalCommand<TApiEntity>(
            this IApizrCrudOptionalMediator mediator, TApiEntity entity,
            CancellationToken cancellationToken) =>
            mediator.SendCreateOptionalCommand<TApiEntity>(entity,
                options => options.WithCancellation(cancellationToken));

        /// <summary>
        /// Send a <see cref="CreateOptionalCommand{TApiEntity}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TApiEntity, ApizrException>> SendCreateOptionalCommand<TApiEntity>(
            this IApizrCrudOptionalMediator mediator, TApiEntity entity, Context context,
            CancellationToken cancellationToken) =>
            mediator.SendCreateOptionalCommand<TApiEntity>(entity,
                options => options.WithContext(context)
                    .WithCancellation(cancellationToken));

        #endregion

        #region SendCreateOptionalCommand<TModelEntity>

        /// <summary>
        /// Send a mapped <see cref="CreateCommand{TModelEntity}"/> to Apizr using MediatR and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelEntity, ApizrException>> SendCreateOptionalCommand<TModelEntity, TApiEntity>(
            this IApizrCrudOptionalMediator mediator, TModelEntity entity, Context context) =>
            mediator.SendCreateOptionalCommand<TModelEntity, TApiEntity>(entity,
                options => options.WithContext(context));

        /// <summary>
        /// Send a mapped <see cref="CreateCommand{TModelEntity}"/> to Apizr using MediatR and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelEntity, ApizrException>> SendCreateOptionalCommand<TModelEntity, TApiEntity>(
            this IApizrCrudOptionalMediator mediator, TModelEntity entity,
            CancellationToken cancellationToken) =>
            mediator.SendCreateOptionalCommand<TModelEntity, TApiEntity>(entity,
                options => options.WithCancellation(cancellationToken));

        /// <summary>
        /// Send a mapped <see cref="CreateCommand{TModelEntity}"/> to Apizr using MediatR and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelEntity, ApizrException>> SendCreateOptionalCommand<TModelEntity, TApiEntity>(
            this IApizrCrudOptionalMediator mediator, TModelEntity entity, Context context,
            CancellationToken cancellationToken) =>
            mediator.SendCreateOptionalCommand<TModelEntity, TApiEntity>(entity,
                options => options.WithContext(context)
                    .WithCancellation(cancellationToken));

        #endregion

        #endregion

        #region ReadAll

        #region SendReadAllOptionalQuery

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TReadAllResult, ApizrException<TReadAllResult>>>
            SendReadAllOptionalQuery<TReadAllResult>(this IApizrCrudOptionalMediator mediator, Context context,
                bool clearCache = false) =>
            mediator.SendReadAllOptionalQuery<TReadAllResult>(options => options.WithContext(context)
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
            SendReadAllOptionalQuery<TReadAllResult>(this IApizrCrudOptionalMediator mediator,
                CancellationToken cancellationToken, bool clearCache = false) =>
            mediator.SendReadAllOptionalQuery<TReadAllResult>(options => options
                .WithCancellation(cancellationToken)
                .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority and returning an optional result
        /// </summary>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TReadAllResult, ApizrException<TReadAllResult>>>
            SendReadAllOptionalQuery<TReadAllResult>(this IApizrCrudOptionalMediator mediator, int priority,
                bool clearCache = false) =>
            mediator.SendReadAllOptionalQuery<TReadAllResult>(priority,
                options => options.ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority and returning an optional result
        /// </summary>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TReadAllResult, ApizrException<TReadAllResult>>>
            SendReadAllOptionalQuery<TReadAllResult>(this IApizrCrudOptionalMediator mediator, int priority,
                Context context, bool clearCache = false) =>
            mediator.SendReadAllOptionalQuery<TReadAllResult>(priority, options => options.WithContext(context)
                .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority and returning an optional result
        /// </summary>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TReadAllResult, ApizrException<TReadAllResult>>>
            SendReadAllOptionalQuery<TReadAllResult>(this IApizrCrudOptionalMediator mediator, int priority,
                CancellationToken cancellationToken, bool clearCache = false) =>
            mediator.SendReadAllOptionalQuery<TReadAllResult>(priority, options => options
                .WithCancellation(cancellationToken)
                .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TReadAllResult, ApizrException<TReadAllResult>>>
            SendReadAllOptionalQuery<TReadAllResult>(this IApizrCrudOptionalMediator mediator, Context context,
                CancellationToken cancellationToken, bool clearCache = false) =>
            mediator.SendReadAllOptionalQuery<TReadAllResult>(options => options.WithContext(context)
                .WithCancellation(cancellationToken)
                .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority and returning an optional result
        /// </summary>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TReadAllResult, ApizrException<TReadAllResult>>>
            SendReadAllOptionalQuery<TReadAllResult>(this IApizrCrudOptionalMediator mediator, int priority,
                Context context,
                CancellationToken cancellationToken, bool clearCache = false) =>
            mediator.SendReadAllOptionalQuery<TReadAllResult>(priority, options => options.WithContext(context)
                .WithCancellation(cancellationToken)
                .ClearCache(clearCache));

        #endregion

        #region SendReadAllOptionalQuery<TModelReadAllResult>

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR and returning mapped optional result
        /// </summary>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult>(this IApizrCrudOptionalMediator mediator,
                bool clearCache = false) =>
            mediator.SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult>(options => options
                .ClearCache(clearCache));

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR and returning mapped optional result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult>(this IApizrCrudOptionalMediator mediator,
                Context context, bool clearCache = false) =>
            mediator.SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult>(options => options
                .WithContext(context)
                .ClearCache(clearCache));

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR and returning mapped optional result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult>(this IApizrCrudOptionalMediator mediator,
                CancellationToken cancellationToken, bool clearCache = false) =>
            mediator.SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult>(options => options
                .WithCancellation(cancellationToken)
                .ClearCache(clearCache));

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR with priority and returning mapped optional result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult>(this IApizrCrudOptionalMediator mediator,
                int priority, bool clearCache = false) =>
            mediator.SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult>(priority, options => options
                .ClearCache(clearCache));

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR with priority and returning mapped optional result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult>(this IApizrCrudOptionalMediator mediator,
                int priority, CancellationToken cancellationToken, bool clearCache = false) =>
            mediator.SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult>(priority, options => options
                .WithCancellation(cancellationToken)
                .ClearCache(clearCache));

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR with priority and returning mapped optional result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult>(this IApizrCrudOptionalMediator mediator,
                int priority, Context context, bool clearCache = false) =>
            mediator.SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult>(priority, options => options
                .WithContext(context)
                .ClearCache(clearCache));

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR and returning mapped optional result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult>(this IApizrCrudOptionalMediator mediator,
                Context context,
                CancellationToken cancellationToken, bool clearCache = false) =>
            mediator.SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult>(options => options
                .WithContext(context)
                .WithCancellation(cancellationToken)
                .ClearCache(clearCache));

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR with priority and returning mapped optional result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult>(this IApizrCrudOptionalMediator mediator,
                int priority, Context context,
                CancellationToken cancellationToken, bool clearCache = false) =>
            mediator.SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult>(priority, options => options
                .WithContext(context)
                .WithCancellation(cancellationToken)
                .ClearCache(clearCache));

        #endregion

        #region SendReadAllOptionalQuery(TReadAllParams)

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TReadAllResult">The api result type</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TReadAllResult, ApizrException<TReadAllResult>>>
            SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(this IApizrCrudOptionalMediator mediator,
                TReadAllParams readAllParams, bool clearCache = false) =>
            mediator.SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(readAllParams, options =>
                options
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TReadAllResult">The api result type</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TReadAllResult, ApizrException<TReadAllResult>>>
            SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(this IApizrCrudOptionalMediator mediator,
                TReadAllParams readAllParams, Context context, bool clearCache = false) =>
            mediator.SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(readAllParams, options =>
                options
                    .WithContext(context)
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TReadAllResult">The api result type</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TReadAllResult, ApizrException<TReadAllResult>>>
            SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(this IApizrCrudOptionalMediator mediator,
                TReadAllParams readAllParams,
                CancellationToken cancellationToken, bool clearCache = false) =>
            mediator.SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(readAllParams, options =>
                options
                    .WithCancellation(cancellationToken)
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority and returning an optional result
        /// </summary>
        /// <typeparam name="TReadAllResult">The api result type</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TReadAllResult, ApizrException<TReadAllResult>>>
            SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(this IApizrCrudOptionalMediator mediator,
                TReadAllParams readAllParams,
                int priority, bool clearCache = false) =>
            mediator.SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(readAllParams, priority, options =>
                options
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority and returning an optional result
        /// </summary>
        /// <typeparam name="TReadAllResult">The api result type</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TReadAllResult, ApizrException<TReadAllResult>>>
            SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(this IApizrCrudOptionalMediator mediator,
                TReadAllParams readAllParams,
                int priority, Context context, bool clearCache = false) =>
            mediator.SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(readAllParams, priority, options =>
                options
                    .WithContext(context)
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority and returning an optional result
        /// </summary>
        /// <typeparam name="TReadAllResult">The api result type</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TReadAllResult, ApizrException<TReadAllResult>>>
            SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(this IApizrCrudOptionalMediator mediator,
                TReadAllParams readAllParams,
                int priority,
                CancellationToken cancellationToken, bool clearCache = false) =>
            mediator.SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(readAllParams, priority, options =>
                options
                    .WithCancellation(cancellationToken)
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TReadAllResult">The api result type</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TReadAllResult, ApizrException<TReadAllResult>>>
            SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(this IApizrCrudOptionalMediator mediator,
                TReadAllParams readAllParams, Context context,
                CancellationToken cancellationToken, bool clearCache = false) =>
            mediator.SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(readAllParams, options =>
                options
                    .WithContext(context)
                    .WithCancellation(cancellationToken)
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority and returning an optional result
        /// </summary>
        /// <typeparam name="TReadAllResult">The api result type</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TReadAllResult, ApizrException<TReadAllResult>>>
            SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(this IApizrCrudOptionalMediator mediator,
                TReadAllParams readAllParams,
                int priority, Context context,
                CancellationToken cancellationToken, bool clearCache = false) =>
            mediator.SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(readAllParams, priority, options =>
                options
                    .WithContext(context)
                    .WithCancellation(cancellationToken)
                    .ClearCache(clearCache));

        #endregion

        #region SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(this IApizrCrudOptionalMediator mediator, TReadAllParams)

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(
                this IApizrCrudOptionalMediator mediator, TReadAllParams readAllParams, bool clearCache = false) =>
            mediator.SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(readAllParams,
                options => options
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(
                this IApizrCrudOptionalMediator mediator, TReadAllParams readAllParams, Context context,
                bool clearCache = false) =>
            mediator.SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(readAllParams,
                options => options
                    .WithContext(context)
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(
                this IApizrCrudOptionalMediator mediator, TReadAllParams readAllParams,
                CancellationToken cancellationToken, bool clearCache = false) =>
            mediator.SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(readAllParams,
                options => options
                    .WithCancellation(cancellationToken)
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR with priority and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(
                this IApizrCrudOptionalMediator mediator, TReadAllParams readAllParams,
                int priority, bool clearCache = false) =>
            mediator.SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(readAllParams,
                priority,
                options => options
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR with priority and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(
                this IApizrCrudOptionalMediator mediator, TReadAllParams readAllParams,
                int priority, Context context, bool clearCache = false) =>
            mediator.SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(readAllParams,
                priority,
                options => options
                    .WithContext(context)
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR with priority and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(
                this IApizrCrudOptionalMediator mediator, TReadAllParams readAllParams,
                int priority,
                CancellationToken cancellationToken, bool clearCache = false) =>
            mediator.SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(readAllParams,
                priority,
                options => options
                    .WithCancellation(cancellationToken)
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(
                this IApizrCrudOptionalMediator mediator, TReadAllParams readAllParams, Context context,
                CancellationToken cancellationToken, bool clearCache = false) =>
            mediator.SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(readAllParams,
                options => options
                    .WithContext(context)
                    .WithCancellation(cancellationToken)
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR with priority and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(
                this IApizrCrudOptionalMediator mediator, TReadAllParams readAllParams,
                int priority, Context context,
                CancellationToken cancellationToken, bool clearCache = false) =>
            mediator.SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(readAllParams,
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
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TApiEntity, ApizrException<TApiEntity>>>
            SendReadOptionalQuery<TApiEntity, TApiEntityKey>(this IApizrCrudOptionalMediator mediator,
                TApiEntityKey key, bool clearCache = false) =>
            mediator.SendReadOptionalQuery<TApiEntity, TApiEntityKey>(key,
                options => options
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TApiEntity, ApizrException<TApiEntity>>>
            SendReadOptionalQuery<TApiEntity, TApiEntityKey>(this IApizrCrudOptionalMediator mediator,
                TApiEntityKey key, Context context, bool clearCache = false) =>
            mediator.SendReadOptionalQuery<TApiEntity, TApiEntityKey>(key,
                options => options
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TApiEntity, ApizrException<TApiEntity>>>
            SendReadOptionalQuery<TApiEntity, TApiEntityKey>(this IApizrCrudOptionalMediator mediator,
                TApiEntityKey key,
                CancellationToken cancellationToken, bool clearCache = false) =>
            mediator.SendReadOptionalQuery<TApiEntity, TApiEntityKey>(key,
                options => options
                    .WithCancellation(cancellationToken)
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR with priority and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TApiEntity, ApizrException<TApiEntity>>>
            SendReadOptionalQuery<TApiEntity, TApiEntityKey>(this IApizrCrudOptionalMediator mediator,
                TApiEntityKey key,
                int priority, bool clearCache = false) =>
            mediator.SendReadOptionalQuery<TApiEntity, TApiEntityKey>(key, priority,
                options => options
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TApiEntity, ApizrException<TApiEntity>>>
            SendReadOptionalQuery<TApiEntity, TApiEntityKey>(this IApizrCrudOptionalMediator mediator,
                TApiEntityKey key, Context context,
                CancellationToken cancellationToken, bool clearCache = false) =>
            mediator.SendReadOptionalQuery<TApiEntity, TApiEntityKey>(key,
                options => options
                    .WithCancellation(cancellationToken)
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR with priority and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TApiEntity, ApizrException<TApiEntity>>>
            SendReadOptionalQuery<TApiEntity, TApiEntityKey>(this IApizrCrudOptionalMediator mediator,
                TApiEntityKey key,
                int priority, Context context, bool clearCache = false) =>
            mediator.SendReadOptionalQuery<TApiEntity, TApiEntityKey>(key, priority,
                options => options
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR with priority and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TApiEntity, ApizrException<TApiEntity>>>
            SendReadOptionalQuery<TApiEntity, TApiEntityKey>(this IApizrCrudOptionalMediator mediator,
                TApiEntityKey key,
                int priority,
                CancellationToken cancellationToken, bool clearCache = false) =>
            mediator.SendReadOptionalQuery<TApiEntity, TApiEntityKey>(key, priority,
                options => options
                    .WithCancellation(cancellationToken)
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR with priority and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TApiEntity, ApizrException<TApiEntity>>>
            SendReadOptionalQuery<TApiEntity, TApiEntityKey>(this IApizrCrudOptionalMediator mediator,
                TApiEntityKey key,
                int priority, Context context,
                CancellationToken cancellationToken, bool clearCache = false) =>
            mediator.SendReadOptionalQuery<TApiEntity, TApiEntityKey>(key, priority,
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
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelEntity, ApizrException<TModelEntity>>>
            SendReadOptionalQuery<TModelEntity, TApiEntity, TApiEntityKey>(this IApizrCrudOptionalMediator mediator,
                TApiEntityKey key, bool clearCache = false) =>
            mediator.SendReadOptionalQuery<TModelEntity, TApiEntity, TApiEntityKey>(key,
                options => options
                    .ClearCache(clearCache));
        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelEntity, ApizrException<TModelEntity>>>
            SendReadOptionalQuery<TModelEntity, TApiEntity, TApiEntityKey>(this IApizrCrudOptionalMediator mediator,
                TApiEntityKey key, Context context, bool clearCache = false) =>
            mediator.SendReadOptionalQuery<TModelEntity, TApiEntity, TApiEntityKey>(key,
                options => options
                    .WithContext(context)
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity,
            TApiEntity, TApiEntityKey>(this IApizrCrudOptionalMediator mediator, TApiEntityKey key,
            CancellationToken cancellationToken, bool clearCache = false) =>
            mediator.SendReadOptionalQuery<TModelEntity, TApiEntity, TApiEntityKey>(key,
                options => options
                    .WithCancellation(cancellationToken)
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR with priority and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity,
            TApiEntity, TApiEntityKey>(this IApizrCrudOptionalMediator mediator, TApiEntityKey key,
            int priority, bool clearCache = false) =>
            mediator.SendReadOptionalQuery<TModelEntity, TApiEntity, TApiEntityKey>(key, priority,
                options => options
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity,
            TApiEntity, TApiEntityKey>(this IApizrCrudOptionalMediator mediator, TApiEntityKey key, Context context,
            CancellationToken cancellationToken, bool clearCache = false) =>
            mediator.SendReadOptionalQuery<TModelEntity, TApiEntity, TApiEntityKey>(key,
                options => options
                    .WithContext(context)
                    .WithCancellation(cancellationToken)
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR with priority and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity,
            TApiEntity, TApiEntityKey>(this IApizrCrudOptionalMediator mediator, TApiEntityKey key,
            int priority, Context context, bool clearCache = false) =>
            mediator.SendReadOptionalQuery<TModelEntity, TApiEntity, TApiEntityKey>(key, priority,
                options => options
                    .WithContext(context)
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR with priority and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity,
            TApiEntity, TApiEntityKey>(this IApizrCrudOptionalMediator mediator, TApiEntityKey key,
            int priority,
            CancellationToken cancellationToken, bool clearCache = false) =>
            mediator.SendReadOptionalQuery<TModelEntity, TApiEntity, TApiEntityKey>(key, priority,
                options => options
                    .WithCancellation(cancellationToken)
                    .ClearCache(clearCache));

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR with priority and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity,
            TApiEntity, TApiEntityKey>(this IApizrCrudOptionalMediator mediator, TApiEntityKey key,
            int priority, Context context,
            CancellationToken cancellationToken, bool clearCache = false) =>
            mediator.SendReadOptionalQuery<TModelEntity, TApiEntity, TApiEntityKey>(key, priority,
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
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand<TApiEntity, TApiEntityKey>(
            this IApizrCrudOptionalMediator mediator, TApiEntityKey key, TApiEntity entity, Context context) =>
            mediator.SendUpdateOptionalCommand<TApiEntity, TApiEntityKey>(key, entity,
                options => options
                    .WithContext(context));

        /// <summary>
        /// Send a <see cref="UpdateCommand{TApiEntityKey, TApiEntity}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand<TApiEntity, TApiEntityKey>(
            this IApizrCrudOptionalMediator mediator, TApiEntityKey key, TApiEntity entity,
            CancellationToken cancellationToken) =>
            mediator.SendUpdateOptionalCommand<TApiEntity, TApiEntityKey>(key, entity,
                options => options
                    .WithCancellation(cancellationToken));

        /// <summary>
        /// Send a <see cref="UpdateCommand{TApiEntityKey, TApiEntity}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand<TApiEntity, TApiEntityKey>(
            this IApizrCrudOptionalMediator mediator, TApiEntityKey key, TApiEntity entity, Context context,
            CancellationToken cancellationToken) =>
            mediator.SendUpdateOptionalCommand<TApiEntity, TApiEntityKey>(key, entity,
                options => options
                    .WithContext(context)
                    .WithCancellation(cancellationToken));

        #endregion

        #region SendUpdateOptionalCommand<TModelEntity>

        /// <summary>
        /// Send a mapped <see cref="UpdateCommand{TApiEntityKey, TModelEntity}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand<TModelEntity, TApiEntity,
            TApiEntityKey>(this IApizrCrudOptionalMediator mediator, TApiEntityKey key,
            TModelEntity entity, Context context) =>
            mediator.SendUpdateOptionalCommand<TModelEntity, TApiEntity, TApiEntityKey>(key, entity,
                options => options
                    .WithContext(context));

        /// <summary>
        /// Send a mapped <see cref="UpdateCommand{TApiEntityKey, TModelEntity}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand<TModelEntity, TApiEntity,
            TApiEntityKey>(this IApizrCrudOptionalMediator mediator, TApiEntityKey key,
            TModelEntity entity, CancellationToken cancellationToken) =>
            mediator.SendUpdateOptionalCommand<TModelEntity, TApiEntity, TApiEntityKey>(key, entity,
                options => options
                    .WithCancellation(cancellationToken));

        /// <summary>
        /// Send a mapped <see cref="UpdateCommand{TApiEntityKey, TModelEntity}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand<TModelEntity, TApiEntity,
            TApiEntityKey>(this IApizrCrudOptionalMediator mediator, TApiEntityKey key,
            TModelEntity entity, Context context,
            CancellationToken cancellationToken) =>
            mediator.SendUpdateOptionalCommand<TModelEntity, TApiEntity, TApiEntityKey>(key, entity,
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
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<Unit, ApizrException>> SendDeleteOptionalCommand<TApiEntity, TApiEntityKey>(
            this IApizrCrudOptionalMediator mediator, TApiEntityKey key, Context context) =>
            mediator.SendDeleteOptionalCommand<TApiEntity, TApiEntityKey>(key,
                options => options
                    .WithContext(context));

        /// <summary>
        /// Send a <see cref="DeleteCommand{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<Unit, ApizrException>> SendDeleteOptionalCommand<TApiEntity, TApiEntityKey>(
            this IApizrCrudOptionalMediator mediator, TApiEntityKey key,
            CancellationToken cancellationToken) =>
            mediator.SendDeleteOptionalCommand<TApiEntity, TApiEntityKey>(key,
                options => options
                    .WithCancellation(cancellationToken));

        /// <summary>
        /// Send a <see cref="DeleteCommand{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<Unit, ApizrException>> SendDeleteOptionalCommand<TApiEntity, TApiEntityKey>(
            this IApizrCrudOptionalMediator mediator, TApiEntityKey key, Context context,
            CancellationToken cancellationToken) =>
            mediator.SendDeleteOptionalCommand<TApiEntity, TApiEntityKey>(key,
                options => options
                    .WithContext(context)
                    .WithCancellation(cancellationToken));

        #endregion
    }
}
