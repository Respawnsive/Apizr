using Refit;

namespace Apizr.Configuring.Request
{
    /// <summary>
    /// The Apizr request options property attribute
    /// </summary>
    public class RequestOptionsAttribute : PropertyAttribute
    {
        /// <summary>
        /// Create some Apizr request options
        /// </summary>
        public RequestOptionsAttribute() : base(Constants.ApizrRequestOptionsKey)
        {

        }
    }
}
