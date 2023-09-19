using System;

namespace Apizr.Cancelling.Attributes
{
    /// <inheritdoc />
    [AttributeUsage(AttributeTargets.Class)]
    public class TimeoutReadAllAttribute : TimeoutAttributeBase
    {
        /// <inheritdoc />
        public TimeoutReadAllAttribute(string timeoutRepresentation) : base(timeoutRepresentation)
        {
        }
    }
}
