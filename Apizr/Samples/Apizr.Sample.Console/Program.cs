using System;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Integrations.MonkeyCache;
using Apizr.Policing;
using Apizr.Requesting;
using Apizr.Sample.Api;
using Apizr.Sample.Api.Models;
using Microsoft.Extensions.DependencyInjection;
using MonkeyCache.FileStore;
using Polly;
using Polly.Extensions.Http;
using Polly.Registry;

namespace Apizr.Sample.Console
{
    class Program
    {
        private static IApizrManager<IReqResService> _reqResService;
        private static IApizrManager<ICrudApi<UserDetails, int>> _genericUserDetailsManager;

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
                Barrel.ApplicationId = nameof(Program);

                _reqResService = Apizr.For<IReqResService>(optionsBuilder => optionsBuilder.WithPolicyRegistry(registry)
                    .WithCacheHandler(
                        () => new MonkeyCacheHandler(Barrel.Current)));

                _genericUserDetailsManager = Apizr.CrudFor<UserDetails, int>(optionsBuilder => optionsBuilder.WithBaseAddress("https://reqres.in/api/users")
                    .WithPolicyRegistry(registry)
                    .WithCacheHandler(
                        () => new MonkeyCacheHandler(Barrel.Current)));

                System.Console.WriteLine("");
                System.Console.WriteLine("Initialization succeed :)");
            }
            else
            {
                var services = new ServiceCollection();

                services.AddPolicyRegistry(registry);

                services.AddApizr<IReqResService>(optionsBuilder => optionsBuilder.WithCacheHandler<AkavacheCacheHandler>());

                services.AddApizr<UserDetails, int>(optionsBuilder => optionsBuilder.WithBaseAddress("https://reqres.in/api/users")
                    .WithCacheHandler<AkavacheCacheHandler>().WithCrudMediation());

                var container = services.BuildServiceProvider(true);
                var scope = container.CreateScope();

                _reqResService = scope.ServiceProvider.GetRequiredService<IApizrManager<IReqResService>>();
                _genericUserDetailsManager =
                    scope.ServiceProvider.GetRequiredService<IApizrManager<ICrudApi<UserDetails, int>>>();

                System.Console.WriteLine("");
                System.Console.WriteLine("Initialization succeed :)");
            }

            UserList userList;
            try
            {
                System.Console.WriteLine("");
                userList = await _reqResService.ExecuteAsync((ct, api) => api.GetUsersAsync(ct), CancellationToken.None);
            }
            catch (ApizrException<UserList> e)
            {
                System.Console.WriteLine("");
                System.Console.WriteLine(e.Message);

                if (e.CachedResult == null)
                    return;

                System.Console.WriteLine("");
                System.Console.WriteLine($"Loading {nameof(UserList)} from cache...");
                userList = e.CachedResult;
            }

            if (userList?.Data != null)
            {
                System.Console.WriteLine("");
                System.Console.WriteLine("Choose one of available users:");
                foreach (var user in userList.Data)
                {
                    System.Console.WriteLine($"{user.Id} - {user.FirstName} {user.LastName}");
                }

                var readUserChoice = System.Console.ReadLine();
                var userChoice = Convert.ToInt32(readUserChoice);

                UserDetails userDetails;
                try
                {
                    userDetails = await _genericUserDetailsManager.ExecuteAsync((ct, api) => api.Read(userChoice, ct), CancellationToken.None);

                    //userDetails = await _reqResService.ExecuteAsync((ct, api) => api.GetUserAsync(userChoice, ct), CancellationToken.None);
                }
                catch (ApizrException<UserDetails> e)
                {
                    System.Console.WriteLine("");
                    System.Console.WriteLine(e.Message);

                    if (e.CachedResult == null)
                        return;

                    System.Console.WriteLine("");
                    System.Console.WriteLine($"Loading {nameof(UserDetails)} from cache...");
                    userDetails = e.CachedResult;
                }

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
    }
}
