namespace Duplocator.Data
{
    /// <summary>
    /// Options for the <see cref="DuplocatorRunner" />.
    /// </summary>
    public class RunnerOptions
    {
        /// <summary>
        /// Creates a new instance of the <see cref="RunnerOptions" /> class.
        /// </summary>
        /// <param name="folderPath">The folder path to process.</param>
        public RunnerOptions(string folderPath)
        {
            FolderPath = folderPath;
        }

        /// <summary>
        /// Gets the folder path to process.
        /// </summary>
        public string FolderPath { get; }
    }
}