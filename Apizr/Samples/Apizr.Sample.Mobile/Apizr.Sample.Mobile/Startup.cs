using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Apizr.Extending;
using Apizr.Policing;
using Apizr.Sample.Api;
using Apizr.Sample.Api.Models;
using Apizr.Sample.Mobile.Infrastructure;
using Apizr.Sample.Mobile.Services.Settings;
using Apizr.Sample.Mobile.ViewModels;
using Apizr.Sample.Mobile.Views;
using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using Polly.Registry;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Navigation;
using Shiny;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;

namespace Apizr.Sample.Mobile
{
    public class Startup : FrameworkStartup
    {
        public override void ConfigureApp(Application app, IContainerRegistry containerRegistry)
        {
#if DEBUG
            Xamarin.Forms.Internals.Log.Listeners.Add(new TraceLogListener());
#endif
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
        }

        protected override void Configure(ILoggingBuilder builder, IServiceCollection services)
        {
            builder.AddConsole(opts =>
                opts.LogToStandardErrorThreshold = LogLevel.Trace
            );

            services.AddSingleton<IAppInfo, AppInfoImplementation>();

            var registry = new PolicyRegistry
            {
                {
                    "TransientHttpError", HttpPolicyExtensions.HandleTransientHttpError().WaitAndRetryAsync(new[]
                    {
                        TimeSpan.FromSeconds(1),
                        TimeSpan.FromSeconds(5),
                        TimeSpan.FromSeconds(10)
                    }, LoggedPolicies.OnLoggedRetry).WithPolicyKey("TransientHttpError")
                }
            };
            services.AddPolicyRegistry(registry);

            services.AddSingleton<IAppSettings, AppSettings>();

            services.AddApizrFor<IReqResService>(options => options.WithCacheHandler<AkavacheCacheHandler>().WithLogging());
            //services.AddApizrCrudFor(optionsBuilder => optionsBuilder.WithCacheHandler<AkavacheCacheHandler>().WithMediation().WithOptionalMediation().WithLogging(), typeof(User));
            //services.AddApizrFor<IHttpBinService>(optionsBuilder => optionsBuilder.WithCacheHandler<AkavacheCacheHandler>().WithLogging().WithAuthenticationHandler<IAppSettings>(settings => settings.Token, OnRefreshToken));

            //services.AddMediatR(typeof(Startup));

            // This is just to let you know what's registered from/for Apizr and ready to use
            foreach (var service in services.Where(d =>
                    (d.ServiceType != null && d.ServiceType.Assembly.FullName.Contains($"{nameof(Apizr)}")) ||
                    (d.ImplementationType != null && d.ImplementationType.Assembly.FullName.Contains($"{nameof(Apizr)}"))))
            //foreach (var service in services)
            {
                System.Console.WriteLine(
                    $"Registered service: {service.ServiceType?.GetFriendlyName()} - {service.ImplementationType?.GetFriendlyName()}");
            }
        }

        private static Task<string> OnRefreshToken(HttpRequestMessage request) => Task.FromResult("tokenValue");
        
        public override Task RunApp(INavigationService navigator)
            => navigator.Navigate("NavigationPage/MainPage");

        public override IServiceProvider CreateServiceProvider(IServiceCollection services)
        {
            ContainerLocator.SetContainerExtension(() => new DryIocContainerExtension());
            var container = ContainerLocator.Container.GetContainer();
            container.Use<IServiceScopeFactory>(r => new DryIocServiceScopeFactory(r));
            container.Populate(services);
            return container.GetServiceProvider();
        }
    }
}
