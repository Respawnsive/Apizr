using System;
using Apizr.Caching;
using Apizr.Connecting;
using Apizr.Mapping;
using Polly.Registry;

namespace Apizr.Configuring.Registry
{
    public class ApizrRegistryBuilder : IApizrRegistryBuilder
    {
        protected readonly ApizrRegistry Registry;

        internal ApizrRegistryBuilder(ApizrRegistry registry)
        {
            Registry = registry;
        }

        public IApizrRegistry ApizrRegistry => Registry;

        public IApizrRegistryBuilder AddFor<TWebApi, TApizrManager>(Func<ILazyWebApi<TWebApi>, IConnectivityHandler, ICacheHandler, IMappingHandler,
                IReadOnlyPolicyRegistry<string>, IApizrOptions<TWebApi>, TApizrManager> apizrManagerFactory,
            Action<IApizrOptionsBuilder> optionsBuilder = null)
            where TApizrManager : IApizrManager<TWebApi>
        {
            var apizrManager = Apizr.For(apizrManagerFactory, Registry.ApizrCommonOptions, optionsBuilder);

            return this;
        }
    }
}
