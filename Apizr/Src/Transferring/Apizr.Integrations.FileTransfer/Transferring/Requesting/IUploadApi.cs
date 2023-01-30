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
    Task UploadAsync(string path, ByteArrayPart byteArrayPart);

    [Multipart]
    [Post("/")]
    Task UploadAsync(ByteArrayPart byteArrayPart, [RequestOptions] IApizrRequestOptions options);

    [Multipart]
    [Post("/{path}")]
    Task UploadAsync(string path, ByteArrayPart byteArrayPart, [RequestOptions] IApizrRequestOptions options);

    #endregion

    #region StreamPart

    [Multipart]
    [Post("/")]
    Task UploadAsync(StreamPart streamPart);

    [Multipart]
    [Post("/{path}")]
    Task UploadAsync(string path, StreamPart streamPart);

    [Multipart]
    [Post("/")]
    Task UploadAsync(StreamPart streamPart, [RequestOptions] IApizrRequestOptions options);

    [Multipart]
    [Post("/{path}")]
    Task UploadAsync(string path, StreamPart streamPart, [RequestOptions] IApizrRequestOptions options);

    #endregion

    #region FileInfoPart

    [Multipart]
    [Post("/")]
    Task UploadAsync(FileInfoPart fileInfoPart);

    [Multipart]
    [Post("/{path}")]
    Task UploadAsync(string path, FileInfoPart fileInfoPart);

    [Multipart]
    [Post("/")]
    Task UploadAsync(FileInfoPart fileInfoPart, [RequestOptions] IApizrRequestOptions options);

    [Multipart]
    [Post("/{path}")]
    Task UploadAsync(string path, FileInfoPart fileInfoPart, [RequestOptions] IApizrRequestOptions options); 

    #endregion
}