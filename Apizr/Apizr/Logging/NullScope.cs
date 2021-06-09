using System;
using System.Collections.Generic;
using System.Text;

namespace Apizr.Logging
{
    public class NullScope : IDisposable
    {
        public static IDisposable Instance { get; } = new NullScope();
        public void Dispose() { }
    }
}
