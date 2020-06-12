using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Apizr.Policing;
using Apizr.Sample.Api;
using Apizr.Sample.Mobile.Services.Settings;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Polly.Registry;
using Shiny;
using Shiny.Logging;
using Shiny.Prism;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;

namespace Apizr.Sample.Mobile
{
    public class Startup : PrismStartup
    {
        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IAppInfo, AppInfoImplementation>();

            Log.UseConsole();
            Log.UseDebug();

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

            services.UseRepositoryCache();

            services.AddSingleton<IAppSettings, AppSettings>();

            services.UseApizr<IReqResService>();
            services.UseApizr<IHttpBinService>(optionsBuilder => optionsBuilder.WithAuthenticationHandler<IAppSettings>(settings => settings.Token, OnRefreshToken));
        }

        private static Task<string?> OnRefreshToken(HttpRequestMessage request)
        {
            return Task.FromResult("tokenValue");
        }
    }
}
