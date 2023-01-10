using System;
using System.Net.Http.Handlers;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Configuring.Shared;
using Apizr.Policing;

namespace Apizr.Integrations.FileTransfer
{
    public class ApizrProgressHandler : ProgressMessageHandler
    {
        /// <inheritdoc />
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return base.SendAsync(request, cancellationToken);
        }

        /// <inheritdoc />
        protected override void OnHttpRequestProgress(HttpRequestMessage request, HttpProgressEventArgs e)
        {
            base.OnHttpRequestProgress(request, e);

            if(request.TryGetApizrProgress(out var apizrProgress))
                apizrProgress.Report(new ApizrProgressEventArgs(ApizrProgressType.Request, e));
        }

        /// <inheritdoc />
        protected override void OnHttpResponseProgress(HttpRequestMessage request, HttpProgressEventArgs e)
        {
            base.OnHttpResponseProgress(request, e);

            if (request.TryGetApizrProgress(out var apizrProgress))
                apizrProgress.Report(new ApizrProgressEventArgs(ApizrProgressType.Response, e));
        }
    }

    public enum ApizrProgressType
    {
        Request,
        Response
    }

    public class ApizrProgressEventArgs : HttpProgressEventArgs
    {
        public ApizrProgressEventArgs(ApizrProgressType progressType, HttpProgressEventArgs e) : 
            this(progressType, e.ProgressPercentage, e.UserState, e.BytesTransferred, e.TotalBytes)
        {
        }

        /// <inheritdoc />
        public ApizrProgressEventArgs(ApizrProgressType progressType, int progressPercentage, object userToken, long bytesTransferred, long? totalBytes) : base(progressPercentage, userToken, bytesTransferred, totalBytes)
        {
            ProgressType = progressType;
        }

        public ApizrProgressType ProgressType { get; }
    }

    public interface IApizrProgress : IProgress<ApizrProgressEventArgs>
    {

    }

    public class ApizrProgress : Progress<ApizrProgressEventArgs>, IApizrProgress
    {
        public ApizrProgress() : base()
        {
            
        }

        public ApizrProgress(Action<ApizrProgressEventArgs> handler) : base(handler)
        {
            
        }
    }

    public static class HttpRequestMessageExtensions
    {
        public static bool ContainsApizrProgress(this HttpRequestMessage request) =>
            request.TryGetOptions(out var requestOptions) &&
            requestOptions.HandlersParameters.ContainsKey(Constants.ApizrProgressKey);

        public static IApizrProgress GetApizrProgress(this HttpRequestMessage request) =>
            request.Properties.TryGetValue(Constants.ApizrProgressKey, out var optionsProperty) &&
            optionsProperty is IApizrProgress optionsValue
                ? optionsValue
                : null;

        public static bool TryGetApizrProgress(this HttpRequestMessage request, out IApizrProgress apizrProgress)
        {
            apizrProgress = request.GetApizrProgress();
            return apizrProgress != null;
        }
    }

    public static class FileTransferOptionsBuilderExtensions
    {
        public static T WithProgress<T>(this T builder)
            where T : IApizrGlobalSharedOptionsBuilderBase
        {
            if (builder is IApizrInternalRegistrationOptionsBuilder registrationBuilder)
                registrationBuilder.AddDelegatingHandler(_ =>
                    new ApizrProgressHandler());

            return builder;
        }

        public static T WithProgress<T>(this T builder, IApizrProgress progress)
            where T : IApizrGlobalSharedOptionsBuilderBase
        {
            if (builder is IApizrInternalRegistrationOptionsBuilder registrationBuilder)
                registrationBuilder.AddDelegatingHandler(_ =>
                    new ApizrProgressHandler());

            if (builder is IApizrInternalOptionsBuilder voidBuilder)
                voidBuilder.SetHandlerParameter(Constants.ApizrProgressKey, progress);

            return builder;
        }
    }
}