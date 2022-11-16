using System;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
using Apizr.Mediation.Cruding;
using MediatR;
using Optional;
using Polly;

namespace Apizr.Optional.Cruding.Sending
{
    /// <summary>
    /// Apizr mediator dedicated to cruding and with optional result
    /// </summary>
    public interface IApizrCrudOptionalMediator : IApizrCrudOptionalMediatorBase
    {
        #region Create
        
        /// <summary>
        /// Send a <see cref="CreateOptionalCommand{TApiEntity}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<Option<TApiEntity, ApizrException>> SendCreateOptionalCommand<TApiEntity>(TApiEntity entity, Action<IApizrResultRequestOptionsBuilder> optionsBuilder = null);
        
        /// <summary>
        /// Send a mapped <see cref="CreateCommand{TModelEntity}"/> to Apizr using MediatR and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<Option<TModelEntity, ApizrException>> SendCreateOptionalCommand<TModelEntity, TApiEntity>(TModelEntity entity, Action<IApizrResultRequestOptionsBuilder> optionsBuilder = null);
        
        #endregion

        #region ReadAll
        
        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery<TReadAllResult>(Action<IApizrResultRequestOptionsBuilder> optionsBuilder = null);
        
        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR and returning mapped optional result
        /// </summary>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <returns></returns>
        Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult>(Action<IApizrResultRequestOptionsBuilder> optionsBuilder = null);
        
        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TReadAllResult">The api result type</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(
            TReadAllParams readAllParams, Action<IApizrResultRequestOptionsBuilder> optionsBuilder = null);
        
        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(TReadAllParams readAllParams, Action<IApizrResultRequestOptionsBuilder> optionsBuilder = null);
        
        #endregion

        #region Read
        
        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery<TApiEntity, TApiEntityKey>(TApiEntityKey key, Action<IApizrResultRequestOptionsBuilder> optionsBuilder = null);
        
        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key, Action<IApizrResultRequestOptionsBuilder> optionsBuilder = null);
        
        #endregion

        #region Update
        
        /// <summary>
        /// Send a <see cref="UpdateCommand{TApiEntityKey, TApiEntity}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand<TApiEntity, TApiEntityKey>(TApiEntityKey key, TApiEntity entity, Action<IApizrUnitRequestOptionsBuilder> optionsBuilder = null);
        
        /// <summary>
        /// Send a mapped <see cref="UpdateCommand{TApiEntityKey, TModelEntity}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key,
            TModelEntity entity, Action<IApizrUnitRequestOptionsBuilder> optionsBuilder = null);
        
        #endregion

        #region Delete

        /// <summary>
        /// Send a <see cref="DeleteCommand{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendDeleteOptionalCommand<TApiEntity, TApiEntityKey>(TApiEntityKey key, Action<IApizrUnitRequestOptionsBuilder> optionsBuilder = null);
        
        #endregion
    }

    /// <summary>
    /// <see cref="IApizrCrudOptionalMediator"/> but dedicated to <typeparamref name="TApiEntity"/> cruding with optional result, getting all shorter
    /// </summary>
    public interface IApizrCrudOptionalMediator<TApiEntity, in TApiEntityKey, TReadAllResult, in TReadAllParams> : IApizrCrudOptionalMediatorBase
        where TApiEntity : class
    {
        #region Create
        
        /// <summary>
        /// Send a <see cref="CreateOptionalCommand{TApiEntity}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <param name="payload">The entity to create</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<Option<TApiEntity, ApizrException>> SendCreateOptionalCommand(TApiEntity payload, Action<IApizrResultRequestOptionsBuilder> optionsBuilder = null);
        
        /// <summary>
        /// Send a mapped <see cref="CreateCommand{TModelEntity}"/> to Apizr using MediatR and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="payload">The entity to create</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<Option<TModelEntity, ApizrException>> SendCreateOptionalCommand<TModelEntity>(TModelEntity payload, Action<IApizrResultRequestOptionsBuilder> optionsBuilder = null);
        
        #endregion

        #region ReadAll
        
        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(Action<IApizrResultRequestOptionsBuilder> optionsBuilder = null);
        
        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR and returning mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult">The mapped result</typeparam>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>>
            SendReadAllOptionalQuery<TModelEntityReadAllResult>(Action<IApizrResultRequestOptionsBuilder> optionsBuilder = null);
        
        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(
            TReadAllParams readAllParams, Action<IApizrResultRequestOptionsBuilder> optionsBuilder = null);
        
        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntityReadAllResult"></typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>>
            SendReadAllOptionalQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams, Action<IApizrResultRequestOptionsBuilder> optionsBuilder = null);
        
        #endregion

        #region Read
        
        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery(TApiEntityKey key, Action<IApizrResultRequestOptionsBuilder> optionsBuilder = null);
        
        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity>(TApiEntityKey key, Action<IApizrResultRequestOptionsBuilder> optionsBuilder = null);
        
        #endregion

        #region Update
        
        /// <summary>
        /// Send a <see cref="UpdateCommand{TApiEntityKey, TApiEntity}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="payload">The entity to update</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand(TApiEntityKey key, TApiEntity payload, Action<IApizrUnitRequestOptionsBuilder> optionsBuilder = null);
        
        /// <summary>
        /// Send a mapped <see cref="UpdateCommand{TApiEntityKey, TModelEntity}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TModelEntity"></typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="payload">The entity to update</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand<TModelEntity>(TApiEntityKey key,
            TModelEntity payload, Action<IApizrUnitRequestOptionsBuilder> optionsBuilder = null);
        
        #endregion

        #region Delete

        /// <summary>
        /// Send a <see cref="DeleteCommand{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <param name="key">The entity key</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendDeleteOptionalCommand(TApiEntityKey key, Action<IApizrUnitRequestOptionsBuilder> optionsBuilder = null);
        
        #endregion
    }
}