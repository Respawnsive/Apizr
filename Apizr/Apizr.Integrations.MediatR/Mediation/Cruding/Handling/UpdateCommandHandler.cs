using System.Threading;
using System.Threading.Tasks;
using Apizr.Mediation.Commanding;
using Apizr.Requesting;
using MediatR;

namespace Apizr.Mediation.Cruding.Handling
{
    public class UpdateCommandHandler<T, TKey, TReadAllResult> : ICommandHandler<UpdateCommand<TKey, T>> where T : class
    {
        private readonly IApizrManager<ICrudApi<T, TKey, TReadAllResult>> _crudApiManager;

        public UpdateCommandHandler(IApizrManager<ICrudApi<T, TKey, TReadAllResult>> crudApiManager)
        {
            _crudApiManager = crudApiManager;
        }

        public virtual Task<Unit> Handle(UpdateCommand<TKey, T> request, CancellationToken cancellationToken)
        {
            return _crudApiManager.ExecuteAsync((ct, api) => api.Update(request.Key, request.Payload, ct), cancellationToken).ContinueWith(t => Unit.Value, cancellationToken);
        }
    }
}
