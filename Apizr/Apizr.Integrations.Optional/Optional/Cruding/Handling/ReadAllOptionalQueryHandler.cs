using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Mediation.Cruding.Handling.Base;
using Apizr.Requesting;
using Optional;
using Optional.Async.Extensions;

namespace Apizr.Optional.Cruding.Handling
{
    public class ReadAllOptionalQueryHandler<T, TKey, TReadAllResult, TReadAllParams> : 
        ReadAllQueryHandlerBase<T, TKey, TReadAllResult, TReadAllParams, ReadAllOptionalQuery<TReadAllParams, TReadAllResult>, Option<TReadAllResult, ApizrException<TReadAllResult>>> 
        where T : class
    {

        public ReadAllOptionalQueryHandler(IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>> crudApiManager) : base(crudApiManager)
        {
        }

        public override Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> Handle(ReadAllOptionalQuery<TReadAllParams, TReadAllResult> request, CancellationToken cancellationToken)
        {
            try
            {
                return request.SomeNotNull(new ApizrException<TReadAllResult>(new NullReferenceException($"Request {typeof(ReadAllOptionalQuery<TReadAllParams, TReadAllResult>).GetFriendlyName()} can not be null")))
                    .MapAsync(_ =>
                        CrudApiManager.ExecuteAsync((ct, api) => api.ReadAll(request.Parameters, ct),
                            cancellationToken));
            }
            catch (ApizrException<TReadAllResult> e)
            {
                return Task.FromResult(Option.None<TReadAllResult, ApizrException<TReadAllResult>>(e));
            }
        }
    }

    public class ReadAllOptionalQueryHandler<T, TKey, TReadAllResult> : 
        ReadAllQueryHandlerBase<T, TKey, TReadAllResult, ReadAllOptionalQuery<TReadAllResult>, Option<TReadAllResult, ApizrException<TReadAllResult>>> 
        where T : class
    {

        public ReadAllOptionalQueryHandler(IApizrManager<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>> crudApiManager) : base(crudApiManager)
        {
        }

        public override Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> Handle(ReadAllOptionalQuery<TReadAllResult> request, CancellationToken cancellationToken)
        {
            try
            {
                return request.SomeNotNull(new ApizrException<TReadAllResult>(new NullReferenceException($"Request {typeof(ReadAllOptionalQuery<TReadAllResult>).GetFriendlyName()} can not be null")))
                    .MapAsync(_ =>
                        CrudApiManager.ExecuteAsync((ct, api) => api.ReadAll(request.Parameters, ct),
                            cancellationToken));
            }
            catch (ApizrException<TReadAllResult> e)
            {
                return Task.FromResult(Option.None<TReadAllResult, ApizrException<TReadAllResult>>(e));
            }
        }
    }
}
