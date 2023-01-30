using Apizr.Extending.Configuring.Common;
using Apizr.Extending.Configuring.Proper;
using Apizr.Extending.Configuring.Registry;

namespace Apizr.Configuring.Registry
{
    public class ApizrExtendedTransferRegistryBuilder : ApizrTransferRegistryBuilderBase<IApizrExtendedTransferRegistryBuilder, IApizrExtendedRegistryBuilder, IApizrExtendedProperOptionsBuilder,
        IApizrExtendedCommonOptionsBuilder>, IApizrExtendedTransferRegistryBuilder
    {
        /// <inheritdoc />
        public ApizrExtendedTransferRegistryBuilder(IApizrExtendedRegistryBuilder builder) : base(builder)
        {
        }

        /// <inheritdoc />
        protected override IApizrExtendedTransferRegistryBuilder Builder => this;
    }
}
