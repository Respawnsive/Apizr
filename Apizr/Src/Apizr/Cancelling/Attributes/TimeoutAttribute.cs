using System;

namespace Apizr.Cancelling.Attributes
{
    /// <inheritdoc />
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
    public class TimeoutAttribute : TimeoutAttributeBase
    {
        /// <inheritdoc />
        public TimeoutAttribute(string timeoutRepresentation) : base(timeoutRepresentation)
        {
        }
    }
}
