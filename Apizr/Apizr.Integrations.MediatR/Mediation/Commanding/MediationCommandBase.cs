using System;
using Apizr.Mediation.Requesting.Base;
using MediatR;
using Polly;

namespace Apizr.Mediation.Commanding
{
    /// <summary>
    /// The top level base mediation command
    /// </summary>
    /// <typeparam name="TModelResultData">The model result type to map to</typeparam>
    /// <typeparam name="TApiRequestData">The api result type to map from</typeparam>
    /// <typeparam name="TApiResultData">The api request type to map to</typeparam>
    /// <typeparam name="TModelRequestData">The model request type to map from</typeparam>
    public abstract class MediationCommandBase<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData> : RequestBase<TModelResultData>, IMediationCommand<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>
    {
        /// <inheritdoc />
        protected MediationCommandBase(Action<Exception> onException = null) : base(onException)
        {

        }

        /// <summary>
        /// The top level base mediation command constructor
        /// </summary>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected MediationCommandBase(Context context, Action<Exception> onException = null) : base(context, onException)
        {

        }
    }

    /// <summary>
    /// The top level base mediation command
    /// </summary>
    /// <typeparam name="TRequestData">The api request type</typeparam>
    /// <typeparam name="TResultData">The api result type</typeparam>
    public abstract class MediationCommandBase<TRequestData, TResultData> : RequestBase<TResultData>, IMediationCommand<TRequestData, TResultData>
    {
        /// <inheritdoc />
        protected MediationCommandBase(Action<Exception> onException = null) : base(onException)
        {

        }

        /// <inheritdoc />
        protected MediationCommandBase(Context context, Action<Exception> onException = null) : base(context, onException)
        {

        }
    }

    /// <summary>
    /// The top level base mediation command
    /// </summary>
    /// <typeparam name="TRequestData">The api request type</typeparam>
    public abstract class MediationCommandBase<TRequestData> : RequestBase<Unit>, IMediationCommand<TRequestData>
    {
        /// <inheritdoc />
        protected MediationCommandBase(Action<Exception> onException = null) : base(onException)
        {

        }

        /// <inheritdoc />
        protected MediationCommandBase(Context context, Action<Exception> onException = null) : base(context, onException)
        {

        }
    }

    /// <summary>
    /// The top level base mediation command
    /// </summary>
    public abstract class MediationCommandBase : RequestBase<Unit>, IMediationCommand
    {
        /// <inheritdoc />
        protected MediationCommandBase(Action<Exception> onException = null) : base(onException)
        {

        }

        /// <inheritdoc />
        protected MediationCommandBase(Context context, Action<Exception> onException = null) : base(context, onException)
        {

        }
    }
}
