using System;
using System.IO;
using System.Net;
using Microsoft.Extensions.Logging;
using Refit;
using Shiny;

namespace Apizr.Sample.Forms.Infrastructure
{
    public static class AsyncErrorHandler
    {
        private const string DefaultMessage = "Une erreur s'est produite. Réessayez plus tard.";

        //private static IDialogService DialogService => ShinyHost.Resolve<IDialogService>();

        private static ILogger Logger => ShinyHost.LoggerFactory.CreateLogger(typeof(AsyncErrorHandler).AssemblyQualifiedName!);

        public static async void HandleException(Exception exception)
        {
            var message = DefaultMessage;

            if (exception.InnerException is IOException)
                message = "Pas de connexion internet";

            else if (exception.InnerException is ApiException ex)
            {
                if (ex.StatusCode == HttpStatusCode.Unauthorized)
                    message = "L'authentification a échoué";

                //try
                //{
                //    var result = await ex.GetContentAsAsync<ApiResult>();
                //    if (!string.IsNullOrWhiteSpace(result?.Message))
                //        message = result.Message;
                //}
                //catch (Exception) { }
            }
            //Device.BeginInvokeOnMainThread(() =>
            //{
            //    DialogService?.HideLoading();
            //    DialogService?.Toast(message);
            //});

            if (message == DefaultMessage) // Trace only unknown exceptions
                Logger.Log(LogLevel.Critical, exception, exception.Message);
        }
    }
}
