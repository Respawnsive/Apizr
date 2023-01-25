using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;
using Apizr.Transferring.Requesting;
using System;

namespace Apizr.Configuring.Registry
{
    public interface IApizrUploadRegistryBuilder<out TApizrRegistry, out TApizrRegistryBuilder, out TApizrProperOptionsBuilder, out TApizrCommonOptionsBuilder>
        where TApizrRegistry : IApizrEnumerableRegistry
        where TApizrRegistryBuilder : IApizrRegistryBuilderBase<TApizrRegistry, TApizrRegistryBuilder, TApizrProperOptionsBuilder, TApizrCommonOptionsBuilder>
        where TApizrProperOptionsBuilder : IApizrGlobalProperOptionsBuilderBase
        where TApizrCommonOptionsBuilder : IApizrGlobalCommonOptionsBuilderBase
    {
        IApizrUploadRegistryBuilder<TApizrRegistry, TApizrRegistryBuilder, TApizrProperOptionsBuilder,
            TApizrCommonOptionsBuilder> AddFor<TWebApi>(Action<TApizrProperOptionsBuilder> optionsBuilder = null)
            where TWebApi : IUploadApi;
    }
}
