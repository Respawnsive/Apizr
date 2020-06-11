using System;

namespace Apizr.Logging
{
    public interface ILogHandler
    {
        void Write(Exception exception, params (string Key, string Value)[] parameters);
        void Write(string title, string description = null, params (string Key, string Value)[] parameters);
    }
}
