using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Integrations.MonkeyCache;
using Apizr.Mediation.Cruding;
using Apizr.Mediation.Cruding.Handling;
using Apizr.Policing;
using Apizr.Requesting;
using Apizr.Sample.Api;
using Apizr.Sample.Api.Models;
using MediatR;
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
        private static IApizrManager<ICrudApi<User, int, PagedResult<User>>> _genericUserManager;
        private static IMediator _mediator;

        static async Task Main(string[] args)
        {
            System.Console.WriteLine("");
            System.Console.WriteLine("");
            System.Console.WriteLine("########################################################################");
            System.Console.WriteLine($"Welcome to Apizr sample Console !!!");
            System.Console.WriteLine("########################################################################");
            System.Console.WriteLine("");
            System.Console.WriteLine("Choose one of available configurations:");
            System.Console.WriteLine("1 - Static instance with MonkeyCache");
            System.Console.WriteLine("2 - Microsoft extensions with Akavache");
            System.Console.WriteLine("3 - Microsoft extensions with Akavache and crud auto mediation");
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

                _genericUserManager = Apizr.CrudFor<User, int, PagedResult<User>>(optionsBuilder => optionsBuilder.WithBaseAddress("https://reqres.in/api/users")
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

                services.AddApizrFor<IReqResService>(optionsBuilder => optionsBuilder.WithCacheHandler<AkavacheCacheHandler>());

                if (configChoice == 2)
                {
                    // Manual registration
                    //services.AddApizrCrudFor<User, int, PagedResult<User>>(optionsBuilder => optionsBuilder.WithBaseAddress("https://reqres.in/api/users").WithCacheHandler<AkavacheCacheHandler>());

                    // Auto assembly detection and registration
                    services.AddApizrCrudFor(optionsBuilder => optionsBuilder.WithCacheHandler<AkavacheCacheHandler>(), typeof(User));
                }
                else
                {
                    // Auto assembly detection, registration and handling with mediation
                    services.AddApizrCrudFor(optionsBuilder => optionsBuilder.WithCacheHandler<AkavacheCacheHandler>().WithCrudMediation(), typeof(User));
                    services.AddMediatR(typeof(Program));
                }
                

                var container = services.BuildServiceProvider(true);
                var scope = container.CreateScope();

                _reqResService = scope.ServiceProvider.GetRequiredService<IApizrManager<IReqResService>>();

                if(configChoice == 2)
                    _genericUserManager = scope.ServiceProvider.GetRequiredService<IApizrManager<ICrudApi<User, int, PagedResult<User>>>>();
                else
                    _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                System.Console.WriteLine("");
                System.Console.WriteLine("Initialization succeed :)");
            }

            IEnumerable<User> users;
            try
            {
                System.Console.WriteLine("");
                //var userList = await _reqResService.ExecuteAsync((ct, api) => api.GetUsersAsync(ct),
                //    CancellationToken.None);
                //users = userList?.Data;
                var pagedUsers = configChoice <= 2
                    ? await _genericUserManager.ExecuteAsync((ct, api) => api.ReadAll(ct), CancellationToken.None)
                    : await _mediator.Send(new ReadAllQuery<PagedResult<User>>());
                users = pagedUsers?.Data;
            }
            catch (ApizrException<PagedResult<User>> e)
            {
                System.Console.WriteLine("");
                System.Console.WriteLine(e.Message);

                if (e.CachedResult == null)
                    return;

                System.Console.WriteLine("");
                System.Console.WriteLine($"Loading {nameof(UserList)} from cache...");
                users = e.CachedResult?.Data;
            }

            if (users != null)
            {
                System.Console.WriteLine("");
                System.Console.WriteLine("Choose one of available users:");
                foreach (var user in users)
                {
                    System.Console.WriteLine($"{user.Id} - {user.FirstName} {user.LastName}");
                }

                var readUserChoice = System.Console.ReadLine();
                var userChoice = Convert.ToInt32(readUserChoice);

                UserDetails userDetails;
                try
                {
                    userDetails = configChoice <= 2
                        ? await _reqResService.ExecuteAsync((ct, api) => api.GetUserAsync(userChoice, ct),
                            CancellationToken.None)
                        : await _mediator.Send(new ReadQuery<UserDetails, int>(userChoice), CancellationToken.None);

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
