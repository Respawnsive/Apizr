using System;

namespace Apizr.Progressing;

public class ApizrProgress : Progress<ApizrProgressEventArgs>, IApizrProgress
{
    public ApizrProgress() : base()
    {
            
    }

    public ApizrProgress(Action<ApizrProgressEventArgs> handler) : base(handler)
    {
            
    }
}