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
    /// The Create optional command handler
    /// </summary>
    /// <typeparam name="TModelEntity">The model entity type</typeparam>
    /// <typeparam name="TApiEntity">The api entity type</typeparam>
    /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
    /// <typeparam name="TReadAllResult">The returned result type</typeparam>
    /// <typeparam name="TReadAllParams">The read all params</typeparam>
    public class CreateOptionalCommandHandler<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> : 
        CreateCommandHandlerBase<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams, CreateOptionalCommand<TModelEntity>, Option<TModelEntity, ApizrException>, IApizrRequestOptions, IApizrRequestOptionsBuilder>
        where TModelEntity : class
        where TApiEntity : class
    {
        public CreateOptionalCommandHandler(IApizrManager<ICrudApi<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>> crudApiManager) : base(crudApiManager)
        {
        }

        public override async Task<Option<TModelEntity, ApizrException>> Handle(CreateOptionalCommand<TModelEntity> request, CancellationToken cancellationToken)
        {
            try
            {
                return await request
                    .SomeNotNull(new ApizrException(
                        new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                    .MapAsync(_ =>
                        CrudApiManager.ExecuteAsync<TModelEntity, TApiEntity>(
                                (options, api, apiData) => api.Create(apiData, options, options.CancellationToken), request.RequestData,
                                request.OptionsBuilder))
                    .ConfigureAwait(false);
            }
            catch (ApizrException e)
            {
                return Option.None<TModelEntity, ApizrException>(e);
            }
        }
    }
}
