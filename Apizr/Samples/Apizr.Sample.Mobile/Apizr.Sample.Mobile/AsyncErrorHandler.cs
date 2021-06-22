using System;
using System.IO;
using Acr.UserDialogs;
using Microsoft.Extensions.Logging;
using Shiny;
using Xamarin.Forms;

namespace Apizr.Sample.Mobile
{
    public static class AsyncErrorHandler
    {
        private static ILogger Logger => ShinyHost.Resolve<ILogger>();

        public static void HandleException(Exception exception)
        {
            var message = exception  is IOException || exception.InnerException is IOException ? "No network" : (exception.Message ?? "Error");
            UserDialogs.Instance.Toast(new ToastConfig(message) { BackgroundColor = Color.Red, MessageTextColor = Color.White });

            Logger.LogError(exception, exception.Message);
        }
    }
}
