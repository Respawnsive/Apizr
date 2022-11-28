using System;

namespace Apizr.Logging
{
    /// <summary>
    /// The Http tracer logging mode
    /// </summary>
    public enum HttpTracerMode
    {
        Unspecified = 0,

        /// <summary>
        /// Logs given parts only when an exception occurs
        /// </summary>
        ExceptionsOnly = 1,

        /// <summary>
        /// Logs given parts only when an exception or an error occurs
        /// </summary>
        ErrorsAndExceptionsOnly = 2,

        /// <summary>
        /// Logs given parts anytime
        /// </summary>
        Everything = 3
    }
}
