using System;
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
    /// The Read optional query handler
    /// </summary>
    /// <typeparam name="TModelEntity">The model entity type</typeparam>
    /// <typeparam name="TApiEntity">The api entity type</typeparam>
    /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
    /// <typeparam name="TReadAllResult">The returned result type</typeparam>
    /// <typeparam name="TReadAllParams">The read all params type</typeparam>
    public class ReadOptionalQueryHandler<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> : 
        ReadQueryHandlerBase<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams, ReadOptionalQuery<TModelEntity, TApiEntityKey>, Option<TModelEntity, ApizrException<TModelEntity>>, IApizrRequestOptions, IApizrRequestOptionsBuilder>
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
                        (options, api) => api.Read(request.Key, options,
                            options.CancellationToken), (Action<IApizrRequestOptionsBuilder>) request.OptionsBuilder))
                    .ConfigureAwait(false);
            }
            catch (ApizrException<TModelEntity> e)
            {
                return Option.None<TModelEntity, ApizrException<TModelEntity>>(e);
            }
        }
    }

    /// <summary>
    /// The Read optional query handler
    /// </summary>
    /// <typeparam name="TModelEntity">The model entity type</typeparam>
    /// <typeparam name="TApiEntity">The api entity type</typeparam>
    /// <typeparam name="TReadAllResult">The returned result type</typeparam>
    /// <typeparam name="TReadAllParams">The read all params type</typeparam>
    public class ReadOptionalQueryHandler<TModelEntity, TApiEntity, TReadAllResult, TReadAllParams> : 
        ReadQueryHandlerBase<TModelEntity, TApiEntity, TReadAllResult, TReadAllParams, ReadOptionalQuery<TModelEntity>, Option<TModelEntity, ApizrException<TModelEntity>>, IApizrRequestOptions, IApizrRequestOptionsBuilder> 
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
                        (options, api) => api.Read(request.Key, options,
                            options.CancellationToken), (Action<IApizrRequestOptionsBuilder>) request.OptionsBuilder))
                    .ConfigureAwait(false);
            }
            catch (ApizrException<TModelEntity> e)
            {
                return Option.None<TModelEntity, ApizrException<TModelEntity>>(e);
            }
        }
    }
}
