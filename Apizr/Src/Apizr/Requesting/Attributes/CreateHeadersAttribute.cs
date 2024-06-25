using System;
using System.Collections.Generic;
using System.Text;
using Refit;

namespace Apizr.Requesting.Attributes
{
    /// <summary>
    /// Tells Apizr to set headers on Create method
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CreateHeadersAttribute : HeadersAttribute
    {
        public CreateHeadersAttribute(params string[] headers) : base(headers)
        {
            
        }
    }
}
