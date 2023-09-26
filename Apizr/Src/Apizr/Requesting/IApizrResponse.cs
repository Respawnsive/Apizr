using Refit;

namespace Apizr.Requesting
{
    /// <summary>
    /// Base interface used to represent an API response managed by Apizr.
    /// </summary>
    public interface IApizrResponse : IApiResponse
    {

    }

    /// <inheritdoc cref="IApizrResponse"/>
    public interface IApizrResponse<out T> : IApiResponse<T>, IApizrResponse
    {

    }
}
