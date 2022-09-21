using System.Threading;
using System.Threading.Tasks;
using Apizr.Mediation.Commanding;
using Apizr.Mediation.Cruding.Base;
using Apizr.Requesting;
using MediatR;

namespace Apizr.Mediation.Cruding.Handling.Base
{
    /// <summary>
    /// The base Update command handler
    /// </summary>
    /// <typeparam name="TModelEntity">The model entity type</typeparam>
    /// <typeparam name="TApiEntity">The api entity type</typeparam>
    /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
    /// <typeparam name="TReadAllResult">The returned result type</typeparam>
    /// <typeparam name="TReadAllParams">The read all params type</typeparam>
    /// <typeparam name="TCommand">The command type to handle</typeparam>
    /// <typeparam name="TCommandResult">The command result type to return</typeparam>
    public abstract class UpdateCommandHandlerBase<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams, TCommand, TCommandResult> :
        CrudRequestHandlerBase<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>,
        IMediationCommandHandler<TCommand, TCommandResult>
        where TModelEntity : class
        where TApiEntity : class 
        where TCommand : UpdateCommandBase<TApiEntityKey, TModelEntity, TCommandResult>
    {
        protected UpdateCommandHandlerBase(IApizrManager<ICrudApi<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>> crudApiManager) : base(crudApiManager)
        {
        }

        /// <summary>
        /// Handling the Update command
        /// </summary>
        /// <param name="request">The Update command</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        public abstract Task<TCommandResult> Handle(TCommand request, CancellationToken cancellationToken);
    }

    /// <summary>
    /// The base Update command handler
    /// </summary>
    /// <typeparam name="TModelEntity">The model entity type</typeparam>
    /// <typeparam name="TApiEntity">The api entity type</typeparam>
    /// <typeparam name="TReadAllResult">The returned result type</typeparam>
    /// <typeparam name="TReadAllParams">The read all params type</typeparam>
    /// <typeparam name="TCommand">The command type to handle</typeparam>
    public abstract class UpdateCommandHandlerBase<TModelEntity, TApiEntity, TReadAllResult, TReadAllParams, TCommand> : 
        CrudRequestHandlerBase<TApiEntity, int, TReadAllResult, TReadAllParams>,
        IMediationCommandHandler<TCommand, Unit>
        where TModelEntity : class
        where TApiEntity : class 
        where TCommand : UpdateCommandBase<int, TModelEntity, Unit>
    {
        protected UpdateCommandHandlerBase(IApizrManager<ICrudApi<TApiEntity, int, TReadAllResult, TReadAllParams>> crudApiManager) : base(crudApiManager)
        {
        }

        /// <summary>
        /// Handling the Update command
        /// </summary>
        /// <param name="request">The Update command</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        public abstract Task<Unit> Handle(TCommand request, CancellationToken cancellationToken);
    }

    /// <summary>
    /// The base Update command handler
    /// </summary>
    /// <typeparam name="TModelEntity">The model entity type</typeparam>
    /// <typeparam name="TApiEntity">The api entity type</typeparam>
    /// <typeparam name="TReadAllResult">The returned result type</typeparam>
    /// <typeparam name="TReadAllParams">The read all params type</typeparam>
    /// <typeparam name="TCommand">The command type to handle</typeparam>
    /// <typeparam name="TCommandResult">The command result type to return</typeparam>
    public abstract class UpdateCommandHandlerBase<TModelEntity, TApiEntity, TReadAllResult, TReadAllParams, TCommand, TCommandResult> : 
        CrudRequestHandlerBase<TApiEntity, int, TReadAllResult, TReadAllParams>,
        IMediationCommandHandler<TCommand, TCommandResult>
        where TModelEntity : class
        where TApiEntity : class 
        where TCommand : UpdateCommandBase<int, TModelEntity, TCommandResult>
    {
        protected UpdateCommandHandlerBase(IApizrManager<ICrudApi<TApiEntity, int, TReadAllResult, TReadAllParams>> crudApiManager) : base(crudApiManager)
        {
        }

        /// <summary>
        /// Handling the Update command
        /// </summary>
        /// <param name="request">The Update command</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        public abstract Task<TCommandResult> Handle(TCommand request, CancellationToken cancellationToken);
    }
}
