using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Fusillade;

namespace Apizr
{
    public interface IApizrManager<TWebApi>
    {
        Task<TResult> ExecuteAsync<TResult>(Expression<Func<TWebApi, Task<TResult>>> executeApiMethod, Priority priority = Priority.UserInitiated);
        Task<TResult> ExecuteAsync<TResult>(Expression<Func<CancellationToken, TWebApi, Task<TResult>>> executeApiMethod, CancellationToken cancellationToken, Priority priority = Priority.UserInitiated);
        Task ExecuteAsync(Expression<Func<TWebApi, Task>> executeApiMethod, Priority priority = Priority.UserInitiated);
        Task ExecuteAsync(Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod, CancellationToken cancellationToken, Priority priority = Priority.UserInitiated);
        Task<bool> ClearCacheAsync(CancellationToken cancellationToken = default);
        Task<bool> ClearCacheAsync<TResult>(Expression<Func<TWebApi, Task<TResult>>> executeApiMethod);
        Task<bool> ClearCacheAsync<TResult>(Expression<Func<CancellationToken, TWebApi, Task<TResult>>> executeApiMethod, CancellationToken cancellationToken);
    }
}
