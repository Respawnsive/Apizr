using System.Threading;
using System.Threading.Tasks;
using Fusillade;
using MediatR;

namespace Apizr.Mediation.Cruding.Sending
{
    /// <summary>
    /// <see cref="IMediator"/> but dedicated to <see cref="TApiEntity"/> cruding, getting all shorter
    /// </summary>
    public interface ICrudMediator<TApiEntity, in TApiEntityKey, TReadAllResult, in TReadAllParams> where TApiEntity : class
    {
        Task<TApiEntity> SendCreateCommand(TApiEntity payload, CancellationToken cancellationToken = default,
            Priority priority = Priority.UserInitiated);

        Task<TModelEntity> SendCreateCommand<TModelEntity>(TModelEntity payload, CancellationToken cancellationToken = default,
            Priority priority = Priority.UserInitiated);

        Task<TReadAllResult> SendReadAllQuery(CancellationToken cancellationToken = default,
            Priority priority = Priority.UserInitiated);

        Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(CancellationToken cancellationToken = default,
            Priority priority = Priority.UserInitiated);

        Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams,
            CancellationToken cancellationToken = default,
            Priority priority = Priority.UserInitiated);

        Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams,
            CancellationToken cancellationToken = default,
            Priority priority = Priority.UserInitiated);

        Task<TApiEntity> SendReadQuery(TApiEntityKey key, CancellationToken cancellationToken = default,
            Priority priority = Priority.UserInitiated);

        Task<TModelEntity> SendReadQuery<TModelEntity>(TApiEntityKey key, CancellationToken cancellationToken = default,
            Priority priority = Priority.UserInitiated);

        Task SendUpdateCommand(TApiEntityKey key, TApiEntity payload, CancellationToken cancellationToken = default,
            Priority priority = Priority.UserInitiated);

        Task SendUpdateCommand<TModelEntity>(TApiEntityKey key, TModelEntity payload, CancellationToken cancellationToken = default,
            Priority priority = Priority.UserInitiated);

        Task SendDeleteCommand(TApiEntityKey key, CancellationToken cancellationToken = default,
            Priority priority = Priority.UserInitiated);
    }
}
