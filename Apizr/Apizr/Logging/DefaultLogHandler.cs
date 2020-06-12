using System;
using System.Text;

namespace Apizr.Logging
{
    public class DefaultLogHandler : ILogHandler
    {
        public void Write(string message, string description = null, params (string Key, string Value)[] parameters)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(message);

            if(!string.IsNullOrWhiteSpace(description))
                stringBuilder.AppendLine(description);

            foreach (var parameter in parameters)
            {
                stringBuilder.AppendLine($"{parameter.Key}: {parameter.Value}");
            }

            var builtMessage = stringBuilder.ToString();

            Console.WriteLine(builtMessage);
        }
    }
}
