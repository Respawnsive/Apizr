using System;

namespace Apizr.Resiliencing
{
    /// <summary>
    /// Tells Apizr to apply some resilience strategies to Read method
    /// You have to provide a strategy registry to Apizr to use this feature
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ReadStrategyAttribute : StrategyAttributeBase
    {
        /// <inheritdoc />
        public ReadStrategyAttribute(params string[] registryKeys) : base(registryKeys)
        {
        }
    }
}
