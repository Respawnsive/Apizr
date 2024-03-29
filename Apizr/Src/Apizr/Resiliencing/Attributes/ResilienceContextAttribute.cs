﻿using Refit;

namespace Apizr.Resiliencing.Attributes
{
    /// <summary>
    /// The Polly's resilience context property attribute
    /// </summary>
    public class ResilienceContextAttribute : PropertyAttribute
    {
        /// <summary>
        /// Create a Polly context
        /// </summary>
        public ResilienceContextAttribute() : base(Constants.ResilienceContextKey)
        {

        }
    }
}
