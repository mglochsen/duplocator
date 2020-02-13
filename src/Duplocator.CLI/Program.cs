using System;
using System.IO;
using System.Linq;
using CommandLine;
using Duplocator.Data;

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
            var runnerOptions = new RunnerOptions(options.FolderPath);
            var runnerResult = new DuplocatorRunner().GetDuplicates(runnerOptions);

            Console.WriteLine($"Found {runnerResult.TotalDuplicates} duplicates in {runnerResult.DuplicateGroups.Count} groups. Elapsed time: {runnerResult.ElapsedTime.TotalSeconds:F1}s");

            if (!string.IsNullOrWhiteSpace(options.ExportFileName))
            {
                var csvContent = string.Join(Environment.NewLine, runnerResult.DuplicateGroups.Select(group => string.Join(",", group.Duplicates.OrderBy(_ => _))));
                File.WriteAllText(options.ExportFileName, csvContent);
            }
        }
    }
}
