using Apizr.Configuring.Registry;

namespace Apizr.Extending.Configuring.Registry
{
    public interface IApizrExtendedRegistry : IApizrEnumerableRegistry
    {
        internal IApizrExtendedRegistry SubRegistry { get; set; }
    }
}
