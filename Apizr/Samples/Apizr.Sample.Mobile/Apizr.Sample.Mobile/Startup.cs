using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Apizr.Sample.Api;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Polly.Registry;
using Shiny;
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

            var registry = new PolicyRegistry
            {
                {
                    "TransientHttpError", HttpPolicyExtensions.HandleTransientHttpError().WaitAndRetryAsync(new[]
                    {
                        TimeSpan.FromSeconds(1),
                        TimeSpan.FromSeconds(5),
                        TimeSpan.FromSeconds(10)
                    })
                }
            };
            services.AddPolicyRegistry(registry);

            services.UseRepositoryCache();

            services.UseApizr<IReqResService>();
        }

        private static Task<string?> OnRefreshToken(HttpRequestMessage request)
        {
            return Task.FromResult("tokenValue");
        }
    }
}
