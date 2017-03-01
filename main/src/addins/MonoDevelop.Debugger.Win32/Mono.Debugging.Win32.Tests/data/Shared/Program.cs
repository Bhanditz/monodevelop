using System;
using System.Diagnostics;

namespace Net45ConsoleApp32bit
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 1 && args[0] == "FAIL")
                throw new InvalidOperationException("Program fiald beacuse you want it :)");
            if ((args.Length == 1 && args[0] == "BREAK"))
                Debugger.Break();
            var str = "Test string";
            var строка = "Тестовая строка";
            Console.WriteLine(str);
            Console.WriteLine(строка);
        }
    }
}
