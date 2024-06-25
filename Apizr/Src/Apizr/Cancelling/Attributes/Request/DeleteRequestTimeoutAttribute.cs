using System;

namespace Apizr.Cancelling.Attributes.Request
{
    /// <inheritdoc />
    [AttributeUsage(AttributeTargets.Class)]
    public class DeleteRequestTimeoutAttribute : TimeoutAttributeBase
    {
        /// <inheritdoc />
        public DeleteRequestTimeoutAttribute(string timeoutRepresentation) : base(timeoutRepresentation)
        {
        }
    }
}
