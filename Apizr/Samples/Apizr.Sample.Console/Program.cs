using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Akavache;
using Apizr.Extending;
using Apizr.Logging;
using Apizr.Mediation.Cruding;
using Apizr.Mediation.Cruding.Sending;
using Apizr.Mediation.Requesting;
using Apizr.Mediation.Requesting.Sending;
using Apizr.Optional.Cruding;
using Apizr.Optional.Cruding.Sending;
using Apizr.Optional.Extending;
using Apizr.Optional.Requesting;
using Apizr.Optional.Requesting.Sending;
using Apizr.Policing;
using Apizr.Requesting;
using Apizr.Sample.Api;
using Apizr.Sample.Api.Models;
using Apizr.Sample.Console.Models;
using AutoMapper;
using Fusillade;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging.Debug;
using Microsoft.Extensions.Options;
using MonkeyCache.FileStore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Polly;
using Polly.Extensions.Http;
using Polly.Registry;
using Refit;

namespace Apizr.Sample.Console
{
    class Program
    {
        /*
         * Next are all the ways to play with Apizr
         */

        // With an api interface
        private static IApizrManager<IReqResService> _reqResManager;

        // With an auto-defined cruding api interface based on an entity class
        private static IApizrManager<ICrudApi<User, int, PagedResult<User>, IDictionary<string, object>>> _userManager;

        // With MediatR
        private static IMediator _mediator;

        // With a mediator dedicated to an api interface (getting things shorter)
        private static IMediator<IReqResService> _reqResMediator;

        // With an optional mediator dedicated to an api interface (getting things shorter)
        private static IOptionalMediator<IReqResService> _reqResOptionalMediator;

        // With a crud mediator dedicated to an entity (getting things shorter)
        private static ICrudMediator<User, int, PagedResult<User>, IDictionary<string, object>> _userMediator;

        // With a crud optional mediator dedicated to an entity (getting things shorter)
        private static ICrudOptionalMediator<User, int, PagedResult<User>, IDictionary<string, object>> _userOptionalMediator;

        // Playing with cookies
        public static CookieContainer CookieContainer = new CookieContainer();


        /*
         * Using examples
         */

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
            System.Console.WriteLine("5 - Microsoft extensions with cache and crud optional mapped mediation (Akavache + MediatR + Optional + AutoMapper)");
            System.Console.WriteLine("Your choice : ");
            var readConfigChoice = System.Console.ReadLine();
            var configChoice = Convert.ToInt32(readConfigChoice);

            System.Console.WriteLine("");
            System.Console.WriteLine("Initializing...");
            var policyRegistry = new PolicyRegistry
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

                var lazyLoggerFactory = new Lazy<ILoggerFactory>(() => LoggerFactory.Create(logging =>
                {
                    logging.AddConsole();
                    logging.AddDebug();
                    //logging.SetMinimumLevel(LogLevel.Trace);
                }));

                var apizrRegistry = Apizr.Create(
                    registry => registry
                        .AddFor<IReqResService>(options => options.WithLogging())
                        .AddCrudFor<User, int, PagedResult<User>>(options => options.WithBaseAddress("https://reqres.in/api/users")),

                    config => config
                        .WithPriorityManagement()
                        .WithPolicyRegistry(policyRegistry)
                        .WithCacheHandler(() => new MonkeyCacheHandler(Barrel.Current))
                        .WithLoggerFactory(() => lazyLoggerFactory.Value));

                _reqResManager = apizrRegistry.GetFor<IReqResService>();

                _userManager = apizrRegistry.GetFor<User, int, PagedResult<User>>();

                //_reqResManager = Apizr.CreateFor<IReqResService>(optionsBuilder => optionsBuilder.WithPolicyRegistry(policyRegistry)
                //    .WithCacheHandler(() => new MonkeyCacheHandler(Barrel.Current))
                //    .WithPriorityManagement()
                //    .WithLoggerFactory(() => lazyLoggerFactory.Value)
                //    .WithLogging());

                //_userManager = Apizr.CreateCrudFor<User, int, PagedResult<User>>(optionsBuilder => optionsBuilder
                //    .WithBaseAddress("https://reqres.in/api/users")
                //    .WithPolicyRegistry(policyRegistry)
                //    .WithCacheHandler(() => new MonkeyCacheHandler(Barrel.Current)));
                ////.WithLogging());


                System.Console.WriteLine("");
                System.Console.WriteLine("Initialization succeed :)");
            }
            else
            {
                var host = Host.CreateDefaultBuilder()
                    .ConfigureLogging(logging =>
                    {
                        logging.AddConsole();
                        logging.AddConsole();
                        logging.SetMinimumLevel(LogLevel.Trace);
                    })
                    .ConfigureServices(services =>
                    {
                        services.AddPolicyRegistry(policyRegistry);
                        services.AddMemoryCache();

                        if (configChoice == 2)
                        {
                            services.AddApizr(
                                registry => registry
                                    .AddFor<IReqResService>(options => options
                                        .WithHttpClientHandler(new HttpClientHandler
                                        {
                                            AutomaticDecompression = DecompressionMethods.All,
                                            CookieContainer = CookieContainer
                                        }))
                                    .AddCrudFor(optionsBuilder => optionsBuilder
                                        .WithLogging(), typeof(User)),

                                config => config
                                    .WithPriorityManagement()
                                    //.WithInMemoryCacheHandler()
                                    //.WithCacheHandler<AkavacheCacheHandler>()
                                    .WithAkavacheCacheHandler()
                                    .WithLogging());


                            /*
                            //services.AddRefitClient<IReqResService>();
                            // Manual registration
                            services.AddApizrFor<IReqResService>(optionsBuilder =>
                                optionsBuilder
                                    .WithPriorityManagement()
                                    .WithHttpClientHandler(new HttpClientHandler
                                    {
                                        AutomaticDecompression = DecompressionMethods.All,
                                        CookieContainer = CookieContainer
                                    })
                                    //.WithInMemoryCacheHandler()
                                    //.WithCacheHandler<AkavacheCacheHandler>()
                                    .WithAkavacheCacheHandler()
                                    .WithLogging());

                            //// Auto assembly detection and registration
                            ////services.AddApizrFor(optionsBuilder => optionsBuilder.WithCacheHandler<AkavacheCacheHandler>().WithHttpTracing(HttpTracer.HttpMessageParts.All), typeof(User));

                            // Manual registration
                            //services.AddApizrCrudFor<User, int, PagedResult<User>>(optionsBuilder => optionsBuilder.WithBaseAddress("https://reqres.in/api/users").WithCacheHandler<AkavacheCacheHandler>());

                            // Auto assembly detection and registration
                            services.AddApizrCrudFor(
                                optionsBuilder => optionsBuilder
                                    .WithHttpClientHandler(new HttpClientHandler
                                    {
                                        AutomaticDecompression = DecompressionMethods.All,
                                        CookieContainer = CookieContainer
                                    })//.WithInMemoryCacheHandler()//.WithCacheHandler<AkavacheCacheHandler>()
                                    .WithLogging(), typeof(User));
                            */
                        }
                        else
                        {
                            if (configChoice == 3)
                            {
                                // Classic auto assembly detection and registration and handling with mediation
                                services.AddApizrFor(
                                    optionsBuilder => optionsBuilder
                                        .WithCacheHandler<AkavacheCacheHandler>()
                                        //.WithMediation()
                                        .WithLogging(),
                                    typeof(User));

                                // Crud manual registration and handling with mediation
                                //services.AddApizrCrudFor<User, int, PagedResult<User>>(optionsBuilder => optionsBuilder.WithBaseAddress("https://reqres.in/api/users").WithCacheHandler<AkavacheCacheHandler>().WithCrudMediation().WithHttpTracing(HttpTracer.HttpMessageParts.All));

                                // Crud auto assembly detection, registration and handling with mediation
                                services.AddApizrCrudFor(
                                    optionsBuilder => optionsBuilder
                                        .WithCacheHandler<AkavacheCacheHandler>()
                                        .WithMediation()
                                        .WithLogging(),
                                    typeof(User));
                            }
                            else
                            {
                                if (configChoice == 4)
                                {
                                    // Classic auto assembly detection and registration and handling with both mediation and optional mediation
                                    services.AddApizrFor(
                                        optionsBuilder =>
                                            optionsBuilder.WithCacheHandler<AkavacheCacheHandler>().WithMediation()
                                                .WithOptionalMediation()
                                                .WithLogging(), typeof(User));

                                    // Auto assembly detection, registration and handling with both mediation and optional mediation
                                    services.AddApizrCrudFor(
                                        optionsBuilder =>
                                            optionsBuilder.WithCacheHandler<AkavacheCacheHandler>().WithMediation()
                                                .WithOptionalMediation()
                                                .WithLogging(), typeof(User));
                                }
                                else
                                {
                                    // Manual registration
                                    //services.AddApizrCrudFor<User, int, PagedResult<User>>(optionsBuilder => optionsBuilder.WithBaseAddress("https://reqres.in/api/users").WithCacheHandler<AkavacheCacheHandler>().WithCrudMediation().WithCrudOptionalMediation().WithHttpTracing(HttpTracer.HttpMessageParts.All));
                                    //services.AddApizrCrudFor<MappedEntity<UserInfos, UserDetails>>(optionsBuilder => optionsBuilder.WithBaseAddress("https://reqres.in/api/users").WithCacheHandler<AkavacheCacheHandler>().WithCrudMediation().WithCrudOptionalMediation().WithMappingHandler<AutoMapperMappingHandler>().WithHttpTracing(HttpTracer.HttpMessageParts.All));

                                    // Classic auto assembly detection and registration and handling with both mediation and optional mediation
                                    services.AddApizrFor(
                                        optionsBuilder => optionsBuilder.WithCacheHandler<AkavacheCacheHandler>()
                                            .WithMediation()
                                            .WithOptionalMediation()
                                            .WithMappingHandler<AutoMapperMappingHandler>()
                                            .WithLogging(), typeof(User),
                                        typeof(Program));

                                    // Auto assembly detection, registration and handling with mediation, optional mediation and mapping
                                    services.AddApizrCrudFor(
                                        optionsBuilder => optionsBuilder.WithCacheHandler<AkavacheCacheHandler>()
                                            .WithMediation()
                                            .WithOptionalMediation()
                                            .WithMappingHandler<AutoMapperMappingHandler>()
                                            .WithLogging(), typeof(User),
                                        typeof(Program));

                                    services.AddAutoMapper(typeof(Program));

                                    // This is just to let you know what's registered from/for Apizr and ready to use
                                    foreach (var service in services.Where(d =>
                                        (d.ServiceType != null) ||
                                        (d.ImplementationType != null)))
                                    {
                                        System.Console.WriteLine(
                                            $"Registered {service.Lifetime} service: {service.ServiceType?.GetFriendlyName()} - {service.ImplementationType?.GetFriendlyName()}");
                                    }
                                }
                            }

                            services.AddMediatR(typeof(Program));
                        }
                    }).Build();

                
                var scope = host.Services.CreateScope();

                _reqResManager = scope.ServiceProvider.GetRequiredService<IApizrManager<IReqResService>>();

                if(configChoice == 2)
                    _userManager = scope.ServiceProvider.GetRequiredService<IApizrManager<ICrudApi<User, int, PagedResult<User>, IDictionary<string, object>>>>();
                else
                {
                    _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    _reqResMediator = scope.ServiceProvider.GetRequiredService<IMediator<IReqResService>>();
                    _userMediator = scope.ServiceProvider.GetRequiredService<ICrudMediator<User, int, PagedResult<User>, IDictionary<string, object>>>();

                    if (configChoice >= 4)
                    {
                        _reqResOptionalMediator = scope.ServiceProvider.GetRequiredService<IOptionalMediator<IReqResService>>();
                        _userOptionalMediator = scope.ServiceProvider.GetRequiredService<ICrudOptionalMediator<User, int, PagedResult<User>, IDictionary<string, object>>>();
                    }
                }

                System.Console.WriteLine("");
                System.Console.WriteLine("Initialization succeed :)");
            }

            IEnumerable<User> users = null;
            try
            {
                System.Console.WriteLine("");
                PagedResult<User> pagedUsers = null;
                var parameters1 = new Dictionary<string, object> { { "param1", 1 } };
                var parameters2 = new ReadAllUsersParams("param1", 1);
                var priority = (int)Priority.UserInitiated;
                var cancellationToken = CancellationToken.None;
                if (configChoice <= 2)
                {
                    //var test = new ReadAllUsersParams("value1", 2);

                    //var userList = await _reqResManager.ExecuteAsync(api => api.GetUsersAsync((int)Priority.UserInitiated));
                    var userList = await _reqResManager.ExecuteAsync((ctx, api) => api.GetUsersAsync((int)Priority.UserInitiated, ctx), new Context{{"key1", "value1"}});
                    //var userList = await _reqResManager.ExecuteAsync((ct, api) => api.GetUsersAsync(ct), CancellationToken.None);
                    //var userList = await _reqResManager.ExecuteAsync(api => api.GetUsersAsync(true));
                    //var userList = await _reqResManager.ExecuteAsync(api => api.GetUsersAsync(parameters1));
                    //var userList = await _reqResManager.ExecuteAsync(api => api.GetUsersAsync(parameters2));
                    //var userList = await _reqResManager.ExecuteAsync(api => api.GetUsersAsync(true, parameters1));
                    //var userList = await _reqResManager.ExecuteAsync((ct, api) => api.GetUsersAsync(true, parameters1, ct), CancellationToken.None);
                    //var userList = await _reqResManager.ExecuteAsync((ct, api) => api.GetUsersAsync(true, parameters1, parameters2, priority, ct), cancellationToken);
                    //var userList = await _reqResManager.ExecuteAsync((ct, api) => api.GetUsersAsync(true, new Dictionary<string, object> { { "param1", 1 }, { "param2", 2 } }, new ReadAllUsersParams{Param2 = 4}, (int)Priority.UserInitiated, ct), cancellationToken);
                    //var userList = await _reqResManager.ExecuteAsync((ct, api) => api.GetUsersAsync(parameters1, ct), CancellationToken.None);
                    users = userList?.Data;

                    //pagedUsers = await _userManager.ExecuteAsync(api => api.ReadAll());
                    //pagedUsers = await _userManager.ExecuteAsync(api => api.ReadAll((int)Priority.UserInitiated));
                    //pagedUsers = await _userManager.ExecuteAsync(api => api.ReadAll(parameters1));
                }
                else if (configChoice == 3)
                {
                    //var userList = await _mediator.Send(new ExecuteRequest<IReqResService, UserList>(api => api.GetUsersAsync()));
                    //var userList = await _reqResMediator.SendFor(api => api.GetUsersAsync());
                    //pagedUsers = await _mediator.Send(new ReadAllQuery<PagedResult<User>>(), CancellationToken.None);
                    //pagedUsers = await _userMediator.SendReadAllQuery(parameters1, priority, cancellationToken);
                    pagedUsers = await _userMediator.SendReadAllQuery();
                }
                else
                {
                    //var optionalUserList = await _mediator.Send(new ExecuteOptionalRequest<IReqResService, UserList>((ct, api) => api.GetUsersAsync(ct)), CancellationToken.None);

                    //var optionalUserList = await _reqResMediator.SendFor((ct, api) => api.GetUsersAsync(ct), CancellationToken.None);

                    //await _reqResOptionalMediator.SendFor(api => api.GetUsersAsync()).OnResultAsync(result => { users = result?.Data; });

                    //var userList = await _reqResOptionalMediator.SendFor(api => api.GetUsersAsync()).CatchAsync(e => System.Console.WriteLine(e.Message));
                    //users = userList?.Data;

                    //var optionalPagedUsers = await _mediator.Send(new ReadAllOptionalQuery<PagedResult<User>>(), CancellationToken.None);

                    var optionalPagedUsers = await _userOptionalMediator.SendReadAllOptionalQuery();
                    optionalPagedUsers.Match(some => pagedUsers = some, none => throw none);

                    // I know this is senseless as optional is here to prevent us from throwing,
                    // but for this example we just throw to catch and extract cached data...
                    // Not a real life scenario :)
                }

                users ??= pagedUsers?.Data;
            }
            catch (ApizrException<UserList> e)
            {
                System.Console.WriteLine("");
                System.Console.WriteLine(e.Message);

                if (e.CachedResult == null)
                    return;

                System.Console.WriteLine("");
                System.Console.WriteLine($"Loading {nameof(UserList)} from cache...");
                users = e.CachedResult?.Data;
            }
            catch (ApizrException<PagedResult<User>> e)
            {
                System.Console.WriteLine("");
                System.Console.WriteLine(e.Message);

                if (e.CachedResult == null)
                    return;

                System.Console.WriteLine("");
                System.Console.WriteLine($"Loading {nameof(PagedResult<User>)} from cache...");
                users = e.CachedResult?.Data;
            }
            catch (Exception e)
            {

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

                UserInfos userInfos;
                try
                {
                    if (configChoice <= 4)
                    {
                        var parameters = new Dictionary<string, object>{{ "param1", "1" } };
                        var userDetails = configChoice <= 2
                            ? await _reqResManager.ExecuteAsync((ct, api) => api.GetUserAsync(userChoice, (int)Priority.UserInitiated, ct),
                                CancellationToken.None)
                            : await _mediator.Send(new ReadQuery<UserDetails>(userChoice), CancellationToken.None);

                        userInfos = new UserInfos
                        {
                            Id = userDetails.User.Id,
                            FirstName = userDetails.User.FirstName,
                            LastName = userDetails.User.LastName,
                            Avatar = userDetails.User.Avatar,
                            Email = userDetails.User.Email,
                            Company = userDetails.Ad?.Company,
                            Url = userDetails.Ad?.Url,
                            Text = userDetails.Ad?.Text
                        };
                    }
                    else
                    {
                        // Classic auto mapped request and result
                        //var minUser = new MinUser { Name = "John" };
                        //var createdMinUser = await _mediator.Send(
                        //    new ExecuteRequest<IReqResService, MinUser, User>((ct, api, mapper) =>
                        //        api.CreateUser(mapper.Map<MinUser, User>(minUser), ct)), CancellationToken.None);

                        // Classic Auto mapped result only
                        //userInfos = await _mediator.Send(new ExecuteRequest<IReqResService, UserInfos, UserDetails>((ct, api) => api.GetUserAsync(userChoice, ct)), CancellationToken.None);

                        // Classic dedicated mediator with auto mapped result
                        //userInfos = await _reqResMediator.SendFor<UserInfos, UserDetails>((ct, api) => api.GetUserAsync(userChoice, ct), CancellationToken.None);

                        // Auto mapped crud
                        userInfos = await _mediator.Send(new ReadQuery<UserInfos>(userChoice), CancellationToken.None);

                        // Crud dedicated mediator with auto mapped optional result
                        var optionalUserInfos = await _userOptionalMediator.SendReadOptionalQuery<UserInfos>(userChoice);
                        optionalUserInfos.Match(some => userInfos = some, none => throw none);

                        // I know this is senseless as optional is here to prevent us from throwing,
                        // but for this example we just throw to catch and extract cached data...
                        // Not a real life scenario :)
                    }
                }
                catch (ApizrException<UserInfos> e)
                {
                    System.Console.WriteLine("");
                    System.Console.WriteLine(e.Message);

                    if (e.CachedResult == null)
                        return;

                    System.Console.WriteLine("");
                    System.Console.WriteLine($"Loading {nameof(UserDetails)} from cache...");
                    userInfos = e.CachedResult;
                }
                catch (ApizrException<UserDetails> e)
                {
                    System.Console.WriteLine("");
                    System.Console.WriteLine(e.Message);

                    if (e.CachedResult == null)
                        return;

                    System.Console.WriteLine("");
                    System.Console.WriteLine($"Loading {nameof(UserDetails)} from cache...");
                    var userDetails = e.CachedResult;
                    userInfos = new UserInfos
                    {
                        Id = userDetails.User.Id,
                        FirstName = userDetails.User.FirstName,
                        LastName = userDetails.User.LastName,
                        Avatar = userDetails.User.Avatar,
                        Email = userDetails.User.Email,
                        Company = userDetails.Ad.Company,
                        Url = userDetails.Ad.Url,
                        Text = userDetails.Ad.Text
                    };
                }

                if (userInfos != null)
                {
                    System.Console.WriteLine("");
                    System.Console.WriteLine($"{nameof(userInfos.Id)}: {userInfos.Id}");
                    System.Console.WriteLine($"{nameof(userInfos.FirstName)}: {userInfos.FirstName}");
                    System.Console.WriteLine($"{nameof(userInfos.LastName)}: {userInfos.LastName}");
                    System.Console.WriteLine($"{nameof(userInfos.Avatar)}: {userInfos.Avatar}");
                    System.Console.WriteLine($"{nameof(userInfos.Company)}: {userInfos.Company}");
                    System.Console.WriteLine($"{nameof(userInfos.Url)}: {userInfos.Url}");
                    System.Console.WriteLine($"{nameof(userInfos.Text)}: {userInfos.Text}");
                }
            }
        }
    }
}
