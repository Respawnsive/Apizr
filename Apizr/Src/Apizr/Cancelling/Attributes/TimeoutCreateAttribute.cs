using System;
using System.Collections.Generic;
using System.Text;

namespace Apizr.Cancelling.Attributes
{
    /// <inheritdoc />
    [AttributeUsage(AttributeTargets.Class)]
    public class TimeoutCreateAttribute : TimeoutAttributeBase
    {
        /// <inheritdoc />
        public TimeoutCreateAttribute(string timeoutRepresentation) : base(timeoutRepresentation)
        {
        }
    }
}
