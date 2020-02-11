using CommandLine;

namespace Duplocator.CLI
{
    public class Options
    {
        [Option('p', "path", Required = true, HelpText = "Folder path to be processed.")]
        public string FolderPath { get; set; }

        [Option('e', "export", Required = false, HelpText = "File path for result export file.")]
        public string ExportFileName { get; set; }
    }
}