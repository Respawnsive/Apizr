using System.Threading;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
using Apizr.Mediation.Cruding.Handling.Base;
using Apizr.Requesting;
using MediatR;

namespace Apizr.Mediation.Cruding.Handling
{
    /// <summary>
    /// The Delete command handler
    /// </summary>
    /// <typeparam name="TModelEntity">The model entity type</typeparam>
    /// <typeparam name="TApiEntity">The api entity type</typeparam>
    /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
    /// <typeparam name="TReadAllResult">The returned result type</typeparam>
    /// <typeparam name="TReadAllParams">The read all params type</typeparam>
    public class SafeDeleteCommandHandler<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> :
        DeleteCommandHandlerBase<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams,
            SafeDeleteCommand<TModelEntity, TApiEntityKey>, IApizrResponse, IApizrRequestOptions, IApizrRequestOptionsBuilder>
        where TModelEntity : class
        where TApiEntity : class
    {
        public SafeDeleteCommandHandler(
            IApizrManager<ICrudApi<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>> crudApiManager) : base(
            crudApiManager)
        {
        }

        /// <inheritdoc />
        public override Task<IApizrResponse> Handle(SafeDeleteCommand<TModelEntity, TApiEntityKey> request,
            CancellationToken cancellationToken) =>
            CrudApiManager
                .ExecuteAsync((options, api) => api.SafeDelete(request.Key, options),
                    request.OptionsBuilder);
    }

    /// <summary>
    /// The Delete command handler
    /// </summary>
    /// <typeparam name="TModelEntity">The model entity type</typeparam>
    /// <typeparam name="TApiEntity">The api entity type</typeparam>
    /// <typeparam name="TReadAllResult">The returned result type</typeparam>
    /// <typeparam name="TReadAllParams">The read all params type</typeparam>
    public class SafeDeleteCommandHandler<TModelEntity, TApiEntity, TReadAllResult, TReadAllParams> :
        DeleteCommandHandlerBase<TModelEntity, TApiEntity, TReadAllResult, TReadAllParams, SafeDeleteCommand<TModelEntity>,
            IApizrResponse, IApizrRequestOptions, IApizrRequestOptionsBuilder>
        where TModelEntity : class
        where TApiEntity : class
    {
        public SafeDeleteCommandHandler(
            IApizrManager<ICrudApi<TApiEntity, int, TReadAllResult, TReadAllParams>> crudApiManager) : base(
            crudApiManager)
        {
        }

        /// <inheritdoc />
        public override Task<IApizrResponse> Handle(SafeDeleteCommand<TModelEntity> request,
            CancellationToken cancellationToken) =>
            CrudApiManager
                .ExecuteAsync((options, api) => api.SafeDelete(request.Key, options),
                    request.OptionsBuilder);
    }
}
