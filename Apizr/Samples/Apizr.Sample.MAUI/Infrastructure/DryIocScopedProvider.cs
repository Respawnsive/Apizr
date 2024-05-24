using DryIoc;
using Prism.Ioc.Internals;

namespace Apizr.Sample.MAUI.Infrastructure
{
    internal class DryIocScopedProvider : IScopedProvider
    {
        private readonly List<IScopedProvider> _children = [];

        public DryIocScopedProvider(IResolverContext resolver)
        {
            Resolver = resolver;
            Resolver.Resolve<ContainerProviderLocator>().Current = this;
        }

        public bool IsAttached { get; set; }

        public IResolverContext? Resolver { get; private set; }

        public IScopedProvider CurrentScope => this;

        public IScopedProvider CreateChildScope()
        {
            var childScope = new DryIocScopedProvider(Resolver.OpenScope());
            _children.Add(childScope);
            return childScope;
        }

        public IScopedProvider CreateScope() => ContainerLocator.Container.CreateScope();

        public void Dispose()
        {
            _children.ForEach(x => x.Dispose());
            _children.Clear();
            Resolver?.Dispose();
            Resolver = null;
        }

        public object Resolve(Type type) => Resolve(type, Array.Empty<(Type, object)>());

        public object Resolve(Type type, string name)
        {
            return Resolve(type, name, []);
        }

        public object Resolve(Type type, params (Type Type, object Instance)[] parameters)
        {
            try
            {
                var list = parameters.Where((Func<(Type, object), bool>)(x => x.Item2 is not IContainerProvider)).Select(x => x.Item2).ToList();
                list.Add(this);
                return Resolver.Resolve(type, list.ToArray());
            }
            catch (Exception ex)
            {
                throw new ContainerResolutionException(type, ex, this);
            }
        }

        public object Resolve(Type type, string name, params (Type Type, object Instance)[] parameters)
        {
            try
            {
                var list = parameters.Where((Func<(Type, object), bool>)(x => !(x.Item2 is IContainerProvider))).Select(x => x.Item2).ToList();
                list.Add(this);
                return Resolver.Resolve(type, name, args: [.. list]);
            }
            catch (Exception ex)
            {
                throw new ContainerResolutionException(type, name, ex, this);
            }
        }
    }
}
