using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

namespace DebuggerTests
{
    internal struct TestStruct
    {
        public int F1;
        public string F2;
        public int[] F3;
        public string[] F4;
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            const double flt = 2.1;
            var strct =
                new TestStruct()
                {
                    F1 = 1,
                    F2 = "Test F2",
                    F3 = new[] {1, 2, 3},
                    F4 = new[] {"T1", "T2", "T3"}

                };
            var intArray = new[] {1, 2, 3, 4, 5};
            var stringArray = new[] { "S1", "S2", "S3" };
            var str = "Test string";
            var строка = "Тестовая строка";
            if (args.Length == 1 && args[0] == "FAIL")
            {
                Console.WriteLine("Throw exception");
                throw new InvalidOperationException("Program failed beacuse you want it :)");
            }
            if ((args.Length == 1 && args[0] == "BREAK"))
            {
                Console.WriteLine("Break with debugger break");
                Debugger.Break();
            }
            if ((args.Length == 2 && args[0] == "SLEEP"))
            {
                var millisecondsTimeout = int.Parse(args[1]);
                Console.WriteLine("Waiting {0} seconds...", millisecondsTimeout / 1000);
                Thread.Sleep(millisecondsTimeout);
            }

            Console.WriteLine(flt);
            Console.WriteLine(strct);
            Console.WriteLine(intArray);
            Console.WriteLine(stringArray);
            Console.WriteLine(str);
            Console.WriteLine(строка);
        }
    }
}
