using Apizr;
using Apizr.Configuring.Registry;
using Microsoft.Extensions.DependencyInjection;

namespace Apizr.Tools.Generator.CLI
{
    public static class ApizrRegistration
    {
        // Static
        public static IApizrRegistry Build()
        {
            var apizrRegisry = ApizrBuilder.CreateRegistry(
                registry => registry
                    .AddManagerFor<IPetService>()
                    .AddManagerFor<IStoreService>()
                    .AddManagerFor<IUserService>(),
                options => options.WithBaseAddress("https://petstore.swagger.io/v2")
            );

            return apizrRegistry;
        }

        // Extended
        public static IServiceCollection AddApizr(this IServiceCollection services)
        {
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