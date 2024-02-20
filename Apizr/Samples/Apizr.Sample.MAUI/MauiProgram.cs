using System.Net;
using Apizr.Sample.MAUI.Views;
using Apizr.Sample.MAUI.ViewModels;
using Apizr.Sample.Models;
using CommunityToolkit.Maui;
using System.Reflection;
using Shiny;
using Polly;
using Polly.Retry;

namespace Apizr.Sample.MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseShinyFramework(
                    new DryIocContainerExtension(),
                    prism => prism.OnAppStart("NavigationPage/MainPage")
                )
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSansRegular.ttf", "OpenSansRegular");
                });

            var services = builder.Services; 

            // Navigation
            services.RegisterForNavigation<MainPage, MainPageViewModel>();

            // Polly
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

            // Apizr
            services.AddApizr(
                apizrRegistry => apizrRegistry
                    .AddManagerFor<IReqResService>()
                    .AddManagerFor<IHttpBinService>(options => options
                        .WithAuthenticationHandler(OnRefreshToken))
                    .AddCrudManagerFor(typeof(User).Assembly),

                config => config
                    .WithAkavacheCacheHandler()
                    .WithLogging()
                    //.WithConnectivityHandler<IConnectivity>(connectivity => connectivity.NetworkAccess == NetworkAccess.Internet)
                    .WithMediation()
                    .WithOptionalMediation());

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            return builder.Build();
        }

        private static Task<string> OnRefreshToken(HttpRequestMessage request) => Task.FromResult("tokenValue");
    }
}