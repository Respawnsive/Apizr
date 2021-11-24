using System.Threading;
using System.Threading.Tasks;
using Apizr.Mapping;
using Apizr.Mediation.Cruding.Handling.Base;
using Apizr.Requesting;

namespace Apizr.Mediation.Cruding.Handling
{
    public class ReadQueryHandler<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> : 
        ReadQueryHandlerBase<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams, ReadQuery<TModelEntity, TApiEntityKey>, TModelEntity>
        where TModelEntity : class
        where TApiEntity : class
    {
        public ReadQueryHandler(IApizrManager<ICrudApi<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>> crudApiManager, IMappingHandler mappingHandler) : base(crudApiManager, mappingHandler)
        {
        }

        public override async Task<TModelEntity> Handle(ReadQuery<TModelEntity, TApiEntityKey> request, CancellationToken cancellationToken)
        {
            return await CrudApiManager
                .ExecuteAsync<TModelEntity, TApiEntity>((ctx, ct, api) => api.Read(request.Key, request.Priority, ctx, ct), request.Context, cancellationToken)
                .ConfigureAwait(false);
        }
    }

    public class ReadQueryHandler<TModelEntity, TApiEntity, TReadAllResult, TReadAllParams> : 
        ReadQueryHandlerBase<TModelEntity, TApiEntity, TReadAllResult, TReadAllParams, ReadQuery<TModelEntity>, TModelEntity>
        where TModelEntity : class
        where TApiEntity : class
    {
        public ReadQueryHandler(IApizrManager<ICrudApi<TApiEntity, int, TReadAllResult, TReadAllParams>> crudApiManager, IMappingHandler mappingHandler) : base(crudApiManager, mappingHandler)
        {
        }

        public override async Task<TModelEntity> Handle(ReadQuery<TModelEntity> request, CancellationToken cancellationToken)
        {
            return await CrudApiManager
                .ExecuteAsync<TModelEntity, TApiEntity>((ctx, ct, api) => api.Read(request.Key, request.Priority, ctx, ct), request.Context, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
