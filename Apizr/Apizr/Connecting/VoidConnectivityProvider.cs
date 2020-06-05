namespace Apizr.Connecting
{
    internal class VoidConnectivityProvider : IConnectivityProvider
    {
        public bool IsConnected() => true;
    }
}