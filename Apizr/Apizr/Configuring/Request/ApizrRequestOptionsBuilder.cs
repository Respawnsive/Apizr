using System;
using System.Threading;
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
    public abstract TApizrRequestOptions ApizrOptions { get; }

    /// <inheritdoc />
    public TApizrRequestOptionsBuilder WithContext(Context context)
    {
        Options.Context = context;

        return Builder;
    }

    /// <inheritdoc />
    public TApizrRequestOptionsBuilder WithCancellationToken(CancellationToken cancellationToken)
    {
        Options.CancellationToken = cancellationToken;

        return Builder;
    }

    /// <param name="clearCache"></param>
    /// <inheritdoc />
    public TApizrRequestOptionsBuilder WithCacheCleared(bool clearCache)
    {
        Options.ClearCache = clearCache;

        return Builder;
    }

    /// <inheritdoc />
    public TApizrRequestOptionsBuilder WithExceptionCatcher(Action<Exception> onException)
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
    public override IApizrUnitRequestOptions ApizrOptions => Options;
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
    public override IApizrCatchUnitRequestOptions ApizrOptions => Options;
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
    public override IApizrResultRequestOptions ApizrOptions => Options;
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
    public override IApizrCatchResultRequestOptions ApizrOptions => Options;
}