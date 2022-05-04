// See https://aka.ms/new-console-template for more information

using System.Reflection;
using System.Text;
using Apizr.Tools.Generator;
using Apizr.Tools.Generator.Models;
using NJsonSchema.CodeGeneration.CSharp;
using NSwag;
using NSwag.CodeGeneration.CSharp;
using NSwag.CodeGeneration.OperationNameGenerators;

var swagger = "https://petstore.swagger.io/v2/swagger.json";
var result = await OpenApiDocument.FromUrlAsync(swagger);

var clientSettings = new ApizrGeneratorSettings
{
    UseActionResultType = true,
    WrapResponses = false,
    OperationNameGenerator = new MultipleClientsFromFirstTagAndOperationIdGenerator()
};

clientSettings.CodeGeneratorSettings.TemplateFactory = new ApizrTemplateFactory(clientSettings.CSharpGeneratorSettings, new Assembly[3]
{
                typeof (CSharpGeneratorSettings).GetTypeInfo().Assembly,
                typeof (CSharpGeneratorBaseSettings).GetTypeInfo().Assembly,
                typeof (ApizrGeneratorSettings).GetTypeInfo().Assembly
});
clientSettings.CSharpGeneratorSettings.Namespace = "Cms";
clientSettings.CSharpGeneratorSettings.ArrayType = "List";
clientSettings.CSharpGeneratorSettings.ArrayInstanceType = "List";
clientSettings.ResponseArrayType = "ICollection";
clientSettings.ResponseDictionaryType = "IDictionary";

clientSettings.ParameterArrayType = "IEnumerable";
clientSettings.ParameterDictionaryType = "IDictionary";
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
Console.WriteLine("Hello World!");
