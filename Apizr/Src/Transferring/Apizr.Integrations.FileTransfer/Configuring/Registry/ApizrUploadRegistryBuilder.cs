using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;

namespace Apizr.Configuring.Registry;

public class ApizrUploadRegistryBuilder : ApizrUploadRegistryBuilderBase<IApizrUploadRegistryBuilder, IApizrRegistryBuilder, IApizrProperOptionsBuilder,
    IApizrCommonOptionsBuilder>, IApizrUploadRegistryBuilder
{
    /// <inheritdoc />
    public ApizrUploadRegistryBuilder(IApizrRegistryBuilder builder) : base(builder)
    {
    }

    /// <inheritdoc />
    protected override IApizrUploadRegistryBuilder Builder => this;
}