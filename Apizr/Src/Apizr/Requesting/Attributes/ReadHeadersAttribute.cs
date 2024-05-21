using System;
using Refit;

namespace Apizr.Requesting.Attributes;

/// <summary>
/// Tells Apizr to set headers on Read method
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class ReadHeadersAttribute : HeadersAttribute
{
    public ReadHeadersAttribute(params string[] headers) : base(headers)
    {
        
    }
}