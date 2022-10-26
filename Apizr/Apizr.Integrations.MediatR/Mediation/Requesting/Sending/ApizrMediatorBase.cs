using Apizr.Configuring.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace Apizr.Mediation.Requesting.Sending
{
    public abstract class ApizrMediatorBase : IApizrMediatorBase
    {
        protected static IApizrUnitRequestOptionsBuilder
            CreateRequestOptionsBuilder(Action<IApizrUnitRequestOptionsBuilder> optionsBuilder) =>
            ApizrManager.CreateRequestOptionsBuilder(optionsBuilder);

        protected static IApizrCatchUnitRequestOptionsBuilder
            CreateRequestOptionsBuilder(Action<IApizrCatchUnitRequestOptionsBuilder> optionsBuilder) =>
            ApizrManager.CreateRequestOptionsBuilder(optionsBuilder);

        protected static IApizrResultRequestOptionsBuilder
            CreateRequestOptionsBuilder(Action<IApizrResultRequestOptionsBuilder> optionsBuilder) =>
            ApizrManager.CreateRequestOptionsBuilder(optionsBuilder);

        protected static IApizrCatchResultRequestOptionsBuilder
            CreateRequestOptionsBuilder(Action<IApizrCatchResultRequestOptionsBuilder> optionsBuilder) =>
            ApizrManager.CreateRequestOptionsBuilder(optionsBuilder);
    }
}
