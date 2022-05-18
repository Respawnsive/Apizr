﻿// See https://aka.ms/new-console-template for more information

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
var withPriority = true;
var withContext = true;
var withToken = true;
#else
var urlArg = new Argument<string>("url", "Swagger.json absolute url");

var nsOption = new Option<string>(new[] {"--namespace", "--ns"},
    () => Assembly.GetExecutingAssembly().GetName().Name ?? "Apizr.Tools.Generations", 
    "Generated files namespace")
{
    IsRequired = false
};

var withPriorityOption = new Option<bool>(new[] {"--withPriority", "--p"},
    () => false, 
    "Add a Priority parameter")
{
    IsRequired = false
};

var withContextOption = new Option<bool>(new[] {"--withContext", "--ctx"},
    () => false, 
    "Add a Context parameter")
{
    IsRequired = false
};

var withTokenOption = new Option<bool>(new[] {"--withCancellationToken", "--ct"},
    () => false, 
    "Add a CancellationToken parameter")
{
    IsRequired = false
};

var rootCommand = new RootCommand
{
    urlArg,
    nsOption,
    withPriorityOption,
    withContextOption,
    withTokenOption
};

rootCommand.SetHandler(async (string url, string ns, bool withPriority, bool withContext, bool withToken) =>
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
        OperationNameGenerator = new MultipleClientsFromFirstTagAndOperationIdGenerator(),
    };
    clientSettings.CodeGeneratorSettings.TemplateFactory = new ApizrTemplateFactory(clientSettings.CSharpGeneratorSettings, assemblies);
    clientSettings.CSharpGeneratorSettings.Namespace = ns;
    clientSettings.CSharpGeneratorSettings.ArrayType = "List";
    clientSettings.CSharpGeneratorSettings.ArrayInstanceType = "List";
    clientSettings.ResponseArrayType = "ICollection";
    clientSettings.ResponseDictionaryType = "IDictionary";
    clientSettings.ParameterArrayType = "IEnumerable";
    clientSettings.ParameterDictionaryType = "IDictionary";
    clientSettings.WithPriority = withPriority;
    clientSettings.WithContext = withContext;
    clientSettings.WithCancellationToken = withToken;

    var result = await OpenApiDocument.FromUrlAsync(url);

    var generator = new ApizrGenerator(result, clientSettings);
    var all = generator.GenerateAll().ToList();
    var dir = Path.Combine("Output", clientSettings.CSharpGeneratorSettings.Namespace);
    var modelsPath = Path.Combine(dir, "Models");
    var servicesPath = Path.Combine(dir, "Services");

    Directory.CreateDirectory(modelsPath);
    Directory.CreateDirectory(servicesPath);

    foreach (var model in all.Where(a => a.Category == CodeArtifactCategory.Contract && a.Type == CodeArtifactType.Class))
    {
        var modelFile = Path.Combine(modelsPath, $"{model.TypeName}.cs");
        File.WriteAllText(modelFile, model.Code, Encoding.UTF8);
        Console.WriteLine($"Model output file：{modelFile}");
    }
    foreach (var service in all.Where(a => a.Category == CodeArtifactCategory.Client && a.Type == CodeArtifactType.Interface))
    {
        var serviceFile = Path.Combine(servicesPath, $"I{service.TypeName}Service.cs");
        File.WriteAllText(serviceFile, service.Code, Encoding.UTF8);
        Console.WriteLine($"Interface output file：{serviceFile}");
    }

    var registration = all.First(a => a.Category == CodeArtifactCategory.Utility && a.Type == CodeArtifactType.Class);
    var registrationFile = Path.Combine(dir, $"{registration.TypeName}.cs");
    File.WriteAllText(registrationFile, registration.Code, Encoding.UTF8);
    Console.WriteLine($"Registration output file：{registrationFile}");

#if DEBUG
Console.WriteLine($"Done!");
#else
}, urlArg, 
nsOption,
withPriorityOption,
withContextOption,
withTokenOption);

return await rootCommand.InvokeAsync(args);
#endif