using Apizr.Policing;
using Apizr.Sample.MAUI.Views;
using Apizr.Sample.MAUI.ViewModels;
using Apizr.Sample.Models;
using CommunityToolkit.Maui;
using Polly;
using Polly.Extensions.Http;
using Polly.Registry;
using System.Reflection;
using Apizr.Resiliencing;
using Shiny;

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

            // Apizr
            var registry = new PolicyRegistry
            {
                {
                    "TransientHttpError", HttpPolicyExtensions.HandleTransientHttpError().WaitAndRetryAsync(new[]
                    {
                        TimeSpan.FromSeconds(1),
                        TimeSpan.FromSeconds(5),
                        TimeSpan.FromSeconds(10)
                    }, LoggedStrategies.OnLoggedRetry).WithPolicyKey("TransientHttpError")
                }
            };
            services.AddPolicyRegistry(registry);

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