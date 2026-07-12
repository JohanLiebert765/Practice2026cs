using CommandLib;

namespace FileSystemCommands
{
    public class DirectorySizeCommand: ICommand
    {
        private readonly string _DirectoryPath;
        public long Size { get; private set; }
        public DirectorySizeCommand(string DirectoryPath)
        {
            _DirectoryPath = DirectoryPath;
        }
        public void Execute()
        {
            var directory = new DirectoryInfo(_DirectoryPath);
            if (!directory.Exists)
            {
                throw new DirectoryNotFoundException($"Не удалось найти каталог: {_DirectoryPath}");
            }
            Size = CalculateSize(directory);
        }
        private long CalculateSize(DirectoryInfo directory)
        {
            long size = 0;
            foreach (var file in directory.GetFiles())
            {
                size += file.Length;
            }
            foreach (var subdirectory in directory.GetDirectories())
            {
                size += CalculateSize(subdirectory);
            }
            return size;
        }
    }
}

