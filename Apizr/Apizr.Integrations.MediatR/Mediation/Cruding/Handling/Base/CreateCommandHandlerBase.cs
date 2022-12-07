using System.Threading;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
using Apizr.Mediation.Commanding;
using Apizr.Mediation.Cruding.Base;
using Apizr.Requesting;

namespace Apizr.Mediation.Cruding.Handling.Base
{
    /// <summary>
    /// The base Create command handler
    /// </summary>
    /// <typeparam name="TModelEntity">The model entity type</typeparam>
    /// <typeparam name="TApiEntity">The api entity type</typeparam>
    /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
    /// <typeparam name="TReadAllResult">The returned result type</typeparam>
    /// <typeparam name="TReadAllParams">The read all params</typeparam>
    /// <typeparam name="TCommand">The command to handle</typeparam>
    /// <typeparam name="TCommandResult">The command result to return</typeparam>
    public abstract class CreateCommandHandlerBase<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult,
        TReadAllParams, TCommand, TCommandResult, TApizrRequestOptions, TApizrRequestOptionsBuilder> :
        CrudRequestHandlerBase<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams, TApizrRequestOptions, TApizrRequestOptionsBuilder>,
        IMediationCommandHandler<TCommand, TCommandResult> 
        where TModelEntity : class
        where TApiEntity : class
        where TCommand : CreateCommandBase<TModelEntity, TCommandResult, TApizrRequestOptions, TApizrRequestOptionsBuilder>
        where TApizrRequestOptions : IApizrRequestOptions
        where TApizrRequestOptionsBuilder : IApizrRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        protected CreateCommandHandlerBase(
            IApizrManager<ICrudApi<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>> crudApiManager) : base(crudApiManager)
        {
        }

        /// <summary>
        /// Handling the Create command
        /// </summary>
        /// <param name="request">The Create command</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        public abstract Task<TCommandResult> Handle(TCommand request, CancellationToken cancellationToken);
    }
}
