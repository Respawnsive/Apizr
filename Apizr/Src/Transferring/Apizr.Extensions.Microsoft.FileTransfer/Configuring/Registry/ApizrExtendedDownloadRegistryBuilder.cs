using Apizr.Extending.Configuring.Common;
using Apizr.Extending.Configuring.Proper;
using Apizr.Extending.Configuring.Registry;

namespace Apizr.Configuring.Registry;

public class ApizrExtendedDownloadRegistryBuilder : ApizrDownloadRegistryBuilderBase<IApizrExtendedDownloadRegistryBuilder, IApizrExtendedRegistryBuilder, IApizrExtendedProperOptionsBuilder,
    IApizrExtendedCommonOptionsBuilder>, IApizrExtendedDownloadRegistryBuilder
{
    /// <inheritdoc />
    public ApizrExtendedDownloadRegistryBuilder(IApizrExtendedRegistryBuilder builder) : base(builder)
    {
    }

    /// <inheritdoc />
    protected override IApizrExtendedDownloadRegistryBuilder Builder => this;
}