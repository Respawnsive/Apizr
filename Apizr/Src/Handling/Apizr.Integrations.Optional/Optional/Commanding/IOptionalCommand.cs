using MediatR;
using Optional;

namespace Apizr.Optional.Commanding
{
    /// <summary>
    /// A mediation command returning an optional error
    /// </summary>
    /// <typeparam name="TError">The optional error</typeparam>
    public interface IOptionalCommand<TError> : IRequest<Option<Unit, TError>>
    {
    }
}
