using System;
using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;
using Apizr.Transferring.Requesting;

namespace Apizr.Configuring.Registry;

public interface IApizrUploadRegistryBuilderBase<out TApizrUploadRegistryBuilder, out TApizrRegistryBuilder, out TApizrProperOptionsBuilder, out TApizrCommonOptionsBuilder>
    where TApizrUploadRegistryBuilder : IApizrUploadRegistryBuilderBase<TApizrUploadRegistryBuilder, TApizrRegistryBuilder, TApizrProperOptionsBuilder, TApizrCommonOptionsBuilder>
    where TApizrRegistryBuilder : IApizrRegistryBuilderBase
    where TApizrProperOptionsBuilder : IApizrGlobalProperOptionsBuilderBase
    where TApizrCommonOptionsBuilder : IApizrGlobalCommonOptionsBuilderBase
{
    TApizrUploadRegistryBuilder AddUploadManager(Action<TApizrProperOptionsBuilder> optionsBuilder = null);

    TApizrUploadRegistryBuilder AddUploadManagerFor<TUploadApi>(Action<TApizrProperOptionsBuilder> optionsBuilder = null)
        where TUploadApi : IUploadApi;
}