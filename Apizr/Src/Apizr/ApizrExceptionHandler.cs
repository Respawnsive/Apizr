using System;
using System.Threading.Tasks;

namespace Apizr;

public class ApizrExceptionHandler : IApizrExceptionHandler
{
    protected readonly Func<ApizrException, Task<bool>> Handler;

    [Obsolete("Catching an exception by an Action is now replaced by a Func returning a handled boolean flag")]
    public ApizrExceptionHandler(Action<ApizrException> handler) : this(ex =>
    {
        handler(ex);
        return Task.FromResult(true);
    })
    {
    }

    public ApizrExceptionHandler(Func<ApizrException, bool> handler) : this(ex => Task.FromResult(handler(ex)))
    {
    }

    public ApizrExceptionHandler(Func<ApizrException, Task<bool>> handler)
    {
        Handler = handler;
    }

    /// <inheritdoc />
    public virtual Task<bool> HandleAsync(ApizrException ex) => Handler(ex);
}

public class ApizrExceptionHandler<TResult> : ApizrExceptionHandler
{
    [Obsolete("Catching an exception by an Action is now replaced by a Func returning a handled boolean flag")]
    public ApizrExceptionHandler(Action<ApizrException<TResult>> handler) : base(ex =>
    {
        if (ex is ApizrException<TResult> apizrException)
        {
            handler(apizrException);
            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    })
    {
    }

    public ApizrExceptionHandler(Func<ApizrException<TResult>, bool> handler) : base(ex =>
    {
        if (ex is ApizrException<TResult> apizrException)
        {
            var handled = handler(apizrException);
            return Task.FromResult(handled);
        }

        return Task.FromResult(false);
    })
    {
    }

    public ApizrExceptionHandler(Func<ApizrException<TResult>, Task<bool>> handler) : base(ex =>
    {
        if (ex is ApizrException<TResult> apizrException)
            return handler(apizrException);

        return Task.FromResult(false);
    })
    {
    }
}