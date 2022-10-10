using Apizr.Configuring.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace Apizr.Mediation.Requesting.Sending
{
    public abstract class ApizrMediatorBase
    {
        protected static IApizrRequestOptionsBuilder
            CreateRequestOptionsBuilder(Action<IApizrRequestOptionsBuilder> optionsBuilder) =>
            ApizrManager.CreateRequestOptionsBuilder(optionsBuilder);
    }
}
