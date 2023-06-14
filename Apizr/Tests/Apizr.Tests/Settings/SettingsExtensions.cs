using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Apizr.Tests.Settings
{
    public static class SettingsExtensions
    {
        public static IServiceCollection AddSettings(this IServiceCollection services)
        {
            // Json settings (loaded from embedded json settings files and bound to properties)
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{typeof(SettingsExtensions).Namespace}.appsettings.json");
            if (stream != null)
            {
                var config = new ConfigurationBuilder()
                    .AddJsonStream(stream)
                    .Build();

                // Add all settings sections here
                services.AddSettings<TestSettings>(config);
            }

            return services;
        }

        private static IServiceCollection AddSettings<TSettings>(this IServiceCollection services,
            IConfiguration config) where TSettings : class
            => services.AddSettings<TSettings>(config, typeof(TSettings).Name);

        private static IServiceCollection AddSettings<TSettings>(this IServiceCollection services,
            IConfiguration config, string sectionName) where TSettings : class =>
            services.Configure<TSettings>(config.GetSection(sectionName),
                    option => option.BindNonPublicProperties = true);
    }
}
