using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fusillade;

namespace Apizr
{
    public interface IApizrManager<TWebApi>
    {
        Task<TResult> ExecuteAsync<TResult>(Expression<Func<TWebApi, Task<TResult>>> executeApiMethod, Priority priority = Priority.UserInitiated);
        Task ExecuteAsync(Expression<Func<TWebApi, Task>> executeApiMethod, Priority priority = Priority.UserInitiated);
    }
}
