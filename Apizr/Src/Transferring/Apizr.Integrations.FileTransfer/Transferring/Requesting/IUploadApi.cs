using Apizr.Configuring.Request;
using Refit;
using System.Threading.Tasks;

namespace Apizr.Transferring.Requesting;

public interface IUploadApi : ITransferApiBase
{
    #region ByteArrayPart

    [Multipart]
    [Post("/")]
    Task UploadAsync(ByteArrayPart byteArrayPart);

    [Multipart]
    [Post("/{path}")]
    Task UploadAsync(ByteArrayPart byteArrayPart, string path);

    [Multipart]
    [Post("/")]
    Task UploadAsync(ByteArrayPart byteArrayPart, [RequestOptions] IApizrRequestOptions options);

    [Multipart]
    [Post("/{path}")]
    Task UploadAsync(ByteArrayPart byteArrayPart, string path, [RequestOptions] IApizrRequestOptions options);

    #endregion

    #region StreamPart

    [Multipart]
    [Post("/")]
    Task UploadAsync(StreamPart streamPart);

    [Multipart]
    [Post("/{path}")]
    Task UploadAsync(StreamPart streamPart, string path);

    [Multipart]
    [Post("/")]
    Task UploadAsync(StreamPart streamPart, [RequestOptions] IApizrRequestOptions options);

    [Multipart]
    [Post("/{path}")]
    Task UploadAsync(StreamPart streamPart, string path, [RequestOptions] IApizrRequestOptions options);

    #endregion

    #region FileInfoPart

    [Multipart]
    [Post("/")]
    Task UploadAsync(FileInfoPart fileInfoPart);

    [Multipart]
    [Post("/{filePath}")]
    Task UploadAsync(FileInfoPart fileInfoPart, string filePath);

    [Multipart]
    [Post("/")]
    Task UploadAsync(FileInfoPart fileInfoPart, [RequestOptions] IApizrRequestOptions options);

    [Multipart]
    [Post("/{filePath}")]
    Task UploadAsync(FileInfoPart fileInfoPart, string filePath, [RequestOptions] IApizrRequestOptions options); 

    #endregion
}