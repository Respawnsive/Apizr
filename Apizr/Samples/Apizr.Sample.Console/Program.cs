using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Sample.Api;
using Apizr.Sample.Api.Models;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Polly.Registry;

namespace Apizr.Sample.Console
{
    class Program
    {
        private static IApizrManager<IReqResService> _reqResService;
        private static IApizrManager<IHttpBinService> _httpBinService;

        static async Task Main(string[] args)
        {
            System.Console.WriteLine("");
            System.Console.WriteLine("");
            System.Console.WriteLine("########################################################################");
            System.Console.WriteLine($"Welcome to Apizr sample Console !!!");
            System.Console.WriteLine("########################################################################");
            System.Console.WriteLine("");
            System.Console.WriteLine("Choose one of available configurations:");
            System.Console.WriteLine("1 - Static instance");
            System.Console.WriteLine("2 - Microsoft extensions");
            System.Console.WriteLine("Your choice : ");
            var readConfigChoice = System.Console.ReadLine();
            var configChoice = Convert.ToInt32(readConfigChoice);

            System.Console.WriteLine("");
            System.Console.WriteLine("Initializing...");
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

            if (configChoice == 1)
            {
                _reqResService = Apizr.For<IReqResService>(optionsBuilder => optionsBuilder.WithPolicyRegistry(registry));

                System.Console.WriteLine("");
                System.Console.WriteLine("Initialization succeed :)");
            }
            else
            {
                var services = new ServiceCollection();

                services.AddSingleton<IReadOnlyPolicyRegistry<string>>(registry);
                services.AddApizr<IReqResService>();

                var container = services.BuildServiceProvider(true);
                var scope = container.CreateScope();

                _reqResService = scope.ServiceProvider.GetRequiredService<IApizrManager<IReqResService>>();

                System.Console.WriteLine("");
                System.Console.WriteLine("Initialization succeed :)");
            }

            try
            {
                System.Console.WriteLine("");
                var userList = await _reqResService.ExecuteAsync(api => api.GetUsersAsync());
                if (userList.Data != null)
                {
                    foreach (var user in userList.Data)
                    {
                        System.Console.WriteLine($"User {user.Id}: {user.FirstName} {user.LastName}");
                    }
                }
            }
            catch (ApizrException e)
            {
                System.Console.WriteLine("");
                System.Console.WriteLine(e.Message);
            }
        }
    }
}
