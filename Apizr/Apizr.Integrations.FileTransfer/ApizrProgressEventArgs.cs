namespace Apizr.Integrations.FileTransfer;

public class ApizrProgressEventArgs : HttpProgressEventArgs
{
    public ApizrProgressEventArgs(ApizrProgressType progressType, HttpProgressEventArgs e) : 
        this(progressType, e.ProgressPercentage, e.UserState, e.BytesTransferred, e.TotalBytes)
    {
    }

    /// <inheritdoc />
    public ApizrProgressEventArgs(ApizrProgressType progressType, int progressPercentage, object userToken, long bytesTransferred, long? totalBytes) : base(progressPercentage, userToken, bytesTransferred, totalBytes)
    {
        ProgressType = progressType;
    }

    public ApizrProgressType ProgressType { get; }
}