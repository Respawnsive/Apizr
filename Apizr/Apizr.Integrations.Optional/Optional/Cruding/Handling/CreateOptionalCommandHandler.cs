using System;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Extending;
using Apizr.Mapping;
using Apizr.Mediation.Cruding.Handling.Base;
using Apizr.Requesting;
using Optional;
using Optional.Async.Extensions;

namespace Apizr.Optional.Cruding.Handling
{
    public class CreateOptionalCommandHandler<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> : 
        CreateCommandHandlerBase<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams, CreateOptionalCommand<TModelEntity>, Option<TModelEntity, ApizrException>>
        where TModelEntity : class
        where TApiEntity : class
    {
        public CreateOptionalCommandHandler(IApizrManager<ICrudApi<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>> crudApiManager, IMappingHandler mappingHandler) : base(crudApiManager, mappingHandler)
        {
        }

        public override async Task<Option<TModelEntity, ApizrException>> Handle(CreateOptionalCommand<TModelEntity> request, CancellationToken cancellationToken)
        {
            try
            {
                return await request
                    .SomeNotNull(new ApizrException(
                        new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                    .MapAsync(_ =>
                        CrudApiManager
                            .ExecuteAsync((ct, api) => api.Create(Map<TModelEntity, TApiEntity>(request.Payload), ct),
                                cancellationToken))
                    .MapAsync(apiResult => Task.FromResult(Map<TApiEntity, TModelEntity>(apiResult)))
                    .ConfigureAwait(false);
            }
            catch (ApizrException e)
            {
                return Option.None<TModelEntity, ApizrException>(e);
            }
        }
    }
}
