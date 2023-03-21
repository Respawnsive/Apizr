using Apizr.Tools.NSwag.Commands.CodeGeneration;
using NConsole;
using NJsonSchema.Infrastructure;
using NSwag.Commands;

namespace Apizr.Tools.NSwag.Commands.Document
{
    [Command(Name = "new", Description = "Creates a new apizr.json file in the current directory.")]
    public class CreateApizrDocumentCommand : IConsoleCommand
    {
        public async Task<object> RunAsync(CommandLineProcessor processor, IConsoleHost host)
        {
            if (!DynamicApis.FileExists("apizr.json"))
            {
                await CreateDocumentAsync("apizr.json");
                host.WriteMessage("apizr.json file created.");
            }
            else
            {
                host.WriteMessage("apizr.json already exists.");
            }

            return null;
        }

        private static async Task CreateDocumentAsync(string filePath)
        {
            var document = new ApizrDocument
            {
                Path = filePath,
                CodeGenerators =
                {
                    OpenApiToApizrClientCommand = new OpenApiToApizrClientCommand()
                },
                Runtime = Runtime.Net70
            };

            await document.SaveAsync();
        }
    }
}
