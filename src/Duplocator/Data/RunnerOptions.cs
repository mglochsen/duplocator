namespace Duplocator.Data
{
    public class RunnerOptions
    {
        public RunnerOptions(string folderPath)
        {
            FolderPath = folderPath;
        }

        public string FolderPath { get; }
    }
}