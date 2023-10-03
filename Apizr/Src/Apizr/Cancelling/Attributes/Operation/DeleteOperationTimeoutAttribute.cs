using System;

namespace Apizr.Cancelling.Attributes.Operation
{
    /// <inheritdoc />
    [AttributeUsage(AttributeTargets.Class)]
    public class DeleteOperationTimeoutAttribute : TimeoutAttributeBase
    {
        /// <inheritdoc />
        public DeleteOperationTimeoutAttribute(string timeoutRepresentation) : base(timeoutRepresentation)
        {
        }
    }
}
