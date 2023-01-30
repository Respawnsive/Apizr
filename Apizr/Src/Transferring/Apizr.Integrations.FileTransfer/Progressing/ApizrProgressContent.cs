// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Copied from https://github.com/aspnet/AspNetWebStack/tree/main/src/System.Net.Http.Formatting but without any external ref and adjusted to Apizr needs

using System.Diagnostics.Contracts;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Apizr.Extending;

namespace Apizr.Progressing;

/// <summary>
/// Wraps an inner <see cref="HttpContent"/> in order to insert a <see cref="ApizrProgressStream"/> on writing data.
/// </summary>
internal class ApizrProgressContent : HttpContent
{
    private readonly HttpContent _innerContent;
    private readonly IApizrProgress _progress;
    private readonly HttpRequestMessage _request;

    public ApizrProgressContent(HttpContent innerContent, IApizrProgress progress, HttpRequestMessage request)
    {
        Contract.Assert(innerContent != null);
        Contract.Assert(progress != null);
        Contract.Assert(request != null);

        _innerContent = innerContent;
        _progress = progress;
        _request = request;

        innerContent.Headers.CopyTo(Headers);
    }

    protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
    {
        var progressStream = new ApizrProgressStream(stream, _progress, _request, response: null);
        return _innerContent.CopyToAsync(progressStream);
    }

    protected override bool TryComputeLength(out long length)
    {
        var contentLength = _innerContent.Headers.ContentLength;
        if (contentLength.HasValue)
        {
            length = contentLength.Value;
            return true;
        }

        length = -1;
        return false;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            _innerContent.Dispose();
        }
    }
}