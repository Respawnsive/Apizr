using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
using MediatR;
using Refit;

namespace Apizr.Mediation.Requesting.Sending
{
    /// <summary>
    /// Apizr mediator to send request using MediatR by calling expression
    /// </summary>
    public class ApizrMediator : ApizrMediatorBase, IApizrMediator, IApizrInternalMediator
    {
        private readonly IMediator _mediator;

        public ApizrMediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Unit

        #region SendFor<TWebApi>

        /// <inheritdoc />
        public Task SendFor<TWebApi>(Expression<Func<TWebApi, Task>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _mediator.Send(new ExecuteUnitRequest<TWebApi>(executeApiMethod, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        /// <inheritdoc />
        public Task<IApizrResponse> SendFor<TWebApi>(Expression<Func<TWebApi, Task<IApiResponse>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _mediator.Send(new ExecuteSafeUnitRequest<TWebApi>(executeApiMethod, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        /// <inheritdoc />
        public Task SendFor<TWebApi>(Expression<Func<IApizrRequestOptions, TWebApi, Task>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _mediator.Send(new ExecuteUnitRequest<TWebApi>(executeApiMethod, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        /// <inheritdoc />
        public Task<IApizrResponse> SendFor<TWebApi>(
            Expression<Func<IApizrRequestOptions, TWebApi, Task<IApiResponse>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _mediator.Send(new ExecuteSafeUnitRequest<TWebApi>(executeApiMethod, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        #endregion

        #region SendFor<TWebApi, TModelData, TApiData>

        /// <inheritdoc />
        public Task SendFor<TWebApi, TModelData, TApiData>(Expression<Func<TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _mediator.Send(
                new ExecuteUnitRequest<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        /// <inheritdoc />
        public Task<IApizrResponse> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<TWebApi, TApiData, Task<IApiResponse>>> executeApiMethod, TModelData modelData,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _mediator.Send(
                new ExecuteSafeUnitRequest<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        /// <inheritdoc />
        public Task SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _mediator.Send(
                new ExecuteUnitRequest<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        /// <inheritdoc />
        public Task<IApizrResponse> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, TApiData, Task<IApiResponse>>> executeApiMethod,
            TModelData modelData,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _mediator.Send(
                new ExecuteSafeUnitRequest<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        #endregion

        #endregion

        #region Result

        #region SendFor<TWebApi, TApiData>

        /// <inheritdoc />
        public Task<TApiData> SendFor<TWebApi, TApiData>(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _mediator.Send(new ExecuteResultRequest<TWebApi, TApiData>(executeApiMethod, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        /// <inheritdoc />
        public Task<IApizrResponse<TApiData>> SendFor<TWebApi, TApiData>(
            Expression<Func<TWebApi, Task<ApiResponse<TApiData>>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => SendFor<TWebApi, TApiData>((_, api) => executeApiMethod.Compile().Invoke(api)
                    .ContinueWith(task => (IApiResponse<TApiData>) task.Result),
                optionsBuilder.WithOriginalExpression(executeApiMethod));

        /// <inheritdoc />
        public Task<IApizrResponse<TApiData>> SendFor<TWebApi, TApiData>(
            Expression<Func<TWebApi, Task<IApiResponse<TApiData>>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => SendFor<TWebApi, TApiData>((_, api) => executeApiMethod.Compile().Invoke(api),
                optionsBuilder.WithOriginalExpression(executeApiMethod));

        /// <inheritdoc />
        public Task<TApiData> SendFor<TWebApi, TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, Task<TApiData>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _mediator.Send(new ExecuteResultRequest<TWebApi, TApiData>(executeApiMethod, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        /// <inheritdoc />
        public Task<IApizrResponse<TApiData>> SendFor<TWebApi, TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, Task<ApiResponse<TApiData>>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => SendFor<TWebApi, TApiData>(
                (opt, api) => executeApiMethod.Compile().Invoke(opt, api)
                    .ContinueWith(task => (IApiResponse<TApiData>) task.Result),
                optionsBuilder.WithOriginalExpression(executeApiMethod));

        /// <inheritdoc />
        public Task<IApizrResponse<TApiData>> SendFor<TWebApi, TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, Task<IApiResponse<TApiData>>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _mediator.Send(new ExecuteSafeResultRequest<TWebApi, TApiData>(executeApiMethod, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        #endregion

        #region SendFor<TWebApi, TModelData, TApiData>

        /// <inheritdoc />
        public Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _mediator.Send(new ExecuteResultRequest<TWebApi, TModelData, TApiData>(executeApiMethod, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        /// <inheritdoc />
        public Task<IApizrResponse<TModelData>> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<TWebApi, Task<ApiResponse<TApiData>>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => SendFor<TWebApi, TModelData, TApiData>(
                (_, api) => executeApiMethod.Compile().Invoke(api)
                    .ContinueWith(task => (IApiResponse<TApiData>) task.Result),
                optionsBuilder.WithOriginalExpression(executeApiMethod));

        /// <inheritdoc />
        public Task<IApizrResponse<TModelData>> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<TWebApi, Task<IApiResponse<TApiData>>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => SendFor<TWebApi, TModelData, TApiData>(
                (_, api) => executeApiMethod.Compile().Invoke(api),
                optionsBuilder.WithOriginalExpression(executeApiMethod));

        /// <inheritdoc />
        public Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, Task<TApiData>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _mediator.Send(new ExecuteResultRequest<TWebApi, TModelData, TApiData>(executeApiMethod, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        /// <inheritdoc />
        public Task<IApizrResponse<TModelData>> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, Task<ApiResponse<TApiData>>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => SendFor<TWebApi, TModelData, TApiData>(
                (opt, api) => executeApiMethod.Compile().Invoke(opt, api)
                    .ContinueWith(task => (IApiResponse<TApiData>) task.Result),
                optionsBuilder.WithOriginalExpression(executeApiMethod));

        /// <inheritdoc />
        public Task<IApizrResponse<TModelData>> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, Task<IApiResponse<TApiData>>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _mediator.Send(
                new ExecuteSafeResultRequest<TWebApi, TModelData, TApiData>(executeApiMethod, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        /// <inheritdoc />
        public Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _mediator.Send(
                new ExecuteResultRequest<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        /// <inheritdoc />
        public Task<IApizrResponse<TModelData>> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<TWebApi, TApiData, Task<ApiResponse<TApiData>>>> executeApiMethod, TModelData modelData,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => SendFor<TWebApi, TModelData, TApiData>(
                (_, api, apiData) => executeApiMethod.Compile().Invoke(api, apiData)
                    .ContinueWith(task => (IApiResponse<TApiData>) task.Result), modelData,
                optionsBuilder.WithOriginalExpression(executeApiMethod));

        /// <inheritdoc />
        public Task<IApizrResponse<TModelData>> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<TWebApi, TApiData, Task<IApiResponse<TApiData>>>> executeApiMethod, TModelData modelData,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => SendFor<TWebApi, TModelData, TApiData>(
                (_, api, apiData) => executeApiMethod.Compile().Invoke(api, apiData), modelData,
                optionsBuilder.WithOriginalExpression(executeApiMethod));

        /// <inheritdoc />
        public Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _mediator.Send(
                new ExecuteResultRequest<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        /// <inheritdoc />
        public Task<IApizrResponse<TModelData>> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, TApiData, Task<ApiResponse<TApiData>>>> executeApiMethod,
            TModelData modelData,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => SendFor<TWebApi, TModelData, TApiData>(
                (opt, api, apiData) => executeApiMethod.Compile().Invoke(opt, api, apiData)
                    .ContinueWith(task => (IApiResponse<TApiData>) task.Result), modelData,
                optionsBuilder.WithOriginalExpression(executeApiMethod));

        /// <inheritdoc />
        public Task<IApizrResponse<TModelData>> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, TApiData, Task<IApiResponse<TApiData>>>> executeApiMethod,
            TModelData modelData,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _mediator.Send(
                new ExecuteSafeResultRequest<TWebApi, TModelData, TApiData>(executeApiMethod, modelData,
                    optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        #endregion

        #region SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>

        /// <inheritdoc />
        public Task<TModelResultData> SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
            TModelRequestData>(Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _mediator.Send(
                new ExecuteResultRequest<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                    executeApiMethod, modelRequestData, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        /// <inheritdoc />
        public Task<IApizrResponse<TModelResultData>> SendFor<TWebApi, TModelResultData, TApiResultData,
            TApiRequestData, TModelRequestData>(
            Expression<Func<TWebApi, TApiRequestData, Task<ApiResponse<TApiResultData>>>> executeApiMethod,
            TModelRequestData modelRequestData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                (_, api, apiData) => executeApiMethod.Compile().Invoke(api, apiData)
                    .ContinueWith(task => (IApiResponse<TApiResultData>) task.Result), modelRequestData,
                optionsBuilder.WithOriginalExpression(executeApiMethod));

        /// <inheritdoc />
        public Task<IApizrResponse<TModelResultData>> SendFor<TWebApi, TModelResultData, TApiResultData,
            TApiRequestData, TModelRequestData>(
            Expression<Func<TWebApi, TApiRequestData, Task<IApiResponse<TApiResultData>>>> executeApiMethod,
            TModelRequestData modelRequestData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                (_, api, apiData) => executeApiMethod.Compile().Invoke(api, apiData), modelRequestData,
                optionsBuilder.WithOriginalExpression(executeApiMethod));

        /// <inheritdoc />
        public Task<TModelResultData> SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
            TModelRequestData>(
            Expression<Func<IApizrRequestOptions, TWebApi, TApiRequestData, Task<TApiResultData>>>
                executeApiMethod,
            TModelRequestData modelRequestData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _mediator.Send(
                new ExecuteResultRequest<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                    executeApiMethod, modelRequestData, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        /// <inheritdoc />
        public Task<IApizrResponse<TModelResultData>> SendFor<TWebApi, TModelResultData, TApiResultData,
            TApiRequestData, TModelRequestData>(
            Expression<Func<IApizrRequestOptions, TWebApi, TApiRequestData, Task<ApiResponse<TApiResultData>>>>
                executeApiMethod,
            TModelRequestData modelRequestData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                (opt, api, apiData) => executeApiMethod.Compile().Invoke(opt, api, apiData)
                    .ContinueWith(task => (IApiResponse<TApiResultData>) task.Result), modelRequestData,
                optionsBuilder.WithOriginalExpression(executeApiMethod));

        /// <inheritdoc />
        public Task<IApizrResponse<TModelResultData>> SendFor<TWebApi, TModelResultData, TApiResultData,
            TApiRequestData, TModelRequestData>(
            Expression<Func<IApizrRequestOptions, TWebApi, TApiRequestData, Task<IApiResponse<TApiResultData>>>>
                executeApiMethod,
            TModelRequestData modelRequestData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _mediator.Send(
                new ExecuteSafeResultRequest<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
                    TModelRequestData>(
                    executeApiMethod, modelRequestData, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        #endregion

        #endregion

        #region Internal

        /// <inheritdoc />
        Task<TResponse> IApizrInternalMediator.Send<TResponse>(IRequest<TResponse> request,
            CancellationToken cancellationToken)
            => _mediator.Send(request, cancellationToken);

        #endregion
    }

    /// <summary>
    /// Apizr mediator to send request using MediatR by calling expression
    /// </summary>
    public class ApizrMediator<TWebApi> : IApizrMediator<TWebApi>
    {
        private readonly IApizrMediator _apizrMediator;

        public ApizrMediator(IApizrMediator apizrMediator)
        {
            _apizrMediator = apizrMediator;
        }

        #region Unit

        #region SendFor

        /// <inheritdoc />
        public Task SendFor(Expression<Func<TWebApi, Task>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor(executeApiMethod, optionsBuilder);

        /// <inheritdoc />
        public Task<IApizrResponse> SendFor(Expression<Func<TWebApi, Task<IApiResponse>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor(executeApiMethod, optionsBuilder);

        /// <inheritdoc />
        public Task SendFor(Expression<Func<IApizrRequestOptions, TWebApi, Task>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor(executeApiMethod, optionsBuilder);

        /// <inheritdoc />
        public Task<IApizrResponse> SendFor(
            Expression<Func<IApizrRequestOptions, TWebApi, Task<IApiResponse>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor(executeApiMethod, optionsBuilder);

        #endregion

        #region SendFor<TModelData, TApiData>

        /// <inheritdoc />
        public Task SendFor<TModelData, TApiData>(Expression<Func<TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor(executeApiMethod, modelData, optionsBuilder);

        /// <inheritdoc />
        public Task<IApizrResponse> SendFor<TModelData, TApiData>(
            Expression<Func<TWebApi, TApiData, Task<IApiResponse>>> executeApiMethod, TModelData modelData,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor(executeApiMethod, modelData, optionsBuilder);

        /// <inheritdoc />
        public Task SendFor<TModelData, TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor(executeApiMethod, modelData, optionsBuilder);

        /// <inheritdoc />
        public Task<IApizrResponse> SendFor<TModelData, TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, TApiData, Task<IApiResponse>>> executeApiMethod,
            TModelData modelData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor(executeApiMethod, modelData, optionsBuilder);

        #endregion

        #endregion

        #region Result

        #region SendFor<TApiData>

        /// <inheritdoc />
        public Task<TApiData> SendFor<TApiData>(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor(executeApiMethod, optionsBuilder);

        /// <inheritdoc />
        public Task<IApizrResponse<TApiData>> SendFor<TApiData>(
            Expression<Func<TWebApi, Task<ApiResponse<TApiData>>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor(executeApiMethod, optionsBuilder);

        /// <inheritdoc />
        public Task<IApizrResponse<TApiData>> SendFor<TApiData>(
            Expression<Func<TWebApi, Task<IApiResponse<TApiData>>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor(executeApiMethod, optionsBuilder);

        /// <inheritdoc />
        public Task<TApiData> SendFor<TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, Task<TApiData>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor(executeApiMethod, optionsBuilder);

        /// <inheritdoc />
        public Task<IApizrResponse<TApiData>> SendFor<TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, Task<ApiResponse<TApiData>>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor(executeApiMethod, optionsBuilder);

        /// <inheritdoc />
        public Task<IApizrResponse<TApiData>> SendFor<TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, Task<IApiResponse<TApiData>>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor(executeApiMethod, optionsBuilder);

        #endregion

        #region SendFor<TModelData, TApiData>

        /// <inheritdoc />
        public Task<TModelData> SendFor<TModelData, TApiData>(
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, optionsBuilder);

        /// <inheritdoc />
        public Task<IApizrResponse<TModelData>> SendFor<TModelData, TApiData>(
            Expression<Func<TWebApi, Task<ApiResponse<TApiData>>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, optionsBuilder);

        /// <inheritdoc />
        public Task<IApizrResponse<TModelData>> SendFor<TModelData, TApiData>(
            Expression<Func<TWebApi, Task<IApiResponse<TApiData>>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, optionsBuilder);

        /// <inheritdoc />
        public Task<TModelData> SendFor<TModelData, TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, Task<TApiData>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, optionsBuilder);

        /// <inheritdoc />
        public Task<IApizrResponse<TModelData>> SendFor<TModelData, TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, Task<ApiResponse<TApiData>>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, optionsBuilder);

        /// <inheritdoc />
        public Task<IApizrResponse<TModelData>> SendFor<TModelData, TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, Task<IApiResponse<TApiData>>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, optionsBuilder);

        /// <inheritdoc />
        public Task<TModelData> SendFor<TModelData, TApiData>(
            Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor(executeApiMethod, modelData, optionsBuilder);

        /// <inheritdoc />
        public Task<IApizrResponse<TModelData>> SendFor<TModelData, TApiData>(
            Expression<Func<TWebApi, TApiData, Task<ApiResponse<TApiData>>>> executeApiMethod, TModelData modelData,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor(executeApiMethod, modelData, optionsBuilder);

        /// <inheritdoc />
        public Task<IApizrResponse<TModelData>> SendFor<TModelData, TApiData>(
            Expression<Func<TWebApi, TApiData, Task<IApiResponse<TApiData>>>> executeApiMethod, TModelData modelData,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor(executeApiMethod, modelData, optionsBuilder);

        /// <inheritdoc />
        public Task<TModelData> SendFor<TModelData, TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor(executeApiMethod, modelData, optionsBuilder);

        /// <inheritdoc />
        public Task<IApizrResponse<TModelData>> SendFor<TModelData, TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, TApiData, Task<ApiResponse<TApiData>>>> executeApiMethod,
            TModelData modelData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor(executeApiMethod, modelData, optionsBuilder);

        /// <inheritdoc />
        public Task<IApizrResponse<TModelData>> SendFor<TModelData, TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, TApiData, Task<IApiResponse<TApiData>>>> executeApiMethod,
            TModelData modelData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor(executeApiMethod, modelData, optionsBuilder);

        #endregion

        #region SendFor<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>

        /// <inheritdoc />
        public Task<TModelResultData> SendFor<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                executeApiMethod, modelRequestData, optionsBuilder);

        /// <inheritdoc />
        public Task<IApizrResponse<TModelResultData>> SendFor<TModelResultData, TApiResultData, TApiRequestData,
            TModelRequestData>(
            Expression<Func<TWebApi, TApiRequestData, Task<ApiResponse<TApiResultData>>>> executeApiMethod,
            TModelRequestData modelRequestData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                executeApiMethod, modelRequestData, optionsBuilder);

        /// <inheritdoc />
        public Task<IApizrResponse<TModelResultData>> SendFor<TModelResultData, TApiResultData, TApiRequestData,
            TModelRequestData>(
            Expression<Func<TWebApi, TApiRequestData, Task<IApiResponse<TApiResultData>>>> executeApiMethod,
            TModelRequestData modelRequestData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                executeApiMethod, modelRequestData, optionsBuilder);

        /// <inheritdoc />
        public Task<TModelResultData> SendFor<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<IApizrRequestOptions, TWebApi, TApiRequestData, Task<TApiResultData>>>
                executeApiMethod,
            TModelRequestData modelRequestData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                executeApiMethod, modelRequestData, optionsBuilder);

        /// <inheritdoc />
        public Task<IApizrResponse<TModelResultData>> SendFor<TModelResultData, TApiResultData, TApiRequestData,
            TModelRequestData>(
            Expression<Func<IApizrRequestOptions, TWebApi, TApiRequestData, Task<ApiResponse<TApiResultData>>>>
                executeApiMethod,
            TModelRequestData modelRequestData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                executeApiMethod, modelRequestData, optionsBuilder);

        /// <inheritdoc />
        public Task<IApizrResponse<TModelResultData>> SendFor<TModelResultData, TApiResultData, TApiRequestData,
            TModelRequestData>(
            Expression<Func<IApizrRequestOptions, TWebApi, TApiRequestData, Task<IApiResponse<TApiResultData>>>>
                executeApiMethod,
            TModelRequestData modelRequestData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                executeApiMethod, modelRequestData, optionsBuilder);

        #endregion

        #endregion
    }
}