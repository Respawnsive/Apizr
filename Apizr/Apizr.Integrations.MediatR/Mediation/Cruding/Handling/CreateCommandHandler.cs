using System.Threading;
using System.Threading.Tasks;
using Apizr.Mediation.Commanding;
using Apizr.Requesting;

namespace Apizr.Mediation.Cruding.Handling
{
    public class CreateCommandHandler<T, TKey, TReadAllResult> : ICommandHandler<CreateCommand<T>, T> where T : class
    {
        private readonly IApizrManager<ICrudApi<T, TKey, TReadAllResult>> _crudApiManager;

        public CreateCommandHandler(IApizrManager<ICrudApi<T, TKey, TReadAllResult>> crudApiManager)
        {
            _crudApiManager = crudApiManager;
        }

        public virtual Task<T> Handle(CreateCommand<T> request, CancellationToken cancellationToken)
        {
            return _crudApiManager.ExecuteAsync((ct, api) => api.Create(request.Payload, ct), cancellationToken);
        }
    }
}
