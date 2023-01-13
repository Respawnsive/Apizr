// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Copied from https://github.com/aspnet/AspNetWebStack/tree/main/src/System.Net.Http.Formatting but without any external ref and adjusted to Apizr needs

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Apizr.Progressing;

/// <summary>
/// Stream that delegates to inner stream.
/// This is taken from System.Net.Http
/// </summary>
internal abstract class ApizrDelegatingStream : Stream
{
    protected ApizrDelegatingStream(Stream innerStream)
    {
        InnerStream = innerStream ?? throw new ArgumentNullException(nameof(innerStream));
    }

    protected Stream InnerStream { get; }

    public override bool CanRead => InnerStream.CanRead;

    public override bool CanSeek => InnerStream.CanSeek;

    public override bool CanWrite => InnerStream.CanWrite;

    public override long Length => InnerStream.Length;

    public override long Position
    {
        get => InnerStream.Position;
        set => InnerStream.Position = value;
    }

    public override int ReadTimeout
    {
        get => InnerStream.ReadTimeout;
        set => InnerStream.ReadTimeout = value;
    }

    public override bool CanTimeout => InnerStream.CanTimeout;

    public override int WriteTimeout
    {
        get => InnerStream.WriteTimeout;
        set => InnerStream.WriteTimeout = value;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing) 
            InnerStream.Dispose();

        base.Dispose(disposing);
    }

    public override long Seek(long offset, SeekOrigin origin) => InnerStream.Seek(offset, origin);

    public override int Read(byte[] buffer, int offset, int count) => InnerStream.Read(buffer, offset, count);

    public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) => InnerStream.ReadAsync(buffer, offset, count, cancellationToken);

#if !NETFX_CORE // BeginX and EndX not supported on Streams in portable libraries
    public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state) => InnerStream.BeginRead(buffer, offset, count, callback, state);

    public override int EndRead(IAsyncResult asyncResult) => InnerStream.EndRead(asyncResult);
#endif

    public override int ReadByte() => InnerStream.ReadByte();

    public override void Flush() => InnerStream.Flush();

    public override Task FlushAsync(CancellationToken cancellationToken) => InnerStream.FlushAsync(cancellationToken);

    public override void SetLength(long value) => InnerStream.SetLength(value);

    public override void Write(byte[] buffer, int offset, int count) => InnerStream.Write(buffer, offset, count);

    public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) => InnerStream.WriteAsync(buffer, offset, count, cancellationToken);

#if !NETFX_CORE // BeginX and EndX not supported on Streams in portable libraries
    public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state) => InnerStream.BeginWrite(buffer, offset, count, callback, state);

    public override void EndWrite(IAsyncResult asyncResult) => InnerStream.EndWrite(asyncResult);
#endif

    public override void WriteByte(byte value) => InnerStream.WriteByte(value);
}