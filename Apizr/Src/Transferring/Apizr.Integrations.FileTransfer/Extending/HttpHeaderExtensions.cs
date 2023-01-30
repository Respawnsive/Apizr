// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Copied from https://github.com/aspnet/AspNetWebStack/tree/main/src/System.Net.Http.Formatting but without any external ref and adjusted to Apizr needs

using System.Diagnostics.Contracts;
using System.Net.Http.Headers;

namespace Apizr.Extending;

internal static class HttpHeaderExtensions
{
    public static void CopyTo(this HttpContentHeaders fromHeaders, HttpContentHeaders toHeaders)
    {
        Contract.Assert(fromHeaders != null, "fromHeaders cannot be null.");
        Contract.Assert(toHeaders != null, "toHeaders cannot be null.");

        foreach (var header in fromHeaders)
        {
            toHeaders.TryAddWithoutValidation(header.Key, header.Value);
        }
    }
}