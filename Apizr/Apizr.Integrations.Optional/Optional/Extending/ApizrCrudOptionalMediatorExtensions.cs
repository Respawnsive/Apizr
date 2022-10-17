using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Mediation.Cruding;
using Apizr.Optional.Cruding;
using MediatR;
using Optional;
using Polly;

namespace Apizr.Optional.Extending
{
    public static class ApizrCrudOptionalMediatorExtensions
    {
        #region Create

        #region SendCreateOptionalCommand

        /// <summary>
        /// Send a <see cref="CreateOptionalCommand{TModelData}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <param name="entity">The entity to create</param>
        /// <returns></returns>
        Task<Option<TApiEntity, ApizrException>> SendCreateOptionalCommand<TApiEntity>(TApiEntity entity);

        /// <summary>
        /// Send a <see cref="CreateOptionalCommand{TApiEntity}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        Task<Option<TApiEntity, ApizrException>> SendCreateOptionalCommand<TApiEntity>(TApiEntity entity, Context context);

        /// <summary>
        /// Send a <see cref="CreateOptionalCommand{TApiEntity}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TApiEntity, ApizrException>> SendCreateOptionalCommand<TApiEntity>(TApiEntity entity,
            CancellationToken cancellationToken);

        /// <summary>
        /// Send a <see cref="CreateOptionalCommand{TApiEntity}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TApiEntity, ApizrException>> SendCreateOptionalCommand<TApiEntity>(TApiEntity entity, Context context,
            CancellationToken cancellationToken);

        #endregion

        #region SendCreateOptionalCommand<TModelEntity>

        /// <summary>
        /// Send a mapped <see cref="CreateCommand{TModelData}"/> to Apizr using MediatR and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <param name="entity">The entity to create</param>
        /// <returns></returns>
        Task<Option<TModelEntity, ApizrException>> SendCreateOptionalCommand<TModelEntity, TApiEntity>(TModelEntity entity);

        /// <summary>
        /// Send a mapped <see cref="CreateCommand{TModelEntity}"/> to Apizr using MediatR and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        Task<Option<TModelEntity, ApizrException>> SendCreateOptionalCommand<TModelEntity, TApiEntity>(TModelEntity entity, Context context);

        /// <summary>
        /// Send a mapped <see cref="CreateCommand{TModelEntity}"/> to Apizr using MediatR and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TModelEntity, ApizrException>> SendCreateOptionalCommand<TModelEntity, TApiEntity>(TModelEntity entity,
            CancellationToken cancellationToken);

        /// <summary>
        /// Send a mapped <see cref="CreateCommand{TModelEntity}"/> to Apizr using MediatR and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <param name="entity">The entity to create</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TModelEntity, ApizrException>> SendCreateOptionalCommand<TModelEntity, TApiEntity>(TModelEntity entity, Context context,
            CancellationToken cancellationToken);

        #endregion

        #endregion

        #region ReadAll

        #region SendReadAllOptionalQuery

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery<TReadAllResult>(bool clearCache = false);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery<TReadAllResult>(Context context, bool clearCache = false);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery<TReadAllResult>(CancellationToken cancellationToken, bool clearCache = false);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority and returning an optional result
        /// </summary>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery<TReadAllResult>(int priority, bool clearCache = false);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority and returning an optional result
        /// </summary>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery<TReadAllResult>(int priority, Context context, bool clearCache = false);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority and returning an optional result
        /// </summary>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery<TReadAllResult>(int priority,
            CancellationToken cancellationToken, bool clearCache = false);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery<TReadAllResult>(Context context,
            CancellationToken cancellationToken, bool clearCache = false);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority and returning an optional result
        /// </summary>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery<TReadAllResult>(int priority, Context context,
            CancellationToken cancellationToken, bool clearCache = false);

        #endregion

        #region SendReadAllOptionalQuery<TModelReadAllResult>

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR and returning mapped optional result
        /// </summary>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <returns></returns>
        Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult>(bool clearCache = false);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR and returning mapped optional result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult>(Context context, bool clearCache = false);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR and returning mapped optional result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult>(CancellationToken cancellationToken, bool clearCache = false);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR with priority and returning mapped optional result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult>(int priority, bool clearCache = false);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR with priority and returning mapped optional result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult>(int priority, CancellationToken cancellationToken, bool clearCache = false);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR with priority and returning mapped optional result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult>(int priority, Context context, bool clearCache = false);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR and returning mapped optional result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult>(Context context,
                CancellationToken cancellationToken, bool clearCache = false);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR with priority and returning mapped optional result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult>(int priority, Context context,
                CancellationToken cancellationToken, bool clearCache = false);

        #endregion

        #region SendReadAllOptionalQuery(TReadAllParams)

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TReadAllResult">The api result type</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(
            TReadAllParams readAllParams, bool clearCache = false);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TReadAllResult">The api result type</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(
            TReadAllParams readAllParams, Context context, bool clearCache = false);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TReadAllResult">The api result type</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(
            TReadAllParams readAllParams,
            CancellationToken cancellationToken, bool clearCache = false);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority and returning an optional result
        /// </summary>
        /// <typeparam name="TReadAllResult">The api result type</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(
            TReadAllParams readAllParams,
            int priority, bool clearCache = false);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority and returning an optional result
        /// </summary>
        /// <typeparam name="TReadAllResult">The api result type</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(
            TReadAllParams readAllParams,
            int priority, Context context, bool clearCache = false);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority and returning an optional result
        /// </summary>
        /// <typeparam name="TReadAllResult">The api result type</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(
            TReadAllParams readAllParams,
            int priority,
            CancellationToken cancellationToken, bool clearCache = false);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TReadAllResult">The api result type</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(
            TReadAllParams readAllParams, Context context,
            CancellationToken cancellationToken, bool clearCache = false);

        /// <summary>
        /// Send a <see cref="ReadAllQuery{TReadAllResult}"/> to Apizr using MediatR with priority and returning an optional result
        /// </summary>
        /// <typeparam name="TReadAllResult">The api result type</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(
            TReadAllParams readAllParams,
            int priority, Context context,
            CancellationToken cancellationToken, bool clearCache = false);

        #endregion

        #region SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(TReadAllParams)

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(TReadAllParams readAllParams, bool clearCache = false);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(TReadAllParams readAllParams, Context context, bool clearCache = false);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(TReadAllParams readAllParams,
                CancellationToken cancellationToken, bool clearCache = false);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR with priority and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(TReadAllParams readAllParams,
                int priority, bool clearCache = false);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR with priority and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(TReadAllParams readAllParams,
                int priority, Context context, bool clearCache = false);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR with priority and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(TReadAllParams readAllParams,
                int priority,
                CancellationToken cancellationToken, bool clearCache = false);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(TReadAllParams readAllParams, Context context,
                CancellationToken cancellationToken, bool clearCache = false);

        /// <summary>
        /// Send a mapped <see cref="ReadAllQuery{TModelEntityReadAllResult}"/> to Apizr using MediatR with priority and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(TReadAllParams readAllParams,
                int priority, Context context,
                CancellationToken cancellationToken, bool clearCache = false);

        #endregion

        #endregion

        #region Read

        #region SendReadOptionalQuery

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery<TApiEntity, TApiEntityKey>(TApiEntityKey key, bool clearCache = false);

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery<TApiEntity, TApiEntityKey>(TApiEntityKey key, Context context, bool clearCache = false);

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery<TApiEntity, TApiEntityKey>(TApiEntityKey key,
            CancellationToken cancellationToken, bool clearCache = false);

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR with priority and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery<TApiEntity, TApiEntityKey>(TApiEntityKey key,
            int priority, bool clearCache = false);

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery<TApiEntity, TApiEntityKey>(TApiEntityKey key, Context context,
            CancellationToken cancellationToken, bool clearCache = false);

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR with priority and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery<TApiEntity, TApiEntityKey>(TApiEntityKey key,
            int priority, Context context, bool clearCache = false);

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR with priority and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery<TApiEntity, TApiEntityKey>(TApiEntityKey key,
            int priority,
            CancellationToken cancellationToken, bool clearCache = false);

        /// <summary>
        /// Send a <see cref="ReadQuery{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR with priority and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery<TApiEntity, TApiEntityKey>(TApiEntityKey key,
            int priority, Context context,
            CancellationToken cancellationToken, bool clearCache = false);

        #endregion

        #region SendReadOptionalQuery<TModelEntity>

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key, bool clearCache = false);

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key, Context context, bool clearCache = false);

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key,
            CancellationToken cancellationToken, bool clearCache = false);

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR with priority and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key,
            int priority, bool clearCache = false);

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key, Context context,
            CancellationToken cancellationToken, bool clearCache = false);

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR with priority and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key,
            int priority, Context context, bool clearCache = false);

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR with priority and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key,
            int priority,
            CancellationToken cancellationToken, bool clearCache = false);

        /// <summary>
        /// Send a <see cref="ReadQuery{TModelEntity, TApiEntityKey}"/> to Apizr using MediatR with priority and returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="priority">The execution priority</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key,
            int priority, Context context,
            CancellationToken cancellationToken, bool clearCache = false);

        #endregion

        #endregion

        #region Update

        #region SendUpdateOptionalCommand

        /// <summary>
        /// Send a <see cref="UpdateCommand{TApiEntityKey, TApiEntity}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand<TApiEntity, TApiEntityKey>(TApiEntityKey key, TApiEntity entity);

        /// <summary>
        /// Send a <see cref="UpdateCommand{TApiEntityKey, TApiEntity}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand<TApiEntity, TApiEntityKey>(TApiEntityKey key, TApiEntity entity, Context context);

        /// <summary>
        /// Send a <see cref="UpdateCommand{TApiEntityKey, TApiEntity}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand<TApiEntity, TApiEntityKey>(TApiEntityKey key, TApiEntity entity,
            CancellationToken cancellationToken);

        /// <summary>
        /// Send a <see cref="UpdateCommand{TApiEntityKey, TApiEntity}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand<TApiEntity, TApiEntityKey>(TApiEntityKey key, TApiEntity entity, Context context,
            CancellationToken cancellationToken);

        #endregion

        #region SendUpdateOptionalCommand<TModelEntity>

        /// <summary>
        /// Send a mapped <see cref="UpdateCommand{TApiEntityKey, TModelEntity}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key,
            TModelEntity entity);

        /// <summary>
        /// Send a mapped <see cref="UpdateCommand{TApiEntityKey, TModelEntity}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key,
            TModelEntity entity, Context context);

        /// <summary>
        /// Send a mapped <see cref="UpdateCommand{TApiEntityKey, TModelEntity}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key,
            TModelEntity entity, CancellationToken cancellationToken);

        /// <summary>
        /// Send a mapped <see cref="UpdateCommand{TApiEntityKey, TModelEntity}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key,
            TModelEntity entity, Context context,
            CancellationToken cancellationToken);

        #endregion

        #endregion

        #region Delete

        /// <summary>
        /// Send a <see cref="DeleteCommand{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendDeleteOptionalCommand<TApiEntity, TApiEntityKey>(TApiEntityKey key);

        /// <summary>
        /// Send a <see cref="DeleteCommand{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendDeleteOptionalCommand<TApiEntity, TApiEntityKey>(TApiEntityKey key, Context context);

        /// <summary>
        /// Send a <see cref="DeleteCommand{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendDeleteOptionalCommand<TApiEntity, TApiEntityKey>(TApiEntityKey key,
            CancellationToken cancellationToken);

        /// <summary>
        /// Send a <see cref="DeleteCommand{TApiEntity, TApiEntityKey}"/> to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="key">The entity key</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendDeleteOptionalCommand<TApiEntity, TApiEntityKey>(TApiEntityKey key, Context context,
            CancellationToken cancellationToken);

        #endregion
    }
}
