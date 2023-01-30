using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;

namespace Apizr.Configuring.Registry;

public class ApizrTransferRegistryBuilder : ApizrTransferRegistryBuilderBase<IApizrTransferRegistryBuilder, IApizrRegistryBuilder, IApizrProperOptionsBuilder,
    IApizrCommonOptionsBuilder>, IApizrTransferRegistryBuilder
{
    /// <inheritdoc />
    public ApizrTransferRegistryBuilder(IApizrRegistryBuilder builder) : base(builder)
    {
    }

    /// <inheritdoc />
    protected override IApizrTransferRegistryBuilder Builder => this;
}