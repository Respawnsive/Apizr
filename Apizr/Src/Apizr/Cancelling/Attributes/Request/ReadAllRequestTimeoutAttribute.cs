using System;

namespace Apizr.Cancelling.Attributes.Request
{
    /// <inheritdoc />
    [AttributeUsage(AttributeTargets.Class)]
    public class ReadAllRequestTimeoutAttribute : TimeoutAttributeBase
    {
        /// <inheritdoc />
        public ReadAllRequestTimeoutAttribute(string timeoutRepresentation) : base(timeoutRepresentation)
        {
        }
    }
}
