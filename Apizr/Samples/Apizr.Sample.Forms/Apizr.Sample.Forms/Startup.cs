using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Apizr.Sample.Forms.Infrastructure;
using Apizr.Sample.Forms.Services.Settings;
using Apizr.Sample.Forms.ViewModels;
using Apizr.Sample.Forms.Views;
using Apizr.Sample.Models;
using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Navigation;
using Shiny;
using Xamarin.Essentials;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;

namespace Apizr.Sample.Forms
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

            services.UseXfMaterialDialogs();

            services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                pipelineBuilder => pipelineBuilder.AddRetry(
                    new RetryStrategyOptions<HttpResponseMessage>
                    {
                        ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                            .Handle<HttpRequestException>()
                            .HandleResult(response =>
                                response.StatusCode is >= HttpStatusCode.InternalServerError
                                    or HttpStatusCode.RequestTimeout),
                        Delay = TimeSpan.FromSeconds(1),
                        MaxRetryAttempts = 3,
                        UseJitter = true,
                        BackoffType = DelayBackoffType.Exponential
                    }));

            services.AddSingleton<IAppSettings, AppSettings>();
            services.AddTransient<IConnectivity, ConnectivityImplementation>();

            services.AddApizr(
                apizrRegistry => apizrRegistry
                    .AddManagerFor<IReqResService>()
                    .AddManagerFor<IHttpBinService>(options => options
                        .WithAuthenticationHandler(OnRefreshToken))
                    .AddCrudManagerFor(typeof(User).Assembly),
                
                config => config
                    .WithAkavacheCacheHandler()
                    .WithLogging()
                    .WithConnectivityHandler<IConnectivity>(connectivity => connectivity.NetworkAccess == NetworkAccess.Internet)
                    .WithMediation()
                    .WithOptionalMediation());

            services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        }

        private static Task<string> OnRefreshToken(HttpRequestMessage request) => Task.FromResult("tokenValue");
        
        public override Task RunApp(INavigationService navigator)
            => navigator.Navigate("NavigationPage/MainPage");

        // This is to register IServiceScopeFactory/DryIocServiceScopeFactory
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
