using System;
using System.Collections.Generic;
using Apizr.Configuring.Registry;
using Apizr.Optional.Cruding.Sending;
using Apizr.Optional.Requesting.Sending;

namespace Apizr.Optional.Configuring.Registry
{
    public interface IApizrOptionalMediationEnumerableRegistry : IEnumerable<KeyValuePair<Type, Func<IApizrOptionalMediatorBase>>>, IApizrEnumerableRegistryBase
    {
        IApizrCrudOptionalMediator<T, TKey, IEnumerable<T>, IDictionary<string, object>> GetFor<T, TKey>() where T : class;

        IApizrCrudOptionalMediator<T, TKey, TReadAllResult, IDictionary<string, object>> GetFor<T, TKey,
            TReadAllResult>() where T : class;

        IApizrCrudOptionalMediator<T, TKey, TReadAllResult, TReadAllParams> GetFor<T, TKey, TReadAllResult,
            TReadAllParams>() where T : class;

        IApizrOptionalMediator<TWebApi> GetFor<TWebApi>();

        bool TryGetFor<T, TKey>(out IApizrCrudOptionalMediator<T, TKey, IEnumerable<T>, IDictionary<string, object>> mediator) where T : class;

        bool TryGetFor<T, TKey, TReadAllResult>(out IApizrCrudOptionalMediator<T, TKey, TReadAllResult, IDictionary<string, object>> mediator) where T : class;

        bool TryGetFor<T, TKey, TReadAllResult, TReadAllParams>(out IApizrCrudOptionalMediator<T, TKey, TReadAllResult, TReadAllParams> mediator) where T : class;

        bool TryGetFor<TWebApi>(out IApizrOptionalMediator<TWebApi> mediator);
    }
}
