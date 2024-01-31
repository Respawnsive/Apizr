using System;

namespace Apizr.Resiliencing
{
    /// <inheritdoc />
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
    public class StrategyAttribute : StrategyAttributeBase
    {
        /// <inheritdoc />
        public StrategyAttribute(params string[] registryKeys) : base(registryKeys)
        {
        }

    }
}
