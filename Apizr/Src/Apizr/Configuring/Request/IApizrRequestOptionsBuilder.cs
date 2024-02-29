using System.Linq.Expressions;
using System.Threading;
using Apizr.Configuring.Shared.Context;
using Polly;

namespace Apizr.Configuring.Request
{
    /// <summary>
    /// Builder options available at request level
    /// </summary>
    public interface IApizrRequestOptionsBuilder<out TApizrOptions, out TApizrOptionsBuilder> :
        IApizrRequestOptionsBuilderBase<TApizrOptions, TApizrOptionsBuilder>
        where TApizrOptions : IApizrRequestOptions
        where TApizrOptionsBuilder : IApizrRequestOptionsBuilder<TApizrOptions, TApizrOptionsBuilder>
    {
        /// <summary>
        /// Set the cancellation token
        /// </summary>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithCancellation(CancellationToken cancellationToken);

        /// <summary>
        /// Tells if you want to clear the potential cached data before requesting
        /// </summary>
        /// <param name="clearCache">Clear cache or not</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithCacheClearing(bool clearCache);

        internal TApizrOptionsBuilder WithOriginalExpression(Expression originalExpression);
        internal TApizrOptionsBuilder WithResilienceContextOptions(IApizrResilienceContextOptions options);
        internal TApizrOptionsBuilder WithContext(ResilienceContext context);
    }
    
    /// <inheritdoc />
    public interface IApizrRequestOptionsBuilder :
        IApizrRequestOptionsBuilder<IApizrRequestOptions, IApizrRequestOptionsBuilder>
    {
        internal IApizrRequestOptions ApizrOptions { get; }
    }
}
