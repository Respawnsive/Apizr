using System;

namespace Apizr.Connecting
{
    /// <summary>
    /// The connectivity handler method mapping default implementation
    /// </summary>
    public class DefaultConnectivityHandler : IConnectivityHandler
    {
        private readonly Func<bool> _connectivityChecker;

        /// <summary>
        /// The connectivity handler constructor
        /// </summary>
        /// <param name="connectivityChecker"></param>
        public DefaultConnectivityHandler(Func<bool> connectivityChecker)
        {
            _connectivityChecker = connectivityChecker;
        }
        
        /// <inheritdoc />
        public bool IsConnected() => _connectivityChecker.Invoke();
    }
}
