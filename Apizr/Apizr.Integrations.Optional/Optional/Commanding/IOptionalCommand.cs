using MediatR;
using Optional;

namespace Apizr.Optional.Commanding
{
    public interface IOptionalCommand<TError> : IRequest<Option<Unit, TError>>
    {
    }
}
