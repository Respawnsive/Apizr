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
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using Polly.Registry;
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

            services.AddApizrFor<IReqResService>(options => options.WithCacheHandler<AkavacheCacheHandler>().WithHttpTracing());
            services.AddApizrCrudFor(optionsBuilder => optionsBuilder.WithCacheHandler<AkavacheCacheHandler>().WithMediation().WithOptionalMediation().WithHttpTracing(), typeof(User));
            services.AddApizrFor<IHttpBinService>(optionsBuilder => optionsBuilder.WithCacheHandler<AkavacheCacheHandler>().WithHttpTracing().WithAuthenticationHandler<IAppSettings>(settings => settings.Token, OnRefreshToken));

            services.AddMediatR(typeof(Startup));

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

        public override void ConfigureApp(IContainerRegistry containerRegistry)
        {
#if DEBUG
            Xamarin.Forms.Internals.Log.Listeners.Add(new TraceLogListener());
#endif
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
        }

        public override Task RunApp(INavigationService navigator)
            => navigator.Navigate("NavigationPage/MainPage");
    }
}
