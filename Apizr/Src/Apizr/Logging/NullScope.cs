using System;

namespace Apizr.Logging
{
    internal sealed class NullScope : IDisposable
    {
        public static IDisposable Instance => new NullScope();
        public void Dispose() { }
    }
}
