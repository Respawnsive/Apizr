using System;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Mediation.Cruding.Handling.Base;
using Apizr.Requesting;
using MediatR;
using Optional;
using Optional.Async.Extensions;

namespace Apizr.Optional.Cruding.Handling
{
    public class DeleteOptionalCommandHandler<T, TKey, TReadAllResult, TReadAllParams> : 
        DeleteCommandHandlerBase<T, TKey, TReadAllResult, TReadAllParams, DeleteOptionalCommand<T, TKey>, Option<Unit, ApizrException>> 
        where T : class
    {
        public DeleteOptionalCommandHandler(IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>> crudApiManager) : base(crudApiManager)
        {
        }

        public override async Task<Option<Unit, ApizrException>> Handle(DeleteOptionalCommand<T, TKey> request, CancellationToken cancellationToken)
        {
            try
            {
                await CrudApiManager.ExecuteAsync((ct, api) => api.Delete(request.Key, ct), cancellationToken).ConfigureAwait(false);

                return Unit.Value.Some<Unit, ApizrException>();
            }
            catch (ApizrException e)
            {
                return Option.None<Unit, ApizrException>(e);
            }
        }
    }

    public class DeleteOptionalCommandHandler<T, TReadAllResult, TReadAllParams> : 
        DeleteCommandHandlerBase<T, TReadAllResult, TReadAllParams, DeleteOptionalCommand<T>, Option<Unit, ApizrException>> 
        where T : class
    {
        public DeleteOptionalCommandHandler(IApizrManager<ICrudApi<T, int, TReadAllResult, TReadAllParams>> crudApiManager) : base(crudApiManager)
        {
        }

        public override async Task<Option<Unit, ApizrException>> Handle(DeleteOptionalCommand<T> request, CancellationToken cancellationToken)
        {
            try
            {
                await CrudApiManager.ExecuteAsync((ct, api) => api.Delete(request.Key, ct), cancellationToken).ConfigureAwait(false);

                return Unit.Value.Some<Unit, ApizrException>();
            }
            catch (ApizrException e)
            {
                return Option.None<Unit, ApizrException>(e);
            }
        }
    }
}
