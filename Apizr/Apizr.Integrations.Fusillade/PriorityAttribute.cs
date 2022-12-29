using Refit;
using System;
using Apizr.Configuring;
using Fusillade;

namespace Apizr
{
    /// <summary>
    /// The Fusillade priority attribute
    /// </summary>
    /// <remarks>
    /// <para>Info:</para>
    /// <para>. Decorating a request parameter with PriorityAttribute is obsolete. Please use the request options parameter instead.</para>
    /// <para>. Decorating anything else must come with the priority value.</para>
    /// <para>. Don't forget to activate priority management fluently at register time.</para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Parameter)]
    public class PriorityAttribute : HandlerParameterAttribute
    {
        /// <summary>
        /// Define priority
        /// </summary>
        /// <remarks>
        /// <para>Warning: Decorating a request parameter with PriorityAttribute is obsolete. Please use the request options parameter instead.</para>
        /// Error: Decorating anything else must come with the priority value.
        /// </remarks>
        [Obsolete("Please use the request options parameter instead")]
        public PriorityAttribute() : base(null, null)
        {
            
        }

        /// <summary>
        /// Define priority
        /// </summary>
        /// <param name="priority">The priority</param>
        public PriorityAttribute(Priority priority) : this((int)priority)
        {
            
        }

        /// <summary>
        /// Define priority
        /// </summary>
        /// <param name="priority">The priority</param>
        public PriorityAttribute(int priority) : base(Constants.PriorityKey, priority)
        {

        }
    }
}
