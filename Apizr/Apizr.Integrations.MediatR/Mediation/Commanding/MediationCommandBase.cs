using System;
using Apizr.Configuring.Request;
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
        protected MediationCommandBase(Context context) : base(context)
        {

        }

        /// <inheritdoc />
        protected MediationCommandBase(Action<Exception> onException) : base(onException)
        {

        }

        /// <inheritdoc />
        protected MediationCommandBase(Context context, Action<Exception> onException) : base(context, onException)
        {

        }

        /// <inheritdoc />
        protected MediationCommandBase(Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(optionsBuilder)
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
        protected MediationCommandBase(Context context) : base(context)
        {

        }

        /// <inheritdoc />
        protected MediationCommandBase(Action<Exception> onException) : base(onException)
        {

        }

        /// <inheritdoc />
        protected MediationCommandBase(Context context, Action<Exception> onException) : base(context, onException)
        {

        }

        /// <inheritdoc />
        protected MediationCommandBase(Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(optionsBuilder)
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
        protected MediationCommandBase(Context context) : base(context)
        {

        }

        /// <inheritdoc />
        protected MediationCommandBase(Action<Exception> onException) : base(onException)
        {

        }

        /// <inheritdoc />
        protected MediationCommandBase(Context context, Action<Exception> onException) : base(context, onException)
        {

        }

        /// <inheritdoc />
        protected MediationCommandBase(Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(optionsBuilder)
        {

        }
    }

    /// <summary>
    /// The top level base mediation command
    /// </summary>
    public abstract class MediationCommandBase : RequestBase<Unit>, IMediationCommand
    {
        /// <inheritdoc />
        protected MediationCommandBase(Context context) : base(context)
        {

        }

        /// <inheritdoc />
        protected MediationCommandBase(Action<Exception> onException) : base(onException)
        {

        }

        /// <inheritdoc />
        protected MediationCommandBase(Context context, Action<Exception> onException) : base(context, onException)
        {

        }

        /// <inheritdoc />
        protected MediationCommandBase(Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(optionsBuilder)
        {

        }
    }
}
