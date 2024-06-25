using System;

namespace Apizr.Cancelling.Attributes.Operation
{
    /// <inheritdoc />
    [AttributeUsage(AttributeTargets.Class)]
    public class ReadAllOperationTimeoutAttribute : TimeoutAttributeBase
    {
        /// <inheritdoc />
        public ReadAllOperationTimeoutAttribute(string timeoutRepresentation) : base(timeoutRepresentation)
        {
        }
    }
}
