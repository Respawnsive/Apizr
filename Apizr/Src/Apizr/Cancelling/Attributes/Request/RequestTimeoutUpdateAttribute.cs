using System;

namespace Apizr.Cancelling.Attributes.Request
{
    /// <inheritdoc />
    [AttributeUsage(AttributeTargets.Class)]
    public class UpdateRequestTimeoutAttribute : TimeoutAttributeBase
    {
        /// <inheritdoc />
        public UpdateRequestTimeoutAttribute(string timeoutRepresentation) : base(timeoutRepresentation)
        {
        }
    }
}
