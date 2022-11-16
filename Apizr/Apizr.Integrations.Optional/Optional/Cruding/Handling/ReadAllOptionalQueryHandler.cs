using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
using Apizr.Extending;
using Apizr.Mediation.Cruding.Handling.Base;
using Apizr.Requesting;
using Optional;
using Optional.Async.Extensions;

namespace Apizr.Optional.Cruding.Handling
{
    /// <summary>
    /// The ReadAll optional query handler
    /// </summary>
    /// <typeparam name="TApiEntity"></typeparam>
    /// <typeparam name="TApiEntityKey">The api entity type</typeparam>
    /// <typeparam name="TModelEntityReadAllResult">The returned model result type</typeparam>
    /// <typeparam name="TApiEntityReadAllResult">The received api result type</typeparam>
    /// <typeparam name="TReadAllParams">The query parameters type</typeparam>
    public class ReadAllOptionalQueryHandler<TApiEntity, TApiEntityKey, TModelEntityReadAllResult, TApiEntityReadAllResult, TReadAllParams> : 
        ReadAllQueryHandlerBase<TApiEntity, TApiEntityKey, TModelEntityReadAllResult, TApiEntityReadAllResult, TReadAllParams, ReadAllOptionalQuery<TReadAllParams, TModelEntityReadAllResult>, Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>, IApizrResultRequestOptions, IApizrResultRequestOptionsBuilder> 
        where TApiEntity : class
    {
        public ReadAllOptionalQueryHandler(IApizrManager<ICrudApi<TApiEntity, TApiEntityKey, TApiEntityReadAllResult, TReadAllParams>> crudApiManager) : base(crudApiManager)
        {
        }

        public override async Task<Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>> Handle(ReadAllOptionalQuery<TReadAllParams, TModelEntityReadAllResult> request, CancellationToken cancellationToken)
        {
            try
            {
                return await request.SomeNotNull(new ApizrException<TModelEntityReadAllResult>(
                        new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                    .MapAsync(_ =>
                        CrudApiManager.ExecuteAsync<TModelEntityReadAllResult, TApiEntityReadAllResult>(
                            (options, api) => api.ReadAll(request.Parameters,
                                options, options.CancellationToken), (Action<IApizrCatchResultRequestOptionsBuilder>) request.OptionsBuilder))
                    .ConfigureAwait(false);
            }
            catch (ApizrException<TModelEntityReadAllResult> e)
            {
                return Option.None<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>(e);
            }
        }
    }

    /// <summary>
    /// The ReadAll optional query handler
    /// </summary>
    /// <typeparam name="TApiEntity"></typeparam>
    /// <typeparam name="TApiEntityKey">The api entity type</typeparam>
    /// <typeparam name="TModelEntityReadAllResult">The returned model result type</typeparam>
    /// <typeparam name="TApiEntityReadAllResult">The received api result type</typeparam>
    public class ReadAllOptionalQueryHandler<TApiEntity, TApiEntityKey, TModelEntityReadAllResult, TApiEntityReadAllResult> : 
        ReadAllQueryHandlerBase<TApiEntity, TApiEntityKey, TModelEntityReadAllResult, TApiEntityReadAllResult, ReadAllOptionalQuery<TModelEntityReadAllResult>, Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>, IApizrResultRequestOptions, IApizrResultRequestOptionsBuilder> 
        where TApiEntity : class
    {
        public ReadAllOptionalQueryHandler(IApizrManager<ICrudApi<TApiEntity, TApiEntityKey, TApiEntityReadAllResult, IDictionary<string, object>>> crudApiManager) : base(crudApiManager)
        {
        }

        public override async Task<Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>> Handle(ReadAllOptionalQuery<TModelEntityReadAllResult> request, CancellationToken cancellationToken)
        {
            try
            {
                return await request.SomeNotNull(new ApizrException<TModelEntityReadAllResult>(
                        new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                    .MapAsync(_ =>
                        CrudApiManager.ExecuteAsync<TModelEntityReadAllResult, TApiEntityReadAllResult>(
                            (options, api) => api.ReadAll(request.Parameters,
                                options, options.CancellationToken), (Action<IApizrCatchResultRequestOptionsBuilder>) request.OptionsBuilder))
                    .ConfigureAwait(false);
            }
            catch (ApizrException<TModelEntityReadAllResult> e)
            {
                return Option.None<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>(e);
            }
        }
    }
}
