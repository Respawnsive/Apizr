using System.Threading;
using System.Threading.Tasks;
using Apizr.Mediation.Cruding;
using MediatR;
using Optional;
using Polly;

namespace Apizr.Optional.Cruding.Sending
{
    /// <summary>
    /// <see cref="IMediator"/> but dedicated to <see cref="TApiEntity"/> cruding with optional result, getting all shorter
    /// </summary>
    public interface ICrudOptionalMediator<TApiEntity, in TApiEntityKey, TReadAllResult, in TReadAllParams>
        where TApiEntity : class
    {
        #region Create

        #region SendCreateOptionalCommand

        /// <summary>
        /// Send a <see cref="CreateOptionalCommand{TApiEntity}"/> to Apizr with MediatR returning an optional result
        /// </summary>
        /// <param name="payload">The entity to create</param>
        /// <returns></returns>
        Task<Option<TApiEntity, ApizrException>> SendCreateOptionalCommand(TApiEntity payload);

        /// <summary>
        /// Send a <see cref="CreateOptionalCommand{TApiEntity}"/> to Apizr with MediatR returning an optional result
        /// </summary>
        /// <param name="payload">The entity to create</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        Task<Option<TApiEntity, ApizrException>> SendCreateOptionalCommand(TApiEntity payload, Context context);

        /// <summary>
        /// Send a <see cref="CreateOptionalCommand{TApiEntity}"/> to Apizr with MediatR returning an optional result
        /// </summary>
        /// <param name="payload">The entity to create</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TApiEntity, ApizrException>> SendCreateOptionalCommand(TApiEntity payload,
            CancellationToken cancellationToken);

        /// <summary>
        /// Send a <see cref="CreateOptionalCommand{TApiEntity}"/> to Apizr with MediatR returning an optional result
        /// </summary>
        /// <param name="payload">The entity to create</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TApiEntity, ApizrException>> SendCreateOptionalCommand(TApiEntity payload, Context context,
            CancellationToken cancellationToken);

        #endregion

        #region SendCreateOptionalCommand<TModelEntity>

        /// <summary>
        /// Send a mapped <see cref="CreateCommand{TModelEntity}"/> to Apizr with MediatR returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="payload">The entity to create</param>
        /// <returns></returns>
        Task<Option<TModelEntity, ApizrException>> SendCreateOptionalCommand<TModelEntity>(TModelEntity payload);

        /// <summary>
        /// Send a mapped <see cref="CreateCommand{TModelEntity}"/> to Apizr with MediatR returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="payload">The entity to create</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        Task<Option<TModelEntity, ApizrException>> SendCreateOptionalCommand<TModelEntity>(TModelEntity payload, Context context);

        /// <summary>
        /// Send a mapped <see cref="CreateCommand{TModelEntity}"/> to Apizr with MediatR returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="payload">The entity to create</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TModelEntity, ApizrException>> SendCreateOptionalCommand<TModelEntity>(TModelEntity payload,
            CancellationToken cancellationToken);

        /// <summary>
        /// Send a mapped <see cref="CreateCommand{TModelEntity}"/> to Apizr with MediatR returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="payload">The entity to create</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TModelEntity, ApizrException>> SendCreateOptionalCommand<TModelEntity>(TModelEntity payload, Context context,
            CancellationToken cancellationToken);

        #endregion

        #endregion

        #region ReadAll

        #region SendReadAllOptionalQuery

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr with MediatR returning an optional result
        /// </summary>
        /// <returns></returns>
        Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery();

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr with MediatR returning an optional result
        /// </summary>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(Context context);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr with MediatR returning an optional result
        /// </summary>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(CancellationToken cancellationToken);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr with MediatR with priority returning an optional result
        /// </summary>
        /// <param name="priority">The execution priority</param>
        /// <returns></returns>
        Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(int priority);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr with MediatR with priority returning an optional result
        /// </summary>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(int priority, Context context);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr with MediatR with priority returning an optional result
        /// </summary>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(int priority,
            CancellationToken cancellationToken);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr with MediatR returning an optional result
        /// </summary>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(Context context,
            CancellationToken cancellationToken);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr with MediatR with priority returning an optional result
        /// </summary>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(int priority, Context context,
            CancellationToken cancellationToken);

        #endregion

        #region SendReadAllOptionalQuery<TModelEntityReadAllResult>

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr with MediatR returning mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult">The mapped result</typeparam>
        /// <returns></returns>
        Task<Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>>
            SendReadAllOptionalQuery<TModelEntityReadAllResult>();

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr with MediatR returning mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult">The mapped result</typeparam>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        Task<Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>>
            SendReadAllOptionalQuery<TModelEntityReadAllResult>(Context context);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr with MediatR returning mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult">The mapped result</typeparam>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>>
            SendReadAllOptionalQuery<TModelEntityReadAllResult>(CancellationToken cancellationToken);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr with MediatR with priority returning mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult">The mapped result</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <returns></returns>
        Task<Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>>
            SendReadAllOptionalQuery<TModelEntityReadAllResult>(int priority);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr with MediatR with priority returning mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult">The mapped result</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        Task<Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>>
            SendReadAllOptionalQuery<TModelEntityReadAllResult>(int priority, Context context);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr with MediatR with priority returning mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult">The mapped result</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>>
            SendReadAllOptionalQuery<TModelEntityReadAllResult>(int priority, CancellationToken cancellationToken);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr with MediatR returning mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult">The mapped result</typeparam>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>>
            SendReadAllOptionalQuery<TModelEntityReadAllResult>(Context context,
                CancellationToken cancellationToken);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr with MediatR with priority returning mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult">The mapped result</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>>
            SendReadAllOptionalQuery<TModelEntityReadAllResult>(int priority, Context context,
                CancellationToken cancellationToken);

        #endregion

        #region SendReadAllOptionalQuery(TReadAllParams)

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr with MediatR returning an optional result
        /// </summary>
        /// <param name="readAllParams">The read all filters</param>
        /// <returns></returns>
        Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(
            TReadAllParams readAllParams);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr with MediatR returning an optional result
        /// </summary>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(
            TReadAllParams readAllParams, Context context);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr with MediatR returning an optional result
        /// </summary>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(
            TReadAllParams readAllParams,
            CancellationToken cancellationToken);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr with MediatR with priority returning an optional result
        /// </summary>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <returns></returns>
        Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(
            TReadAllParams readAllParams,
            int priority);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr with MediatR with priority returning an optional result
        /// </summary>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(
            TReadAllParams readAllParams,
            int priority, Context context);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr with MediatR with priority returning an optional result
        /// </summary>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(
            TReadAllParams readAllParams,
            int priority,
            CancellationToken cancellationToken);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr with MediatR returning an optional result
        /// </summary>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(
            TReadAllParams readAllParams, Context context,
            CancellationToken cancellationToken);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr with MediatR with priority returning an optional result
        /// </summary>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(
            TReadAllParams readAllParams,
            int priority, Context context,
            CancellationToken cancellationToken);

        #endregion

        #region SendReadAllOptionalQuery<TModelEntityReadAllResult>(TReadAllParams)

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr with MediatR returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult"></typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <returns></returns>
        Task<Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>>
            SendReadAllOptionalQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr with MediatR returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult"></typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        Task<Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>>
            SendReadAllOptionalQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams, Context context);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr with MediatR returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult"></typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>>
            SendReadAllOptionalQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams,
                CancellationToken cancellationToken);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr with MediatR with priority returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult"></typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <returns></returns>
        Task<Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>>
            SendReadAllOptionalQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams,
                int priority);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr with MediatR with priority returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult"></typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        Task<Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>>
            SendReadAllOptionalQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams,
                int priority, Context context);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr with MediatR with priority returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult"></typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>>
            SendReadAllOptionalQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams,
                int priority,
                CancellationToken cancellationToken);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr with MediatR returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult"></typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>>
            SendReadAllOptionalQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams, Context context,
                CancellationToken cancellationToken);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr with MediatR with priority returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult"></typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>>
            SendReadAllOptionalQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams,
                int priority, Context context,
                CancellationToken cancellationToken);

        #endregion

        #endregion

        #region Read

        #region SendReadOptionalQuery

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr with MediatR returning an optional result
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <returns></returns>
        Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery(TApiEntityKey key);

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr with MediatR returning an optional result
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery(TApiEntityKey key, Context context);

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr with MediatR returning an optional result
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery(TApiEntityKey key,
            CancellationToken cancellationToken);

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr with MediatR with priority returning an optional result
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <returns></returns>
        Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery(TApiEntityKey key,
            int priority);

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr with MediatR with priority returning an optional result
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery(TApiEntityKey key,
            int priority, Context context);

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr with MediatR with priority returning an optional result
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery(TApiEntityKey key,
            int priority,
            CancellationToken cancellationToken);

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr with MediatR returning an optional result
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery(TApiEntityKey key, Context context,
            CancellationToken cancellationToken);

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr with MediatR with priority returning an optional result
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery(TApiEntityKey key,
            int priority, Context context,
            CancellationToken cancellationToken);

        #endregion

        #region SendReadOptionalQuery<TModelEntity>

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr with MediatR returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="key">The entity key</param>
        /// <returns></returns>
        Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity>(TApiEntityKey key);

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr with MediatR returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity>(TApiEntityKey key, Context context);

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr with MediatR returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity>(TApiEntityKey key,
            CancellationToken cancellationToken);

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr with MediatR with priority returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <returns></returns>
        Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity>(TApiEntityKey key,
            int priority);

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr with MediatR with priority returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity>(TApiEntityKey key,
            int priority, Context context);

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr with MediatR with priority returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity>(TApiEntityKey key,
            int priority,
            CancellationToken cancellationToken);

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr with MediatR returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity>(TApiEntityKey key, Context context,
            CancellationToken cancellationToken);

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr with MediatR with priority returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity>(TApiEntityKey key,
            int priority, Context context,
            CancellationToken cancellationToken);

        #endregion

        #endregion

        #region Update

        #region SendUpdateOptionalCommand

        /// <summary>
        /// Send a <see cref="UpdateCommand{TApiEntityKey, TApiEntity}"/> to Apizr with MediatR returning an optional result
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="payload">The entity to update</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand(TApiEntityKey key, TApiEntity payload);

        /// <summary>
        /// Send a <see cref="UpdateCommand{TApiEntityKey, TApiEntity}"/> to Apizr with MediatR returning an optional result
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="payload">The entity to update</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand(TApiEntityKey key, TApiEntity payload, Context context);

        /// <summary>
        /// Send a <see cref="UpdateCommand{TApiEntityKey, TApiEntity}"/> to Apizr with MediatR returning an optional result
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="payload">The entity to update</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand(TApiEntityKey key, TApiEntity payload,
            CancellationToken cancellationToken);

        /// <summary>
        /// Send a <see cref="UpdateCommand{TApiEntityKey, TApiEntity}"/> to Apizr with MediatR returning an optional result
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="payload">The entity to update</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand(TApiEntityKey key, TApiEntity payload, Context context,
            CancellationToken cancellationToken);

        #endregion

        #region SendUpdateOptionalCommand<TModelEntity>

        /// <summary>
        /// Send a mapped <see cref="UpdateCommand{TApiEntityKey, TModelEntity}"/> to Apizr with MediatR returning an optional result
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="payload">The entity to update</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand<TModelEntity>(TApiEntityKey key,
            TModelEntity payload);

        /// <summary>
        /// Send a mapped <see cref="UpdateCommand{TApiEntityKey, TModelEntity}"/> to Apizr with MediatR returning an optional result
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="payload">The entity to update</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand<TModelEntity>(TApiEntityKey key,
            TModelEntity payload, Context context);

        /// <summary>
        /// Send a mapped <see cref="UpdateCommand{TApiEntityKey, TModelEntity}"/> to Apizr with MediatR returning an optional result
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="payload">The entity to update</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand<TModelEntity>(TApiEntityKey key,
            TModelEntity payload, CancellationToken cancellationToken);

        /// <summary>
        /// Send a mapped <see cref="UpdateCommand{TApiEntityKey, TModelEntity}"/> to Apizr with MediatR returning an optional result
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="payload">The entity to update</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand<TModelEntity>(TApiEntityKey key,
            TModelEntity payload, Context context,
            CancellationToken cancellationToken);

        #endregion

        #endregion

        #region Delete

        /// <summary>
        /// Send a <see cref="DeleteCommand{TApiEntity, TApiEntityKey}"/> to Apizr with MediatR returning an optional result
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendDeleteOptionalCommand(TApiEntityKey key);

        /// <summary>
        /// Send a <see cref="DeleteCommand{TApiEntity, TApiEntityKey}"/> to Apizr with MediatR returning an optional result
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendDeleteOptionalCommand(TApiEntityKey key, Context context);

        /// <summary>
        /// Send a <see cref="DeleteCommand{TApiEntity, TApiEntityKey}"/> to Apizr with MediatR returning an optional result
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendDeleteOptionalCommand(TApiEntityKey key,
            CancellationToken cancellationToken);

        /// <summary>
        /// Send a <see cref="DeleteCommand{TApiEntity, TApiEntityKey}"/> to Apizr with MediatR returning an optional result
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendDeleteOptionalCommand(TApiEntityKey key, Context context,
            CancellationToken cancellationToken); 

        #endregion
    }
}