using System;

namespace Apizr.Configuring.Registry
{
    /// <summary>
    /// Registry options available for static registrations
    /// </summary>
    public interface IApizrRegistry : IApizrEnumerableRegistry
    {
        /// <summary>
        /// Populate all registered types and its factories
        /// </summary>
        /// <param name="populateAction">The action to execute when populating</param>
        void Populate(Action<Type, Func<object>> populateAction);
    }
}
