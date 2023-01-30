using Apizr.Extending.Configuring.Common;
using Apizr.Extending.Configuring.Proper;
using Apizr.Extending.Configuring.Registry;

namespace Apizr.Configuring.Registry;

public class ApizrExtendedUploadRegistryBuilder : ApizrUploadRegistryBuilderBase<IApizrExtendedUploadRegistryBuilder, IApizrExtendedRegistryBuilder, IApizrExtendedProperOptionsBuilder,
    IApizrExtendedCommonOptionsBuilder>, IApizrExtendedUploadRegistryBuilder
{
    /// <inheritdoc />
    public ApizrExtendedUploadRegistryBuilder(IApizrExtendedRegistryBuilder builder) : base(builder)
    {
    }

    /// <inheritdoc />
    protected override IApizrExtendedUploadRegistryBuilder Builder => this;
}