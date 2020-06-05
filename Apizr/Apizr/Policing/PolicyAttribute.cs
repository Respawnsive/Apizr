using System;

namespace Apizr.Policing
{

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Interface | AttributeTargets.Method)]
    public class PolicyAttribute : Attribute
    {
        /// <summary>
        /// You must add your registry to IServiceCollection to use this attribute <example>services.AddPolicyRegistry(YourRegistry);</example>
        /// </summary>
        /// <param name="registryKeys">Policy registry keys</param>
        public PolicyAttribute(params string[] registryKeys)
        {
            this.RegistryKeys = registryKeys;
        }

        public string[] RegistryKeys { get; }
    }
}
