using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;

namespace Rx.ConsoleApp
{
    partial class Program
    {
        static void Main(string[] args)
        {
            var ticks = Observable.Timer(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1), Scheduler.Immediate);

            ticks.Subscribe(x => Print("1", ConsoleColor.Green));

            ticks.Subscribe(x =>
            {
                Thread.Sleep(TimeSpan.FromSeconds(3));

                Print("2", ConsoleColor.Yellow);
            });

            // todo: implement

            Console.ReadLine();
        }
    }
}