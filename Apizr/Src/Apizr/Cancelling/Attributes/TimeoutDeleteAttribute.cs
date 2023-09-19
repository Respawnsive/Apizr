using System;

namespace Apizr.Cancelling.Attributes
{
    /// <inheritdoc />
    [AttributeUsage(AttributeTargets.Class)]
    public class TimeoutDeleteAttribute : TimeoutAttributeBase
    {
        /// <inheritdoc />
        public TimeoutDeleteAttribute(string timeoutRepresentation) : base(timeoutRepresentation)
        {
        }
    }
}
