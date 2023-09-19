using System;

namespace Apizr.Cancelling.Attributes
{
    /// <inheritdoc />
    [AttributeUsage(AttributeTargets.Class)]
    public class TimeoutReadAttribute : TimeoutAttributeBase
    {
        /// <inheritdoc />
        public TimeoutReadAttribute(string timeoutRepresentation) : base(timeoutRepresentation)
        {
        }
    }
}
