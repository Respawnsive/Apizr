using System;

namespace Apizr.Policing;

/// <summary>
/// Tells Apizr to apply some policies to Read method
/// You have to provide a policy registry to Apizr to use this feature
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
[Obsolete("Use a Strategy instead")]
public class ReadPolicyAttribute : PolicyAttributeBase
{
    /// <inheritdoc />
    public ReadPolicyAttribute(params string[] registryKeys) : base(registryKeys)
    {
    }
}