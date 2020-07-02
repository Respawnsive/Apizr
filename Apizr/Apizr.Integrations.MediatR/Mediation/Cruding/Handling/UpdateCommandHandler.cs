using System.Threading;
using System.Threading.Tasks;
using Apizr.Mediation.Cruding.Handling.Base;
using Apizr.Requesting;
using MediatR;

namespace Apizr.Mediation.Cruding.Handling
{
    public class UpdateCommandHandler<T, TKey, TReadAllResult, TReadAllParams> : 
        UpdateCommandHandlerBase<T, TKey, TReadAllResult, TReadAllParams, UpdateCommand<TKey, T>, Unit> 
        where T : class
    {
        public UpdateCommandHandler(IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>> crudApiManager) : base(crudApiManager)
        {
        }

        public override Task<Unit> Handle(UpdateCommand<TKey, T> request, CancellationToken cancellationToken)
        {
            return CrudApiManager.ExecuteAsync((ct, api) => api.Update(request.Key, request.Payload, ct), cancellationToken).ContinueWith(t => Unit.Value, cancellationToken);
        }
    }

    public class UpdateCommandHandler<T, TReadAllResult, TReadAllParams> : 
        UpdateCommandHandlerBase<T, TReadAllResult, TReadAllParams, UpdateCommand<T>> 
        where T : class
    {
        public UpdateCommandHandler(IApizrManager<ICrudApi<T, int, TReadAllResult, TReadAllParams>> crudApiManager) : base(crudApiManager)
        {
        }

        public override Task<Unit> Handle(UpdateCommand<T> request, CancellationToken cancellationToken)
        {
            return CrudApiManager.ExecuteAsync((ct, api) => api.Update(request.Key, request.Payload, ct), cancellationToken).ContinueWith(t => Unit.Value, cancellationToken);
        }
    }
}
