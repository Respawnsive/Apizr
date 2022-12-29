using System;
using System.Collections.Generic;
using System.Text;
using Refit;

namespace Apizr.Configuring
{
    public class HandlerParameterAttribute : PropertyAttribute
    {
        public HandlerParameterAttribute(string key, object value) : base(key)
        {
            Value = value;
        }
        
        public object Value { get; set; }
    }
}
