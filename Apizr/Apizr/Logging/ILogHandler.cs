using System;

namespace Apizr.Logging
{
    public interface ILogHandler
    {
        void Write(string message, string description = null, params (string Key, string Value)[] parameters);
    }
}
