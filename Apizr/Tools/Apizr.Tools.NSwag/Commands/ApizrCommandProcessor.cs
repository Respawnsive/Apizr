using System.Diagnostics;
using System.Reflection;
using Apizr.Tools.NSwag.Commands.CodeGeneration;
using NConsole;
using NJsonSchema;
using NJsonSchema.Infrastructure;
using NSwag;
using NSwag.Commands.CodeGeneration;

namespace Apizr.Tools.NSwag.Commands
{
    /// <summary></summary>
    public class ApizrCommandProcessor
    {
        private readonly IConsoleHost _host;

        /// <summary>Initializes a new instance of the <see cref="ApizrCommandProcessor" /> class.</summary>
        /// <param name="host">The host.</param>
        public ApizrCommandProcessor(IConsoleHost host)
        {
            _host = host;
        }

        /// <summary>Processes the command line arguments.</summary>
        /// <param name="args">The arguments.</param>
        /// <returns>The result.</returns>
        public int Process(string[] args) => ProcessAsync(args).GetAwaiter().GetResult();

        /// <summary>Processes the command line arguments.</summary>
        /// <param name="args">The arguments.</param>
        /// <returns>The result.</returns>
        public async Task<int> ProcessAsync(string[] args)
        {
            _host.WriteMessage("toolchain v" + OpenApiDocument.ToolchainVersion +
                " (NJsonSchema v" + JsonSchema.ToolchainVersion + ")\n");
            _host.WriteMessage("Visit http://NSwag.org for more information.\n");

            WriteBinDirectory();

            if (args.Length == 0)
            {
                _host.WriteMessage("Execute the 'help' command to show a list of all the available commands.\n");
            }

            try
            {
                var processor = new CommandLineProcessor(_host);

                processor.RegisterCommandsFromAssembly(typeof(OpenApiToApizrClientCommand).GetTypeInfo().Assembly);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                await processor.ProcessAsync(args).ConfigureAwait(false);
                stopwatch.Stop();

                _host.WriteMessage("\nDuration: " + stopwatch.Elapsed + "\n");
            }
            catch (Exception exception)
            {
                _host.WriteError(exception.ToString());
                return -1;
            }

            WaitWhenDebuggerAttached();
            return 0;
        }

        private void WriteBinDirectory()
        {
            try
            {
                Assembly entryAssembly;
                var getEntryAssemblyMethod = typeof(Assembly).GetRuntimeMethod("GetEntryAssembly", Array.Empty<Type>());
                if (getEntryAssemblyMethod != null)
                {
                    entryAssembly = (Assembly)getEntryAssemblyMethod.Invoke(null, Array.Empty<object>());
                }
                else
                {
                    entryAssembly = typeof(ApizrCommandProcessor).GetTypeInfo().Assembly;
                }

                var binDirectory = DynamicApis.PathGetDirectoryName(new Uri(entryAssembly.CodeBase).LocalPath);
                _host.WriteMessage("NSwag bin directory: " + binDirectory + "\n");
            }
            catch (Exception exception)
            {
                _host.WriteMessage("NSwag bin directory could not be determined: " + exception.Message + "\n");
            }
        }

        private void WaitWhenDebuggerAttached()
        {
            if (Debugger.IsAttached)
            {
                _host.ReadValue("Press <enter> key to exit");
            }
        }
    }
}
