using Apizr.Connecting;
using Shiny.Net;

namespace Apizr
{
    public class ShinyConnectivityHandler : IConnectivityHandler
    {
        private readonly IConnectivity _shinyConnectivity;

        public ShinyConnectivityHandler(IConnectivity shinyConnectivity)
        {
            _shinyConnectivity = shinyConnectivity;
        }

        public bool IsConnected() => _shinyConnectivity.IsInternetAvailable();
    }
}
