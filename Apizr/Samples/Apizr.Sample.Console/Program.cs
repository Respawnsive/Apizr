using System;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Policing;
using Apizr.Sample.Api;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Polly.Registry;

namespace Apizr.Sample.Console
{
    class Program
    {
        private static IApizrManager<IReqResService> _reqResService;

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
                    }, LoggedPolicies.OnLoggedRetry).WithPolicyKey("TransientHttpError")
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

                services.AddPolicyRegistry(registry);
                services.AddApizr<IReqResService>(optionsBuilder => optionsBuilder.WithCacheHandler<AkavacheCacheHandler>());

                var container = services.BuildServiceProvider(true);
                var scope = container.CreateScope();

                _reqResService = scope.ServiceProvider.GetRequiredService<IApizrManager<IReqResService>>();

                System.Console.WriteLine("");
                System.Console.WriteLine("Initialization succeed :)");
            }

            try
            {
                System.Console.WriteLine("");
                var cancellationToken = CancellationToken.None;
                var userList = await _reqResService.ExecuteAsync((ct, api) => api.GetUsersAsync(ct), cancellationToken);
                if (userList.Data != null)
                {
                    System.Console.WriteLine("Choose one of available users:");
                    foreach (var user in userList.Data)
                    {
                        System.Console.WriteLine($"{user.Id} - {user.FirstName} {user.LastName}");
                    }

                    var readUserChoice = System.Console.ReadLine();
                    var userChoice = Convert.ToInt32(readUserChoice);
                    var userDetails = await _reqResService.ExecuteAsync((ct, api) => api.GetUserAsync(userChoice, ct),
                        cancellationToken);
                    if (userDetails != null)
                    {
                        System.Console.WriteLine("");
                        System.Console.WriteLine($"{nameof(userDetails.User.Id)}: {userDetails.User.Id}");
                        System.Console.WriteLine($"{nameof(userDetails.User.FirstName)}: {userDetails.User.FirstName}");
                        System.Console.WriteLine($"{nameof(userDetails.User.LastName)}: {userDetails.User.LastName}");
                        System.Console.WriteLine($"{nameof(userDetails.User.Avatar)}: {userDetails.User.Avatar}");
                        System.Console.WriteLine($"{nameof(userDetails.Ad.Company)}: {userDetails.Ad.Company}");
                        System.Console.WriteLine($"{nameof(userDetails.Ad.Url)}: {userDetails.Ad.Url}");
                        System.Console.WriteLine($"{nameof(userDetails.Ad.Text)}: {userDetails.Ad.Text}");
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
