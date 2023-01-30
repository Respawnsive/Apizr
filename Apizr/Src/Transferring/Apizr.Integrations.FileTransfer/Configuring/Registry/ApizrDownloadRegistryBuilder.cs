using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;

namespace Apizr.Configuring.Registry;

public class ApizrDownloadRegistryBuilder : ApizrDownloadRegistryBuilderBase<IApizrDownloadRegistryBuilder, IApizrRegistryBuilder, IApizrProperOptionsBuilder,
    IApizrCommonOptionsBuilder>, IApizrDownloadRegistryBuilder
{
    /// <inheritdoc />
    public ApizrDownloadRegistryBuilder(IApizrRegistryBuilder builder) : base(builder)
    {
    }

    /// <inheritdoc />
    protected override IApizrDownloadRegistryBuilder Builder => this;
}