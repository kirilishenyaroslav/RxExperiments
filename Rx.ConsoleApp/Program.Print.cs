using System;

namespace Rx.ConsoleApp
{
    partial class Program
    {
        static void Print(string s, ConsoleColor color)
        {
            lock (guard)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write($"[{DateTimeOffset.Now}]: ");

                Console.ForegroundColor = color;
                Console.WriteLine(s);
            }
        }

        static object guard = new object();
    }
}
