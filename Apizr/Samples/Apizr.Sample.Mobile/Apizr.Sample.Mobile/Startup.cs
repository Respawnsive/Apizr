using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Apizr.Extending;
using Apizr.Logging;
using Apizr.Policing;
using Apizr.Sample.Api;
using Apizr.Sample.Api.Models;
using Apizr.Sample.Mobile.Services.Settings;
using MediatR;
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

            services.UseApizrFor<IReqResService>();
            services.UseApizrCrudFor(optionsBuilder => optionsBuilder.WithMediation().WithOptionalMediation().WithLoggingVerbosity(HttpTracer.HttpMessageParts.All, ApizrLogLevel.High), typeof(User));
            services.UseApizrFor<IHttpBinService>(optionsBuilder => optionsBuilder.WithAuthenticationHandler<IAppSettings>(settings => settings.Token, OnRefreshToken));

            services.AddSingleton<ServiceFactory>(serviceProvider => serviceType =>
            {
                var enumerableType = serviceType
                    .GetInterfaces()
                    .Concat(new[] { serviceType })
                    .FirstOrDefault(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>));

                var typeToResolve = enumerableType != null ? enumerableType.GenericTypeArguments[0] : serviceType;

                object? result = null;
                try
                {
                    result = enumerableType != null
                        ? serviceProvider.GetServices(typeToResolve)
                        : serviceProvider.GetService(typeToResolve);
                }
                catch (Exception)
                {
                    // ignored
                }

                return result ?? Array.CreateInstance(typeToResolve, 0);
            });

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

        private static Task<string?> OnRefreshToken(HttpRequestMessage request)
        {
            return Task.FromResult("tokenValue");
        }
    }
}
