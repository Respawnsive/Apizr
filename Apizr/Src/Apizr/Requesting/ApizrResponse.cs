using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Refit;

namespace Apizr.Requesting;

/// <summary>
/// Implementation of <see cref="IApizrResponse{T}"/> that provides additional functionalities.
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class ApizrResponse<T> : IApizrResponse<T>
{
    private readonly IApiResponse<T> _apiResponse;
    private bool _disposed;

    /// <summary>
    /// Create an instance of <see cref="ApizrResponse{T}"/> with type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="apiResponse">Original Refit's ApiResponse</param>
    /// <param name="content">Data returned from response or cache</param>
    public ApizrResponse(IApiResponse<T> apiResponse, T content)
    {
        _apiResponse = apiResponse;
        Content = content;
    }

    /// <inheritdoc />
    public HttpResponseHeaders Headers => _apiResponse.Headers;

    /// <inheritdoc />
    public HttpContentHeaders ContentHeaders => _apiResponse.ContentHeaders;

    /// <inheritdoc />
    public bool IsSuccessStatusCode => _apiResponse.IsSuccessStatusCode;

    /// <inheritdoc />
    public HttpStatusCode StatusCode => _apiResponse.StatusCode;

    /// <inheritdoc />
    public string ReasonPhrase => _apiResponse.ReasonPhrase;

    /// <inheritdoc />
    public HttpRequestMessage RequestMessage => _apiResponse.RequestMessage;

    /// <inheritdoc />
    public Version Version => _apiResponse.Version;

    /// <inheritdoc />
    public ApiException Error => _apiResponse.Error;

    /// <inheritdoc />
    public T Content { get; }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
    }

    private void Dispose(bool disposing)
    {
        if (!disposing || _disposed)
            return;

        _disposed = true;

        _apiResponse.Dispose();
    }
}