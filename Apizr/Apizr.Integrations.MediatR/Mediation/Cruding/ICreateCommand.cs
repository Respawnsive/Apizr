using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Apizr.Mediation.Cruding
{
    public interface ICreateCommand<out TEntity> : IRequest<TEntity> where TEntity : class, new()
    {
    }
}
