using System;

namespace Apizr.Resiliencing
{
    /// <summary>
    /// Tells Apizr to apply some resilience strategies to Update method
    /// You have to provide a strategy registry to Apizr to use this feature
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class UpdateStrategyAttribute : StrategyAttributeBase
    {
        /// <inheritdoc />
        public UpdateStrategyAttribute(params string[] registryKeys) : base(registryKeys)
        {
        }
    }
}
