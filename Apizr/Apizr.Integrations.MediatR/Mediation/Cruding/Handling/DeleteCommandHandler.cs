using System.Threading;
using System.Threading.Tasks;
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
    public class DeleteCommandHandler<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> :
        DeleteCommandHandlerBase<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams,
            DeleteCommand<TModelEntity, TApiEntityKey>, Unit>
        where TModelEntity : class
        where TApiEntity : class
    {
        public DeleteCommandHandler(
            IApizrManager<ICrudApi<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>> crudApiManager) : base(
            crudApiManager)
        {
        }

        /// <inheritdoc />
        public override Task<Unit> Handle(DeleteCommand<TModelEntity, TApiEntityKey> request,
            CancellationToken cancellationToken)
        {
            return CrudApiManager
                .ExecuteAsync((options, api) => api.Delete(request.Key, options.Context, options.CancellationToken),
                    request.OptionsBuilder).ContinueWith(_ => Unit.Value, cancellationToken);
        }
    }

    /// <summary>
    /// The Delete command handler
    /// </summary>
    /// <typeparam name="TModelEntity">The model entity type</typeparam>
    /// <typeparam name="TApiEntity">The api entity type</typeparam>
    /// <typeparam name="TReadAllResult">The returned result type</typeparam>
    /// <typeparam name="TReadAllParams">The read all params type</typeparam>
    public class DeleteCommandHandler<TModelEntity, TApiEntity, TReadAllResult, TReadAllParams> :
        DeleteCommandHandlerBase<TModelEntity, TApiEntity, TReadAllResult, TReadAllParams, DeleteCommand<TModelEntity>,
            Unit>
        where TModelEntity : class
        where TApiEntity : class
    {
        public DeleteCommandHandler(
            IApizrManager<ICrudApi<TApiEntity, int, TReadAllResult, TReadAllParams>> crudApiManager) : base(
            crudApiManager)
        {
        }

        /// <inheritdoc />
        public override Task<Unit> Handle(DeleteCommand<TModelEntity> request,
            CancellationToken cancellationToken)
        {
            return CrudApiManager
                .ExecuteAsync((options, api) => api.Delete(request.Key, options.Context, options.CancellationToken),
                    request.OptionsBuilder).ContinueWith(_ => Unit.Value, cancellationToken);
        }
    }
}
