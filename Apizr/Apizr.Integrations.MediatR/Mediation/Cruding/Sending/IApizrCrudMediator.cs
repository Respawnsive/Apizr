using System;
using System.Threading;
using System.Threading.Tasks;
using Polly;

namespace Apizr.Mediation.Cruding.Sending
{
    /// <summary>
    /// Apizr mediator dedicated to cruding
    /// </summary>
    public interface IApizrCrudMediator : IApizrCrudMediatorBase
    {
        #region Create

        #region SendCreateCommand

        /// <summary>
        /// Send a <see cref="CreateCommand{TApiEntity}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task<TApiEntity> SendCreateCommand<TApiEntity>(TApiEntity entity, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="CreateCommand{TApiEntity}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="context">The Polly context</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task<TApiEntity> SendCreateCommand<TApiEntity>(TApiEntity entity, Context context, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="CreateCommand{TApiEntity}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task<TApiEntity> SendCreateCommand<TApiEntity>(TApiEntity entity, CancellationToken cancellationToken, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="CreateCommand{TApiEntity}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task<TApiEntity> SendCreateCommand<TApiEntity>(TApiEntity entity, Context context, CancellationToken cancellationToken, Action<Exception> onException = null);

        #endregion

        #region SendCreateCommand<TModelEntity>

        /// <summary>
        /// Send a mapped <see cref="CreateCommand{TModelEntity}"/> to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelEntity> SendCreateCommand<TModelEntity, TApiEntity>(TModelEntity entity, Action<Exception> onException = null);

        /// <summary>
        /// Send a mapped <see cref="CreateCommand{TModelEntity}"/> to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="context">The Polly context</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelEntity> SendCreateCommand<TModelEntity, TApiEntity>(TModelEntity entity, Context context, Action<Exception> onException = null);

        /// <summary>
        /// Send a mapped <see cref="CreateCommand{TModelEntity}"/> to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelEntity> SendCreateCommand<TModelEntity, TApiEntity>(TModelEntity entity, CancellationToken cancellationToken, Action<Exception> onException = null);

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
        Task<TModelEntity> SendCreateCommand<TModelEntity, TApiEntity>(TModelEntity entity, Context context, CancellationToken cancellationToken, Action<Exception> onException = null);

        #endregion

        #endregion

        #region ReadAll

        #region SendReadAllQuery

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TReadAllResult> SendReadAllQuery<TReadAllResult>(bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TReadAllResult> SendReadAllQuery<TReadAllResult>(Context context, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TReadAllResult> SendReadAllQuery<TReadAllResult>(CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority
        /// </summary>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TReadAllResult> SendReadAllQuery<TReadAllResult>(int priority, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority
        /// </summary>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TReadAllResult> SendReadAllQuery<TReadAllResult>(int priority, Context context, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority
        /// </summary>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TReadAllResult> SendReadAllQuery<TReadAllResult>(int priority, CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority
        /// </summary>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TReadAllResult> SendReadAllQuery<TReadAllResult>(Context context, CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

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
        Task<TReadAllResult> SendReadAllQuery<TReadAllResult>(int priority, Context context, CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        #endregion

        #region SendReadAllQuery<TModelReadAllResult>

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelReadAllResult}"/> to Apizr using MediatR returning mapped result
        /// </summary>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TApiReadAllResult>(bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelReadAllResult}"/> to Apizr using MediatR returning mapped result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TApiReadAllResult>(Context context, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelReadAllResult}"/> to Apizr using MediatR returning mapped result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TApiReadAllResult>(CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelReadAllResult}"/> to Apizr using MediatR with priority returning mapped result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TApiReadAllResult>(int priority, bool clearCache = false, Action<Exception> onException = null);

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
        Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TApiReadAllResult>(int priority,
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
        Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TApiReadAllResult>(int priority, Context context, bool clearCache = false, Action<Exception> onException = null);

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
        Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TApiReadAllResult>(Context context,
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
        Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TApiReadAllResult>(int priority, Context context,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        #endregion

        #region SendReadAllQuery(TReadAllParams)

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TReadAllResult">The api result type</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TReadAllResult> SendReadAllQuery<TReadAllResult, TReadAllParams>(TReadAllParams readAllParams, bool clearCache = false, Action<Exception> onException = null);

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
        Task<TReadAllResult> SendReadAllQuery<TReadAllResult, TReadAllParams>(TReadAllParams readAllParams, Context context, bool clearCache = false, Action<Exception> onException = null);

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
        Task<TReadAllResult> SendReadAllQuery<TReadAllResult, TReadAllParams>(TReadAllParams readAllParams,
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
        Task<TReadAllResult> SendReadAllQuery<TReadAllResult, TReadAllParams>(TReadAllParams readAllParams,
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
        Task<TReadAllResult> SendReadAllQuery<TReadAllResult, TReadAllParams>(TReadAllParams readAllParams,
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
        Task<TReadAllResult> SendReadAllQuery<TReadAllResult, TReadAllParams>(TReadAllParams readAllParams,
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
        Task<TReadAllResult> SendReadAllQuery<TReadAllResult, TReadAllParams>(TReadAllParams readAllParams, Context context,
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
        Task<TReadAllResult> SendReadAllQuery<TReadAllResult, TReadAllParams>(TReadAllParams readAllParams,
            int priority, Context context,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        #endregion

        #region SendReadAllQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(TReadAllParams)

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
        Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(TReadAllParams readAllParams, bool clearCache = false, Action<Exception> onException = null);

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
        Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(TReadAllParams readAllParams, Context context, bool clearCache = false, Action<Exception> onException = null);

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
        Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(TReadAllParams readAllParams,
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
        Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(TReadAllParams readAllParams,
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
        Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(TReadAllParams readAllParams,
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
        Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(TReadAllParams readAllParams,
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
        Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(TReadAllParams readAllParams, Context context,
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
        Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(TReadAllParams readAllParams,
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
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TApiEntity> SendReadQuery<TApiEntity, TApiEntityKey>(TApiEntityKey key, bool clearCache = false, Action<Exception> onException = null);

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
        Task<TApiEntity> SendReadQuery<TApiEntity, TApiEntityKey>(TApiEntityKey key, Context context, bool clearCache = false, Action<Exception> onException = null);

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
        Task<TApiEntity> SendReadQuery<TApiEntity, TApiEntityKey>(TApiEntityKey key,
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
        Task<TApiEntity> SendReadQuery<TApiEntity, TApiEntityKey>(TApiEntityKey key,
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
        Task<TApiEntity> SendReadQuery<TApiEntity, TApiEntityKey>(TApiEntityKey key, Context context,
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
        Task<TApiEntity> SendReadQuery<TApiEntity, TApiEntityKey>(TApiEntityKey key,
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
        Task<TApiEntity> SendReadQuery<TApiEntity, TApiEntityKey>(TApiEntityKey key,
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
        Task<TApiEntity> SendReadQuery<TApiEntity, TApiEntityKey>(TApiEntityKey key,
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
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelEntity> SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key, bool clearCache = false, Action<Exception> onException = null);

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
        Task<TModelEntity> SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key, Context context, bool clearCache = false, Action<Exception> onException = null);

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
        Task<TModelEntity> SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key,
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
        Task<TModelEntity> SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key,
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
        Task<TModelEntity> SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key, Context context,
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
        Task<TModelEntity> SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key,
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
        Task<TModelEntity> SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key,
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
        Task<TModelEntity> SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key,
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
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task SendUpdateCommand<TApiEntity, TApiEntityKey>(TApiEntityKey key,
            TApiEntity entity, Action<Exception> onException = null);

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
        Task SendUpdateCommand<TApiEntity, TApiEntityKey>(TApiEntityKey key,
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
        Task SendUpdateCommand<TApiEntity, TApiEntityKey>(TApiEntityKey key,
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
        Task SendUpdateCommand<TApiEntity, TApiEntityKey>(TApiEntityKey key,
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
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task SendUpdateCommand<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key,
            TModelEntity entity, Action<Exception> onException = null);

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
        Task SendUpdateCommand<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key,
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
        Task SendUpdateCommand<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key,
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
        Task SendUpdateCommand<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key,
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
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task SendDeleteCommand<TApiEntity, TApiEntityKey>(TApiEntityKey key, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="DeleteCommand{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="context">The Polly context</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task SendDeleteCommand<TApiEntity, TApiEntityKey>(TApiEntityKey key, Context context, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="DeleteCommand{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task SendDeleteCommand<TApiEntity, TApiEntityKey>(TApiEntityKey key,
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
        Task SendDeleteCommand<TApiEntity, TApiEntityKey>(TApiEntityKey key, Context context,
            CancellationToken cancellationToken, Action<Exception> onException = null);

        #endregion
    }

    /// <summary>
    /// Apizr mediator dedicated to <typeparamref name="TApiEntity"/> cruding, getting all shorter
    /// </summary>
    public interface IApizrCrudMediator<TApiEntity, in TApiEntityKey, TReadAllResult, in TReadAllParams> : IApizrCrudMediatorBase
        where TApiEntity : class
    {
        #region Create

        #region SendCreateCommand

        /// <summary>
        /// Send a <see cref="CreateCommand{TApiEntity}"/> to Apizr using MediatR
        /// </summary>
        /// <param name="entity">The entity to create</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task<TApiEntity> SendCreateCommand(TApiEntity entity, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="CreateCommand{TApiEntity}"/> to Apizr using MediatR
        /// </summary>
        /// <param name="entity">The entity to create</param>
        /// <param name="context">The Polly context</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task<TApiEntity> SendCreateCommand(TApiEntity entity, Context context, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="CreateCommand{TApiEntity}"/> to Apizr using MediatR
        /// </summary>
        /// <param name="entity">The entity to create</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task<TApiEntity> SendCreateCommand(TApiEntity entity, CancellationToken cancellationToken, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="CreateCommand{TApiEntity}"/> to Apizr using MediatR
        /// </summary>
        /// <param name="entity">The entity to create</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task<TApiEntity> SendCreateCommand(TApiEntity entity, Context context, CancellationToken cancellationToken, Action<Exception> onException = null);

        #endregion

        #region SendCreateCommand<TModelEntity>

        /// <summary>
        /// Send a mapped <see cref="CreateCommand{TModelEntity}"/> to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelEntity> SendCreateCommand<TModelEntity>(TModelEntity entity, Action<Exception> onException = null);

        /// <summary>
        /// Send a mapped <see cref="CreateCommand{TModelEntity}"/> to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="context">The Polly context</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelEntity> SendCreateCommand<TModelEntity>(TModelEntity entity, Context context, Action<Exception> onException = null);

        /// <summary>
        /// Send a mapped <see cref="CreateCommand{TModelEntity}"/> to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelEntity> SendCreateCommand<TModelEntity>(TModelEntity entity, CancellationToken cancellationToken, Action<Exception> onException = null);

        /// <summary>
        /// Send a mapped <see cref="CreateCommand{TModelEntity}"/> to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelEntity> SendCreateCommand<TModelEntity>(TModelEntity entity, Context context, CancellationToken cancellationToken, Action<Exception> onException = null);

        #endregion

        #endregion

        #region ReadAll

        #region SendReadAllQuery

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR
        /// </summary>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TReadAllResult> SendReadAllQuery(bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR
        /// </summary>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TReadAllResult> SendReadAllQuery(Context context, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR
        /// </summary>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TReadAllResult> SendReadAllQuery(CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority
        /// </summary>
        /// <param name="priority">The execution priority</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TReadAllResult> SendReadAllQuery(int priority, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority
        /// </summary>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TReadAllResult> SendReadAllQuery(int priority, Context context, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority
        /// </summary>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TReadAllResult> SendReadAllQuery(int priority, CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority
        /// </summary>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TReadAllResult> SendReadAllQuery(Context context, CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority
        /// </summary>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TReadAllResult> SendReadAllQuery(int priority, Context context, CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        #endregion

        #region SendReadAllQuery<TModelEntityReadAllResult>

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR returning mapped result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult">The mapped result</typeparam>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR returning mapped result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult">The mapped result</typeparam>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(Context context, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR returning mapped result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult">The mapped result</typeparam>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR with priority returning mapped result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult">The mapped result</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(int priority, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR with priority returning mapped result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult">The mapped result</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(int priority,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR with priority returning mapped result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult">The mapped result</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(int priority, Context context, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR with priority returning mapped result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult">The mapped result</typeparam>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(Context context,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR with priority returning mapped result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult">The mapped result</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(int priority, Context context,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        #endregion

        #region SendReadAllQuery(TReadAllParams)

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR
        /// </summary>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR
        /// </summary>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams, Context context, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR
        /// </summary>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority
        /// </summary>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams,
            int priority, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority
        /// </summary>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams,
            int priority, Context context, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority
        /// </summary>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams,
            int priority,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR
        /// </summary>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams, Context context,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority
        /// </summary>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams,
            int priority, Context context,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        #endregion

        #region SendReadAllQuery<TModelEntityReadAllResult>(TReadAllParams)

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult"></typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult"></typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams, Context context, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult"></typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR with priority returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult"></typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams,
            int priority, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR with priority returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult"></typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams,
            int priority, Context context, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR with priority returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult"></typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams,
            int priority,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult"></typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams, Context context,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR with priority returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult"></typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams,
            int priority, Context context,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        #endregion

        #endregion

        #region Read

        #region SendReadQuery

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TApiEntity> SendReadQuery(TApiEntityKey key, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TApiEntity> SendReadQuery(TApiEntityKey key, Context context, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TApiEntity> SendReadQuery(TApiEntityKey key,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR with priority
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TApiEntity> SendReadQuery(TApiEntityKey key,
            int priority, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TApiEntity> SendReadQuery(TApiEntityKey key, Context context,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR with priority
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TApiEntity> SendReadQuery(TApiEntityKey key,
            int priority, Context context, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR with priority
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TApiEntity> SendReadQuery(TApiEntityKey key,
            int priority,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR with priority
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TApiEntity> SendReadQuery(TApiEntityKey key,
            int priority, Context context,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        #endregion

        #region SendReadQuery<TModelEntity>

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelEntity> SendReadQuery<TModelEntity>(TApiEntityKey key, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelEntity> SendReadQuery<TModelEntity>(TApiEntityKey key, Context context, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelEntity> SendReadQuery<TModelEntity>(TApiEntityKey key,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR with priority returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelEntity> SendReadQuery<TModelEntity>(TApiEntityKey key,
            int priority, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelEntity> SendReadQuery<TModelEntity>(TApiEntityKey key, Context context,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR with priority returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelEntity> SendReadQuery<TModelEntity>(TApiEntityKey key,
            int priority, Context context, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR with priority returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelEntity> SendReadQuery<TModelEntity>(TApiEntityKey key,
            int priority,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR with priority returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelEntity> SendReadQuery<TModelEntity>(TApiEntityKey key,
            int priority, Context context,
            CancellationToken cancellationToken, bool clearCache = false, Action<Exception> onException = null);

        #endregion

        #endregion

        #region Update

        #region SendUpdateCommand

        /// <summary>
        /// Send a <see cref="UpdateCommand{TApiEntityKey, TApiEntity}"/> to Apizr using MediatR
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task SendUpdateCommand(TApiEntityKey key,
            TApiEntity entity, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="UpdateCommand{TApiEntityKey, TApiEntity}"/> to Apizr using MediatR
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="context">The Polly context</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task SendUpdateCommand(TApiEntityKey key,
            TApiEntity entity, Context context, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="UpdateCommand{TApiEntityKey, TApiEntity}"/> to Apizr using MediatR
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task SendUpdateCommand(TApiEntityKey key,
            TApiEntity entity,
            CancellationToken cancellationToken, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="UpdateCommand{TApiEntityKey, TApiEntity}"/> to Apizr using MediatR
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task SendUpdateCommand(TApiEntityKey key,
            TApiEntity entity, Context context,
            CancellationToken cancellationToken, Action<Exception> onException = null);

        #endregion

        #region SendUpdateCommand<TModelEntity>

        /// <summary>
        /// Send a mapped <see cref="UpdateCommand{TApiEntityKey, TModelEntity}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task SendUpdateCommand<TModelEntity>(TApiEntityKey key,
            TModelEntity entity, Action<Exception> onException = null);

        /// <summary>
        /// Send a mapped <see cref="UpdateCommand{TApiEntityKey, TModelEntity}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="context">The Polly context</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task SendUpdateCommand<TModelEntity>(TApiEntityKey key,
            TModelEntity entity, Context context, Action<Exception> onException = null);

        /// <summary>
        /// Send a mapped <see cref="UpdateCommand{TApiEntityKey, TModelEntity}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task SendUpdateCommand<TModelEntity>(TApiEntityKey key,
            TModelEntity entity,
            CancellationToken cancellationToken, Action<Exception> onException = null);

        /// <summary>
        /// Send a mapped <see cref="UpdateCommand{TApiEntityKey, TModelEntity}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task SendUpdateCommand<TModelEntity>(TApiEntityKey key,
            TModelEntity entity, Context context,
            CancellationToken cancellationToken, Action<Exception> onException = null);

        #endregion

        #endregion

        #region Delete

        /// <summary>
        /// Send a <see cref="DeleteCommand{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task SendDeleteCommand(TApiEntityKey key, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="DeleteCommand{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="context">The Polly context</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task SendDeleteCommand(TApiEntityKey key, Context context, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="DeleteCommand{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task SendDeleteCommand(TApiEntityKey key,
            CancellationToken cancellationToken, Action<Exception> onException = null);

        /// <summary>
        /// Send a <see cref="DeleteCommand{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task SendDeleteCommand(TApiEntityKey key, Context context,
            CancellationToken cancellationToken, Action<Exception> onException = null); 

        #endregion
    }
}