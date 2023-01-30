// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Copied from https://github.com/aspnet/AspNetWebStack/tree/main/src/System.Net.Http.Formatting but without any external ref and adjusted to Apizr needs

using System;
using System.Diagnostics.Contracts;
using System.Threading;

namespace Apizr.Progressing;

internal abstract class ApizrAsyncResult : IAsyncResult
{
    private readonly AsyncCallback _callback;
    private bool _isCompleted;
    private bool _completedSynchronously;
    private bool _endCalled;
    private Exception _exception;

    protected ApizrAsyncResult(AsyncCallback callback, object state)
    {
        _callback = callback;
        AsyncState = state;
    }

    public object AsyncState { get; }

    public WaitHandle AsyncWaitHandle
    {
        get
        {
            Contract.Assert(false, "AsyncWaitHandle is not supported -- use callbacks instead.");
            return null;
        }
    }

    public bool CompletedSynchronously => _completedSynchronously;

    public bool HasCallback => _callback != null;

    public bool IsCompleted => _isCompleted;

    protected void Complete(bool completedSynchronously)
    {
        if (_isCompleted)
            throw new InvalidOperationException(string.Format(FileTransferResources.AsyncResult_MultipleCompletes,
                GetType().Name));

        _completedSynchronously = completedSynchronously;
        _isCompleted = true;

        if (_callback == null) 
            return;

        try
        {
            _callback(this);
        }
        catch (Exception e)
        {
            throw new InvalidOperationException(FileTransferResources.AsyncResult_CallbackThrewException, e);
        }
    }

    protected void Complete(bool completedSynchronously, Exception exception)
    {
        _exception = exception;
        Complete(completedSynchronously);
    }

    protected static TAsyncResult End<TAsyncResult>(IAsyncResult result) where TAsyncResult : ApizrAsyncResult
    {
        if (result == null)
            throw new ArgumentNullException(nameof(result));

        if (result is not TAsyncResult thisPtr)
            throw new ArgumentNullException(nameof(result), FileTransferResources.AsyncResult_ResultMismatch);

        if (!thisPtr._isCompleted) 
            thisPtr.AsyncWaitHandle.WaitOne();

        if (thisPtr._endCalled)
            throw new InvalidOperationException(FileTransferResources.AsyncResult_MultipleEnds);

        thisPtr._endCalled = true;

        if (thisPtr._exception != null)
            throw thisPtr._exception;

        return thisPtr;
    }
}