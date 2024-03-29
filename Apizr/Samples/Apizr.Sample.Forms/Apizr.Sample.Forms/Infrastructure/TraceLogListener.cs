﻿#if DEBUG
using System.Diagnostics;
using Xamarin.Forms.Internals;

namespace Apizr.Sample.Forms.Infrastructure
{
    public class TraceLogListener : LogListener
    {
        public override void Warning(string category, string message) =>
            Trace.WriteLine($"  {category}: {message}");
    }
}
#endif