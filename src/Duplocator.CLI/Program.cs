using System;
using System.Linq;
using CommandLine;

namespace Duplocator.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default
                .ParseArguments<Options>(args)
                .WithParsed(Run);
        }

        private static void Run(Options options)
        {
            var duplicates = new Locator().GetDuplicates(options.FolderPath).ToArray();

            Console.WriteLine($"Found {duplicates.SelectMany(_ => _).Count()} duplicates in {duplicates.Count()} groups.");
        }
    }
}
