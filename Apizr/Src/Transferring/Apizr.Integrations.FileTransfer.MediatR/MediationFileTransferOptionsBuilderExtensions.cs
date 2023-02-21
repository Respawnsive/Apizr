using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Apizr.Extending;
using System.Threading.Tasks;
using Apizr.Extending.Configuring.Common;
using Apizr.Extending.Configuring.Manager;
using Apizr.Mapping;
using Apizr.Mediation.Configuring.Registry;
using Apizr.Mediation.Cruding.Handling;
using Apizr.Mediation.Cruding.Sending;
using Apizr.Mediation.Cruding;
using Apizr.Mediation.Requesting.Handling;
using Apizr.Mediation.Requesting.Sending;
using Apizr.Mediation.Requesting;
using Apizr.Transferring.Requesting;
using MediatR;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Refit;
using System.IO;

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
                    if (typeof(IUploadApi) == webApiType)
                    {
                        var shortRequestType = typeof(UploadCommand);
                        var shortRequestHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(shortRequestType, typeof(Unit));
                        var shortRequestHandlerImplementationType = typeof(UploadCommandHandler<IUploadApi>);

                        services.TryAddSingleton(shortRequestHandlerServiceType, shortRequestHandlerImplementationType);
                    }
                    else if (typeof(IUploadApi).IsAssignableFromGenericType(webApiType))
                    {
                        var requestType = typeof(UploadCommand<>).MakeGenericType(webApiType);
                        var requestHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(Unit));
                        var requestHandlerImplementationType = typeof(UploadCommandHandler<>).MakeGenericType(webApiType);

                        services.TryAddSingleton(requestHandlerServiceType, requestHandlerImplementationType);
                    }
                    else if (typeof(IDownloadApi) == webApiType)
                    {
                        var shortRequestType = typeof(DownloadQuery);
                        var shortRequestHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(shortRequestType, typeof(FileInfo));
                        var shortRequestHandlerImplementationType = typeof(DownloadQueryHandler<IDownloadApi>);

                        services.TryAddSingleton(shortRequestHandlerServiceType, shortRequestHandlerImplementationType);
                    }
                    else if (typeof(IDownloadApi).IsAssignableFrom(webApiType))
                    {
                        var requestType = typeof(DownloadQuery<>).MakeGenericType(webApiType);
                        var requestHandlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(FileInfo));
                        var requestHandlerImplementationType = typeof(DownloadQueryHandler<>).MakeGenericType(webApiType);

                        services.TryAddSingleton(requestHandlerServiceType, requestHandlerImplementationType);
                    }
                    else if (typeof(IDownloadApi<>).IsAssignableFromGenericType(webApiType))
                    {
                        var downloadParamsType = webApiType.GetInterfaces()
                            .FirstOrDefault(type => type.IsGenericType)
                            ?.GetGenericArguments().First();
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
