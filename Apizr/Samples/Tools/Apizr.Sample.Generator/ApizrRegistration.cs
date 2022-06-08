using Apizr;
using Apizr.Configuring.Registry;
using Microsoft.Extensions.DependencyInjection;
using System;
using Apizr.Policing;
using Polly;
using Polly.Extensions.Http;
using Polly.Registry;

[assembly: Policy("TransientHttpError")]
namespace Test
{
    public static class ApizrRegistration
    {
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

        public static IApizrRegistry Build() =>
            ApizrBuilder.CreateRegistry(
                registry => registry
                    .AddManagerFor<IPetService>()
                    .AddManagerFor<IStoreService>()
                    .AddManagerFor<IUserService>(),
                options => options.WithBaseAddress("https://petstore.swagger.io/v2")
                    .WithPolicyRegistry(ApizrPolicyRegistry)
            );

        public static IServiceCollection AddApizr(this IServiceCollection services)
        {
            services.AddPolicyRegistry(ApizrPolicyRegistry);

            services.AddApizr(
                registry => registry
                    .AddManagerFor<IPetService>()
                    .AddManagerFor<IStoreService>()
                    .AddManagerFor<IUserService>(),
                options => options.WithBaseAddress("https://petstore.swagger.io/v2")
            );

            return services;
        }
    }
}