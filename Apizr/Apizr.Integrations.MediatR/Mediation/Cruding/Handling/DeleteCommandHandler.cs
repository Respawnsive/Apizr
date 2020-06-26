using System.Threading;
using System.Threading.Tasks;
using Apizr.Mediation.Commanding;
using Apizr.Requesting;
using MediatR;

namespace Apizr.Mediation.Cruding.Handling
{
    public class DeleteCommandHandler<T, TKey, TReadAllResult> : ICommandHandler<DeleteCommand<TKey>> where T : class
    {
        private readonly IApizrManager<ICrudApi<T, TKey, TReadAllResult>> _crudApiManager;

        public DeleteCommandHandler(IApizrManager<ICrudApi<T, TKey, TReadAllResult>> crudApiManager)
        {
            _crudApiManager = crudApiManager;
        }

        public virtual Task<Unit> Handle(DeleteCommand<TKey> request, CancellationToken cancellationToken)
        {
            return _crudApiManager.ExecuteAsync((ct, api) => api.Delete(request.Key, ct), cancellationToken).ContinueWith(t => Unit.Value, cancellationToken);
        }
    }
}
