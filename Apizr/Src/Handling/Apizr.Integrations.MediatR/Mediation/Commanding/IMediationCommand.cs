using MediatR;

namespace Apizr.Mediation.Commanding
{
    /// <summary>
    /// A mediation command sending mapped request and returning a mapped result
    /// </summary>
    /// <typeparam name="TModelResultData">The model result type to map to</typeparam>
    /// <typeparam name="TApiRequestData">The api result type to map from</typeparam>
    /// <typeparam name="TApiResultData">The api request type to map to</typeparam>
    /// <typeparam name="TModelRequestData">The model request type to map from</typeparam>
    public interface IMediationCommand<out TModelResultData, TApiResultData, TApiRequestData, TModelRequestData> :
        IRequest<TModelResultData>
    {
    }

    /// <summary>
    /// A mediation command sending a request and returning a result
    /// </summary>
    /// <typeparam name="TRequestData">The api request type</typeparam>
    /// <typeparam name="TResultData">The api result type</typeparam>
    public interface IMediationCommand<out TRequestData, out TResultData> :
        IRequest<TResultData>
    {
    }

    /// <summary>
    /// A mediation command sending a request
    /// </summary>
    /// <typeparam name="TRequestData">The api request type</typeparam>
    public interface IMediationCommand<out TRequestData> :
        IRequest<Unit>
    {
    }

    /// <summary>
    /// A mediation command
    /// </summary>
    public interface IMediationCommand : IRequest<Unit>
    {
    }
}
