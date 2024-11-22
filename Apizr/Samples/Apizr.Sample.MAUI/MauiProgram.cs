using System.Net;
using Apizr.Sample.MAUI.Views;
using Apizr.Sample.MAUI.ViewModels;
using Apizr.Sample.Models;
using CommunityToolkit.Maui;
using System.Reflection;
using Apizr.Sample.MAUI.Infrastructure;
using MetroLog.MicrosoftExtensions;
using MetroLog.Operators;
using Polly;
using Polly.Retry;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

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
                .UsePrism(
                    new DryIocContainerExtension(),
                    prism => prism
                        .RegisterTypes(containerRegistry =>
                        {
                            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
                        })
                        .AddGlobalNavigationObserver(context => context.Subscribe(x =>
                        {
                            Console.WriteLine(x.Type == NavigationRequestType.Navigate
                                ? $"Navigation: {x.Uri}"
                                : $"Navigation: {x.Type}");

                            var status = x.Cancelled ? "Cancelled" : x.Result.Success ? "Success" : "Failed";
                            Console.WriteLine($@"Result: {status}");

                            if (status == "Failed" && !string.IsNullOrEmpty(x.Result?.Exception?.Message))
                                Console.Error.WriteLine(x.Result.Exception.Message);
                        }))
                        .CreateWindow(navigationService => navigationService.CreateBuilder()
                            .AddNavigationPage()
                            .AddSegment<MainPageViewModel>()
                            .NavigateAsync(HandleNavigationError))
                )
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            #region Configuration

            // Json settings (loaded from embedded appsettings.json)
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly!.GetManifestResourceStream($"{assembly.GetName().Name}.appsettings.json")!;
            var jsonConfig = new ConfigurationBuilder().AddJsonStream(stream!).Build();
            builder.Configuration.AddConfiguration(jsonConfig);

            #endregion

            #region Logging

            builder.Logging
#if DEBUG
                .AddTraceLogger(
                    options =>
                    {
                        options.MinLevel = LogLevel.Trace;
                        options.MaxLevel = LogLevel.Critical;
                    }) // Will write to the Debug Output
#endif
                .AddInMemoryLogger(
                    options =>
                    {
                        options.MaxLines = 1024;
                        options.MinLevel = LogLevel.Debug;
                        options.MaxLevel = LogLevel.Critical;
                    })
#if RELEASE
                .AddStreamingFileLogger(
                    options =>
                    {
                        options.RetainDays = 2;
                        options.FolderPath = Path.Combine(
                            FileSystem.CacheDirectory,
                            "MetroLogs");
                    })
#endif
                .AddConsoleLogger(
                    options =>
                    {
                        options.MinLevel = LogLevel.Information;
                        options.MaxLevel = LogLevel.Critical;
                    }); // Will write to the Console Output (logcat for android)

            builder.Services.AddSingleton(LogOperatorRetriever.Instance);

            #endregion

            var services = builder.Services; 

            // Navigation
            services.RegisterForNavigation<MainPage, MainPageViewModel>();

            // Plugins
            services.AddSingleton(_ => SecureStorage.Default)
                .AddSingleton(_ => FilePicker.Default);

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
                    .AddManagerFor<IHttpBinService>()//options => options.WithAuthenticationHandler(typeof(AuthHandler<>)))
                    .AddUploadManager(options => options
                        //.WithBaseAddress("https://httpbin.org/post")
                        .WithBaseAddress("https://rx6z0kd7-7015.uks1.devtunnels.ms/upload")
                        .WithHeaders(["Authorization: Bearer"])
                        .WithLogging())
                    .AddCrudManagerFor([typeof(User)]),

                config => config
                    .WithConfiguration(builder.Configuration)
                    .WithDelegatingHandler(serviceProvider => new TestRequestHandler(serviceProvider.GetRequiredService<ISecureStorage>()))
                    .WithAkavacheCacheHandler()
                    //.WithLogging()
                    //.WithConnectivityHandler<IConnectivity>(connectivity => connectivity.NetworkAccess == NetworkAccess.Internet)
                    .WithMediation()
                    .WithOptionalMediation()
                    .WithAuthenticationHandler(typeof(AuthenticationHandler<>)));

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddTransient(typeof(AuthenticationHandler<>));

            return builder.Build();
        }

        private static void HandleNavigationError(Exception ex)
        {
            Console.WriteLine(ex);
            System.Diagnostics.Debugger.Break();
        }

        private static Task<string> OnRefreshToken(HttpRequestMessage request) => Task.FromResult("tokenValue");
    }
}