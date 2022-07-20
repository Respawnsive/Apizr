using NConsole;
using NJsonSchema.Infrastructure;
using NSwag.Commands;

namespace Apizr.Tools.NSwag.Commands.Document
{
    [Command(Name = "run", Description = "Executes an .nswag file. If 'input' is not specified then all *.nswag files and the apizr.json file is executed.")]
    public class ExecuteApizrDocumentCommand : IConsoleCommand
    {
        [Argument(Position = 1, IsRequired = false)]
        public string Input { get; set; }

        [Argument(Name = nameof(Variables), IsRequired = false)]
        public string Variables { get; set; }

        public async Task<object> RunAsync(CommandLineProcessor processor, IConsoleHost host)
        {
            if (!string.IsNullOrEmpty(Input) && !Input.StartsWith("/") && !Input.StartsWith("-"))
            {
                await ExecuteDocumentAsync(host, Input);
            }
            else
            {
                var hasNSwagJson = DynamicApis.FileExists("apizr.json");
                if (hasNSwagJson)
                {
                    await ExecuteDocumentAsync(host, "apizr.json");
                }

                var currentDirectory = DynamicApis.DirectoryGetCurrentDirectory();
                var files = DynamicApis.DirectoryGetFiles(currentDirectory, "*.nswag");
                if (files.Any())
                {
                    foreach (var file in files)
                    {
                        await ExecuteDocumentAsync(host, file);
                    }
                }
                else if (!hasNSwagJson)
                {
                    host.WriteMessage("Current directory does not contain any .nswag files.");
                }
            }
            return null;
        }

        private async Task ExecuteDocumentAsync(IConsoleHost host, string filePath)
        {
            host.WriteMessage("\nExecuting file '" + filePath + "' with variables '" + Variables + "'...\n");

            var document = await ApizrDocument.LoadWithTransformationsAsync(filePath, Variables);
            if (document.Runtime != Runtime.Default)
            {
                if (document.Runtime != RuntimeUtilities.CurrentRuntime)
                {
                    throw new InvalidOperationException("The specified runtime in the document (" + document.Runtime + ") differs " +
                                                        "from the current process runtime (" + RuntimeUtilities.CurrentRuntime + "). " +
                                                        "Change the runtime with the '/runtime:" + document.Runtime + "' parameter " +
                                                        "or run the file with the correct command line binary.");
                }

                if (document.SelectedSwaggerGenerator == document.SwaggerGenerators.WebApiToOpenApiCommand &&
                    document.SwaggerGenerators.WebApiToOpenApiCommand.IsAspNetCore == false &&
                    document.Runtime != Runtime.Debug &&
                    document.Runtime != Runtime.WinX86 &&
                    document.Runtime != Runtime.WinX64)
                {
                    throw new InvalidOperationException("The runtime " + document.Runtime + " in the document must be used " +
                                                        "with ASP.NET Core. Enable /isAspNetCore:true.");
                }
            }

            await document.ExecuteAsync();
            host.WriteMessage("Done.\n");
        }
    }
}
