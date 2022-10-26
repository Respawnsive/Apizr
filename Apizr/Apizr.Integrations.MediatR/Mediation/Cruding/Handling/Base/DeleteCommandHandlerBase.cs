using System.Threading;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
using Apizr.Mediation.Commanding;
using Apizr.Mediation.Cruding.Base;
using Apizr.Requesting;

namespace Apizr.Mediation.Cruding.Handling.Base
{
    /// <summary>
    /// The base Delete command handler
    /// </summary>
    /// <typeparam name="TModelEntity">The model entity type</typeparam>
    /// <typeparam name="TApiEntity">The api entity type</typeparam>
    /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
    /// <typeparam name="TReadAllResult">The returned result type</typeparam>
    /// <typeparam name="TReadAllParams">The read all params type</typeparam>
    /// <typeparam name="TCommand">The command type to handle</typeparam>
    /// <typeparam name="TCommandResult">The command result type to return</typeparam>
    public abstract class DeleteCommandHandlerBase<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams, TCommand, TCommandResult, TApizrRequestOptions, TApizrRequestOptionsBuilder> :
        CrudRequestHandlerBase<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams, TApizrRequestOptions, TApizrRequestOptionsBuilder>,
        IMediationCommandHandler<TCommand, TCommandResult>
        where TModelEntity : class
        where TApiEntity : class 
        where TCommand : DeleteCommandBase<TModelEntity, TApiEntityKey, TCommandResult, TApizrRequestOptions, TApizrRequestOptionsBuilder>
        where TApizrRequestOptions : IApizrUnitRequestOptions
        where TApizrRequestOptionsBuilder : IApizrUnitRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        protected DeleteCommandHandlerBase(IApizrManager<ICrudApi<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>> crudApiManager) : base(crudApiManager)
        {
        }

        /// <summary>
        /// Handling the Delete command
        /// </summary>
        /// <param name="request">The Delete command</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        public abstract Task<TCommandResult> Handle(TCommand request, CancellationToken cancellationToken);
    }

    /// <summary>
    /// The base Delete command handler
    /// </summary>
    /// <typeparam name="TModelEntity">The model entity type</typeparam>
    /// <typeparam name="TApiEntity">The api entity type</typeparam>
    /// <typeparam name="TReadAllResult">The returned result type</typeparam>
    /// <typeparam name="TReadAllParams">The read all params</typeparam>
    /// <typeparam name="TCommand">The command to handle</typeparam>
    /// <typeparam name="TCommandResult">The command result to return</typeparam>
    public abstract class DeleteCommandHandlerBase<TModelEntity, TApiEntity, TReadAllResult, TReadAllParams, TCommand, TCommandResult, TApizrRequestOptions, TApizrRequestOptionsBuilder> :
        CrudRequestHandlerBase<TApiEntity, int, TReadAllResult, TReadAllParams, TApizrRequestOptions, TApizrRequestOptionsBuilder>,
        IMediationCommandHandler<TCommand, TCommandResult>
        where TModelEntity : class
        where TApiEntity : class
        where TCommand : DeleteCommandBase<TModelEntity, int, TCommandResult, TApizrRequestOptions, TApizrRequestOptionsBuilder>
        where TApizrRequestOptions : IApizrUnitRequestOptions
        where TApizrRequestOptionsBuilder : IApizrUnitRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        protected DeleteCommandHandlerBase(IApizrManager<ICrudApi<TApiEntity, int, TReadAllResult, TReadAllParams>> crudApiManager) : base(crudApiManager)
        {
        }

        /// <summary>
        /// Handling the Delete command
        /// </summary>
        /// <param name="request">The Delete command</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        public abstract Task<TCommandResult> Handle(TCommand request, CancellationToken cancellationToken);
    }
}
