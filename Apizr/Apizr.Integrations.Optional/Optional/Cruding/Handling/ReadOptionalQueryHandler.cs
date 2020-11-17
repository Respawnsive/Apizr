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
    public class ReadOptionalQueryHandler<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> : 
        ReadQueryHandlerBase<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams, ReadOptionalQuery<TModelEntity, TApiEntityKey>, Option<TModelEntity, ApizrException<TModelEntity>>>
        where TModelEntity : class
        where TApiEntity : class
    {
        public ReadOptionalQueryHandler(IApizrManager<ICrudApi<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>> crudApiManager, IMappingHandler mappingHandler) : base(crudApiManager, mappingHandler)
        {
        }

        public override async Task<Option<TModelEntity, ApizrException<TModelEntity>>> Handle(ReadOptionalQuery<TModelEntity, TApiEntityKey> request, CancellationToken cancellationToken)
        {
            try
            {
                return await request.SomeNotNull(new ApizrException<TModelEntity>(
                        new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                    .MapAsync(_ => CrudApiManager.ExecuteAsync((ct, api) => api.Read(request.Key, ct),
                        cancellationToken, request.Priority))
                    .MapAsync(result => Task.FromResult(Map<TApiEntity, TModelEntity>(result)))
                    .ConfigureAwait(false);
            }
            catch (ApizrException<TApiEntity> e)
            {
                return Option.None<TModelEntity, ApizrException<TModelEntity>>(
                    new ApizrException<TModelEntity>(e.InnerException, Map<TApiEntity, TModelEntity>(e.CachedResult)));
            }
        }
    }

    public class ReadOptionalQueryHandler<TModelEntity, TApiEntity, TReadAllResult, TReadAllParams> : 
        ReadQueryHandlerBase<TModelEntity, TApiEntity, TReadAllResult, TReadAllParams, ReadOptionalQuery<TModelEntity>, Option<TModelEntity, ApizrException<TModelEntity>>> 
        where TApiEntity : class
    {
        public ReadOptionalQueryHandler(IApizrManager<ICrudApi<TApiEntity, int, TReadAllResult, TReadAllParams>> crudApiManager, IMappingHandler mappingHandler) : base(crudApiManager, mappingHandler)
        {
        }

        public override async Task<Option<TModelEntity, ApizrException<TModelEntity>>> Handle(ReadOptionalQuery<TModelEntity> request, CancellationToken cancellationToken)
        {
            try
            {
                return await request.SomeNotNull(new ApizrException<TModelEntity>(
                        new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                    .MapAsync(_ => CrudApiManager.ExecuteAsync((ct, api) => api.Read(request.Key, ct),
                        cancellationToken, request.Priority))
                    .MapAsync(result => Task.FromResult(Map<TApiEntity, TModelEntity>(result)))
                    .ConfigureAwait(false);
            }
            catch (ApizrException<TApiEntity> e)
            {
                return Option.None<TModelEntity, ApizrException<TModelEntity>>(
                    new ApizrException<TModelEntity>(e.InnerException, Map<TApiEntity, TModelEntity>(e.CachedResult)));
            }
        }
    }
}
