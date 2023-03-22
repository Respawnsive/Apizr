// See https://aka.ms/new-console-template for more information

using Apizr.Tools.NSwag;
using Apizr.Tools.NSwag.Commands;
using NConsole;
using NSwag.Commands;

Console.Write("Apizr dedicated version of NSwag command line tool for " + RuntimeUtilities.CurrentRuntime + ", ");
var processor = new ApizrCommandProcessor(new CoreConsoleHost());
return await processor.ProcessAsync(args);