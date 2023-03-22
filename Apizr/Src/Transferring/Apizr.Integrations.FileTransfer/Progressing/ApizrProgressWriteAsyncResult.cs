// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Copied from https://github.com/aspnet/AspNetWebStack/tree/main/src/System.Net.Http.Formatting but without any external ref and adjusted to Apizr needs

using System;
using System.Diagnostics.Contracts;
using System.IO;

namespace Apizr.Progressing;

internal class ApizrProgressWriteAsyncResult : ApizrAsyncResult
{
    private static readonly AsyncCallback WriteCompletedAsyncCallback = WriteCompletedCallback;

    private readonly Stream _innerStream;
    private readonly ApizrProgressStream _progressStream;
    private readonly int _count;

    public ApizrProgressWriteAsyncResult(Stream innerStream, ApizrProgressStream progressStream, byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        : base(callback, state)
    {
        Contract.Assert(innerStream != null);
        Contract.Assert(progressStream != null);
        Contract.Assert(buffer != null);

        _innerStream = innerStream;
        _progressStream = progressStream;
        _count = count;

        try
        {
            var result = innerStream.BeginWrite(buffer, offset, count, WriteCompletedAsyncCallback, this);
            if (result.CompletedSynchronously)
            {
                WriteCompleted(result);
            }
        }
        catch (Exception e)
        {
            Complete(true, e);
        }
    }

    private static void WriteCompletedCallback(IAsyncResult result)
    {
        if (result.CompletedSynchronously)
            return;

        var thisPtr = (ApizrProgressWriteAsyncResult)result.AsyncState;
        try
        {
            thisPtr.WriteCompleted(result);
        }
        catch (Exception e)
        {
            thisPtr.Complete(false, e);
        }
    }

    private void WriteCompleted(IAsyncResult result)
    {
        _innerStream.EndWrite(result);
        _progressStream.ReportBytesSent(_count, AsyncState);
        Complete(result.CompletedSynchronously);
    }

    public static void End(IAsyncResult result)
    {
        End<ApizrProgressWriteAsyncResult>(result);
    }
}