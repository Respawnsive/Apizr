using Apizr.Caching.Attributes;
using Apizr.Caching;

namespace Apizr.Transferring.Requesting;

[WebApi, Cache(CacheMode.None)]
public interface ITransferApiBase
{
}