using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Apizr.Mediation.Cruding.Sending
{
    /// <summary>
    /// <see cref="IMediator"/> but dedicated to <see cref="TApiEntity"/> cruding, getting all shorter
    /// </summary>
    public interface ICrudMediator<TApiEntity, in TApiEntityKey, TReadAllResult, in TReadAllParams>
        where TApiEntity : class
    {
        /// <summary>
        /// Send a <see cref="CreateCommand{TApiEntity}"/> to Apizr with MediatR
        /// </summary>
        /// <param name="payload">The entity to create</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<TApiEntity> SendCreateCommand(TApiEntity payload, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send a mapped <see cref="CreateCommand{TModelEntity}"/> to Apizr with MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="payload">The entity to create</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<TModelEntity> SendCreateCommand<TModelEntity>(TModelEntity payload, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr with MediatR
        /// </summary>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<TReadAllResult> SendReadAllQuery(CancellationToken cancellationToken = default);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr with MediatR with priority
        /// </summary>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<TReadAllResult> SendReadAllQuery(int priority, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr with MediatR returning mapped result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult">The mapped result</typeparam>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(CancellationToken cancellationToken = default);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr with MediatR with priority returning mapped result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult">The mapped result</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(int priority,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr with MediatR
        /// </summary>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr with MediatR with priority
        /// </summary>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams,
            int priority,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr with MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult"></typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams,
            CancellationToken cancellationToken = default);

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
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr with MediatR
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<TApiEntity> SendReadQuery(TApiEntityKey key,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr with MediatR with priority
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<TApiEntity> SendReadQuery(TApiEntityKey key,
            int priority,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr with MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<TModelEntity> SendReadQuery<TModelEntity>(TApiEntityKey key,
            CancellationToken cancellationToken = default);

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
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Send a <see cref="UpdateCommand{TApiEntityKey, TApiEntity}"/> to Apizr with MediatR
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="payload">The entity to update</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task SendUpdateCommand(TApiEntityKey key, 
            TApiEntity payload,
            CancellationToken cancellationToken = default);

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
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Send a <see cref="DeleteCommand{TApiEntity, TApiEntityKey}"/> to Apizr with MediatR
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task SendDeleteCommand(TApiEntityKey key,
            CancellationToken cancellationToken = default);
    }
}