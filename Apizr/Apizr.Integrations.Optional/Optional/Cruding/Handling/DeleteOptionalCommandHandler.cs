using System;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
using Apizr.Extending;
using Apizr.Mapping;
using Apizr.Mediation.Cruding.Handling.Base;
using Apizr.Requesting;
using MediatR;
using Optional;
using Optional.Async.Extensions;

namespace Apizr.Optional.Cruding.Handling
{
    /// <summary>
    /// The Delete optional command handler
    /// </summary>
    /// <typeparam name="TModelEntity">The model entity type</typeparam>
    /// <typeparam name="TApiEntity">The api entity type</typeparam>
    /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
    /// <typeparam name="TReadAllResult">The returned result type</typeparam>
    /// <typeparam name="TReadAllParams">The read all params type</typeparam>
    public class DeleteOptionalCommandHandler<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> : 
        DeleteCommandHandlerBase<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams, DeleteOptionalCommand<TModelEntity, TApiEntityKey>, Option<Unit, ApizrException>, IApizrRequestOptions, IApizrRequestOptionsBuilder>
        where TModelEntity : class
        where TApiEntity : class
    {
        public DeleteOptionalCommandHandler(IApizrManager<ICrudApi<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>> crudApiManager) : base(crudApiManager)
        {
        }

        public override async Task<Option<Unit, ApizrException>> Handle(DeleteOptionalCommand<TModelEntity, TApiEntityKey> request, CancellationToken cancellationToken)
        {
            try
            {
                return await request
                    .SomeNotNull(new ApizrException(
                        new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                    .MapAsync(_ => CrudApiManager.ExecuteAsync(
                            (options, api) => api.Delete(request.Key, options.Context, options.CancellationToken),
                            (Action<IApizrRequestOptionsBuilder>) request.OptionsBuilder).ContinueWith(_ => Unit.Value, cancellationToken))
                    .ConfigureAwait(false);
            }
            catch (ApizrException e)
            {
                return Option.None<Unit, ApizrException>(e);
            }
        }
    }

    /// <summary>
    /// The Delete optional command handler
    /// </summary>
    /// <typeparam name="TModelEntity">The model entity type</typeparam>
    /// <typeparam name="TApiEntity">The api entity type</typeparam>
    /// <typeparam name="TReadAllResult">The returned result type</typeparam>
    /// <typeparam name="TReadAllParams">The read all params type</typeparam>
    public class DeleteOptionalCommandHandler<TModelEntity, TApiEntity, TReadAllResult, TReadAllParams> : 
        DeleteCommandHandlerBase<TModelEntity, TApiEntity, TReadAllResult, TReadAllParams, DeleteOptionalCommand<TModelEntity>, Option<Unit, ApizrException>, IApizrRequestOptions, IApizrRequestOptionsBuilder>
        where TModelEntity : class
        where TApiEntity : class
    {
        public DeleteOptionalCommandHandler(IApizrManager<ICrudApi<TApiEntity, int, TReadAllResult, TReadAllParams>> crudApiManager) : base(crudApiManager)
        {
        }

        public override async Task<Option<Unit, ApizrException>> Handle(DeleteOptionalCommand<TModelEntity> request, CancellationToken cancellationToken)
        {
            try
            {
                return await request
                    .SomeNotNull(new ApizrException(
                        new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                    .MapAsync(_ => CrudApiManager.ExecuteAsync(
                            (options, api) => api.Delete(request.Key, options.Context, options.CancellationToken),
                            (Action<IApizrRequestOptionsBuilder>) request.OptionsBuilder).ContinueWith(_ => Unit.Value, cancellationToken))
                    .ConfigureAwait(false);
            }
            catch (ApizrException e)
            {
                return Option.None<Unit, ApizrException>(e);
            }
        }
    }
}
