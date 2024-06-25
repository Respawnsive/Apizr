using System;

namespace Apizr.Cancelling.Attributes.Operation
{
    /// <inheritdoc />
    [AttributeUsage(AttributeTargets.Class)]
    public class CreateOperationTimeoutAttribute : TimeoutAttributeBase
    {
        /// <inheritdoc />
        public CreateOperationTimeoutAttribute(string timeoutRepresentation) : base(timeoutRepresentation)
        {
        }
    }
}
