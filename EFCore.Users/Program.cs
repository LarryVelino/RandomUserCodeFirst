using System;

namespace EFCore.Users
{
    using System.Runtime.Loader;
    using System.Threading;

    public class Program
    {
        static void Main(string[] args)
        {
            var ended = new ManualResetEventSlim();
            var starting = new ManualResetEventSlim();

            AssemblyLoadContext.Default.Unloading += ctx =>
                {
                    System.Console.WriteLine("Unloading fired");
                    starting.Set();
                    System.Console.WriteLine("Waiting for completion");
                    ended.Wait();
                };

            System.Console.WriteLine("Waiting for signals");
            starting.Wait();

            System.Console.WriteLine("Received signal gracefully shutting down");
            Thread.Sleep(5000);
            ended.Set();
        }
    }
}
