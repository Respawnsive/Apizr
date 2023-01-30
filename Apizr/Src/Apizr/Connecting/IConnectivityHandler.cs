namespace Apizr.Connecting
{
    /// <summary>
    /// The connectivity handler method mapping interface
    /// Implement it to provide some connectivity features to Apizr
    /// </summary>
    public interface IConnectivityHandler
    {
        /// <summary>
        /// Map Apizr connectivity check to your connectivity handler
        /// </summary>
        /// <returns></returns>
        bool IsConnected();
    }
}
