using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fusillade;
using MediatR;
using Optional;

namespace Apizr.Optional.Cruding.Sending
{
    /// <summary>
    /// <see cref="IMediator"/> but dedicated to <see cref="TApiEntity"/> cruding with optional result, getting all shorter
    /// </summary>
    public interface ICrudOptionalMediator<TApiEntity, in TApiEntityKey, TReadAllResult, in TReadAllParams>
        where TApiEntity : class
    {
        Task<Option<TApiEntity, ApizrException>> SendCreateOptionalCommand(TApiEntity payload,
            CancellationToken cancellationToken = default,
            Priority priority = Priority.UserInitiated);

        Task<Option<TModelEntity, ApizrException>> SendCreateOptionalCommand<TModelEntity>(TModelEntity payload,
            CancellationToken cancellationToken = default,
            Priority priority = Priority.UserInitiated);

        Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(
            CancellationToken cancellationToken = default,
            Priority priority = Priority.UserInitiated);

        Task<Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>>
            SendReadAllOptionalQuery<TModelEntityReadAllResult>(CancellationToken cancellationToken = default,
                Priority priority = Priority.UserInitiated);

        Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(
            TReadAllParams readAllParams,
            CancellationToken cancellationToken = default,
            Priority priority = Priority.UserInitiated);

        Task<Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>>
            SendReadAllOptionalQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams,
                CancellationToken cancellationToken = default,
                Priority priority = Priority.UserInitiated);

        Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery(TApiEntityKey key,
            CancellationToken cancellationToken = default,
            Priority priority = Priority.UserInitiated);

        Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity>(TApiEntityKey key,
            CancellationToken cancellationToken = default,
            Priority priority = Priority.UserInitiated);

        Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand(TApiEntityKey key, TApiEntity payload,
            CancellationToken cancellationToken = default,
            Priority priority = Priority.UserInitiated);

        Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand<TModelEntity>(TApiEntityKey key,
            TModelEntity payload, CancellationToken cancellationToken = default,
            Priority priority = Priority.UserInitiated);

        Task<Option<Unit, ApizrException>> SendDeleteOptionalCommand(TApiEntityKey key,
            CancellationToken cancellationToken = default,
            Priority priority = Priority.UserInitiated);
    }
}