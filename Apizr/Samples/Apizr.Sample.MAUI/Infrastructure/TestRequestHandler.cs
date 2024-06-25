namespace Apizr.Sample.MAUI.Infrastructure
{
    public class TestRequestHandler : DelegatingHandler
    {
        private readonly ISecureStorage _secureStorage;

        public TestRequestHandler(ISecureStorage secureStorage)
        {
            _secureStorage = secureStorage;
        }

        /// <inheritdoc />
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var identityKey = await _secureStorage.GetAsync("IdentityKey");
            if (!string.IsNullOrWhiteSpace(identityKey))
            {
                request.Headers.Add("X-Identity-Key", identityKey);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
