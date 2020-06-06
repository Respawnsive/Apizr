namespace Apizr.Connecting
{
    public class VoidConnectivityProvider : IConnectivityProvider
    {
        public bool IsConnected() => true;
    }
}