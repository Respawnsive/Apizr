using System;

namespace Apizr.Integrations.FileTransfer;

public class ApizrProgress : Progress<ApizrProgressEventArgs>, IApizrProgress
{
    public ApizrProgress() : base()
    {
            
    }

    public ApizrProgress(Action<ApizrProgressEventArgs> handler) : base(handler)
    {
            
    }
}