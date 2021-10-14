using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Mapping;

namespace Apizr
{
    public interface IApizrManager
    {}

    /// <summary>
    /// The manager encapsulating <see cref="TWebApi"/>'s task
    /// </summary>
    /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
    public interface IApizrManager<TWebApi> : IApizrManager
    {
        /// <summary>
        /// Original Refit Api without any Apizr management. You should use ExecuteAsync instead to get all the Apizr goodness :)
        /// </summary>
        TWebApi Api { get; }

        /// <summary>
        /// Execute a managed <see cref="TWebApi"/>'s task
        /// </summary>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/>'s task to execute</param>
        /// <returns></returns>
        Task ExecuteAsync(Expression<Func<TWebApi, Task>> executeApiMethod);

        /// <summary>
        /// Execute a managed <see cref="TWebApi"/>'s task
        /// </summary>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/>'s task to execute</param>
        /// <returns></returns>
        Task ExecuteAsync(Expression<Func<TWebApi, IMappingHandler, Task>> executeApiMethod);

        /// <summary>
        /// Execute a managed <see cref="TWebApi"/>'s task
        /// </summary>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/>'s task to execute</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        Task ExecuteAsync(Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod, CancellationToken cancellationToken);

        /// <summary>
        /// Execute a managed <see cref="TWebApi"/>'s task
        /// </summary>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/>'s task to execute</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        Task ExecuteAsync(Expression<Func<CancellationToken, TWebApi, IMappingHandler, Task>> executeApiMethod, CancellationToken cancellationToken);

        /// <summary>
        /// Execute a managed <see cref="TWebApi"/>'s task
        /// </summary>
        /// <typeparam name="TResult">The <see cref="TWebApi"/>'s task result</typeparam>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/>'s task to execute</param>
        /// <returns></returns>
        Task<TResult> ExecuteAsync<TResult>(Expression<Func<TWebApi, Task<TResult>>> executeApiMethod);

        /// <summary>
        /// Execute a managed <see cref="TWebApi"/>'s task
        /// </summary>
        /// <typeparam name="TResult">The <see cref="TWebApi"/>'s task result</typeparam>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/>'s task to execute with mapping</param>
        /// <returns></returns>
        Task<TResult> ExecuteAsync<TResult>(Expression<Func<TWebApi, IMappingHandler, Task<TResult>>> executeApiMethod);

        /// <summary>
        /// Execute a managed <see cref="TWebApi"/>'s task
        /// </summary>
        /// <typeparam name="TResult">The <see cref="TWebApi"/>'s task result</typeparam>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/>'s task to execute</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        Task<TResult> ExecuteAsync<TResult>(Expression<Func<CancellationToken, TWebApi, Task<TResult>>> executeApiMethod, CancellationToken cancellationToken);

        /// <summary>
        /// Execute a managed <see cref="TWebApi"/>'s task
        /// </summary>
        /// <typeparam name="TResult">The <see cref="TWebApi"/>'s task result</typeparam>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/>'s task to execute</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        Task<TResult> ExecuteAsync<TResult>(Expression<Func<CancellationToken, TWebApi, IMappingHandler, Task<TResult>>> executeApiMethod, CancellationToken cancellationToken);

        /// <summary>
        /// Clear all cache
        /// </summary>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        Task<bool> ClearCacheAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Clear the cache of a specific request
        /// </summary>
        /// <typeparam name="TResult">The <see cref="TWebApi"/>'s task result</typeparam>
        /// <param name="executeApiMethod">The specific <see cref="TWebApi"/>'s task to clear cache for</param>
        /// <returns></returns>
        Task<bool> ClearCacheAsync<TResult>(Expression<Func<TWebApi, Task<TResult>>> executeApiMethod);

        /// <summary>
        /// Clear the cache of a specific request
        /// </summary>
        /// <typeparam name="TResult">The <see cref="TWebApi"/>'s task result</typeparam>
        /// <param name="executeApiMethod">The specific <see cref="TWebApi"/>'s task to clear cache for</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        Task<bool> ClearCacheAsync<TResult>(Expression<Func<CancellationToken, TWebApi, Task<TResult>>> executeApiMethod, CancellationToken cancellationToken);
    }
}
