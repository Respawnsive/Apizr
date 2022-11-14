using System;
using System.Threading;
using Apizr.Configuring.Common;
using Apizr.Logging;
using Microsoft.Extensions.Logging;
using Polly;

namespace Apizr.Configuring.Request;

public abstract class
    ApizrRequestOptionsBuilderBase<TApizrRequestOptions, TApizrRequestOptionsBuilder> : IApizrRequestOptionsBuilderBase<
        TApizrRequestOptions, TApizrRequestOptionsBuilder>
    where TApizrRequestOptions : IApizrRequestOptionsBase
    where TApizrRequestOptionsBuilder :
    IApizrRequestOptionsBuilderBase<TApizrRequestOptions, TApizrRequestOptionsBuilder>
{
    protected readonly ApizrRequestOptions Options;

    protected ApizrRequestOptionsBuilderBase(ApizrRequestOptions options)
    {
        Options = options;
    }

    protected abstract TApizrRequestOptionsBuilder Builder { get; }

    /// <inheritdoc />
    public TApizrRequestOptionsBuilder WithLogging(HttpTracerMode httpTracerMode = HttpTracerMode.Everything,
        HttpMessageParts trafficVerbosity = HttpMessageParts.All, params LogLevel[] logLevels)
    {
        Options.HttpTracerMode = httpTracerMode;
        Options.TrafficVerbosity = trafficVerbosity;
        Options.LogLevels = logLevels;

        return Builder;
    }

    /// <inheritdoc />
    public TApizrRequestOptionsBuilder WithContext(Context context)
    {
        Options.Context = context;

        return Builder;
    }

    /// <inheritdoc />
    public TApizrRequestOptionsBuilder CancelWith(CancellationToken cancellationToken)
    {
        Options.CancellationToken = cancellationToken;

        return Builder;
    }

    /// <inheritdoc />
    public TApizrRequestOptionsBuilder AddHandlersParameter(string key, object value)
    {
        Options.HandlersParameters[key] = value;

        return Builder;
    }

    /// <param name="clearCache"></param>
    /// <inheritdoc />
    public TApizrRequestOptionsBuilder ClearCache(bool clearCache)
    {
        Options.ClearCache = clearCache;

        return Builder;
    }

    /// <inheritdoc />
    public TApizrRequestOptionsBuilder Catch(Action<ApizrException> onException)
        => Catch(onException, false);

    /// <inheritdoc />
    public TApizrRequestOptionsBuilder Catch(Action<ApizrException> onException, bool letThrowOnExceptionWithEmptyCache)
    {
        Options.OnException = onException;

        return Builder;
    }
}

public class ApizrUnitRequestOptionsBuilder :
    ApizrRequestOptionsBuilderBase<IApizrUnitRequestOptions, IApizrUnitRequestOptionsBuilder>,
    IApizrUnitRequestOptionsBuilder
{
    /// <inheritdoc />
    public ApizrUnitRequestOptionsBuilder(ApizrRequestOptions options) : base(options)
    {
    }

    /// <inheritdoc />
    protected override IApizrUnitRequestOptionsBuilder Builder => this;

    /// <inheritdoc />
    IApizrRequestOptions IApizrRequestOptionsBuilderBase.ApizrOptions => Options;
}

public class ApizrCatchUnitRequestOptionsBuilder :
    ApizrRequestOptionsBuilderBase<IApizrCatchUnitRequestOptions, IApizrCatchUnitRequestOptionsBuilder>,
    IApizrCatchUnitRequestOptionsBuilder
{
    /// <inheritdoc />
    public ApizrCatchUnitRequestOptionsBuilder(ApizrRequestOptions options) : base(options)
    {
    }

    /// <inheritdoc />
    protected override IApizrCatchUnitRequestOptionsBuilder Builder => this;

    /// <inheritdoc />
    IApizrRequestOptions IApizrRequestOptionsBuilderBase.ApizrOptions => Options;
}

public class ApizrResultRequestOptionsBuilder :
    ApizrRequestOptionsBuilderBase<IApizrResultRequestOptions, IApizrResultRequestOptionsBuilder>,
    IApizrResultRequestOptionsBuilder
{
    /// <inheritdoc />
    public ApizrResultRequestOptionsBuilder(ApizrRequestOptions options) : base(options)
    {
    }

    /// <inheritdoc />
    protected override IApizrResultRequestOptionsBuilder Builder => this;

    /// <inheritdoc />
    IApizrRequestOptions IApizrRequestOptionsBuilderBase.ApizrOptions => Options;
}

public class ApizrCatchResultRequestOptionsBuilder :
    ApizrRequestOptionsBuilderBase<IApizrCatchResultRequestOptions, IApizrCatchResultRequestOptionsBuilder>,
    IApizrCatchResultRequestOptionsBuilder
{
    /// <inheritdoc />
    public ApizrCatchResultRequestOptionsBuilder(ApizrRequestOptions options) : base(options)
    {
    }

    /// <inheritdoc />
    protected override IApizrCatchResultRequestOptionsBuilder Builder => this;

    /// <inheritdoc />
    IApizrRequestOptions IApizrRequestOptionsBuilderBase.ApizrOptions => Options;
}