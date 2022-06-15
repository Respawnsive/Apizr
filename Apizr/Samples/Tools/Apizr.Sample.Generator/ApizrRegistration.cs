using Apizr;
using Apizr.Configuring.Registry;
using Microsoft.Extensions.DependencyInjection;
using System;
using Apizr.Policing;
using Polly;
using Polly.Extensions.Http;
using Polly.Registry;
using Apizr.Logging.Attributes;
using Apizr.Caching.Attributes;
using Akavache;

[assembly: Policy("TransientHttpError")] // Adjust policies if needed
[assembly: Log] // Adjust log levels if needed
[assembly: Cache] // Adjust cache mode and duration if needed
namespace Test
{
    public static class ApizrRegistration
    {
        // Define your PolicyRegistry
        public static PolicyRegistry ApizrPolicyRegistry = new PolicyRegistry
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

        // Static
        public static IApizrRegistry Build()
        {
            var apizrRegisry = ApizrBuilder.CreateRegistry(
                registry => registry
                    .AddManagerFor<IPetService>()
                    .AddManagerFor<IStoreService>()
                    .AddManagerFor<IUserService>(),
                options => options.WithBaseAddress("https://petstore.swagger.io/v2")
                    .WithPolicyRegistry(ApizrPolicyRegistry)
                    .WithPriorityManagement()
                    .WithAkavacheCacheHandler()
            );

            return apizrRegistry;
        }

        // Extended
        public static IServiceCollection AddApizr(this IServiceCollection services)
        {
            services.AddPolicyRegistry(ApizrPolicyRegistry);

            services.AddApizr(
                registry => registry
                    .AddManagerFor<IPetService>()
                    .AddManagerFor<IStoreService>()
                    .AddManagerFor<IUserService>(),
                options => options.WithBaseAddress("https://petstore.swagger.io/v2")
                    .WithPriorityManagement()
                    .WithAkavacheCacheHandler()
            );

            return services;
        }
    }
}