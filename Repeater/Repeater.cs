using System;
using System.Diagnostics;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;
using System.IO;

namespace Repeater
{
    class Repeater
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Found " + args.Length + " parameters instead of 2");
                DieWithUsage();
                return;
            }

            int interval;
            if (!int.TryParse(args[0], out interval))
            {
                Console.WriteLine(args[0] + " is not an integer.");
                return;
            }

            if (!File.Exists(args[1]))
            {
                Console.WriteLine("There is no file at \"" + args[1] + "\"");
                return;
            }

            Run(args[1]);
            Timer t = new Timer(interval * 1000);
            t.Elapsed += delegate(object sender, ElapsedEventArgs e)
            {
                Run(args[1]);
            };
            t.Start();
            Thread.Sleep(Timeout.Infinite);
        }

        static void Run(string path)
        {
            Console.WriteLine(DateTime.Now + " -- Starting...");
            ProcessStartInfo info = new ProcessStartInfo(path);
            info.WindowStyle = ProcessWindowStyle.Hidden;
            Process p = Process.Start(info);
            p.WaitForExit();
            Console.WriteLine(DateTime.Now + " -- Done.");
        }

        static void DieWithUsage()
        {
            Console.WriteLine();
            Console.WriteLine("Usage:  Repeater.exe <interval> <path>");
            Console.WriteLine("<interval> is the integer number of seconds between executions of the file at <path>.");
            Console.WriteLine("Be sure to quote the file path if it contains spaces.");
        }
    }
}
