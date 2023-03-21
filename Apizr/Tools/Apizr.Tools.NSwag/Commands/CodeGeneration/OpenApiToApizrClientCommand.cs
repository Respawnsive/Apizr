using System.Reflection;
using Apizr.Tools.NSwag.Extensions;
using Apizr.Tools.NSwag.Generation;
using Apizr.Tools.NSwag.Generation.Models;
using NConsole;
using NJsonSchema.CodeGeneration;
using NJsonSchema.CodeGeneration.CSharp;
using NSwag.CodeGeneration.CSharp;
using NSwag.Commands.CodeGeneration;
using Parlot.Fluent;

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
            get => Settings.RegistrationType;
            set => Settings.RegistrationType = value;
        }

        [Argument(Name = "WithPriority", IsRequired = false, Description = "Add a Priority parameter into Get methods")]
        public bool WithPriority
        {
            get => Settings.WithPriority;
            set => Settings.WithPriority = value;
        }

        [Argument(Name = "WithRetry", IsRequired = false, Description = "Add retry management")]
        public bool WithRetry
        {
            get => Settings.WithRetry;
            set => Settings.WithRetry = value;
        }

        [Argument(Name = "WithLogs", IsRequired = false, Description = "Add logs")]
        public bool WithLogs
        {
            get => Settings.WithLogs;
            set => Settings.WithLogs = value;
        }

        [Argument(Name = "WithRequestOptions", IsRequired = false, Description = "Add Request Options")]
        public bool WithRequestOptions
        {
            get => Settings.WithRequestOptions;
            set => Settings.WithRequestOptions = value;
        }

        [Argument(Name = "WithCacheProvider", IsRequired = false, Description = "Add cache management (None, Akavache, MonkeyCache, InMemory, Distributed or Custom)")]
        public CacheProviderType WithCacheProvider
        {
            get => Settings.WithCacheProvider;
            set => Settings.WithCacheProvider = value;
        }

        [Argument(Name = "WithMediation", IsRequired = false, Description = "Add MediatR")]
        public bool WithMediation
        {
            get => Settings.WithMediation;
            set => Settings.WithMediation = value;
        }

        [Argument(Name = "WithOptionalMediation", IsRequired = false, Description = "Add Optional.Async")]
        public bool WithOptionalMediation
        {
            get => Settings.WithOptionalMediation;
            set => Settings.WithOptionalMediation = value;
        }

        [Argument(Name = "WithMapping", IsRequired = false, Description = "Add data mapping")]
        public bool WithMapping
        {
            get => Settings.WithMapping;
            set => Settings.WithMapping = value;
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
            var dir = OutputFilePath;
            var modelsPath = Path.Combine(dir, "Models");
            var apisPath = Path.Combine(dir, "Apis");

            Directory.CreateDirectory(modelsPath);
            Directory.CreateDirectory(apisPath);

            foreach (var model in all.Where(a => a.Category == CodeArtifactCategory.Contract))
            {
                var modelFile = Path.Combine(modelsPath, $"{model.TypeName}.cs");
                result.Add(modelFile, model.Code);
            }
            foreach (var service in all.Where(a => a is {Category: CodeArtifactCategory.Client, Type: CodeArtifactType.Interface}))
            {
                var apiFile = $"{service.TypeName}.cs";
                var serviceFile = Path.Combine(apisPath, apiFile);
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
