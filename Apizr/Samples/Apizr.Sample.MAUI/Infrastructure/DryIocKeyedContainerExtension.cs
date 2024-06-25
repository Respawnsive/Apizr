using System.Reflection;
using System.Reflection.Emit;
using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using Prism.Ioc.Internals;
using IContainer = DryIoc.IContainer;

namespace Apizr.Sample.MAUI.Infrastructure
{
    public class DryIocKeyedContainerExtension :
        IContainerExtension<IContainer>,
        IContainerInfo,
        IServiceCollectionAware
    {
        private DryIocScopedProvider? _currentScope;

        private static Rules GetDefaultRules()
        {
            var defaultRules = Rules.Default.WithConcreteTypeDynamicRegistrations(reuse: Reuse.Transient)
                .With(Made.Of(FactoryMethod.ConstructorWithResolvableArguments)).WithFuncAndLazyWithoutRegistration()
                .WithTrackingDisposableTransients().WithFactorySelector(Rules.SelectLastRegisteredFactory());
            try
            {
                AssemblyBuilder
                    .DefineDynamicAssembly(new AssemblyName("PrismDynamicAssembly"), AssemblyBuilderAccess.Run)
                    .DefineDynamicModule("DynamicModule");
                return defaultRules;
            }
            catch
            {
                return defaultRules.WithUseInterpretation();
            }
        }

        public static Rules DefaultRules { get; } = GetDefaultRules();

        public IContainer Instance { get; }

        public DryIocKeyedContainerExtension()
            : this(DryIocContainerExtension.DefaultRules)
        {
        }

        public DryIocKeyedContainerExtension(Rules rules)
            : this(new DryIoc.Container(rules))
        {
        }

        public DryIocKeyedContainerExtension(IContainer container)
        {
            Instance = container;
            Instance.RegisterInstance((IContainerExtension) this);
            Instance.Register<ContainerProviderLocator>(Reuse.Scoped);
            Instance.RegisterDelegate(r =>
            {
                if (!r.IsScoped())
                    return this;
                return r.Resolve<ContainerProviderLocator>().Current ??
                       throw new InvalidOperationException("The Container has not been set for this Scope.");
            }, Reuse.Transient);
            ExceptionExtensions.RegisterFrameworkExceptionType(typeof(ContainerException));
        }

        public IScopedProvider? CurrentScope => _currentScope;

        public IContainerRegistry RegisterInstance(Type type, object instance)
        {
            Instance.RegisterInstance(type, instance);
            return this;
        }

        public IContainerRegistry RegisterInstance(Type type, object instance, string name)
        {
            Instance.RegisterInstance(type, instance, IfAlreadyRegistered.Replace,
                serviceKey: name);
            return this;
        }

        public IContainerRegistry RegisterSingleton(Type from, Type to)
        {
            Instance.Register(from, to, Reuse.Singleton);
            return this;
        }

        public IContainerRegistry RegisterSingleton(Type from, Type to, string name)
        {
            Instance.Register(from, to, Reuse.Singleton,
                ifAlreadyRegistered: IfAlreadyRegistered.Replace, serviceKey: name);
            return this;
        }

        public IContainerRegistry RegisterSingleton(Type type, Func<object> factoryMethod)
        {
            Instance.RegisterDelegate(type, _ => factoryMethod(),
                Reuse.Singleton);
            return this;
        }

        public IContainerRegistry RegisterSingleton(
            Type type,
            Func<IContainerProvider, object> factoryMethod)
        {
            Instance.RegisterDelegate(type, factoryMethod, Reuse.Singleton);
            return this;
        }

        public IContainerRegistry RegisterManySingleton(Type type, params Type[] serviceTypes)
        {
            if (serviceTypes.Length == 0)
                serviceTypes = type.GetInterfaces();
            Instance.RegisterMany(serviceTypes, type, Reuse.Singleton);
            return this;
        }

        public IContainerRegistry RegisterScoped(Type from, Type to)
        {
            Instance.Register(from, to, Reuse.ScopedOrSingleton);
            return this;
        }

        public IContainerRegistry RegisterScoped(Type type, Func<object> factoryMethod)
        {
            Instance.RegisterDelegate(type, _ => factoryMethod(),
                Reuse.ScopedOrSingleton);
            return this;
        }

        public IContainerRegistry RegisterScoped(
            Type type,
            Func<IContainerProvider, object> factoryMethod)
        {
            Instance.RegisterDelegate(type, factoryMethod, Reuse.ScopedOrSingleton);
            return this;
        }

        public IContainerRegistry Register(Type from, Type to)
        {
            Instance.Register(from, to, Reuse.Transient);
            return this;
        }

        public IContainerRegistry Register(Type from, Type to, string name)
        {
            Instance.Register(from, to, Reuse.Transient,
                ifAlreadyRegistered: IfAlreadyRegistered.Replace, serviceKey: name);
            return this;
        }

        public IContainerRegistry Register(Type type, Func<object> factoryMethod)
        {
            Instance.RegisterDelegate(type, _ => factoryMethod(),
                Reuse.Transient);
            return this;
        }

        public IContainerRegistry Register(Type type, Func<IContainerProvider, object> factoryMethod)
        {
            Instance.RegisterDelegate(type, factoryMethod, Reuse.Transient);
            return this;
        }

        public IContainerRegistry RegisterMany(Type type, params Type[] serviceTypes)
        {
            if (serviceTypes.Length == 0)
                serviceTypes = type.GetInterfaces();
            Instance.RegisterMany(serviceTypes, type, Reuse.Transient);
            return this;
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
                var list = parameters
                    .Where((Func<(Type, object), bool>) (x => !(x.Item2 is IContainerProvider)))
                    .Select(x => x.Item2).ToList();
                list.Add(this);
                return Instance.Resolve(type, list.ToArray());
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
                var list = parameters
                    .Where((Func<(Type, object), bool>) (x => x.Item2 is not IContainerProvider))
                    .Select(x => x.Item2).ToList();
                list.Add(this);
                return Instance.Resolve(type, name, args: [.. list]);
            }
            catch (Exception ex)
            {
                throw new ContainerResolutionException(type, name, ex, this);
            }
        }

        public bool IsRegistered(Type type) => Instance.IsRegistered(type);

        public bool IsRegistered(Type type, string name)
        {
            return Instance.IsRegistered(type, name) ||
                   Instance.IsRegistered(type, name, FactoryType.Wrapper);
        }

        Type? IContainerInfo.GetRegistrationType(string key)
        {
            var registrationInfo = Instance
                .GetServiceRegistrations()
                .FirstOrDefault(r => key.Equals(r.OptionalServiceKey?.ToString(), StringComparison.Ordinal));
            if (registrationInfo.OptionalServiceKey == null)
                registrationInfo = Instance
                    .GetServiceRegistrations()
                    .FirstOrDefault(r => key.Equals(r.ImplementationType.Name, StringComparison.Ordinal));
            return registrationInfo.ImplementationType;
        }

        Type? IContainerInfo.GetRegistrationType(Type serviceType)
        {
            var registrationInfo = Instance
                .GetServiceRegistrations()
                .FirstOrDefault(x => x.ServiceType == serviceType);
            return registrationInfo.ServiceType is not null ? registrationInfo.ImplementationType : null;
        }

        public virtual IScopedProvider CreateScope() => CreateScopeInternal();

        protected IScopedProvider CreateScopeInternal()
        {
            _currentScope = new DryIocScopedProvider(Instance.OpenScope());
            return _currentScope;
        }

        public IServiceProvider CreateServiceProvider()
        {
            var serviceProvider = new DryIocServiceProvider(Instance);

            // those are singletons
            var singletons = Instance.SingletonScope;
            singletons.Use<IServiceProvider>(serviceProvider);
            singletons.Use<ISupportRequiredService>(serviceProvider);
            singletons.Use<IKeyedServiceProvider>(serviceProvider);

            singletons.Use<IServiceScopeFactory>(serviceProvider);
            singletons.Use<IServiceProviderIsService>(serviceProvider);
            singletons.Use<IServiceProviderIsKeyedService>(serviceProvider);

            return serviceProvider;
        }

        public void Populate(IServiceCollection services)
        {
            Instance.Populate(services);
            Instance.Validate();
        }
    }
}
