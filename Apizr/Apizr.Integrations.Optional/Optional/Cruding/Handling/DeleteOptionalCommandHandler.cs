using System;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Extending;
using Apizr.Mapping;
using Apizr.Mediation.Cruding.Handling.Base;
using Apizr.Requesting;
using MediatR;
using Optional;
using Optional.Async.Extensions;

namespace Apizr.Optional.Cruding.Handling
{
    public class DeleteOptionalCommandHandler<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> : 
        DeleteCommandHandlerBase<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams, DeleteOptionalCommand<TModelEntity, TApiEntityKey>, Option<Unit, ApizrException>>
        where TModelEntity : class
        where TApiEntity : class
    {
        public DeleteOptionalCommandHandler(IApizrManager<ICrudApi<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>> crudApiManager, IMappingHandler mappingHandler) : base(crudApiManager, mappingHandler)
        {
        }

        public override async Task<Option<Unit, ApizrException>> Handle(DeleteOptionalCommand<TModelEntity, TApiEntityKey> request, CancellationToken cancellationToken)
        {
            try
            {
                return await request
                    .SomeNotNull(new ApizrException(
                        new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                    .MapAsync(async _ =>
                    {
                        await CrudApiManager
                            .ExecuteAsync((ctx, ct, api) => api.Delete(request.Key, ctx, ct), request.Context, cancellationToken);

                        return Unit.Value;
                    })
                    .ConfigureAwait(false);
            }
            catch (ApizrException e)
            {
                return Option.None<Unit, ApizrException>(e);
            }
        }
    }

    public class DeleteOptionalCommandHandler<TModelEntity, TApiEntity, TReadAllResult, TReadAllParams> : 
        DeleteCommandHandlerBase<TModelEntity, TApiEntity, TReadAllResult, TReadAllParams, DeleteOptionalCommand<TModelEntity>, Option<Unit, ApizrException>>
        where TModelEntity : class
        where TApiEntity : class
    {
        public DeleteOptionalCommandHandler(IApizrManager<ICrudApi<TApiEntity, int, TReadAllResult, TReadAllParams>> crudApiManager, IMappingHandler mappingHandler) : base(crudApiManager, mappingHandler)
        {
        }

        public override async Task<Option<Unit, ApizrException>> Handle(DeleteOptionalCommand<TModelEntity> request, CancellationToken cancellationToken)
        {
            try
            {
                return await request
                    .SomeNotNull(new ApizrException(
                        new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                    .MapAsync(async _ =>
                    {
                        await CrudApiManager
                            .ExecuteAsync((ctx, ct, api) => api.Delete(request.Key, ctx, ct), request.Context, cancellationToken);

                        return Unit.Value;
                    })
                    .ConfigureAwait(false);
            }
            catch (ApizrException e)
            {
                return Option.None<Unit, ApizrException>(e);
            }
        }
    }
}
