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
    public abstract class MediationCommandBase<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData, TApizrRequestOptions, TApizrRequestOptionsBuilder> : RequestBase<TModelResultData, TApizrRequestOptions, TApizrRequestOptionsBuilder>, IMediationCommand<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>
        where TApizrRequestOptions : IApizrRequestOptionsBase
        where TApizrRequestOptionsBuilder : IApizrRequestOptionsBuilderBase<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        /// <inheritdoc />
        protected MediationCommandBase(Action<TApizrRequestOptionsBuilder> optionsBuilder = null) : base(optionsBuilder)
        {

        }
    }

    /// <summary>
    /// The top level base mediation command
    /// </summary>
    /// <typeparam name="TRequestData">The api request type</typeparam>
    /// <typeparam name="TResultData">The api result type</typeparam>
    public abstract class MediationCommandBase<TRequestData, TResultData, TApizrRequestOptions, TApizrRequestOptionsBuilder> : RequestBase<TResultData, TApizrRequestOptions, TApizrRequestOptionsBuilder>, IMediationCommand<TRequestData, TResultData>
        where TApizrRequestOptions : IApizrRequestOptionsBase
        where TApizrRequestOptionsBuilder : IApizrRequestOptionsBuilderBase<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        /// <inheritdoc />
        protected MediationCommandBase(Action<TApizrRequestOptionsBuilder> optionsBuilder = null) : base(optionsBuilder)
        {

        }
    }

    /// <summary>
    /// The top level base mediation command
    /// </summary>
    /// <typeparam name="TRequestData">The api request type</typeparam>
    public abstract class MediationCommandBase<TRequestData, TApizrRequestOptions, TApizrRequestOptionsBuilder> : RequestBase<Unit, TApizrRequestOptions, TApizrRequestOptionsBuilder>, IMediationCommand<TRequestData>
        where TApizrRequestOptions : IApizrRequestOptionsBase
        where TApizrRequestOptionsBuilder : IApizrRequestOptionsBuilderBase<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        /// <inheritdoc />
        protected MediationCommandBase(Action<TApizrRequestOptionsBuilder> optionsBuilder = null) : base(optionsBuilder)
        {

        }
    }

    /// <summary>
    /// The top level base mediation command
    /// </summary>
    public abstract class MediationCommandBase<TApizrRequestOptions, TApizrRequestOptionsBuilder> : RequestBase<Unit, TApizrRequestOptions, TApizrRequestOptionsBuilder>, IMediationCommand
        where TApizrRequestOptions : IApizrRequestOptionsBase
        where TApizrRequestOptionsBuilder : IApizrRequestOptionsBuilderBase<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        /// <inheritdoc />
        protected MediationCommandBase(Action<TApizrRequestOptionsBuilder> optionsBuilder = null) : base(optionsBuilder)
        {

        }
    }
}
