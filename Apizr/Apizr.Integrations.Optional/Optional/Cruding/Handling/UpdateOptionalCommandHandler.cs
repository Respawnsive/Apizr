using System;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Extending;
using Apizr.Mediation.Cruding.Handling.Base;
using Apizr.Requesting;
using MediatR;
using Optional;
using Optional.Async.Extensions;

namespace Apizr.Optional.Cruding.Handling
{
    /// <summary>
    /// The Update optional command handler
    /// </summary>
    /// <typeparam name="TModelEntity">The model entity type</typeparam>
    /// <typeparam name="TApiEntity">The api entity type</typeparam>
    /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
    /// <typeparam name="TReadAllResult">The returned result type</typeparam>
    /// <typeparam name="TReadAllParams">The read all params type</typeparam>
    public class UpdateOptionalCommandHandler<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> : 
        UpdateCommandHandlerBase<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams, UpdateOptionalCommand<TApiEntityKey, TModelEntity>, Option<Unit, ApizrException>>
        where TModelEntity : class
        where TApiEntity : class
    {
        public UpdateOptionalCommandHandler(IApizrManager<ICrudApi<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>> crudApiManager) : base(crudApiManager)
        {
        }

        public override async Task<Option<Unit, ApizrException>> Handle(UpdateOptionalCommand<TApiEntityKey, TModelEntity> request, CancellationToken cancellationToken)
        {
            try
            {
                return await request
                    .SomeNotNull(new ApizrException(
                        new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                    .MapAsync(_ => CrudApiManager
                        .ExecuteAsync<TModelEntity, TApiEntity>(
                            (options, api, apiEntity) =>
                                api.Update(request.Key, apiEntity, options.Context, options.CancellationToken),
                            request.RequestData, request.OptionsBuilder)
                        .ContinueWith(_ => Unit.Value, cancellationToken))
                    .ConfigureAwait(false);
            }
            catch (ApizrException e)
            {
                return Option.None<Unit, ApizrException>(e);
            }
        }
    }

    /// <summary>
    /// The Update optional command handler
    /// </summary>
    /// <typeparam name="TModelEntity">The model entity type</typeparam>
    /// <typeparam name="TApiEntity">The api entity type</typeparam>
    /// <typeparam name="TReadAllResult">The returned result type</typeparam>
    /// <typeparam name="TReadAllParams">The read all params type</typeparam>
    public class UpdateOptionalCommandHandler<TModelEntity, TApiEntity, TReadAllResult, TReadAllParams> : 
        UpdateCommandHandlerBase<TModelEntity, TApiEntity, TReadAllResult, TReadAllParams, UpdateOptionalCommand<TModelEntity>, Option<Unit, ApizrException>>
        where TModelEntity : class
        where TApiEntity : class
    {
        public UpdateOptionalCommandHandler(IApizrManager<ICrudApi<TApiEntity, int, TReadAllResult, TReadAllParams>> crudApiManager) : base(crudApiManager)
        {
        }

        public override async Task<Option<Unit, ApizrException>> Handle(UpdateOptionalCommand<TModelEntity> request, CancellationToken cancellationToken)
        {
            try
            {
                return await request
                    .SomeNotNull(new ApizrException(
                        new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                    .MapAsync(_ => CrudApiManager
                        .ExecuteAsync<TModelEntity, TApiEntity>(
                            (options, api, apiEntity) =>
                                api.Update(request.Key, apiEntity, options.Context, options.CancellationToken),
                            request.RequestData, request.OptionsBuilder)
                        .ContinueWith(_ => Unit.Value, cancellationToken))
                    .ConfigureAwait(false);
            }
            catch (ApizrException e)
            {
                return Option.None<Unit, ApizrException>(e);
            }
        }
    }
}
