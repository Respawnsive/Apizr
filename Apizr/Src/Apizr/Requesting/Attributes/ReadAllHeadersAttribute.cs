using System;
using Refit;

namespace Apizr.Requesting.Attributes;

/// <summary>
/// Tells Apizr to set headers on ReadAll method
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class ReadAllHeadersAttribute : HeadersAttribute
{
    public ReadAllHeadersAttribute(params string[] headers) : base(headers)
    {

    }
}