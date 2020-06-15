using System;

namespace Apizr.Logging
{
    /// <summary>
    /// The logging handler method mapping interface
    /// Implement it to provide some logging features to Apizr
    /// </summary>
    public interface ILogHandler
    {
        /// <summary>
        /// Map Apizr log writing to your logging handler
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="description">Some optional description</param>
        /// <param name="parameters">Some optional parameters</param>
        void Write(string message, string description = null, params (string Key, string Value)[] parameters);
    }
}
