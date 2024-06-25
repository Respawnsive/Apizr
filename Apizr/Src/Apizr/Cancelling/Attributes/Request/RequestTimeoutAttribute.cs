using System;

namespace Apizr.Cancelling.Attributes.Request
{
    /// <inheritdoc />
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
    public class RequestTimeoutAttribute : TimeoutAttributeBase
    {
        /// <inheritdoc />
        public RequestTimeoutAttribute(string timeoutRepresentation) : base(timeoutRepresentation)
        {
        }
    }
}
