using System;
using Apizr.Logging;
using Shiny.Logging;

namespace Apizr
{
    public class ShinyLogHandler : ILogHandler
    {
        public void Write(Exception exception, params (string Key, string Value)[] parameters)
        {
            Log.Write(exception, parameters);
        }

        public void Write(string eventName, string description, params (string Key, string Value)[] parameters)
        {
            Log.Write(eventName, description, parameters);
        }
    }
}
