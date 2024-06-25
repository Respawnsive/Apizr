using System;
using System.Collections.Generic;
using System.Text;

namespace Apizr.Configuring
{
    public enum ApizrLifetimeScope
    {
        /// <summary>
        /// Set once for all api requests (static values)
        /// </summary>
        Api,

        /// <summary>
        /// Refresh values for each api request (dynamic values)
        /// </summary>
        Request
    }
}
