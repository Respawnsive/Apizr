using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NConsole;

namespace Apizr.Tools.NSwag
{
    public class CoreConsoleHost : IConsoleHost
    {
        public void WriteMessage(string message)
        {
            Console.Write(message);
        }

        public void WriteError(string message)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(message);
            Console.ForegroundColor = color;
        }

        public string ReadValue(string message)
        {
            Console.Write(message);
            return Console.ReadLine();
        }
    }
}
