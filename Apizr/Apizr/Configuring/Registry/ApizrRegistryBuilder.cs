using System;
using Apizr.Caching;
using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;
using Apizr.Connecting;
using Apizr.Mapping;
using Polly.Registry;

namespace Apizr.Configuring.Registry
{
    public class ApizrRegistryBuilder : IApizrRegistryBuilder
    {
        protected readonly ApizrRegistry Registry = new ApizrRegistry();
        protected readonly IApizrCommonOptions CommonOptions;

        internal ApizrRegistryBuilder(IApizrCommonOptions commonOptions)
        {
            CommonOptions = commonOptions;
        }

        public IApizrRegistry ApizrRegistry => Registry;

        public IApizrRegistryBuilder AddFor<TWebApi, TApizrManager>(Func<ILazyFactory<TWebApi>, IConnectivityHandler, ICacheHandler, IMappingHandler,
                IReadOnlyPolicyRegistry<string>, IApizrOptions<TWebApi>, TApizrManager> apizrManagerFactory,
            Action<IApizrProperOptionsBuilder> properOptionsBuilder = null)
            where TApizrManager : IApizrManager<TWebApi>
        {
            var properOptions = Apizr.CreateApizrProperOptions<TWebApi>(CommonOptions, properOptionsBuilder);
            var managerFactory = new Func<TApizrManager>(() => Apizr.For(apizrManagerFactory, CommonOptions, properOptions));
            Registry.AddOrUpdateFor<TWebApi, TApizrManager>(managerFactory);

            return this;
        }
    }
}
