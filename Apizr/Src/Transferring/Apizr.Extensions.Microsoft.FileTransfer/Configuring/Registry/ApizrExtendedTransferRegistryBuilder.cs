using System;
using Apizr.Extending.Configuring.Common;
using Apizr.Extending.Configuring.Proper;
using Apizr.Extending.Configuring.Registry;
using Apizr.Logging;
using Apizr.Transferring.Managing;
using Apizr.Transferring.Requesting;

namespace Apizr.Configuring.Registry
{
    public class ApizrExtendedTransferRegistryBuilder : ApizrTransferRegistryBuilderBase<
        IApizrExtendedTransferRegistryBuilder, IApizrExtendedRegistryBuilder, IApizrExtendedProperOptionsBuilder,
        IApizrExtendedCommonOptionsBuilder>, IApizrExtendedTransferRegistryBuilder
    {
        private readonly IApizrInternalExtendedRegistryBuilder<IApizrExtendedProperOptionsBuilder> _internalBuilder;

        /// <inheritdoc />
        public ApizrExtendedTransferRegistryBuilder(IApizrExtendedRegistryBuilder builder)
        {
            _internalBuilder = builder as IApizrInternalExtendedRegistryBuilder<IApizrExtendedProperOptionsBuilder>;
        }

        /// <inheritdoc />
        protected override IApizrExtendedTransferRegistryBuilder Builder => this;

        /// <inheritdoc />
        public override IApizrExtendedTransferRegistryBuilder AddTransferManagerFor<TTransferApi>(
            Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null)
        {
            // Upload
            _internalBuilder
                ?.AddWrappingManagerFor<TTransferApi, IApizrUploadManager<TTransferApi>,
                    ApizrUploadManager<TTransferApi>>(
                    optionsBuilder.IgnoreMessageParts(HttpMessageParts.RequestBody));

            // Download
            _internalBuilder
                ?.AddWrappingManagerFor<TTransferApi, IApizrDownloadManager<TTransferApi>,
                    ApizrDownloadManager<TTransferApi>>(
                    optionsBuilder.IgnoreMessageParts(HttpMessageParts.ResponseBody));

            if (typeof(TTransferApi) == typeof(ITransferApi))
            {
                // Transfer
                _internalBuilder
                    ?.AddWrappingManagerFor<ITransferApi, IApizrTransferManager,
                        ApizrTransferManager>(
                        optionsBuilder.IgnoreMessageParts(HttpMessageParts.RequestBody | HttpMessageParts.ResponseBody));

                _internalBuilder?.AddAliasingManagerFor<IApizrTransferManager<ITransferApi>, IApizrTransferManager>();
            }
            else
            {
                // Transfer
                _internalBuilder
                    ?.AddWrappingManagerFor<TTransferApi, IApizrTransferManager<TTransferApi>,
                        ApizrTransferManager<TTransferApi>>(
                        optionsBuilder.IgnoreMessageParts(HttpMessageParts.RequestBody | HttpMessageParts.ResponseBody));
            }

            return Builder;
        }

        /// <inheritdoc />
        public override IApizrExtendedTransferRegistryBuilder
            AddTransferManagerFor<TTransferApi, TDownloadParams>(
                Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null)
        {
            // Upload
            _internalBuilder
                ?.AddWrappingManagerFor<TTransferApi, IApizrUploadManager<TTransferApi>,
                    ApizrUploadManager<TTransferApi>>(
                    optionsBuilder.IgnoreMessageParts(HttpMessageParts.RequestBody));

            // Download
            _internalBuilder
                ?.AddWrappingManagerFor<TTransferApi, IApizrDownloadManager<TTransferApi, TDownloadParams>,
                    ApizrDownloadManager<TTransferApi, TDownloadParams>>(
                    optionsBuilder.IgnoreMessageParts(HttpMessageParts.ResponseBody));

            // Transfer
            _internalBuilder
                ?.AddWrappingManagerFor<TTransferApi, IApizrTransferManager<TTransferApi, TDownloadParams>,
                    ApizrTransferManager<TTransferApi, TDownloadParams>>(
                    optionsBuilder.IgnoreMessageParts(HttpMessageParts.RequestBody | HttpMessageParts.ResponseBody));

            return Builder;
        }
    }
}
