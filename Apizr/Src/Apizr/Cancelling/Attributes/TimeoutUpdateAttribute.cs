using System;

namespace Apizr.Cancelling.Attributes
{
    /// <inheritdoc />
    [AttributeUsage(AttributeTargets.Class)]
    public class TimeoutUpdateAttribute : TimeoutAttributeBase
    {
        /// <inheritdoc />
        public TimeoutUpdateAttribute(string timeoutRepresentation) : base(timeoutRepresentation)
        {
        }
    }
}
