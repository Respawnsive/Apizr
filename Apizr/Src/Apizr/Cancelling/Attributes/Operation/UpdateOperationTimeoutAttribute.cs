using System;

namespace Apizr.Cancelling.Attributes.Operation
{
    /// <inheritdoc />
    [AttributeUsage(AttributeTargets.Class)]
    public class UpdateOperationTimeoutAttribute : TimeoutAttributeBase
    {
        /// <inheritdoc />
        public UpdateOperationTimeoutAttribute(string timeoutRepresentation) : base(timeoutRepresentation)
        {
        }
    }
}
