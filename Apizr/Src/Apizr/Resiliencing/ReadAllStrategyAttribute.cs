using System;

namespace Apizr.Resiliencing
{
    /// <summary>
    /// Tells Apizr to apply some resilience strategies to ReadAll method
    /// You have to provide a strategy registry to Apizr to use this feature
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ReadAllStrategyAttribute : StrategyAttributeBase
    {
        /// <inheritdoc />
        public ReadAllStrategyAttribute(params string[] registryKeys) : base(registryKeys)
        {
        }
    }
}
