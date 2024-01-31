using System;

namespace Apizr.Resiliencing
{
    /// <summary>
    /// Tells Apizr to apply some policies to Create method
    /// You have to provide a strategy registry to Apizr to use this feature
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CreateStrategyAttribute : StrategyAttributeBase
    {
        /// <inheritdoc />
        public CreateStrategyAttribute(params string[] registryKeys) : base(registryKeys)
        {
        }
    }
}
