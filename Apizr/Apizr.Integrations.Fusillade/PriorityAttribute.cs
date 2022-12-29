using Refit;
using System;
using Fusillade;

namespace Apizr
{
    /// <summary>
    /// The Fusillade priority attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Parameter)]
    public class PriorityAttribute : PropertyAttribute
    {
        /// <summary>
        /// Set a priority for the current request
        /// </summary>
        [Obsolete("Use the request options parameter instead")]
        public PriorityAttribute() : base(Constants.PriorityKey)
        {
            
        }

        public PriorityAttribute(Priority priority) : this((int)priority)
        {
            
        }

        public PriorityAttribute(int priority)
        {
            Priority = priority;
        }

        public int Priority { get; }
    }
}
