using Refit;

namespace Apizr.Configuring.Request
{
    /// <summary>
    /// The Apizr request options property attribute
    /// </summary>
    public class OptionsAttribute : PropertyAttribute
    {
        /// <summary>
        /// Create some Apizr request options
        /// </summary>
        public OptionsAttribute() : base(Constants.ApizrRequestOptionsKey)
        {

        }
    }
}
