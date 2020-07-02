using System;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Mediation.Cruding.Handling.Base;
using Apizr.Mediation.Querying;
using Apizr.Requesting;
using Optional;
using Optional.Async.Extensions;

namespace Apizr.Optional.Cruding.Handling
{
    public class ReadOptionalQueryHandler<T, TKey, TReadAllResult, TReadAllParams> : 
        ReadQueryHandlerBase<T, TKey, TReadAllResult, TReadAllParams, ReadOptionalQuery<T, TKey>, Option<T, ApizrException<T>>> 
        where T : class
    {
        public ReadOptionalQueryHandler(IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>> crudApiManager) : base(crudApiManager)
        {
        }

        public override Task<Option<T, ApizrException<T>>> Handle(ReadOptionalQuery<T, TKey> request, CancellationToken cancellationToken)
        {
            try
            {
                return request.SomeNotNull(new ApizrException<T>(new NullReferenceException($"Request {typeof(ReadOptionalQuery<T, TKey>).GetFriendlyName()} can not be null")))
                    .MapAsync(_ =>
                        CrudApiManager.ExecuteAsync((ct, api) => api.Read(request.Key, ct), cancellationToken));
            }
            catch (ApizrException<T> e)
            {
                return Task.FromResult(Option.None<T, ApizrException<T>>(e));
            }
        }

    }

    public class ReadOptionalQueryHandler<T, TReadAllResult, TReadAllParams> : 
        ReadQueryHandlerBase<T, TReadAllResult, TReadAllParams, ReadOptionalQuery<T>, Option<T, ApizrException<T>>> 
        where T : class
    {
        public ReadOptionalQueryHandler(IApizrManager<ICrudApi<T, int, TReadAllResult, TReadAllParams>> crudApiManager) : base(crudApiManager)
        {
        }

        public override Task<Option<T, ApizrException<T>>> Handle(ReadOptionalQuery<T> request, CancellationToken cancellationToken)
        {
            try
            {
                return request.SomeNotNull(new ApizrException<T>(new NullReferenceException($"Request {typeof(ReadOptionalQuery<T>).GetFriendlyName()} can not be null")))
                    .MapAsync(_ =>
                        CrudApiManager.ExecuteAsync((ct, api) => api.Read(request.Key, ct), cancellationToken));
            }
            catch (ApizrException<T> e)
            {
                return Task.FromResult(Option.None<T, ApizrException<T>>(e));
            }
        }

    }
}
