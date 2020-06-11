using System;
using System.Text;

namespace Apizr.Logging
{
    public class DefaultLogHandler : ILogHandler
    {
        public void Write(Exception exception, params (string Key, string Value)[] parameters)
            => Write("Apizr exception", exception.ToString(), parameters);

        public void Write(string title, string description = null, params (string Key, string Value)[] parameters)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(title);

            if(!string.IsNullOrWhiteSpace(description))
                stringBuilder.AppendLine(description);

            foreach (var parameter in parameters)
            {
                stringBuilder.AppendLine($"{parameter.Key}: {parameter.Value}");
            }

            var message = stringBuilder.ToString();

            Console.WriteLine(message);
        }
    }
}
