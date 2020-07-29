using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Integrations.MonkeyCache;
using Apizr.Mediation.Cruding;
using Apizr.Mediation.Requesting;
using Apizr.Mediation.Requesting.Sending;
using Apizr.Optional.Cruding;
using Apizr.Optional.Requesting;
using Apizr.Policing;
using Apizr.Requesting;
using Apizr.Sample.Api;
using Apizr.Sample.Api.Models;
using Apizr.Sample.Console.Models;
using AutoMapper;
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
        private static IApizrManager<ICrudApi<User, int, PagedResult<User>, IDictionary<string, object>>> _genericUserManager;
        private static IMediator _mediator;
        private static IMediator<IReqResService> _reqResMediator;

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

                if (configChoice <= 2)
                {
                    // Manual registration
                    services.AddApizrFor<IReqResService>(optionsBuilder => optionsBuilder.WithCacheHandler<AkavacheCacheHandler>().WithHttpTracing(HttpTracer.HttpMessageParts.All));

                    // Auto assembly detection and registration
                    //services.AddApizrFor(optionsBuilder => optionsBuilder.WithCacheHandler<AkavacheCacheHandler>().WithHttpTracing(HttpTracer.HttpMessageParts.All), typeof(User));

                    if (configChoice == 2)
                    {
                        // Manual registration
                        //services.AddApizrCrudFor<User, int, PagedResult<User>>(optionsBuilder => optionsBuilder.WithBaseAddress("https://reqres.in/api/users").WithCacheHandler<AkavacheCacheHandler>());

                        // Auto assembly detection and registration
                        services.AddApizrCrudFor(optionsBuilder => optionsBuilder.WithCacheHandler<AkavacheCacheHandler>().WithHttpTracing(HttpTracer.HttpMessageParts.All), typeof(User));
                    }
                }
                else
                {
                    if (configChoice == 3)
                    {
                        // Classic auto assembly detection and registration and handling with mediation
                        services.AddApizrFor(optionsBuilder => optionsBuilder.WithCacheHandler<AkavacheCacheHandler>().WithMediation().WithHttpTracing(HttpTracer.HttpMessageParts.All), typeof(User));

                        // Crud manual registration and handling with mediation
                        //services.AddApizrCrudFor<User, int, PagedResult<User>>(optionsBuilder => optionsBuilder.WithBaseAddress("https://reqres.in/api/users").WithCacheHandler<AkavacheCacheHandler>().WithCrudMediation().WithHttpTracing(HttpTracer.HttpMessageParts.All));

                        // Crud auto assembly detection, registration and handling with mediation
                        services.AddApizrCrudFor(optionsBuilder => optionsBuilder.WithCacheHandler<AkavacheCacheHandler>().WithMediation().WithHttpTracing(HttpTracer.HttpMessageParts.All), typeof(User));
                    }
                    else
                    {
                        if(configChoice == 4)
                        {
                            // Classic auto assembly detection and registration and handling with both mediation and optional mediation
                            services.AddApizrFor(optionsBuilder => optionsBuilder.WithCacheHandler<AkavacheCacheHandler>().WithMediation().WithOptionalMediation().WithHttpTracing(HttpTracer.HttpMessageParts.All), typeof(User));

                            // Auto assembly detection, registration and handling with both mediation and optional mediation
                            services.AddApizrCrudFor(optionsBuilder => optionsBuilder.WithCacheHandler<AkavacheCacheHandler>().WithMediation().WithOptionalMediation().WithHttpTracing(HttpTracer.HttpMessageParts.All), typeof(User));
                        }
                        else
                        {
                            // Manual registration
                            //services.AddApizrCrudFor<User, int, PagedResult<User>>(optionsBuilder => optionsBuilder.WithBaseAddress("https://reqres.in/api/users").WithCacheHandler<AkavacheCacheHandler>().WithCrudMediation().WithCrudOptionalMediation().WithHttpTracing(HttpTracer.HttpMessageParts.All));
                            //services.AddApizrCrudFor<MappedEntity<UserInfos, UserDetails>>(optionsBuilder => optionsBuilder.WithBaseAddress("https://reqres.in/api/users").WithCacheHandler<AkavacheCacheHandler>().WithCrudMediation().WithCrudOptionalMediation().WithMappingHandler<AutoMapperMappingHandler>().WithHttpTracing(HttpTracer.HttpMessageParts.All));
                            
                            // Classic auto assembly detection and registration and handling with both mediation and optional mediation
                            services.AddApizrFor(optionsBuilder => optionsBuilder.WithCacheHandler<AkavacheCacheHandler>().WithMediation().WithOptionalMediation().WithMappingHandler<AutoMapperMappingHandler>().WithHttpTracing(HttpTracer.HttpMessageParts.All), typeof(User), typeof(Program));

                            // Auto assembly detection, registration and handling with mediation, optional mediation and mapping
                            services.AddApizrCrudFor(optionsBuilder => optionsBuilder.WithCacheHandler<AkavacheCacheHandler>().WithMediation().WithOptionalMediation().WithMappingHandler<AutoMapperMappingHandler>().WithHttpTracing(HttpTracer.HttpMessageParts.All), typeof(User), typeof(Program));

                            services.AddAutoMapper(typeof(Program));
                        }
                    }

                    // todo: register it on Apizr side
                    services.AddTransient<IMediator<IReqResService>, Mediator<IReqResService>>();

                    services.AddMediatR(typeof(Program));
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
                _reqResMediator = scope.ServiceProvider.GetRequiredService<IMediator<IReqResService>>();

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
                //var userList = await _reqResService.ExecuteAsync((ct, api) => api.GetUsersAsync(ct), CancellationToken.None);
                //users = userList?.Data;
                PagedResult<User> pagedUsers = null;
                if (configChoice <= 2)
                {
                    pagedUsers = await _genericUserManager.ExecuteAsync((ct, api) => api.ReadAll(ct), CancellationToken.None); 
                }
                else if (configChoice == 3)
                {
                    //var userList = await _mediator.Send(new ExecuteRequest<IReqResService, UserList>(api => api.GetUsersAsync()));
                    //var userList = await _reqResMediator.SendFor(api => api.GetUsersAsync());
                    pagedUsers = await _mediator.Send(new ReadAllQuery<PagedResult<User>>(), CancellationToken.None);
                }
                else
                {
                    //var optionalUserList = await _mediator.Send(new ExecuteOptionalRequest<IReqResService, UserList>((ct, api) => api.GetUsersAsync(ct)), CancellationToken.None);
                    //var optionalUserList = await _reqResMediator.SendFor((ct, api) => api.GetUsersAsync(ct), CancellationToken.None);
                    var optionalPagedUsers = await _mediator.Send(new ReadAllOptionalQuery<PagedResult<User>>(), CancellationToken.None);
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

                UserInfos userInfos;
                try
                {
                    if (configChoice <= 4)
                    {
                        var userDetails = configChoice <= 2
                            ? await _reqResService.ExecuteAsync((ct, api) => api.GetUserAsync(userChoice, ct),
                                CancellationToken.None)
                            : await _mediator.Send(new ReadQuery<UserDetails>(userChoice), CancellationToken.None);

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
                    else
                    {
                        // Classic auto mapped request and result
                        //var minUser = new MinUser {Name = "John"};
                        //var createdMinUser = await _mediator.Send(
                        //    new ExecuteRequest<IReqResService, MinUser, User>((ct, api, mapper) =>
                        //        api.CreateUser(mapper.Map<MinUser, User>(minUser), ct)), CancellationToken.None);

                        // Classic Auto mapped result only
                        //userInfos = await _mediator.Send(new ExecuteRequest<IReqResService, UserInfos, UserDetails>((ct, api) => api.GetUserAsync(userChoice, ct)), CancellationToken.None);
                        
                        // Auto mapped crud
                        userInfos = await _mediator.Send(new ReadQuery<UserInfos>(userChoice), CancellationToken.None);
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
