using Refit;

namespace Apizr.Policing
{
    /// <summary>
    /// The Polly context property attribute
    /// </summary>
    public class ContextAttribute : PropertyAttribute
    {
        /// <summary>
        /// Create a Polly context
        /// </summary>
        public ContextAttribute() : base(Constants.PollyExecutionContextKey)
        {

        }
    }
}
