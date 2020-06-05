namespace Apizr
{
    public abstract class ApizrOptionsBuilderBase<TApizrOptions> : IApizrOptionsBuilder<TApizrOptions> where TApizrOptions : class, IApizrOptions
    {
        protected readonly TApizrOptions Options;

        protected ApizrOptionsBuilderBase(TApizrOptions apizrOptions)
        {
            Options = apizrOptions;
        }

        public IApizrOptions ApizrOptions => Options;
    }
}
