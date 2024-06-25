using System;
using Refit;

namespace Apizr.Requesting.Attributes;

/// <summary>
/// Tells Apizr to set headers on Update method
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class UpdateHeadersAttribute : HeadersAttribute
{
    public UpdateHeadersAttribute(params string[] headers) : base(headers)
    {
        
    }
}