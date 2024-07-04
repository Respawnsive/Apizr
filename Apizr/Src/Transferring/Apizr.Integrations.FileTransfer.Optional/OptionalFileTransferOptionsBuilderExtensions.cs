using Apizr.Extending.Configuring.Common;
using Apizr.Extending.Configuring.Manager;
using System.IO;
using System.Linq;
using Apizr.Extending;
using Apizr.Transferring.Requesting;
using MediatR;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Apizr.Optional.Requesting;
using Apizr.Optional.Requesting.Handling;
using Optional;
using Apizr.Optional.Requesting.Sending;
using System.Net.Http;

[assembly: Apizr.Preserve]
namespace Apizr
{
    public static class OptionalFileTransferOptionsBuilderExtensions
    {
        /// <summary>
        /// Let Apizr handle file transfer requests management with some mediation and optional result
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <returns></returns>
        public static IApizrExtendedCommonOptionsBuilder WithFileTransferOptionalMediation(this IApizrExtendedCommonOptionsBuilder optionsBuilder)
        {
            WithFileTransferOptionalMediation(optionsBuilder.ApizrOptions);

            return optionsBuilder;
        }
        /// <summary>
        /// Let Apizr handle file transfer requests management with some mediation and optional result
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <returns></returns>
        public static IApizrExtendedManagerOptionsBuilder WithFileTransferOptionalMediation(this IApizrExtendedManagerOptionsBuilder optionsBuilder)
        {
            WithFileTransferOptionalMediation(optionsBuilder.ApizrOptions);

            return optionsBuilder;
        }

        private static void WithFileTransferOptionalMediation(IApizrExtendedCommonOptions commonOptions)
        {
            commonOptions.PostRegistrationActions.Add((managerOptions, services) =>
            {
                if (typeof(ITransferApiBase).IsAssignableFrom(managerOptions.WebApiType))
                {
                    // Register optional mediator
                    services.TryAddSingleton<IApizrOptionalMediator, ApizrOptionalMediator>();

                    // Typed optional mediator
                    var typedOptionalMediatorServiceType = typeof(IApizrOptionalMediator<>).MakeGenericType(managerOptions.WebApiType);
                    var typedOptionalMediatorImplementationType = typeof(ApizrOptionalMediator<>).MakeGenericType(managerOptions.WebApiType);

                    // Register typed optional mediator
                    services.TryAddSingleton(typedOptionalMediatorServiceType, typedOptionalMediatorImplementationType);

                    // Upload
                    if (typeof(IUploadApi).IsAssignableFrom(managerOptions.WebApiType))
                    {
                        var requestType = typeof(UploadOptionalCommand<>).MakeGenericType(managerOptions.WebApiType);
                        var requestHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(Option<HttpResponseMessage, ApizrException>));
                        var requestHandlerImplementationType = typeof(UploadOptionalCommandHandler<>).MakeGenericType(managerOptions.WebApiType);

                        services.TryAddSingleton(requestHandlerServiceType, requestHandlerImplementationType);

                        // Short
                        if (typeof(IUploadApi) == managerOptions || typeof(ITransferApi) == managerOptions)
                        {
                            var shortRequestType = typeof(UploadOptionalCommand);
                            var shortRequestHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(shortRequestType, typeof(Option<HttpResponseMessage, ApizrException>));
                            var shortRequestHandlerImplementationType = typeof(UploadOptionalCommandHandler<>).MakeGenericType(managerOptions.WebApiType);

                            services.TryAddSingleton(shortRequestHandlerServiceType, shortRequestHandlerImplementationType);
                        }
                    }
                    else if (typeof(IUploadApi<>).IsAssignableFromGenericType(managerOptions.WebApiType))
                    {
                        var uploadReturnType = managerOptions.WebApiType.GetInterfaces().FirstOrDefault(type => type.IsGenericType)?.GetGenericArguments().First();
                        var resultType = typeof(Option<,>).MakeGenericType(uploadReturnType, typeof(ApizrException));
                        var requestType = typeof(UploadOptionalCommand<,>).MakeGenericType(managerOptions.WebApiType, uploadReturnType);
                        var requestHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(requestType, resultType);
                        var requestHandlerImplementationType = typeof(UploadOptionalCommandHandler<,>).MakeGenericType(managerOptions.WebApiType, uploadReturnType);

                        services.TryAddSingleton(requestHandlerServiceType, requestHandlerImplementationType);
                        
                        var resultWithType = typeof(Option<,>).MakeGenericType(uploadReturnType, typeof(ApizrException));
                        var requestWithType = typeof(UploadWithOptionalCommand<>).MakeGenericType(uploadReturnType);
                        var requestWithHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(requestWithType, resultWithType);
                        var requestWithHandlerImplementationType = typeof(UploadWithOptionalCommandHandler<>).MakeGenericType(uploadReturnType);

                        services.TryAddSingleton(requestWithHandlerServiceType, requestWithHandlerImplementationType);
                    }

                    // Download
                    if (typeof(IDownloadApi).IsAssignableFrom(managerOptions.WebApiType))
                    {
                        var requestType = typeof(DownloadOptionalQuery<>).MakeGenericType(managerOptions.WebApiType);
                        var requestHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(Option<FileInfo, ApizrException>));
                        var requestHandlerImplementationType = typeof(DownloadOptionalQueryHandler<>).MakeGenericType(managerOptions.WebApiType);

                        services.TryAddSingleton(requestHandlerServiceType, requestHandlerImplementationType);

                        // Short
                        if (typeof(IDownloadApi) == managerOptions || typeof(ITransferApi) == managerOptions)
                        {
                            var shortRequestType = typeof(DownloadOptionalQuery);
                            var shortRequestHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(shortRequestType, typeof(Option<FileInfo, ApizrException>));
                            var shortRequestHandlerImplementationType = typeof(DownloadOptionalQueryHandler<>).MakeGenericType(managerOptions.WebApiType);

                            services.TryAddSingleton(shortRequestHandlerServiceType, shortRequestHandlerImplementationType);
                        }
                    }
                    else if (typeof(IDownloadApi<>).IsAssignableFromGenericType(managerOptions.WebApiType))
                    {
                        var downloadParamsType = managerOptions.WebApiType.GetInterfaces().FirstOrDefault(type => type.IsGenericType)?.GetGenericArguments().First();
                        var requestType = typeof(DownloadOptionalQuery<,>).MakeGenericType(managerOptions.WebApiType, downloadParamsType);
                        var requestHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(Option<FileInfo, ApizrException>));
                        var requestHandlerImplementationType = typeof(DownloadOptionalQueryHandler<,>).MakeGenericType(managerOptions.WebApiType, downloadParamsType);

                        services.TryAddSingleton(requestHandlerServiceType, requestHandlerImplementationType);
                        
                        var requestWithType = typeof(DownloadWithOptionalQuery<>).MakeGenericType(downloadParamsType);
                        var requestWithHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(requestWithType, typeof(Option<FileInfo, ApizrException>));
                        var requestWithHandlerImplementationType = typeof(DownloadWithOptionalQueryHandler<>).MakeGenericType(downloadParamsType);

                        services.TryAddSingleton(requestWithHandlerServiceType, requestWithHandlerImplementationType);
                    }
                }
            });
        }
    }
}
