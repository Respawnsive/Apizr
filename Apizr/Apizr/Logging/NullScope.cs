using System;
using System.Collections.Generic;
using System.Text;

namespace Apizr.Logging
{
    internal sealed class NullScope : IDisposable
    {
        public static IDisposable Instance => new NullScope();
        public void Dispose() { }
    }
}
