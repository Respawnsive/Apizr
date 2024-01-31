using Apizr.Resiliencing;
using Refit;
using System;

namespace Apizr.Policing
{
    /// <summary>
    /// The Polly context property attribute
    /// </summary>
    [Obsolete("Use a Strategy instead")]
    public class ContextAttribute : ResilienceContextAttribute
    {
        /// <summary>
        /// Create a Polly context
        /// </summary>
        public ContextAttribute()
        {

        }
    }
}
