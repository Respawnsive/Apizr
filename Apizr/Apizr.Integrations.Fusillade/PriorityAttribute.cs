using Refit;

namespace Apizr
{
    /// <summary>
    /// The Fusillade priority attribute
    /// </summary>
    public class PriorityAttribute : PropertyAttribute
    {
        /// <summary>
        /// Set a priority for the current request
        /// </summary>
        public PriorityAttribute() : base(Constants.PriorityKey)
        {
            
        }
    }
}
