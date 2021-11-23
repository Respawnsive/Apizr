using System.Threading;
using System.Threading.Tasks;
using Apizr.Mediation.Requesting.Sending;
using Polly;

namespace Apizr.Mediation.Cruding.Sending
{
    public interface IApizrCrudMediator : IApizrMediatorBase
    {

    }

    /// <summary>
    /// Apizr mediator dedicated to <see cref="TApiEntity"/> cruding, getting all shorter
    /// </summary>
    public interface IApizrCrudMediator<TApiEntity, in TApiEntityKey, TReadAllResult, in TReadAllParams> : IApizrCrudMediator
        where TApiEntity : class
    {
        #region Create

        #region SendCreateCommand

        /// <summary>
        /// Send a <see cref="CreateCommand{TApiEntity}"/> to Apizr with MediatR
        /// </summary>
        /// <param name="payload">The entity to create</param>
        /// <returns></returns>
        Task<TApiEntity> SendCreateCommand(TApiEntity payload);

        /// <summary>
        /// Send a <see cref="CreateCommand{TApiEntity}"/> to Apizr with MediatR
        /// </summary>
        /// <param name="payload">The entity to create</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        Task<TApiEntity> SendCreateCommand(TApiEntity payload, Context context);

        /// <summary>
        /// Send a <see cref="CreateCommand{TApiEntity}"/> to Apizr with MediatR
        /// </summary>
        /// <param name="payload">The entity to create</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<TApiEntity> SendCreateCommand(TApiEntity payload, CancellationToken cancellationToken);

        /// <summary>
        /// Send a <see cref="CreateCommand{TApiEntity}"/> to Apizr with MediatR
        /// </summary>
        /// <param name="payload">The entity to create</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<TApiEntity> SendCreateCommand(TApiEntity payload, Context context, CancellationToken cancellationToken);

        #endregion

        #region SendCreateCommand<TModelEntity>

        /// <summary>
        /// Send a mapped <see cref="CreateCommand{TModelEntity}"/> to Apizr with MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="payload">The entity to create</param>
        /// <returns></returns>
        Task<TModelEntity> SendCreateCommand<TModelEntity>(TModelEntity payload);

        /// <summary>
        /// Send a mapped <see cref="CreateCommand{TModelEntity}"/> to Apizr with MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="payload">The entity to create</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        Task<TModelEntity> SendCreateCommand<TModelEntity>(TModelEntity payload, Context context);

        /// <summary>
        /// Send a mapped <see cref="CreateCommand{TModelEntity}"/> to Apizr with MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="payload">The entity to create</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<TModelEntity> SendCreateCommand<TModelEntity>(TModelEntity payload, CancellationToken cancellationToken);

        /// <summary>
        /// Send a mapped <see cref="CreateCommand{TModelEntity}"/> to Apizr with MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="payload">The entity to create</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<TModelEntity> SendCreateCommand<TModelEntity>(TModelEntity payload, Context context, CancellationToken cancellationToken);

        #endregion

        #endregion

        #region ReadAll

        #region SendReadAllQuery

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr with MediatR
        /// </summary>
        /// <returns></returns>
        Task<TReadAllResult> SendReadAllQuery();

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr with MediatR
        /// </summary>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        Task<TReadAllResult> SendReadAllQuery(Context context);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr with MediatR
        /// </summary>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<TReadAllResult> SendReadAllQuery(CancellationToken cancellationToken);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr with MediatR with priority
        /// </summary>
        /// <param name="priority">The execution priority</param>
        /// <returns></returns>
        Task<TReadAllResult> SendReadAllQuery(int priority);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr with MediatR with priority
        /// </summary>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        Task<TReadAllResult> SendReadAllQuery(int priority, Context context);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr with MediatR with priority
        /// </summary>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<TReadAllResult> SendReadAllQuery(int priority, CancellationToken cancellationToken);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr with MediatR with priority
        /// </summary>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<TReadAllResult> SendReadAllQuery(Context context, CancellationToken cancellationToken);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr with MediatR with priority
        /// </summary>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<TReadAllResult> SendReadAllQuery(int priority, Context context, CancellationToken cancellationToken);

        #endregion

        #region SendReadAllQuery<TModelEntityReadAllResult>

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr with MediatR returning mapped result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult">The mapped result</typeparam>
        /// <returns></returns>
        Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>();

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr with MediatR returning mapped result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult">The mapped result</typeparam>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(Context context);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr with MediatR returning mapped result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult">The mapped result</typeparam>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(CancellationToken cancellationToken);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr with MediatR with priority returning mapped result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult">The mapped result</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <returns></returns>
        Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(int priority);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr with MediatR with priority returning mapped result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult">The mapped result</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(int priority,
            CancellationToken cancellationToken);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr with MediatR with priority returning mapped result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult">The mapped result</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(int priority, Context context);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr with MediatR with priority returning mapped result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult">The mapped result</typeparam>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(Context context,
            CancellationToken cancellationToken);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr with MediatR with priority returning mapped result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult">The mapped result</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(int priority, Context context,
            CancellationToken cancellationToken);

        #endregion

        #region SendReadAllQuery(TReadAllParams)

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr with MediatR
        /// </summary>
        /// <param name="readAllParams">The read all filters</param>
        /// <returns></returns>
        Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr with MediatR
        /// </summary>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams, Context context);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr with MediatR
        /// </summary>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams,
            CancellationToken cancellationToken);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr with MediatR with priority
        /// </summary>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <returns></returns>
        Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams,
            int priority);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr with MediatR with priority
        /// </summary>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams,
            int priority, Context context);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr with MediatR with priority
        /// </summary>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams,
            int priority,
            CancellationToken cancellationToken);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr with MediatR
        /// </summary>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams, Context context,
            CancellationToken cancellationToken);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr with MediatR with priority
        /// </summary>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams,
            int priority, Context context,
            CancellationToken cancellationToken);

        #endregion

        #region SendReadAllQuery<TModelEntityReadAllResult>(TReadAllParams)

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr with MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult"></typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <returns></returns>
        Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr with MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult"></typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams, Context context);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr with MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult"></typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams,
            CancellationToken cancellationToken);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr with MediatR with priority returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult"></typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <returns></returns>
        Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams,
            int priority);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr with MediatR with priority returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult"></typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams,
            int priority, Context context);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr with MediatR with priority returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult"></typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams,
            int priority,
            CancellationToken cancellationToken);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr with MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult"></typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams, Context context,
            CancellationToken cancellationToken);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr with MediatR with priority returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult"></typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams,
            int priority, Context context,
            CancellationToken cancellationToken);

        #endregion

        #endregion

        #region Read

        #region SendReadQuery

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr with MediatR
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <returns></returns>
        Task<TApiEntity> SendReadQuery(TApiEntityKey key);

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr with MediatR
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        Task<TApiEntity> SendReadQuery(TApiEntityKey key, Context context);

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr with MediatR
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<TApiEntity> SendReadQuery(TApiEntityKey key,
            CancellationToken cancellationToken);

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr with MediatR with priority
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <returns></returns>
        Task<TApiEntity> SendReadQuery(TApiEntityKey key,
            int priority);

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr with MediatR
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<TApiEntity> SendReadQuery(TApiEntityKey key, Context context,
            CancellationToken cancellationToken);

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr with MediatR with priority
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        Task<TApiEntity> SendReadQuery(TApiEntityKey key,
            int priority, Context context);

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr with MediatR with priority
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<TApiEntity> SendReadQuery(TApiEntityKey key,
            int priority,
            CancellationToken cancellationToken);

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr with MediatR with priority
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<TApiEntity> SendReadQuery(TApiEntityKey key,
            int priority, Context context,
            CancellationToken cancellationToken);

        #endregion

        #region SendReadQuery<TModelEntity>

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr with MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="key">The entity key</param>
        /// <returns></returns>
        Task<TModelEntity> SendReadQuery<TModelEntity>(TApiEntityKey key);

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr with MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        Task<TModelEntity> SendReadQuery<TModelEntity>(TApiEntityKey key, Context context);

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr with MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<TModelEntity> SendReadQuery<TModelEntity>(TApiEntityKey key,
            CancellationToken cancellationToken);

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr with MediatR with priority returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <returns></returns>
        Task<TModelEntity> SendReadQuery<TModelEntity>(TApiEntityKey key,
            int priority);

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr with MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<TModelEntity> SendReadQuery<TModelEntity>(TApiEntityKey key, Context context,
            CancellationToken cancellationToken);

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr with MediatR with priority returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        Task<TModelEntity> SendReadQuery<TModelEntity>(TApiEntityKey key,
            int priority, Context context);

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr with MediatR with priority returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<TModelEntity> SendReadQuery<TModelEntity>(TApiEntityKey key,
            int priority,
            CancellationToken cancellationToken);

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr with MediatR with priority returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<TModelEntity> SendReadQuery<TModelEntity>(TApiEntityKey key,
            int priority, Context context,
            CancellationToken cancellationToken);

        #endregion

        #endregion

        #region Update

        #region SendUpdateCommand

        /// <summary>
        /// Send a <see cref="UpdateCommand{TApiEntityKey, TApiEntity}"/> to Apizr with MediatR
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="payload">The entity to update</param>
        /// <returns></returns>
        Task SendUpdateCommand(TApiEntityKey key,
            TApiEntity payload);

        /// <summary>
        /// Send a <see cref="UpdateCommand{TApiEntityKey, TApiEntity}"/> to Apizr with MediatR
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="payload">The entity to update</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        Task SendUpdateCommand(TApiEntityKey key,
            TApiEntity payload, Context context);

        /// <summary>
        /// Send a <see cref="UpdateCommand{TApiEntityKey, TApiEntity}"/> to Apizr with MediatR
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="payload">The entity to update</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task SendUpdateCommand(TApiEntityKey key,
            TApiEntity payload,
            CancellationToken cancellationToken);

        /// <summary>
        /// Send a <see cref="UpdateCommand{TApiEntityKey, TApiEntity}"/> to Apizr with MediatR
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="payload">The entity to update</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task SendUpdateCommand(TApiEntityKey key,
            TApiEntity payload, Context context,
            CancellationToken cancellationToken);

        #endregion

        #region SendUpdateCommand<TModelEntity>

        /// <summary>
        /// Send a mapped <see cref="UpdateCommand{TApiEntityKey, TModelEntity}"/> to Apizr with MediatR
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="payload">The entity to update</param>
        /// <returns></returns>
        Task SendUpdateCommand<TModelEntity>(TApiEntityKey key,
            TModelEntity payload);

        /// <summary>
        /// Send a mapped <see cref="UpdateCommand{TApiEntityKey, TModelEntity}"/> to Apizr with MediatR
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="payload">The entity to update</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        Task SendUpdateCommand<TModelEntity>(TApiEntityKey key,
            TModelEntity payload, Context context);

        /// <summary>
        /// Send a mapped <see cref="UpdateCommand{TApiEntityKey, TModelEntity}"/> to Apizr with MediatR
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="payload">The entity to update</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task SendUpdateCommand<TModelEntity>(TApiEntityKey key,
            TModelEntity payload,
            CancellationToken cancellationToken);

        /// <summary>
        /// Send a mapped <see cref="UpdateCommand{TApiEntityKey, TModelEntity}"/> to Apizr with MediatR
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="payload">The entity to update</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task SendUpdateCommand<TModelEntity>(TApiEntityKey key,
            TModelEntity payload, Context context,
            CancellationToken cancellationToken);  

        #endregion

        #endregion

        #region Delete

        /// <summary>
        /// Send a <see cref="DeleteCommand{TApiEntity, TApiEntityKey}"/> to Apizr with MediatR
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <returns></returns>
        Task SendDeleteCommand(TApiEntityKey key);

        /// <summary>
        /// Send a <see cref="DeleteCommand{TApiEntity, TApiEntityKey}"/> to Apizr with MediatR
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        Task SendDeleteCommand(TApiEntityKey key, Context context);

        /// <summary>
        /// Send a <see cref="DeleteCommand{TApiEntity, TApiEntityKey}"/> to Apizr with MediatR
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task SendDeleteCommand(TApiEntityKey key,
            CancellationToken cancellationToken);

        /// <summary>
        /// Send a <see cref="DeleteCommand{TApiEntity, TApiEntityKey}"/> to Apizr with MediatR
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task SendDeleteCommand(TApiEntityKey key, Context context,
            CancellationToken cancellationToken); 

        #endregion
    }
}