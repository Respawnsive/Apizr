using System.Linq;
using Apizr.Extending;
using Apizr.Extending.Configuring.Common;
using Apizr.Extending.Configuring.Manager;
using Apizr.Mediation.Requesting.Handling;
using Apizr.Mediation.Requesting;
using Apizr.Transferring.Requesting;
using MediatR;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.IO;
using Apizr.Mediation.Requesting.Sending;
using System.Net.Http;

[assembly: Apizr.Preserve]
namespace Apizr
{
    public static class MediationFileTransferOptionsBuilderExtensions
    {
        /// <summary>
        /// Let Apizr handle file transfer requests management with some mediation
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <returns></returns>
        public static IApizrExtendedCommonOptionsBuilder WithFileTransferMediation(this IApizrExtendedCommonOptionsBuilder optionsBuilder)
        {
            WithFileTransferMediation(optionsBuilder.ApizrOptions);

            return optionsBuilder;
        }
        /// <summary>
        /// Let Apizr handle file transfer requests management with some mediation
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <returns></returns>
        public static IApizrExtendedManagerOptionsBuilder WithFileTransferMediation(this IApizrExtendedManagerOptionsBuilder optionsBuilder)
        {
            WithFileTransferMediation(optionsBuilder.ApizrOptions);

            return optionsBuilder;
        }

        private static void WithFileTransferMediation(IApizrExtendedCommonOptions commonOptions)
        {
            commonOptions.PostRegistrationActions.Add((managerOptions, services) =>
            {
                if (typeof(ITransferApiBase).IsAssignableFrom(managerOptions.WebApiType))
                {
                    // Register mediator
                    services.TryAddSingleton<IApizrMediator, ApizrMediator>();

                    // Typed mediator
                    var typedMediatorServiceType = typeof(IApizrMediator<>).MakeGenericType(managerOptions.WebApiType);
                    var typedMediatorImplementationType = typeof(ApizrMediator<>).MakeGenericType(managerOptions.WebApiType);

                    // Register typed mediator
                    services.TryAddSingleton(typedMediatorServiceType, typedMediatorImplementationType);

                    // Upload
                    if (typeof(IUploadApi).IsAssignableFrom(managerOptions.WebApiType))
                    {
                        var requestType = typeof(UploadCommand<>).MakeGenericType(managerOptions.WebApiType);
                        var requestHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(HttpResponseMessage));
                        var requestHandlerImplementationType = typeof(UploadCommandHandler<>).MakeGenericType(managerOptions.WebApiType);

                        services.TryAddSingleton(requestHandlerServiceType, requestHandlerImplementationType);

                        // Short
                        if (typeof(IUploadApi) == managerOptions.WebApiType || typeof(ITransferApi) == managerOptions.WebApiType)
                        {
                            var shortRequestType = typeof(UploadCommand);
                            var shortRequestHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(shortRequestType, typeof(HttpResponseMessage));
                            var shortRequestHandlerImplementationType = typeof(UploadCommandHandler<>).MakeGenericType(managerOptions.WebApiType);

                            services.TryAddSingleton(shortRequestHandlerServiceType, shortRequestHandlerImplementationType);
                        }
                    }
                    else if (typeof(IUploadApi<>).IsAssignableFromGenericType(managerOptions.WebApiType))
                    {
                        var uploadReturnType = managerOptions.WebApiType.GetInterfaces().FirstOrDefault(type => type.IsGenericType)?.GetGenericArguments().First();
                        var requestType = typeof(UploadCommand<,>).MakeGenericType(managerOptions.WebApiType, uploadReturnType);
                        var requestHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(requestType, uploadReturnType);
                        var requestHandlerImplementationType = typeof(UploadCommandHandler<,>).MakeGenericType(managerOptions.WebApiType, uploadReturnType);

                        services.TryAddSingleton(requestHandlerServiceType, requestHandlerImplementationType);
                        
                        var requestWithType = typeof(UploadWithCommand<>).MakeGenericType(uploadReturnType);
                        var requestWithHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(requestWithType, uploadReturnType);
                        var requestWithHandlerImplementationType = typeof(UploadWithCommandHandler<>).MakeGenericType(uploadReturnType);

                        services.TryAddSingleton(requestWithHandlerServiceType, requestWithHandlerImplementationType);
                    }

                    // Download
                    if (typeof(IDownloadApi).IsAssignableFrom(managerOptions.WebApiType))
                    {
                        var requestType = typeof(DownloadQuery<>).MakeGenericType(managerOptions.WebApiType);
                        var requestHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(FileInfo));
                        var requestHandlerImplementationType = typeof(DownloadQueryHandler<>).MakeGenericType(managerOptions.WebApiType);

                        services.TryAddSingleton(requestHandlerServiceType, requestHandlerImplementationType);

                        // Short
                        if (typeof(IDownloadApi) == managerOptions.WebApiType || typeof(ITransferApi) == managerOptions.WebApiType)
                        {
                            var shortRequestType = typeof(DownloadQuery);
                            var shortRequestHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(shortRequestType, typeof(FileInfo));
                            var shortRequestHandlerImplementationType = typeof(DownloadQueryHandler<>).MakeGenericType(managerOptions.WebApiType);

                            services.TryAddSingleton(shortRequestHandlerServiceType, shortRequestHandlerImplementationType);
                        }
                    }
                    else if (typeof(IDownloadApi<>).IsAssignableFromGenericType(managerOptions.WebApiType))
                    {
                        var downloadParamsType = managerOptions.WebApiType.GetInterfaces().FirstOrDefault(type => type.IsGenericType)?.GetGenericArguments().First();
                        var requestType = typeof(DownloadQuery<,>).MakeGenericType(managerOptions.WebApiType, downloadParamsType);
                        var requestHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(FileInfo));
                        var requestHandlerImplementationType = typeof(DownloadQueryHandler<,>).MakeGenericType(managerOptions.WebApiType, downloadParamsType);

                        services.TryAddSingleton(requestHandlerServiceType, requestHandlerImplementationType);
                        
                        var requestWithType = typeof(DownloadWithQuery<>).MakeGenericType(downloadParamsType);
                        var requestWithHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(requestWithType, typeof(FileInfo));
                        var requestWithHandlerImplementationType = typeof(DownloadWithQueryHandler<>).MakeGenericType(downloadParamsType);

                        services.TryAddSingleton(requestWithHandlerServiceType, requestWithHandlerImplementationType);
                    }
                }
            });
        }
    }
}
