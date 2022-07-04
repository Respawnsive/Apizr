namespace Apizr.Tools.Generator.CLI
{
    public class ApizrGeneratorOptions
    {
        public ApizrGeneratorOptions(string outputPath, string ns, ApizrRegistrationType registrationType, bool withPriority, bool withContext, bool withCancellationToken, bool withRetry, bool withLogs, CacheProviderType cacheProviderType, bool withMediation, bool withOptionalMediation, bool withMapping)
        {
            OutputPath = outputPath;
            Namespace = ns;
            RegistrationType = registrationType;
            WithPriority = withPriority;
            WithContext = withContext;
            WithCancellationToken = withCancellationToken;
            WithRetry = withRetry;
            WithLogs = withLogs;
            CacheProviderType = cacheProviderType;
            WithMediation = withMediation;
            WithOptionalMediation = withOptionalMediation;
            WithMapping = withMapping;
        }

        public string OutputPath { get; set; }
        public string Namespace { get; set; }
        public ApizrRegistrationType RegistrationType { get; set; }
        public bool WithPriority { get; set; }
        public bool WithContext { get; set; }
        public bool WithCancellationToken { get; set; }
        public bool WithRetry { get; set; }
        public bool WithLogs { get; set; }
        public CacheProviderType CacheProviderType { get; set; }
        public bool WithMediation { get; set; }
        public bool WithOptionalMediation { get; set; }
        public bool WithMapping { get; set; }
    }
}
