using System;
using System.Collections.Generic;
using System.Text;

namespace Apizr.Cancelling.Attributes
{
    /// <summary>
    /// Tells Apizr to set a specific timeout value
    /// </summary>
    public abstract class TimeoutAttributeBase : Attribute
    {
        /// <summary>
        /// This specific request timeout
        /// </summary>
        public TimeSpan? Timeout { get; }

        /// <summary>
        /// Request times out after a specific duration
        /// </summary>
        /// <param name="timeoutRepresentation">TimeSpan representation to parse</param>
        protected TimeoutAttributeBase(string timeoutRepresentation)
        {
            Timeout = TimeSpan.Parse(timeoutRepresentation);
        }
    }
}
