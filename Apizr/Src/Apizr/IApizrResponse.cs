using System;
using System.Collections.Generic;
using System.Text;
using Refit;

namespace Apizr
{
    /// <summary>
    /// Base interface used to represent an API response managed by Apizr.
    /// </summary>
    public interface IApizrResponse : IApiResponse
    {
    }

    /// <summary>
    /// Interface used to represent an API response managed by Apizr.
    /// </summary>
    /// <typeparam name="T">Deserialized request content as <typeparamref name="T"/></typeparam>
    public interface IApizrResponse<out T> : IApiResponse<T>, IApizrResponse
    {
    }
}
