using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Optional;

namespace Apizr.Optional.Commanding
{
    public interface IOptionalCommand<TError> : IRequest<Option<Unit, TError>>
    {
    }
}
