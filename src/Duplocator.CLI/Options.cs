using CommandLine;

namespace Duplocator.CLI
{
    public class Options
    {
        [Option('p', "path", Required = true, HelpText = "Folder path to be processed.")]
        public string FolderPath { get; set; }
    }
}