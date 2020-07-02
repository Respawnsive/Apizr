using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Integrations.MonkeyCache;
using Apizr.Mediation.Cruding;
using Apizr.Mediation.Cruding.Handling;
using Apizr.Optional.Cruding;
using Apizr.Optional.Cruding.Handling;
using Apizr.Policing;
using Apizr.Requesting;
using Apizr.Sample.Api;
using Apizr.Sample.Api.Models;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MonkeyCache.FileStore;
using Optional;
using Polly;
using Polly.Extensions.Http;
using Polly.Registry;

namespace Apizr.Sample.Console
{
    class Program
    {
        private static IApizrManager<IReqResService> _reqResService;
        private static IApizrManager<ICrudApi<User, int, PagedResult<User>, IDictionary<string, object>>> _genericUserManager;
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
            System.Console.WriteLine("1 - Static instance with cache (MonkeyCache)");
            System.Console.WriteLine("2 - Microsoft extensions with cache (Akavache)");
            System.Console.WriteLine("3 - Microsoft extensions with cache and crud mediation (Akavache + MediatR)");
            System.Console.WriteLine("4 - Microsoft extensions with cache and crud optional mediation (Akavache + MediatR + Optional)");
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
                    .WithCacheHandler(() => new MonkeyCacheHandler(Barrel.Current))
                    .WithHttpTracing(HttpTracer.HttpMessageParts.All));


                System.Console.WriteLine("");
                System.Console.WriteLine("Initialization succeed :)");
            }
            else
            {
                var services = new ServiceCollection();

                services.AddPolicyRegistry(registry);

                services.AddApizrFor<IReqResService>(optionsBuilder => optionsBuilder.WithCacheHandler<AkavacheCacheHandler>().WithHttpTracing(HttpTracer.HttpMessageParts.All));

                if (configChoice == 2)
                {
                    // Manual registration
                    //services.AddApizrCrudFor<User, int, PagedResult<User>>(optionsBuilder => optionsBuilder.WithBaseAddress("https://reqres.in/api/users").WithCacheHandler<AkavacheCacheHandler>());

                    // Auto assembly detection and registration
                    services.AddApizrCrudFor(optionsBuilder => optionsBuilder.WithCacheHandler<AkavacheCacheHandler>().WithHttpTracing(HttpTracer.HttpMessageParts.All), typeof(User));
                }
                else
                {
                    if(configChoice == 3)
                        // Auto assembly detection, registration and handling with mediation
                        services.AddApizrCrudFor(optionsBuilder => optionsBuilder.WithCacheHandler<AkavacheCacheHandler>().WithCrudMediation().WithHttpTracing(HttpTracer.HttpMessageParts.All), typeof(User));
                    else
                        // Auto assembly detection, registration and handling with optional mediation
                        services.AddApizrCrudFor(optionsBuilder => optionsBuilder.WithCacheHandler<AkavacheCacheHandler>().WithCrudOptionalMediation().WithHttpTracing(HttpTracer.HttpMessageParts.All), typeof(User));

                    services.AddMediatR(typeof(Program));

                    //// Read
                    //services
                    //    .AddTransient<IRequestHandler<ReadQuery<User>, User>,
                    //        ReadQueryHandler<User, IEnumerable<User>, IDictionary<string, object>>>();
                    //services
                    //    .AddTransient<IRequestHandler<ReadOptionalQuery<User>, Option<User, ApizrException<User>>>,
                    //        ReadOptionalQueryHandler<User, IEnumerable<User>, IDictionary<string, object>>>();

                    //// ReadAll
                    //services
                    //    .AddTransient<IRequestHandler<ReadAllQuery<IEnumerable<User>>, IEnumerable<User>>,
                    //        ReadAllQueryHandler<User, int, IEnumerable<User>>>();
                    //services
                    //    .AddTransient<IRequestHandler<ReadAllOptionalQuery<IEnumerable<User>>, Option<IEnumerable<User>, ApizrException<IEnumerable<User>>>>,
                    //        ReadAllOptionalQueryHandler<User, int, IEnumerable<User>>>();

                    //// Create
                    //services
                    //    .AddTransient<IRequestHandler<CreateCommand<User>, User>,
                    //        CreateCommandHandler<User, int, IEnumerable<User>, IDictionary<string, object>>>();
                    //services
                    //    .AddTransient<IRequestHandler<CreateOptionalCommand<User>, Option<User, ApizrException>>,
                    //        CreateOptionalCommandHandler<User, int, IEnumerable<User>, IDictionary<string, object>>>();

                    //// Update
                    //services
                    //    .AddTransient<IRequestHandler<UpdateCommand<User>, Unit>,
                    //        UpdateCommandHandler<User, int, IEnumerable<User>, IDictionary<string, object>>>();
                    //services
                    //    .AddTransient<IRequestHandler<UpdateOptionalCommand<User>, Option<Unit, ApizrException>>,
                    //        UpdateOptionalCommandHandler<User, int, IEnumerable<User>, IDictionary<string, object>>>();

                    //// Delete
                    //services
                    //    .AddTransient<IRequestHandler<DeleteCommand<User>, Unit>,
                    //        DeleteCommandHandler<User, IEnumerable<User>, IDictionary<string, object>>>();
                    //services
                    //    .AddTransient<IRequestHandler<DeleteOptionalCommand<User>, Option<Unit, ApizrException>>,
                    //        DeleteOptionalCommandHandler<User, IEnumerable<User>, IDictionary<string, object>>>();
                }

                // This is just to let you know what's registered from/for Apizr and ready to use
                foreach (var service in services.Where(d =>
                    (d.ServiceType != null && d.ServiceType.Assembly.FullName.Contains($"{nameof(Apizr)}")) ||
                    (d.ImplementationType != null && d.ImplementationType.Assembly.FullName.Contains($"{nameof(Apizr)}"))))
                {
                    System.Console.WriteLine(
                        $"Registered service: {service.ServiceType?.GetFriendlyName()} - {service.ImplementationType?.GetFriendlyName()}");
                }


                var container = services.BuildServiceProvider(true);
                var scope = container.CreateScope();

                _reqResService = scope.ServiceProvider.GetRequiredService<IApizrManager<IReqResService>>();

                if(configChoice == 2)
                    _genericUserManager = scope.ServiceProvider.GetRequiredService<IApizrManager<ICrudApi<User, int, PagedResult<User>, IDictionary<string, object>>>>();
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
                PagedResult<User> pagedUsers = null;
                if (configChoice <= 2)
                {
                    pagedUsers = await _genericUserManager.ExecuteAsync((ct, api) => api.ReadAll(ct), CancellationToken.None); 
                }
                else if (configChoice == 3)
                {
                    pagedUsers = await _mediator.Send(new ReadAllQuery<PagedResult<User>>());
                }
                else
                {
                    var optionalPagedUsers = await _mediator.Send(new ReadAllOptionalQuery<PagedResult<User>>());
                    optionalPagedUsers.Match(some => pagedUsers = some, none => throw none); 
                    //I know this is senseless as optional is here to prevent us from throwing, but for this sample...
                }
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
