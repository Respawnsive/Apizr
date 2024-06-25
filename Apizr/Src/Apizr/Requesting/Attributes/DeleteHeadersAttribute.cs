using System;
using Refit;

namespace Apizr.Requesting.Attributes;

/// <summary>
/// Tells Apizr to set headers on Delete method
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class DeleteHeadersAttribute : HeadersAttribute
{
    public DeleteHeadersAttribute(params string[] headers) : base(headers)
    {
        
    }
}