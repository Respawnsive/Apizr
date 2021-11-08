using System;
using System.Collections.Generic;
using Apizr.Mediation.Cruding.Sending;
using Apizr.Mediation.Requesting.Sending;
using Apizr.Requesting;

namespace Apizr.Mediation.Configuring.Registry
{
    public interface IApizrMediationRegistryBase : IEnumerable<KeyValuePair<Type, Func<IMediator>>>
    {
        ICrudMediator<T, TKey, IEnumerable<T>, IDictionary<string, object>> GetFor<T, TKey>() where T : class;

        ICrudMediator<T, TKey, TReadAllResult, IDictionary<string, object>> GetFor<T, TKey,
            TReadAllResult>() where T : class;

        ICrudMediator<T, TKey, TReadAllResult, TReadAllParams> GetFor<T, TKey, TReadAllResult,
            TReadAllParams>() where T : class;

        IMediator<TWebApi> GetFor<TWebApi>();

        bool TryGetFor<T, TKey>(out ICrudMediator<T, TKey, IEnumerable<T>, IDictionary<string, object>> mediator) where T : class;

        bool TryGetFor<T, TKey, TReadAllResult>(out ICrudMediator<T, TKey, TReadAllResult, IDictionary<string, object>> mediator) where T : class;

        bool TryGetFor<T, TKey, TReadAllResult, TReadAllParams>(out ICrudMediator<T, TKey, TReadAllResult, TReadAllParams> mediator) where T : class;

        bool TryGetFor<TWebApi>(out IMediator<TWebApi> mediator);

        int Count { get; }

        bool ContainsFor<T, TKey>() where T : class;

        bool ContainsFor<T, TKey, TReadAllResult>() where T : class;

        bool ContainsFor<T, TKey, TReadAllResult, TReadAllParams>() where T : class;

        bool ContainsFor<TWebApi>();
    }
}
