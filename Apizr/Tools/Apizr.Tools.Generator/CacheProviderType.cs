using System;
using System.Collections.Generic;
using System.Text;

namespace Apizr.Tools.Generator
{
    public enum CacheProviderType
    {
        None = -1,
        Akavache,
        MonkeyCache,
        InMemory,
        Distributed,
        Custom
    }
}
