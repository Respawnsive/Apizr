using System;
using Apizr.Configuring.Request;
using Apizr.Mediation.Commanding;
using MediatR;
using Polly;

namespace Apizr.Mediation.Cruding.Base
{
    /// <summary>
    /// The top level base mediation Delete command
    /// </summary>
    /// <typeparam name="T">The api entity type</typeparam>
    /// <typeparam name="TKey">The entity's crud key type</typeparam>
    /// <typeparam name="TResultData">The api result type</typeparam>
    public abstract class DeleteCommandBase<T, TKey, TResultData, TApizrRequestOptions, TApizrRequestOptionsBuilder> : MediationCommandBase<TKey, TResultData, TApizrRequestOptions, TApizrRequestOptionsBuilder>
        where TApizrRequestOptions : IApizrUnitRequestOptions
        where TApizrRequestOptionsBuilder : IApizrUnitRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        /// <summary>
        /// The top level base mediation Delete command constructor
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        protected DeleteCommandBase(TKey key, Action<TApizrRequestOptionsBuilder> optionsBuilder = null) : base(optionsBuilder)
        {
            Key = key;
        }

        /// <summary>
        /// The entity's crud key
        /// </summary>
        public TKey Key { get; }
    }

    /// <summary>
    /// The top level base mediation Delete command
    /// </summary>
    /// <typeparam name="T">The api entity type</typeparam>
    /// <typeparam name="TResultData">The api result type</typeparam>
    public abstract class DeleteCommandBase<T, TResultData, TApizrRequestOptions, TApizrRequestOptionsBuilder> : DeleteCommandBase<T, int, TResultData, TApizrRequestOptions, TApizrRequestOptionsBuilder>
        where TApizrRequestOptions : IApizrUnitRequestOptions
        where TApizrRequestOptionsBuilder : IApizrUnitRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        /// <inheritdoc />
        protected DeleteCommandBase(int key, Action<TApizrRequestOptionsBuilder> optionsBuilder = null) : base(key, optionsBuilder)  
        {
        }
    }

    /// <summary>
    /// The top level base mediation Delete command
    /// </summary>
    /// <typeparam name="T">The api entity type</typeparam>
    public abstract class DeleteCommandBase<T, TApizrRequestOptions, TApizrRequestOptionsBuilder> : DeleteCommandBase<T, Unit, TApizrRequestOptions, TApizrRequestOptionsBuilder>
        where TApizrRequestOptions : IApizrUnitRequestOptions
        where TApizrRequestOptionsBuilder : IApizrUnitRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        /// <inheritdoc />
        protected DeleteCommandBase(int key, Action<TApizrRequestOptionsBuilder> optionsBuilder = null) : base(key, optionsBuilder)
        {
        }
    }
}