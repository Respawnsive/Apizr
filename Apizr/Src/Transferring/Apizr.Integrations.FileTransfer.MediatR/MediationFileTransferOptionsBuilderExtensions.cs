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

        private static void WithFileTransferMediation(IApizrExtendedCommonOptions apizrOptions)
        {
            apizrOptions.PostRegistrationActions.Add((webApiType, services) =>
            {
                if (typeof(ITransferApiBase).IsAssignableFrom(webApiType))
                {
                    // Register mediator
                    services.TryAddSingleton<IApizrMediator, ApizrMediator>();

                    // Typed mediator
                    var typedMediatorServiceType = typeof(IApizrMediator<>).MakeGenericType(webApiType);
                    var typedMediatorImplementationType = typeof(ApizrMediator<>).MakeGenericType(webApiType);

                    // Register typed mediator
                    services.TryAddSingleton(typedMediatorServiceType, typedMediatorImplementationType);

                    // Upload
                    if (typeof(IUploadApi).IsAssignableFrom(webApiType))azertyuiop
                    {
                        var requestType = typeof(UploadCommand<>).MakeGenericType(webApiType);
                        var requestHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(Unit));
                        var requestHandlerImplementationType = typeof(UploadCommandHandler<>).MakeGenericType(webApiType);

                        services.TryAddSingleton(requestHandlerServiceType, requestHandlerImplementationType);

                        // Short
                        if (typeof(IUploadApi) == webApiType || typeof(ITransferApi) == webApiType)
                        {
                            var shortRequestType = typeof(UploadCommand);
                            var shortRequestHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(shortRequestType, typeof(Unit));
                            var shortRequestHandlerImplementationType = typeof(UploadCommandHandler<>).MakeGenericType(webApiType);

                            services.TryAddSingleton(shortRequestHandlerServiceType, shortRequestHandlerImplementationType);
                        }
                    }
                    
                    // Download
                    if (typeof(IDownloadApi).IsAssignableFrom(webApiType))
                    {
                        var requestType = typeof(DownloadQuery<>).MakeGenericType(webApiType);
                        var requestHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(FileInfo));
                        var requestHandlerImplementationType = typeof(DownloadQueryHandler<>).MakeGenericType(webApiType);

                        services.TryAddSingleton(requestHandlerServiceType, requestHandlerImplementationType);

                        // Short
                        if (typeof(IDownloadApi) == webApiType || typeof(ITransferApi) == webApiType)
                        {
                            var shortRequestType = typeof(DownloadQuery);
                            var shortRequestHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(shortRequestType, typeof(FileInfo));
                            var shortRequestHandlerImplementationType = typeof(DownloadQueryHandler<>).MakeGenericType(webApiType);

                            services.TryAddSingleton(shortRequestHandlerServiceType, shortRequestHandlerImplementationType);
                        }
                    }
                    else if (typeof(IDownloadApi<>).IsAssignableFromGenericType(webApiType))
                    {
                        var downloadParamsType = webApiType.GetInterfaces().FirstOrDefault(type => type.IsGenericType)?.GetGenericArguments().First();
                        var requestType = typeof(DownloadQuery<,>).MakeGenericType(webApiType, downloadParamsType);
                        var requestHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(FileInfo));
                        var requestHandlerImplementationType = typeof(DownloadQueryHandler<,>).MakeGenericType(webApiType, downloadParamsType);

                        services.TryAddSingleton(requestHandlerServiceType, requestHandlerImplementationType);

                    }
                }
            });
        }
    }
}
