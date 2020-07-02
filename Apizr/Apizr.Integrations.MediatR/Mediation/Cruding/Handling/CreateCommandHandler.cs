using System.Threading;
using System.Threading.Tasks;
using Apizr.Mediation.Cruding.Handling.Base;
using Apizr.Requesting;

namespace Apizr.Mediation.Cruding.Handling
{
    public class CreateCommandHandler<T, TKey, TReadAllResult, TReadAllParams> : 
        CreateCommandHandlerBase<T, TKey, TReadAllResult, TReadAllParams, CreateCommand<T>, T> 
        where T : class
    {
        public CreateCommandHandler(IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>> crudApiManager) : base(
            crudApiManager)
        {
        }

        public override Task<T> Handle(CreateCommand<T> request, CancellationToken cancellationToken)
        {
            return CrudApiManager.ExecuteAsync((ct, api) => api.Create(request.Payload, ct), cancellationToken);
        }
    }
}
