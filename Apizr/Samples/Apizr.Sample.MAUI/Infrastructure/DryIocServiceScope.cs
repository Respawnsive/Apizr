using DryIoc;

namespace Apizr.Sample.MAUI.Infrastructure
{
    internal sealed class DryIocServiceScope : IServiceScope
    {
        private readonly IResolverContext _resolverContext;

        public IServiceProvider ServiceProvider => _resolverContext;

        public DryIocServiceScope(IResolverContext resolverContext)
        {
            _resolverContext = resolverContext;
        }

        public void Dispose() => _resolverContext.Dispose();
    }
}
