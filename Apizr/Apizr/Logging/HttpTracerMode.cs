namespace Apizr.Logging
{
    /// <summary>
    /// The Http tracer logging mode
    /// </summary>
    public enum HttpTracerMode
    {
        /// <summary>
        /// Logs given parts only when an exception occurs
        /// </summary>
        ExceptionsOnly,

        /// <summary>
        /// Logs given parts only when an exception or an error occurs
        /// </summary>
        ErrorsAndExceptionsOnly,

        /// <summary>
        /// Logs given parts anytime
        /// </summary>
        Everything
    }
}
