using System;

namespace Apizr.Cancelling.Attributes.Request
{
    /// <inheritdoc />
    [AttributeUsage(AttributeTargets.Class)]
    public class ReadRequestTimeoutAttribute : TimeoutAttributeBase
    {
        /// <inheritdoc />
        public ReadRequestTimeoutAttribute(string timeoutRepresentation) : base(timeoutRepresentation)
        {
        }
    }
}
