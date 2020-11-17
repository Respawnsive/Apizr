using System;
using System.Collections.Generic;
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
    public class ReadAllOptionalQueryHandler<TApiEntity, TApiEntityKey, TModelEntityReadAllResult, TApiEntityReadAllResult, TReadAllParams> : 
        ReadAllQueryHandlerBase<TApiEntity, TApiEntityKey, TModelEntityReadAllResult, TApiEntityReadAllResult, TReadAllParams, ReadAllOptionalQuery<TReadAllParams, TModelEntityReadAllResult>, Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>> 
        where TApiEntity : class
    {
        public ReadAllOptionalQueryHandler(IApizrManager<ICrudApi<TApiEntity, TApiEntityKey, TApiEntityReadAllResult, TReadAllParams>> crudApiManager, IMappingHandler mappingHandler) : base(crudApiManager, mappingHandler)
        {
        }

        public override async Task<Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>> Handle(ReadAllOptionalQuery<TReadAllParams, TModelEntityReadAllResult> request, CancellationToken cancellationToken)
        {
            try
            {
                return await request.SomeNotNull(new ApizrException<TModelEntityReadAllResult>(
                        new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                    .MapAsync(_ =>
                        CrudApiManager.ExecuteAsync((ct, api) => api.ReadAll(request.Parameters, ct), cancellationToken,
                            request.Priority))
                    .MapAsync(apiResult =>
                        Task.FromResult(Map<TApiEntityReadAllResult, TModelEntityReadAllResult>(apiResult)))
                    .ConfigureAwait(false);
            }
            catch (ApizrException<TApiEntityReadAllResult> e)
            {
                return Option.None<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>(
                    new ApizrException<TModelEntityReadAllResult>(e.InnerException,
                        Map<TApiEntityReadAllResult, TModelEntityReadAllResult>(e.CachedResult)));
            }
        }
    }

    public class ReadAllOptionalQueryHandler<TApiEntity, TApiEntityKey, TModelEntityReadAllResult, TApiEntityReadAllResult> : 
        ReadAllQueryHandlerBase<TApiEntity, TApiEntityKey, TModelEntityReadAllResult, TApiEntityReadAllResult, ReadAllOptionalQuery<TModelEntityReadAllResult>, Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>> 
        where TApiEntity : class
    {
        public ReadAllOptionalQueryHandler(IApizrManager<ICrudApi<TApiEntity, TApiEntityKey, TApiEntityReadAllResult, IDictionary<string, object>>> crudApiManager, IMappingHandler mappingHandler) : base(crudApiManager, mappingHandler)
        {
        }

        public override async Task<Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>> Handle(ReadAllOptionalQuery<TModelEntityReadAllResult> request, CancellationToken cancellationToken)
        {
            try
            {
                return await request.SomeNotNull(new ApizrException<TModelEntityReadAllResult>(
                        new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                    .MapAsync(_ =>
                        CrudApiManager.ExecuteAsync((ct, api) => api.ReadAll(request.Parameters, ct), cancellationToken,
                            request.Priority))
                    .MapAsync(apiResult =>
                        Task.FromResult(Map<TApiEntityReadAllResult, TModelEntityReadAllResult>(apiResult)))
                    .ConfigureAwait(false);
            }
            catch (ApizrException<TApiEntityReadAllResult> e)
            {
                return Option.None<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>(
                    new ApizrException<TModelEntityReadAllResult>(e.InnerException,
                        Map<TApiEntityReadAllResult, TModelEntityReadAllResult>(e.CachedResult)));
            }
        }
    }
}
