using System;

namespace Apizr
{
    public class LazyWebApi<TWebApi> : Lazy<TWebApi>, ILazyWebApi<TWebApi>
    {
        public LazyWebApi(Func<TWebApi> valueFactory) : base(valueFactory)
        {
        }

        public LazyWebApi(Func<object> valueFactory) : base(() => (TWebApi)valueFactory.Invoke())
        {
        }
    }
}