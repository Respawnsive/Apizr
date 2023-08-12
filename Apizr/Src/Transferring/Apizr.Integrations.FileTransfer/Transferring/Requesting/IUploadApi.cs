using Apizr.Configuring.Request;
using Refit;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Apizr.Transferring.Requesting;

public interface IUploadApi<TApiResultData> : ITransferApiBase
{
    #region ByteArrayPart

    [Multipart]
    [Post("")]
    Task<TApiResultData> UploadAsync([AliasAs("file")] ByteArrayPart byteArrayPart);

    [Multipart]
    [Post("/{path}"), QueryUriFormat(UriFormat.Unescaped)]
    Task<TApiResultData> UploadAsync([AliasAs("file")] ByteArrayPart byteArrayPart, string path);

    [Multipart]
    [Post("")]
    Task<TApiResultData> UploadAsync([AliasAs("file")] ByteArrayPart byteArrayPart, [RequestOptions] IApizrRequestOptions options);

    [Multipart]
    [Post("/{path}"), QueryUriFormat(UriFormat.Unescaped)]
    Task<TApiResultData> UploadAsync([AliasAs("file")] ByteArrayPart byteArrayPart, string path, [RequestOptions] IApizrRequestOptions options);

    #endregion

    #region StreamPart

    [Multipart]
    [Post("")]
    Task<TApiResultData> UploadAsync([AliasAs("file")] StreamPart streamPart);

    [Multipart]
    [Post("/{path}"), QueryUriFormat(UriFormat.Unescaped)]
    Task<TApiResultData> UploadAsync([AliasAs("file")] StreamPart streamPart, string path);

    [Multipart]
    [Post("")]
    Task<TApiResultData> UploadAsync([AliasAs("file")] StreamPart streamPart, [RequestOptions] IApizrRequestOptions options);

    [Multipart]
    [Post("/{path}"), QueryUriFormat(UriFormat.Unescaped)]
    Task<TApiResultData> UploadAsync([AliasAs("file")] StreamPart streamPart, string path, [RequestOptions] IApizrRequestOptions options);

    #endregion

    #region FileInfoPart

    [Multipart]
    [Post("")]
    Task<TApiResultData> UploadAsync([AliasAs("file")] FileInfoPart fileInfoPart);

    [Multipart]
    [Post("/{filePath}"), QueryUriFormat(UriFormat.Unescaped)]
    Task<TApiResultData> UploadAsync([AliasAs("file")] FileInfoPart fileInfoPart, string filePath);

    [Multipart]
    [Post("")]
    Task<TApiResultData> UploadAsync([AliasAs("file")] FileInfoPart fileInfoPart, [RequestOptions] IApizrRequestOptions options);

    [Multipart]
    [Post("/{filePath}"), QueryUriFormat(UriFormat.Unescaped)]
    Task<TApiResultData> UploadAsync([AliasAs("file")] FileInfoPart fileInfoPart, string filePath, [RequestOptions] IApizrRequestOptions options);

    #endregion
}

public interface IUploadApi : IUploadApi<HttpResponseMessage>
{
}