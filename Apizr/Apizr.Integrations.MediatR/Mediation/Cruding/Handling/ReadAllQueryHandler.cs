using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Mapping;
using Apizr.Mediation.Cruding.Handling.Base;
using Apizr.Requesting;

namespace Apizr.Mediation.Cruding.Handling
{
    public class ReadAllQueryHandler<TApiEntity, TApiEntityKey, TModelEntityReadAllResult, TApiEntityReadAllResult, TReadAllParams> : 
        ReadAllQueryHandlerBase<TApiEntity, TApiEntityKey, TModelEntityReadAllResult, TApiEntityReadAllResult, TReadAllParams, ReadAllQuery<TReadAllParams, TModelEntityReadAllResult>, TModelEntityReadAllResult>
        where TApiEntity : class
    {
        public ReadAllQueryHandler(IApizrManager<ICrudApi<TApiEntity, TApiEntityKey, TApiEntityReadAllResult, TReadAllParams>> crudApiManager, IMappingHandler mappingHandler) : base(crudApiManager, mappingHandler)
        {
        }

        public override Task<TModelEntityReadAllResult> Handle(ReadAllQuery<TReadAllParams, TModelEntityReadAllResult> request, CancellationToken cancellationToken)
        {
            return CrudApiManager.ExecuteAsync((ct, api) => api.ReadAll(request.Parameters, ct), cancellationToken)
                .ContinueWith(task => Map<TApiEntityReadAllResult, TModelEntityReadAllResult>(task.Result), cancellationToken);
        }
    }

    public class ReadAllQueryHandler<TApiEntity, TApiEntityKey, TModelEntityReadAllResult, TApiEntityReadAllResult> : 
        ReadAllQueryHandlerBase<TApiEntity, TApiEntityKey, TModelEntityReadAllResult, TApiEntityReadAllResult, ReadAllQuery<TModelEntityReadAllResult>, TModelEntityReadAllResult>
        where TApiEntity : class
    {

        public ReadAllQueryHandler(IApizrManager<ICrudApi<TApiEntity, TApiEntityKey, TApiEntityReadAllResult, IDictionary<string, object>>> crudApiManager, IMappingHandler mappingHandler) : base(crudApiManager, mappingHandler)
        {
        }

        public override Task<TModelEntityReadAllResult> Handle(ReadAllQuery<TModelEntityReadAllResult> request, CancellationToken cancellationToken)
        {
            return CrudApiManager.ExecuteAsync((ct, api) => api.ReadAll(request.Parameters, ct), cancellationToken)
                .ContinueWith(task => Map<TApiEntityReadAllResult, TModelEntityReadAllResult>(task.Result), cancellationToken);
        }
    }
}
