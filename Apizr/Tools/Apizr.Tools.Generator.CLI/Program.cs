// See https://aka.ms/new-console-template for more information

using System.CommandLine;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Apizr.Tools.Generator;
using Apizr.Tools.Generator.Models;
using NJsonSchema.CodeGeneration.CSharp;
using NSwag;
using NSwag.CodeGeneration.CSharp;
using NSwag.CodeGeneration.OperationNameGenerators;

var urlArg = new Argument<string>("url", "Swagger.json absolute url");

var nsOption = new Option<string>(new[] {"--namespace", "--ns"},
    () => Assembly.GetExecutingAssembly().GetName().Name ?? "Apizr.Tools.Generations", 
    "Generated files namespace")
{
    IsRequired = false
};

var rootCommand = new RootCommand
{
    urlArg,
    nsOption
};

rootCommand.SetHandler(async (string url, string ns) =>
{
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


    //var swagger = "https://petstore.swagger.io/v2/swagger.json";
    var result = await OpenApiDocument.FromUrlAsync(url);

    var clientGenerator = new ApizrGenerator(result, clientSettings);
    var data = clientGenerator.GetAllClientType();
    var models = clientGenerator.GetAllGenerateDtoType();
    var dir = Path.Combine("output", clientSettings.CSharpGeneratorSettings.Namespace);
    var apisPath = Path.Combine(dir, "HttpApis");
    var modelsPath = Path.Combine(dir, "HttpModels");

    Directory.CreateDirectory(apisPath);
    Directory.CreateDirectory(modelsPath);

    foreach (var api in data)
    {
        var file = Path.Combine(apisPath, $"{api.TypeName}.cs");
        File.WriteAllText(file, api.Code, Encoding.UTF8);
        Console.WriteLine($"Interface output file：{file}");
    }
    foreach (var model in models)
    {
        var file = Path.Combine(modelsPath, $"{model.TypeName}.cs");
        File.WriteAllText(file, model.Code, Encoding.UTF8);
        Console.WriteLine($"Model output file：{file}");
    }
}, urlArg, nsOption);

return await rootCommand.InvokeAsync(args);
