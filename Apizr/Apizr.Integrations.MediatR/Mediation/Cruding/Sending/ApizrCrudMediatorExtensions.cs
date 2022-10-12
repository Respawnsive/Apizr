using System;
using System.Threading;
using System.Threading.Tasks;
using Polly;

namespace Apizr.Mediation.Cruding.Sending
{
    /// <summary>
    /// Apizr mediator dedicated to cruding
    /// </summary>
    public static class ApizrCrudMediatorExtensions
    {
        #region Untyped

        #region Create

        #region SendCreateCommand

        /// <summary>
        /// Send a <see cref="CreateCommand{TApiEntity}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="onException">Handle exception</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TApiEntity> SendCreateCommand<TApiEntity>(this IApizrCrudMediator mediator, TApiEntity entity, Action<Exception> onException);

        /// <summary>
        /// Send a <see cref="CreateCommand{TApiEntity}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="context">The Polly context</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TApiEntity> SendCreateCommand<TApiEntity>(this IApizrCrudMediator mediator, TApiEntity entity, Context context, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="CreateCommand{TApiEntity}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TApiEntity> SendCreateCommand<TApiEntity>(this IApizrCrudMediator mediator, TApiEntity entity, CancellationToken cancellationToken, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="CreateCommand{TApiEntity}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TApiEntity> SendCreateCommand<TApiEntity>(this IApizrCrudMediator mediator, TApiEntity entity, Context context, CancellationToken cancellationToken, Action<Exception> onException = null);

        #endregion

        #region SendCreateCommand<TModelEntity>

        /// <summary>
        /// Send a mapped <see cref="CreateCommand{TModelEntity}"/> to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="onException">Handle exception</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelEntity> SendCreateCommand<TModelEntity, TApiEntity>(this IApizrCrudMediator mediator, TModelEntity entity, Action<Exception> onException);

        /// <summary>
        /// Send a mapped <see cref="CreateCommand{TModelEntity}"/> to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="context">The Polly context</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelEntity> SendCreateCommand<TModelEntity, TApiEntity>(this IApizrCrudMediator mediator, TModelEntity entity, Context context, Action<Exception> onException = null);

        /// <summary>
        /// Send a mapped <see cref="CreateCommand{TModelEntity}"/> to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelEntity> SendCreateCommand<TModelEntity, TApiEntity>(this IApizrCrudMediator mediator, TModelEntity entity, CancellationToken cancellationToken, Action<Exception> onException = null);

        /// <summary>
        /// Send a mapped <see cref="CreateCommand{TModelEntity}"/> to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelEntity> SendCreateCommand<TModelEntity, TApiEntity>(this IApizrCrudMediator mediator, TModelEntity entity, Context context, CancellationToken cancellationToken, Action<Exception> onException = null);

        #endregion

        #endregion

        #region ReadAll

        #region SendReadAllQuery

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <param name="onException">Handle exception and return cached result</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TReadAllResult> SendReadAllQuery<TReadAllResult>(this IApizrCrudMediator mediator, Action<Exception> onException);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TReadAllResult> SendReadAllQuery<TReadAllResult>(this IApizrCrudMediator mediator, bool clearCache, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TReadAllResult> SendReadAllQuery<TReadAllResult>(this IApizrCrudMediator mediator, Context context, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TReadAllResult> SendReadAllQuery<TReadAllResult>(this IApizrCrudMediator mediator, CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority
        /// </summary>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TReadAllResult> SendReadAllQuery<TReadAllResult>(this IApizrCrudMediator mediator, int priority, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority
        /// </summary>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TReadAllResult> SendReadAllQuery<TReadAllResult>(this IApizrCrudMediator mediator, int priority, Context context, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority
        /// </summary>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TReadAllResult> SendReadAllQuery<TReadAllResult>(this IApizrCrudMediator mediator, int priority, CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority
        /// </summary>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TReadAllResult> SendReadAllQuery<TReadAllResult>(this IApizrCrudMediator mediator, Context context, CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority
        /// </summary>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TReadAllResult> SendReadAllQuery<TReadAllResult>(this IApizrCrudMediator mediator, int priority, Context context, CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        #endregion

        #region SendReadAllQuery<TModelReadAllResult>

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelReadAllResult}"/> to Apizr using MediatR returning mapped result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <param name="onException">Handle exception and return cached result</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TApiReadAllResult>(this IApizrCrudMediator mediator, Action<Exception> onException);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelReadAllResult}"/> to Apizr using MediatR returning mapped result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TApiReadAllResult>(this IApizrCrudMediator mediator, bool clearCache, Action<Exception> onException = null);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelReadAllResult}"/> to Apizr using MediatR returning mapped result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TApiReadAllResult>(this IApizrCrudMediator mediator, Context context, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelReadAllResult}"/> to Apizr using MediatR returning mapped result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TApiReadAllResult>(this IApizrCrudMediator mediator, CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelReadAllResult}"/> to Apizr using MediatR with priority returning mapped result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TApiReadAllResult>(this IApizrCrudMediator mediator, int priority, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelReadAllResult}"/> to Apizr using MediatR with priority returning mapped result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TApiReadAllResult>(this IApizrCrudMediator mediator, int priority,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelReadAllResult}"/> to Apizr using MediatR with priority returning mapped result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TApiReadAllResult>(this IApizrCrudMediator mediator, int priority, Context context, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelReadAllResult}"/> to Apizr using MediatR with priority returning mapped result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TApiReadAllResult>(this IApizrCrudMediator mediator, Context context,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelReadAllResult}"/> to Apizr using MediatR with priority returning mapped result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TApiReadAllResult>(this IApizrCrudMediator mediator, int priority, Context context,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        #endregion

        #region SendReadAllQuery(TReadAllParams)

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TReadAllResult">The api result type</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="onException">Handle exception and return cached result</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TReadAllResult> SendReadAllQuery<TReadAllResult, TReadAllParams>(this IApizrCrudMediator mediator, TReadAllParams readAllParams, Action<Exception> onException);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TReadAllResult">The api result type</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TReadAllResult> SendReadAllQuery<TReadAllResult, TReadAllParams>(this IApizrCrudMediator mediator, TReadAllParams readAllParams, bool clearCache, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TReadAllResult">The api result type</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TReadAllResult> SendReadAllQuery<TReadAllResult, TReadAllParams>(this IApizrCrudMediator mediator, TReadAllParams readAllParams, Context context, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TReadAllResult">The api result type</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TReadAllResult> SendReadAllQuery<TReadAllResult, TReadAllParams>(this IApizrCrudMediator mediator, TReadAllParams readAllParams,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority
        /// </summary>
        /// <typeparam name="TReadAllResult">The api result type</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TReadAllResult> SendReadAllQuery<TReadAllResult, TReadAllParams>(this IApizrCrudMediator mediator, TReadAllParams readAllParams,
            int priority, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority
        /// </summary>
        /// <typeparam name="TReadAllResult">The api result type</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TReadAllResult> SendReadAllQuery<TReadAllResult, TReadAllParams>(this IApizrCrudMediator mediator, TReadAllParams readAllParams,
            int priority, Context context, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority
        /// </summary>
        /// <typeparam name="TReadAllResult">The api result type</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TReadAllResult> SendReadAllQuery<TReadAllResult, TReadAllParams>(this IApizrCrudMediator mediator, TReadAllParams readAllParams,
            int priority,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TReadAllResult">The api result type</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TReadAllResult> SendReadAllQuery<TReadAllResult, TReadAllParams>(this IApizrCrudMediator mediator, TReadAllParams readAllParams, Context context,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority
        /// </summary>
        /// <typeparam name="TReadAllResult">The api result type</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TReadAllResult> SendReadAllQuery<TReadAllResult, TReadAllParams>(this IApizrCrudMediator mediator, TReadAllParams readAllParams,
            int priority, Context context,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        #endregion

        #region SendReadAllQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(this IApizrCrudMediator mediator, TReadAllParams)

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelReadAllResult}"/> to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="onException">Handle exception and return cached result</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(this IApizrCrudMediator mediator, TReadAllParams readAllParams, Action<Exception> onException);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelReadAllResult}"/> to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(this IApizrCrudMediator mediator, TReadAllParams readAllParams, bool clearCache, Action<Exception> onException = null);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelReadAllResult}"/> to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(this IApizrCrudMediator mediator, TReadAllParams readAllParams, Context context, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelReadAllResult}"/> to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(this IApizrCrudMediator mediator, TReadAllParams readAllParams,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelReadAllResult}"/> to Apizr using MediatR with priority returning a mapped result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(this IApizrCrudMediator mediator, TReadAllParams readAllParams,
            int priority, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelReadAllResult}"/> to Apizr using MediatR with priority returning a mapped result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(this IApizrCrudMediator mediator, TReadAllParams readAllParams,
            int priority, Context context, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelReadAllResult}"/> to Apizr using MediatR with priority returning a mapped result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(this IApizrCrudMediator mediator, TReadAllParams readAllParams,
            int priority,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelReadAllResult}"/> to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(this IApizrCrudMediator mediator, TReadAllParams readAllParams, Context context,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelReadAllResult}"/> to Apizr using MediatR with priority returning a mapped result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(this IApizrCrudMediator mediator, TReadAllParams readAllParams,
            int priority, Context context,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        #endregion

        #endregion

        #region Read

        #region SendReadQuery

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="onException">Handle exception and return cached result</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TApiEntity> SendReadQuery<TApiEntity, TApiEntityKey>(this IApizrCrudMediator mediator, TApiEntityKey key, Action<Exception> onException);

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TApiEntity> SendReadQuery<TApiEntity, TApiEntityKey>(this IApizrCrudMediator mediator, TApiEntityKey key, bool clearCache, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TApiEntity> SendReadQuery<TApiEntity, TApiEntityKey>(this IApizrCrudMediator mediator, TApiEntityKey key, Context context, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TApiEntity> SendReadQuery<TApiEntity, TApiEntityKey>(this IApizrCrudMediator mediator, TApiEntityKey key,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR with priority
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TApiEntity> SendReadQuery<TApiEntity, TApiEntityKey>(this IApizrCrudMediator mediator, TApiEntityKey key,
            int priority, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TApiEntity> SendReadQuery<TApiEntity, TApiEntityKey>(this IApizrCrudMediator mediator, TApiEntityKey key, Context context,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR with priority
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TApiEntity> SendReadQuery<TApiEntity, TApiEntityKey>(this IApizrCrudMediator mediator, TApiEntityKey key,
            int priority, Context context, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR with priority
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TApiEntity> SendReadQuery<TApiEntity, TApiEntityKey>(this IApizrCrudMediator mediator, TApiEntityKey key,
            int priority,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR with priority
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TApiEntity> SendReadQuery<TApiEntity, TApiEntityKey>(this IApizrCrudMediator mediator, TApiEntityKey key,
            int priority, Context context,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        #endregion

        #region SendReadQuery<TModelEntity>

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="onException">Handle exception and return cached result</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelEntity> SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(this IApizrCrudMediator mediator, TApiEntityKey key, Action<Exception> onException);

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelEntity> SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(this IApizrCrudMediator mediator, TApiEntityKey key, bool clearCache, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelEntity> SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(this IApizrCrudMediator mediator, TApiEntityKey key, Context context, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelEntity> SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(this IApizrCrudMediator mediator, TApiEntityKey key,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR with priority returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelEntity> SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(this IApizrCrudMediator mediator, TApiEntityKey key,
            int priority, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelEntity> SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(this IApizrCrudMediator mediator, TApiEntityKey key, Context context,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR with priority returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelEntity> SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(this IApizrCrudMediator mediator, TApiEntityKey key,
            int priority, Context context, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR with priority returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelEntity> SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(this IApizrCrudMediator mediator, TApiEntityKey key,
            int priority,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR with priority returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelEntity> SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(this IApizrCrudMediator mediator, TApiEntityKey key,
            int priority, Context context,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        #endregion

        #endregion

        #region Update

        #region SendUpdateCommand

        /// <summary>
        /// Send a <see cref="UpdateCommand{TApiEntityKey, TApiEntity}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="onException">Handle exception</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task SendUpdateCommand<TApiEntity, TApiEntityKey>(this IApizrCrudMediator mediator, TApiEntityKey key,
            TApiEntity entity, Action<Exception> onException);

        /// <summary>
        /// Send a <see cref="UpdateCommand{TApiEntityKey, TApiEntity}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="context">The Polly context</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task SendUpdateCommand<TApiEntity, TApiEntityKey>(this IApizrCrudMediator mediator, TApiEntityKey key,
            TApiEntity entity, Context context, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="UpdateCommand{TApiEntityKey, TApiEntity}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task SendUpdateCommand<TApiEntity, TApiEntityKey>(this IApizrCrudMediator mediator, TApiEntityKey key,
            TApiEntity entity,
            CancellationToken cancellationToken, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="UpdateCommand{TApiEntityKey, TApiEntity}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task SendUpdateCommand<TApiEntity, TApiEntityKey>(this IApizrCrudMediator mediator, TApiEntityKey key,
            TApiEntity entity, Context context,
            CancellationToken cancellationToken, Action<Exception> onException = null);

        #endregion

        #region SendUpdateCommand<TModelEntity>

        /// <summary>
        /// Send a mapped <see cref="UpdateCommand{TApiEntityKey, TModelEntity}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="onException">Handle exception</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task SendUpdateCommand<TModelEntity, TApiEntity, TApiEntityKey>(this IApizrCrudMediator mediator, TApiEntityKey key,
            TModelEntity entity, Action<Exception> onException);

        /// <summary>
        /// Send a mapped <see cref="UpdateCommand{TApiEntityKey, TModelEntity}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="context">The Polly context</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task SendUpdateCommand<TModelEntity, TApiEntity, TApiEntityKey>(this IApizrCrudMediator mediator, TApiEntityKey key,
            TModelEntity entity, Context context, Action<Exception> onException = null);

        /// <summary>
        /// Send a mapped <see cref="UpdateCommand{TApiEntityKey, TModelEntity}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task SendUpdateCommand<TModelEntity, TApiEntity, TApiEntityKey>(this IApizrCrudMediator mediator, TApiEntityKey key,
            TModelEntity entity,
            CancellationToken cancellationToken, Action<Exception> onException = null);

        /// <summary>
        /// Send a mapped <see cref="UpdateCommand{TApiEntityKey, TModelEntity}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task SendUpdateCommand<TModelEntity, TApiEntity, TApiEntityKey>(this IApizrCrudMediator mediator, TApiEntityKey key,
            TModelEntity entity, Context context,
            CancellationToken cancellationToken, Action<Exception> onException = null);

        #endregion

        #endregion

        #region Delete

        /// <summary>
        /// Send a <see cref="DeleteCommand{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="onException">Handle exception</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task SendDeleteCommand<TApiEntity, TApiEntityKey>(this IApizrCrudMediator mediator, TApiEntityKey key, Action<Exception> onException);

        /// <summary>
        /// Send a <see cref="DeleteCommand{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="context">The Polly context</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task SendDeleteCommand<TApiEntity, TApiEntityKey>(this IApizrCrudMediator mediator, TApiEntityKey key, Context context, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="DeleteCommand{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task SendDeleteCommand<TApiEntity, TApiEntityKey>(this IApizrCrudMediator mediator, TApiEntityKey key,
            CancellationToken cancellationToken, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="DeleteCommand{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task SendDeleteCommand<TApiEntity, TApiEntityKey>(this IApizrCrudMediator mediator, TApiEntityKey key, Context context,
            CancellationToken cancellationToken, Action<Exception> onException = null);

        #endregion 

        #endregion
    }
}