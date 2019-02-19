using System;
using Pioneer.Logs.Tubs.NetCoreConsole;

namespace Pioneer.Logs.Console.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            PioneerLogsTub.RegisterLogger();

            try
            {
                System.Console.WriteLine("********* DriveSafe LPR API Load Test Mocking *********");
                System.Console.WriteLine();

                System.Console.Write("Enter number of iterations:");
                RunPerformanceLoad(int.Parse(System.Console.ReadLine()));

                System.Console.WriteLine();
                System.Console.WriteLine("Done");

                System.Console.ReadKey();
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                System.Console.ReadKey();
                throw;
            }
        }

        private static void RunPerformanceLoad(int iterations)
        {
            for (var i = 0; i < iterations; i++)
            {
                PioneerLogsTub.StartPerformanceTracker(i.ToString());
                PioneerLogsTub.StopPerformanceTracker();
            }
        }
    }
}
