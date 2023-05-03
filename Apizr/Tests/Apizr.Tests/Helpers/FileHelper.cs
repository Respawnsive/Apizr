using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.VisualBasic.FileIO;
using Refit;

namespace Apizr.Tests.Helpers
{
    internal static class FileHelper
    {
        internal static StreamPart GetTestFileStreamPart(string fileName)
        {
            var fileExtension = "pdf";
            var fileType = "application/pdf";
            var relativeFilePath = $"Files/Test_{fileName}.{fileExtension}";

            const char namespaceSeparator = '.';

            // get calling assembly
            var assembly = Assembly.GetCallingAssembly();

            // compute resource name suffix
            var relativeName = "." + relativeFilePath
                .Replace('\\', namespaceSeparator)
                .Replace('/', namespaceSeparator)
                .Replace(' ', '_');

            // get resource stream
            var fullName = assembly
                .GetManifestResourceNames()
                .FirstOrDefault(name => name.EndsWith(relativeName, StringComparison.InvariantCulture));
            if (fullName == null)
            {
                throw new Exception($"Unable to find resource for path \"{relativeFilePath}\". Resource with name ending on \"{relativeName}\" was not found in assembly.");
            }

            var stream = assembly.GetManifestResourceStream(fullName);
            if (stream == null)
            {
                throw new Exception($"Unable to find resource for path \"{relativeFilePath}\". Resource named \"{fullName}\" was not found in assembly.");
            }

            return new StreamPart(stream, $"test_{fileName}-streampart.{fileExtension}", $"{fileType}");
        }
    }
}
