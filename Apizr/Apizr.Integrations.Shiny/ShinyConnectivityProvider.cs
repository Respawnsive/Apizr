using Apizr.Connecting;
using Shiny.Net;

namespace Apizr
{
    public class ShinyConnectivityProvider : IConnectivityProvider
    {
        private readonly IConnectivity _shinyConnectivity;

        public ShinyConnectivityProvider(IConnectivity shinyConnectivity)
        {
            _shinyConnectivity = shinyConnectivity;
        }

        public bool IsConnected() => _shinyConnectivity.IsInternetAvailable();
    }
}
