using System;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Mediation.Cruding.Handling.Base;
using Apizr.Requesting;
using Optional;
using Optional.Async.Extensions;

namespace Apizr.Optional.Cruding.Handling
{
    public class CreateOptionalCommandHandler<T, TKey, TReadAllResult, TReadAllParams> : 
        CreateCommandHandlerBase<T, TKey, TReadAllResult, TReadAllParams, CreateOptionalCommand<T>, Option<T, ApizrException>> 
        where T : class
    {
        public CreateOptionalCommandHandler(IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>> crudApiManager) : base(crudApiManager)
        {
        }

        public override Task<Option<T, ApizrException>> Handle(CreateOptionalCommand<T> request, CancellationToken cancellationToken)
        {
            try
            {
                return CrudApiManager.ExecuteAsync((ct, api) => api.Create(request.Payload, ct), cancellationToken)
                    .SomeNotNullAsync(new ApizrException(new NullReferenceException("Create method returned null")));
            }
            catch (ApizrException e)
            {
                return Task.FromResult(Option.None<T, ApizrException>(e));
            }
        }
    }
}
