using CommandLib;

namespace FileSystemCommands
{
    public class FindFilesCommand: ICommand
    {
        private readonly string _DirectoryPath;
        private readonly string _Mask;
        public List<string> FoundFiles { get; private set; }
        public FindFilesCommand(string DirectoryPath, string Mask)
        {
            _DirectoryPath = DirectoryPath;
            _Mask = Mask;
            FoundFiles = new List<string>();
        }
        public void Execute()
        {
            var directory = new DirectoryInfo(_DirectoryPath);
            if(!directory.Exists)
            {
                throw new DirectoryNotFoundException($"Не удалось найти каталог: {_DirectoryPath}");
            }
            var files = directory.GetFiles(_Mask, SearchOption.AllDirectories);
            foreach (var file in files)
            {
                FoundFiles.Add(file.FullName);
            }
        }
    }
}

