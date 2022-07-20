using System.Reflection;
using Apizr.Tools.NSwag.Generation;
using Apizr.Tools.NSwag.Generation.Models;
using NConsole;
using NJsonSchema.CodeGeneration;
using NJsonSchema.CodeGeneration.CSharp;
using NSwag.CodeGeneration.CSharp;
using NSwag.Commands.CodeGeneration;

namespace Apizr.Tools.NSwag.Commands.CodeGeneration
{

    [Command(Name = "swagger2apizr", Description = "Generates Apizr client code from a Swagger/OpenAPI specification.")]
    public class OpenApiToApizrClientCommand : OpenApiToCSharpCommandBase<ApizrGeneratorSettings>
    {
        public OpenApiToApizrClientCommand() : this(new ApizrGeneratorSettings())
        {
            
        }

        public OpenApiToApizrClientCommand(ApizrGeneratorSettings settings) : base(settings)
        {
            var assemblies = new[]
            {
                typeof(CSharpGeneratorSettings).GetTypeInfo().Assembly,
                typeof(CSharpGeneratorBaseSettings).GetTypeInfo().Assembly,
                typeof(ApizrGeneratorSettings).GetTypeInfo().Assembly
            };
            settings.CodeGeneratorSettings.TemplateFactory = new ApizrTemplateFactory(settings.CSharpGeneratorSettings, assemblies);
        }

        [Argument(Name = "RegistrationType", IsRequired = false, Description = "Generate a registration helper class (None, Static, Extended or Both)")]
        public ApizrRegistrationType RegistrationType
        {
            get { return Settings.RegistrationType; }
            set { Settings.RegistrationType = value; }
        }

        [Argument(Name = "WithPriority", IsRequired = false, Description = "Add a Priority parameter into Get methods")]
        public bool WithPriority
        {
            get { return Settings.WithPriority; }
            set { Settings.WithPriority = value; }
        }

        [Argument(Name = "WithContext", IsRequired = false, Description = "Add a Polly Context parameter into all methods")]
        public bool WithContext
        {
            get { return Settings.WithContext; }
            set { Settings.WithContext = value; }
        }

        [Argument(Name = "WithCancellationToken", IsRequired = false, Description = "Add a CancellationToken parameter into all methods")]
        public bool WithCancellationToken
        {
            get { return Settings.WithCancellationToken; }
            set { Settings.WithCancellationToken = value; }
        }

        [Argument(Name = "WithRetry", IsRequired = false, Description = "Add retry management")]
        public bool WithRetry
        {
            get { return Settings.WithRetry; }
            set { Settings.WithRetry = value; }
        }

        [Argument(Name = "WithLogs", IsRequired = false, Description = "Add logs")]
        public bool WithLogs
        {
            get { return Settings.WithLogs; }
            set { Settings.WithLogs = value; }
        }

        [Argument(Name = "WithCacheProvider", IsRequired = false, Description = "Add cache management (None, Akavache, MonkeyCache, InMemory, Distributed or Custom)")]
        public CacheProviderType WithCacheProvider
        {
            get { return Settings.WithCacheProvider; }
            set { Settings.WithCacheProvider = value; }
        }

        [Argument(Name = "WithMediation", IsRequired = false, Description = "Add MediatR")]
        public bool WithMediation
        {
            get { return Settings.WithMediation; }
            set { Settings.WithMediation = value; }
        }

        [Argument(Name = "WithOptionalMediation", IsRequired = false, Description = "Add Optional.Async")]
        public bool WithOptionalMediation
        {
            get { return Settings.WithOptionalMediation; }
            set { Settings.WithOptionalMediation = value; }
        }

        [Argument(Name = "WithMapping", IsRequired = false, Description = "Add data mapping")]
        public bool WithMapping
        {
            get { return Settings.WithMapping; }
            set { Settings.WithMapping = value; }
        }

        public override async Task<object> RunAsync(CommandLineProcessor processor, IConsoleHost host)
        {
            var result = await RunAsync();
            foreach (var pair in result)
            {
                await TryWriteFileOutputAsync(pair.Key, host, () => pair.Value).ConfigureAwait(false);
            }

            return result;
        }

        public async Task<Dictionary<string, string>> RunAsync()
        {
            var result = new Dictionary<string, string>();
            var document = await GetInputSwaggerDocument().ConfigureAwait(false);
            var generator = new ApizrGenerator(document, Settings);

            var all = generator.GenerateAll().ToList();
            var dir = Path.Combine(OutputFilePath, Settings.CSharpGeneratorSettings.Namespace);
            var modelsPath = Path.Combine(dir, "Models");
            var servicesPath = Path.Combine(dir, "Services");

            Directory.CreateDirectory(modelsPath);
            Directory.CreateDirectory(servicesPath);

            foreach (var model in all.Where(a => a.Category == CodeArtifactCategory.Contract))
            {
                var modelFile = Path.Combine(modelsPath, $"{model.TypeName}.cs");
                result.Add(modelFile, model.Code);
            }
            foreach (var service in all.Where(a => a.Category == CodeArtifactCategory.Client && a.Type == CodeArtifactType.Interface))
            {
                var serviceFile = Path.Combine(servicesPath, $"I{service.TypeName ?? "Api"}Service.cs");
                result.Add(serviceFile, service.Code);
            }
            foreach (var other in all.Where(a => a.Category != CodeArtifactCategory.Contract && a.Category != CodeArtifactCategory.Client))
            {
                var otherFile = Path.Combine(dir, $"{other.TypeName}.cs");
                result.Add(otherFile, other.Code);
            }

            return result;
        }
    }
}
