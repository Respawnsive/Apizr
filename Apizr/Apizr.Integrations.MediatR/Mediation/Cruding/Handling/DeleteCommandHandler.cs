using System.Threading;
using System.Threading.Tasks;
using Apizr.Mediation.Cruding.Handling.Base;
using Apizr.Requesting;
using MediatR;

namespace Apizr.Mediation.Cruding.Handling
{
    public class DeleteCommandHandler<T, TKey, TReadAllResult, TReadAllParams> : 
        DeleteCommandHandlerBase<T, TKey, TReadAllResult, TReadAllParams, DeleteCommand<T, TKey>, Unit> 
        where T : class
    {
        public DeleteCommandHandler(IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>> crudApiManager) : base(crudApiManager)
        {
        }

        public override Task<Unit> Handle(DeleteCommand<T, TKey> request, CancellationToken cancellationToken)
        {
            return CrudApiManager.ExecuteAsync((ct, api) => api.Delete(request.Key, ct), cancellationToken).ContinueWith(t => Unit.Value, cancellationToken);
        }
    }

    public class DeleteCommandHandler<T, TReadAllResult, TReadAllParams> : 
        DeleteCommandHandlerBase<T, TReadAllResult, TReadAllParams, DeleteCommand<T>, Unit> 
        where T : class
    {
        public DeleteCommandHandler(IApizrManager<ICrudApi<T, int, TReadAllResult, TReadAllParams>> crudApiManager) : base(crudApiManager)
        {
        }

        public override Task<Unit> Handle(DeleteCommand<T> request, CancellationToken cancellationToken)
        {
            return CrudApiManager.ExecuteAsync((ct, api) => api.Delete(request.Key, ct), cancellationToken).ContinueWith(t => Unit.Value, cancellationToken);
        }
    }
}
