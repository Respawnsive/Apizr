using System;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Extending;
using Apizr.Mediation.Cruding.Handling.Base;
using Apizr.Requesting;
using Optional;
using Optional.Async.Extensions;

namespace Apizr.Optional.Cruding.Handling
{
    public class ReadOptionalQueryHandler<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> : 
        ReadQueryHandlerBase<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams, ReadOptionalQuery<TModelEntity, TApiEntityKey>, Option<TModelEntity, ApizrException<TModelEntity>>>
        where TModelEntity : class
        where TApiEntity : class
    {
        public ReadOptionalQueryHandler(IApizrManager<ICrudApi<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>> crudApiManager) : base(crudApiManager)
        {
        }

        public override async Task<Option<TModelEntity, ApizrException<TModelEntity>>> Handle(ReadOptionalQuery<TModelEntity, TApiEntityKey> request, CancellationToken cancellationToken)
        {
            try
            {
                return await request.SomeNotNull(new ApizrException<TModelEntity>(
                        new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                    .MapAsync(_ => CrudApiManager.ExecuteAsync<TModelEntity, TApiEntity>(
                        (ctx, ct, api) => api.Read(request.Key, request.Priority, ctx, ct), request.Context,
                        cancellationToken, request.ClearCache))
                    .ConfigureAwait(false);
            }
            catch (ApizrException<TModelEntity> e)
            {
                return Option.None<TModelEntity, ApizrException<TModelEntity>>(e);
            }
        }
    }

    public class ReadOptionalQueryHandler<TModelEntity, TApiEntity, TReadAllResult, TReadAllParams> : 
        ReadQueryHandlerBase<TModelEntity, TApiEntity, TReadAllResult, TReadAllParams, ReadOptionalQuery<TModelEntity>, Option<TModelEntity, ApizrException<TModelEntity>>> 
        where TApiEntity : class
    {
        public ReadOptionalQueryHandler(IApizrManager<ICrudApi<TApiEntity, int, TReadAllResult, TReadAllParams>> crudApiManager) : base(crudApiManager)
        {
        }

        public override async Task<Option<TModelEntity, ApizrException<TModelEntity>>> Handle(ReadOptionalQuery<TModelEntity> request, CancellationToken cancellationToken)
        {
            try
            {
                return await request.SomeNotNull(new ApizrException<TModelEntity>(
                        new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                    .MapAsync(_ => CrudApiManager.ExecuteAsync<TModelEntity, TApiEntity>(
                        (ctx, ct, api) => api.Read(request.Key, request.Priority, ctx, ct), request.Context,
                        cancellationToken, request.ClearCache))
                    .ConfigureAwait(false);
            }
            catch (ApizrException<TModelEntity> e)
            {
                return Option.None<TModelEntity, ApizrException<TModelEntity>>(e);
            }
        }
    }
}
