using System;

namespace Apizr.Resiliencing
{
    /// <summary>
    /// Tells Apizr to apply some resilience strategies to Delete method
    /// You have to provide a strategy registry to Apizr to use this feature
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DeleteStrategyAttribute : StrategyAttributeBase
    {
        /// <inheritdoc />
        public DeleteStrategyAttribute(params string[] registryKeys) : base(registryKeys)
        {
        }
    }
}
