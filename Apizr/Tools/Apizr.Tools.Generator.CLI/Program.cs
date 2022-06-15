// See https://aka.ms/new-console-template for more information

using System.CommandLine;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Apizr.Tools.Generator;
using Apizr.Tools.Generator.Models;
using NJsonSchema.CodeGeneration;
using NJsonSchema.CodeGeneration.CSharp;
using NSwag;
using NSwag.CodeGeneration.CSharp;
using NSwag.CodeGeneration.OperationNameGenerators;

// cmd: C:\Dev\Community\Apizr\Apizr\tools\apizr.tools.generator.cli\bin\debug\net6.0> .\Apizr.Tools.Generator.CLI.exe "http://localhost/ApizrSampleApi/swagger/v1/swagger.json"

#if DEBUG
var ns = "Test";
//var url = "http://localhost/ApizrSampleApi/swagger/v1/swagger.json";
var url = "https://petstore.swagger.io/v2/swagger.json";
var registrationType = ApizrRegistrationType.Both;
var withPriority = true;
var withContext = true;
var withToken = true;
var withRetry = true;
var withLogs = true;
var withCacheProvider = CacheProviderType.Akavache;
var withMediation = true;
var withOptionalMediation = true;
var urlArg = new Argument<string>("url", "Swagger.json absolute url");

var nsOption = new Option<string>(new[] {"--namespace", "--ns"},
    () => Assembly.GetExecutingAssembly().GetName().Name ?? "Apizr.Tools.Generations", 
    "Generated files namespace")
{
    IsRequired = false
};

var registrationTypeOption = new Option<ApizrRegistrationType>(new[] {"--registrationType", "--register", "--rg" },
    () => ApizrRegistrationType.Both, 
    "Set generated registration type")
{
    IsRequired = false
};

var withPriorityOption = new Option<bool>(new[] {"--withPriority", "--priority", "--pr"},
    () => false, 
    "Add a Priority parameter")
{
    IsRequired = false
};

var withContextOption = new Option<bool>(new[] {"--withContext", "--context", "--ctx"},
    () => false, 
    "Add a Polly Context parameter")
{
    IsRequired = false
};

var withTokenOption = new Option<bool>(new[] {"--withCancellationToken", "--token", "--ct"},
    () => false, 
    "Add a CancellationToken parameter")
{
    IsRequired = false
};

var withRetryOption = new Option<bool>(new[] {"--withRetry", "--retry", "--rt"},
    () => false, 
    "Add retry management")
{
    IsRequired = false
};

var withLogsOption = new Option<bool>(new[] { "--withLogs", "--log", "--l" },
    () => false,
    "Add logs")
{
    IsRequired = false
};

var withCacheProviderOption = new Option<CacheProviderType>(new[] { "--withCacheProvider", "--cache", "--cp" },
    () => CacheProviderType.None,
    "Add cache management")
{
    IsRequired = false
};

var withMediationOption = new Option<bool>(new[] { "--withMediation", "--mediation", "--med" },
    () => false,
    "Add MediatR")
{
    IsRequired = false
};

var withOptionalMediationOption = new Option<bool>(new[] { "--withOptionalMediation", "--optional", "--opt" },
    () => false,
    "Add Optional")
{
    IsRequired = false
};

var rootCommand = new RootCommand
{
    urlArg,
    nsOption,
    registrationTypeOption,
    withPriorityOption,
    withContextOption,
    withTokenOption,
    withRetryOption,
    withLogsOption,
    withCacheProviderOption,
    withMediationOption,
    withOptionalMediationOption
};

#else
rootCommand.SetHandler(async (string url, string ns, ApizrRegistrationType registrationType, bool withPriority, bool withContext, bool withToken, bool withRetry, bool withLogs, CacheProviderType withCacheProvider, bool withMediation, bool withOptionalMediation) =>
{
#endif

    var assemblies = new[]
    {
        typeof(CSharpGeneratorSettings).GetTypeInfo().Assembly,
        typeof(CSharpGeneratorBaseSettings).GetTypeInfo().Assembly,
        typeof(ApizrGeneratorSettings).GetTypeInfo().Assembly
    };

    var clientSettings = new ApizrGeneratorSettings
    {
        UseActionResultType = true,
        WrapResponses = false,
        OperationNameGenerator = new MultipleClientsFromFirstTagAndOperationIdGenerator()//SingleClientFromOperationIdOperationNameGenerator() or MultipleClientsFromFirstTagAndOperationIdGenerator(),
    };
    clientSettings.CodeGeneratorSettings.TemplateFactory = new ApizrTemplateFactory(clientSettings.CSharpGeneratorSettings, assemblies);
    clientSettings.CSharpGeneratorSettings.Namespace = ns;
    clientSettings.CSharpGeneratorSettings.ArrayType = "List";
    clientSettings.CSharpGeneratorSettings.ArrayInstanceType = "List";
    clientSettings.ResponseArrayType = "ICollection";
    clientSettings.ResponseDictionaryType = "IDictionary";
    clientSettings.ParameterArrayType = "IEnumerable";
    clientSettings.ParameterDictionaryType = "IDictionary";
    clientSettings.RegistrationType = registrationType;
    clientSettings.WithPriority = withPriority;
    clientSettings.WithContext = withContext;
    clientSettings.WithCancellationToken = withToken;
    clientSettings.WithRetry = withRetry;
    clientSettings.WithLogs = withLogs;
    clientSettings.WithCacheProvider = withCacheProvider;
    clientSettings.WithMediation = withMediation;
    clientSettings.WithOptionalMediation = withOptionalMediation;

    var result = await OpenApiDocument.FromUrlAsync(url);

    var generator = new ApizrGenerator(result, clientSettings);
    var all = generator.GenerateAll().ToList();
    var dir = Path.Combine("Output", clientSettings.CSharpGeneratorSettings.Namespace);
    var modelsPath = Path.Combine(dir, "Models");
    var servicesPath = Path.Combine(dir, "Services");

    Directory.CreateDirectory(modelsPath);
    Directory.CreateDirectory(servicesPath);

    foreach (var model in all.Where(a => a.Category == CodeArtifactCategory.Contract))
    {
        var modelFile = Path.Combine(modelsPath, $"{model.TypeName}.cs");
        File.WriteAllText(modelFile, model.Code, Encoding.UTF8);
        Console.WriteLine($"Model output file：{modelFile}");
    }
    foreach (var service in all.Where(a => a.Category == CodeArtifactCategory.Client && a.Type == CodeArtifactType.Interface))
    {
        var serviceFile = Path.Combine(servicesPath, $"I{service.TypeName ?? "Api"}Service.cs");
        File.WriteAllText(serviceFile, service.Code, Encoding.UTF8);
        Console.WriteLine($"Interface output file：{serviceFile}");
    }

    var registration = all.FirstOrDefault(a => a.Category == CodeArtifactCategory.Utility && a.Type == CodeArtifactType.Class);
    if (registration != null)
    {
        var registrationFile = Path.Combine(dir, $"{registration.TypeName}.cs");
        File.WriteAllText(registrationFile, registration.Code, Encoding.UTF8);
        Console.WriteLine($"Registration output file：{registrationFile}"); 
    }

#if DEBUG
Console.WriteLine($"Done!");
#else
}, urlArg, 
nsOption,
registrationTypeOption,
withPriorityOption,
withContextOption,
withTokenOption,
withRetryOption,
withLogsOption,
withCacheProviderOption,
withMediationOption,
withOptionalMediationOption);

return await rootCommand.InvokeAsync(args);
#endif