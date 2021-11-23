using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Mapping;
using MediatR;
using Polly;

namespace Apizr.Mediation.Requesting.Sending
{
    public class ApizrMediator : IApizrMediator
    {
        public Task SendFor<TWebApi>(Expression<Func<TWebApi, Task>> executeApiMethod)
        {
            throw new NotImplementedException();
        }

        public Task SendFor<TWebApi>(Expression<Func<Context, TWebApi, Task>> executeApiMethod, Context context = null)
        {
            throw new NotImplementedException();
        }

        public Task SendFor<TWebApi>(Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task SendFor<TWebApi>(Expression<Func<Context, CancellationToken, TWebApi, Task>> executeApiMethod, Context context = null, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task SendFor<TWebApi, TModelData, TApiData>(Expression<Func<TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData)
        {
            throw new NotImplementedException();
        }

        public Task SendFor<TWebApi, TModelData, TApiData>(Expression<Func<CancellationToken, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData,
            CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task SendFor<TWebApi, TModelData, TApiData>(Expression<Func<Context, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData, Context context = null)
        {
            throw new NotImplementedException();
        }

        public Task SendFor<TWebApi, TModelData, TApiData>(Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData, Context context = null,
            CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<TApiData> SendFor<TWebApi, TApiData>(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod)
        {
            throw new NotImplementedException();
        }

        public Task<TApiData> SendFor<TWebApi, TApiData>(Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod, Context context = null)
        {
            throw new NotImplementedException();
        }

        public Task<TApiData> SendFor<TWebApi, TApiData>(Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<TApiData> SendFor<TWebApi, TApiData>(Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, Context context = null, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod)
        {
            throw new NotImplementedException();
        }

        public Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod, Context context = null)
        {
            throw new NotImplementedException();
        }

        public Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, Context context = null,
            CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData)
        {
            throw new NotImplementedException();
        }

        public Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(Expression<Func<Context, TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData, Context context = null)
        {
            throw new NotImplementedException();
        }

        public Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(Expression<Func<CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData,
            CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData, Context context = null,
            CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<TModelResultData> SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData)
        {
            throw new NotImplementedException();
        }

        public Task<TModelResultData> SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(Expression<Func<CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<TModelResultData> SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(Expression<Func<Context, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, Context context = null)
        {
            throw new NotImplementedException();
        }

        public Task<TModelResultData> SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(Expression<Func<Context, CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, Context context = null, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }

    public class ApizrMediator<TWebApi> : IApizrMediator<TWebApi>
    {
        private readonly IMediator _mediator;

        public ApizrMediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task SendFor(Expression<Func<TWebApi, Task>> executeApiMethod)
        {
            throw new NotImplementedException();
        }

        public Task SendFor(Expression<Func<Context, TWebApi, Task>> executeApiMethod, Context context = null)
        {
            throw new NotImplementedException();
        }

        public Task SendFor(Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task SendFor(Expression<Func<Context, CancellationToken, TWebApi, Task>> executeApiMethod, Context context = null, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task SendFor<TModelData, TApiData>(Expression<Func<TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData)
        {
            throw new NotImplementedException();
        }

        public Task SendFor<TModelData, TApiData>(Expression<Func<CancellationToken, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData,
            CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task SendFor<TModelData, TApiData>(Expression<Func<Context, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData, Context context = null)
        {
            throw new NotImplementedException();
        }

        public Task SendFor<TModelData, TApiData>(Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData, Context context = null,
            CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<TApiData> SendFor<TApiData>(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod)
        {
            throw new NotImplementedException();
        }

        public Task<TApiData> SendFor<TApiData>(Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod, Context context = null)
        {
            throw new NotImplementedException();
        }

        public Task<TApiData> SendFor<TApiData>(Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<TApiData> SendFor<TApiData>(Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, Context context = null, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<TModelData> SendFor<TModelData, TApiData>(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod)
        {
            throw new NotImplementedException();
        }

        public Task<TModelData> SendFor<TModelData, TApiData>(Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod, Context context = null)
        {
            throw new NotImplementedException();
        }

        public Task<TModelData> SendFor<TModelData, TApiData>(Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<TModelData> SendFor<TModelData, TApiData>(Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, Context context = null,
            CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<TModelData> SendFor<TModelData, TApiData>(Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData)
        {
            throw new NotImplementedException();
        }

        public Task<TModelData> SendFor<TModelData, TApiData>(Expression<Func<Context, TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData, Context context = null)
        {
            throw new NotImplementedException();
        }

        public Task<TModelData> SendFor<TModelData, TApiData>(Expression<Func<CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData,
            CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<TModelData> SendFor<TModelData, TApiData>(Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData, Context context = null,
            CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<TModelResultData> SendFor<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData)
        {
            throw new NotImplementedException();
        }

        public Task<TModelResultData> SendFor<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(Expression<Func<CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<TModelResultData> SendFor<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(Expression<Func<Context, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, Context context = null)
        {
            throw new NotImplementedException();
        }

        public Task<TModelResultData> SendFor<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(Expression<Func<Context, CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, Context context = null, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
