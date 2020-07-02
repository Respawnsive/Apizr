using System.Threading;
using System.Threading.Tasks;
using Apizr.Mediation.Cruding.Handling.Base;
using Apizr.Requesting;
using MediatR;
using Optional;

namespace Apizr.Optional.Cruding.Handling
{
    public class UpdateOptionalCommandHandler<T, TKey, TReadAllResult, TReadAllParams> : 
        UpdateCommandHandlerBase<T, TKey, TReadAllResult, TReadAllParams, UpdateOptionalCommand<TKey, T>, Option<Unit, ApizrException>>
        where T : class
    {
        public UpdateOptionalCommandHandler(IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>> crudApiManager) : base(crudApiManager)
        {
        }

        public override async Task<Option<Unit, ApizrException>> Handle(UpdateOptionalCommand<TKey, T> request, CancellationToken cancellationToken)
        {
            try
            {
                await CrudApiManager.ExecuteAsync((ct, api) => api.Update(request.Key, request.Payload, ct), cancellationToken).ConfigureAwait(false);

                return Unit.Value.Some<Unit, ApizrException>();
            }
            catch (ApizrException e)
            {
                return Option.None<Unit, ApizrException>(e);
            }
        }
    }

    public class UpdateOptionalCommandHandler<T, TReadAllResult, TReadAllParams> : 
        UpdateCommandHandlerBase<T, TReadAllResult, TReadAllParams, UpdateOptionalCommand<T>, Option<Unit, ApizrException>>
        where T : class
    {
        public UpdateOptionalCommandHandler(IApizrManager<ICrudApi<T, int, TReadAllResult, TReadAllParams>> crudApiManager) : base(crudApiManager)
        {
        }

        public override async Task<Option<Unit, ApizrException>> Handle(UpdateOptionalCommand<T> request, CancellationToken cancellationToken)
        {

            try
            {
                await CrudApiManager.ExecuteAsync((ct, api) => api.Update(request.Key, request.Payload, ct), cancellationToken).ConfigureAwait(false);

                return Unit.Value.Some<Unit, ApizrException>();
            }
            catch (ApizrException e)
            {
                return Option.None<Unit, ApizrException>(e);
            }
        }
    }
}
