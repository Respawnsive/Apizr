using System;
using System.Threading.Tasks;
using Apizr.Configuring.Request;

namespace Apizr.Mediation.Cruding.Sending
{
    /// <summary>
    /// Apizr mediator dedicated to cruding
    /// </summary>
    public interface IApizrCrudMediator : IApizrCrudMediatorBase
    {
        #region Create

        /// <summary>
        /// Send a <see cref="CreateCommand{TApiEntity}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<TApiEntity> SendCreateCommand<TApiEntity>(TApiEntity entity, Action<IApizrRequestOptionsBuilder> optionsBuilder = null);
        

        /// <summary>
        /// Send a mapped <see cref="CreateCommand{TModelEntity}"/> to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<TModelEntity> SendCreateCommand<TModelEntity, TApiEntity>(TModelEntity entity, Action<IApizrRequestOptionsBuilder> optionsBuilder = null);

        #endregion

        #region ReadAll

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<TReadAllResult> SendReadAllQuery<TReadAllResult>(Action<IApizrRequestOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelReadAllResult}"/> to Apizr using MediatR returning mapped result
        /// </summary>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <returns></returns>
        Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TApiReadAllResult>(Action<IApizrRequestOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TReadAllResult">The api result type</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<TReadAllResult> SendReadAllQuery<TReadAllResult, TReadAllParams>(TReadAllParams readAllParams, Action<IApizrRequestOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelReadAllResult}"/> to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(TReadAllParams readAllParams, Action<IApizrRequestOptionsBuilder> optionsBuilder = null);
        
        #endregion

        #region Read

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<TApiEntity> SendReadQuery<TApiEntity, TApiEntityKey>(TApiEntityKey key, Action<IApizrRequestOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<TModelEntity> SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key, Action<IApizrRequestOptionsBuilder> optionsBuilder = null);
        
        #endregion

        #region Update

        /// <summary>
        /// Send a <see cref="UpdateCommand{TApiEntityKey, TApiEntity}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task SendUpdateCommand<TApiEntity, TApiEntityKey>(TApiEntityKey key,
            TApiEntity entity, Action<IApizrRequestOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Send a mapped <see cref="UpdateCommand{TApiEntityKey, TModelEntity}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task SendUpdateCommand<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key,
            TModelEntity entity, Action<IApizrRequestOptionsBuilder> optionsBuilder = null);
        
        #endregion

        #region Delete

        /// <summary>
        /// Send a <see cref="DeleteCommand{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task SendDeleteCommand<TApiEntity, TApiEntityKey>(TApiEntityKey key, Action<IApizrRequestOptionsBuilder> optionsBuilder = null);
        
        #endregion
    }

    /// <summary>
    /// Apizr mediator dedicated to <typeparamref name="TApiEntity"/> cruding, getting all shorter
    /// </summary>
    public interface IApizrCrudMediator<TApiEntity, in TApiEntityKey, TReadAllResult, in TReadAllParams> : IApizrCrudMediatorBase
        where TApiEntity : class
    {
        #region Create

        /// <summary>
        /// Send a <see cref="CreateCommand{TApiEntity}"/> to Apizr using MediatR
        /// </summary>
        /// <param name="entity">The entity to create</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<TApiEntity> SendCreateCommand(TApiEntity entity, Action<IApizrRequestOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Send a mapped <see cref="CreateCommand{TModelEntity}"/> to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<TModelEntity> SendCreateCommand<TModelEntity>(TModelEntity entity, Action<IApizrRequestOptionsBuilder> optionsBuilder = null);
        
        #endregion

        #region ReadAll

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR
        /// </summary>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<TReadAllResult> SendReadAllQuery(Action<IApizrRequestOptionsBuilder> optionsBuilder = null);
        
        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR returning mapped result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult">The mapped result</typeparam>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(Action<IApizrRequestOptionsBuilder> optionsBuilder = null);
        
        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR
        /// </summary>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams, Action<IApizrRequestOptionsBuilder> optionsBuilder = null);
        
        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult"></typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams, Action<IApizrRequestOptionsBuilder> optionsBuilder = null);
        
        #endregion

        #region Read

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<TApiEntity> SendReadQuery(TApiEntityKey key, Action<IApizrRequestOptionsBuilder> optionsBuilder = null);
        
        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<TModelEntity> SendReadQuery<TModelEntity>(TApiEntityKey key, Action<IApizrRequestOptionsBuilder> optionsBuilder = null);
        
        #endregion

        #region Update
        
        /// <summary>
        /// Send a <see cref="UpdateCommand{TApiEntityKey, TApiEntity}"/> to Apizr using MediatR
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task SendUpdateCommand(TApiEntityKey key,
            TApiEntity entity, Action<IApizrRequestOptionsBuilder> optionsBuilder = null);
        
        /// <summary>
        /// Send a mapped <see cref="UpdateCommand{TApiEntityKey, TModelEntity}"/> to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task SendUpdateCommand<TModelEntity>(TApiEntityKey key,
            TModelEntity entity, Action<IApizrRequestOptionsBuilder> optionsBuilder = null);
        
        #endregion

        #region Delete

        /// <summary>
        /// Send a <see cref="DeleteCommand{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task SendDeleteCommand(TApiEntityKey key, Action<IApizrRequestOptionsBuilder> optionsBuilder = null);

        #endregion
    }
}