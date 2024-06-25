using System;

namespace Apizr.Cancelling.Attributes.Operation
{
    /// <inheritdoc />
    [AttributeUsage(AttributeTargets.Class)]
    public class ReadOperationTimeoutAttribute : TimeoutAttributeBase
    {
        /// <inheritdoc />
        public ReadOperationTimeoutAttribute(string timeoutRepresentation) : base(timeoutRepresentation)
        {
        }
    }
}
