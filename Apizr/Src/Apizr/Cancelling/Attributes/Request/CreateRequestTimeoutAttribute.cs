using System;

namespace Apizr.Cancelling.Attributes.Request
{
    /// <inheritdoc />
    [AttributeUsage(AttributeTargets.Class)]
    public class CreateRequestTimeoutAttribute : TimeoutAttributeBase
    {
        /// <inheritdoc />
        public CreateRequestTimeoutAttribute(string timeoutRepresentation) : base(timeoutRepresentation)
        {
        }
    }
}
