using System;
using System.Collections.Generic;
using Apizr.Configuring.Registry;
using Apizr.Mediation.Cruding.Sending;
using Apizr.Mediation.Requesting.Sending;

namespace Apizr.Mediation.Configuring.Registry
{
    public interface IApizrMediationEnumerableRegistry : IEnumerable<KeyValuePair<Type, Func<IApizrMediatorBase>>>, IApizrEnumerableRegistryBase
    {
        IApizrCrudMediator<T, TKey, IEnumerable<T>, IDictionary<string, object>> GetFor<T, TKey>() where T : class;

        IApizrCrudMediator<T, TKey, TReadAllResult, IDictionary<string, object>> GetFor<T, TKey,
            TReadAllResult>() where T : class;

        IApizrCrudMediator<T, TKey, TReadAllResult, TReadAllParams> GetFor<T, TKey, TReadAllResult,
            TReadAllParams>() where T : class;

        IApizrMediator<TWebApi> GetFor<TWebApi>();

        bool TryGetFor<T, TKey>(out IApizrCrudMediator<T, TKey, IEnumerable<T>, IDictionary<string, object>> mediator) where T : class;

        bool TryGetFor<T, TKey, TReadAllResult>(out IApizrCrudMediator<T, TKey, TReadAllResult, IDictionary<string, object>> mediator) where T : class;

        bool TryGetFor<T, TKey, TReadAllResult, TReadAllParams>(out IApizrCrudMediator<T, TKey, TReadAllResult, TReadAllParams> mediator) where T : class;

        bool TryGetFor<TWebApi>(out IApizrMediator<TWebApi> mediator);
    }
}
