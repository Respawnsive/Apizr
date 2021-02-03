using System;
using System.Collections.Generic;
using System.Text;
using Refit;

namespace Apizr.Integrations.Fusillade
{
    public class PriorityAttribute : PropertyAttribute
    {
        public PriorityAttribute() : base("Priority")
        {
            
        }
    }
}
