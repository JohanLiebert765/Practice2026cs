using Xunit;
using FileSystemCommands;

namespace task08tests
{
    public class FileSystemCommandsTests
    {
        [Fact]
        public void DirectorySizeCommand_ShouldCalculateSize()
        {
            var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
            if(Directory.Exists(testDir))
            {
                Directory.Delete(testDir, true);
            }
            Directory.CreateDirectory(testDir);
            File.WriteAllText(Path.Combine(testDir, "test1.txt"), "Hello");
            File.WriteAllText(Path.Combine(testDir, "test2.txt"), "World");

            var command = new DirectorySizeCommand(testDir);
            command.Execute();
            Assert.Equal(10, command.Size);

            Directory.Delete(testDir, true);
        }

        [Fact]
        public void FindFilesCommand_ShouldFindMatchingFiles()
        {
            var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
            if(Directory.Exists(testDir))
            {
                Directory.Delete(testDir, true);
            }
            Directory.CreateDirectory(testDir);
            File.WriteAllText(Path.Combine(testDir, "file1.txt"), "Text");
            File.WriteAllText(Path.Combine(testDir, "file2.log"), "Log");

            var command = new FindFilesCommand(testDir, "*.txt");
            command.Execute();
            Assert.Single(command.FoundFiles);
            Assert.Contains("file1.txt", command.FoundFiles[0]);

            Directory.Delete(testDir, true);
        }

        [Fact]
        public void DirectorySizeCommand_ShouldCalculateSizeWithSubDirectories()
        {
            var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
            if(Directory.Exists(testDir))
            {
                Directory.Delete(testDir, true);
            }
            var subDir = Path.Combine(testDir, "SubDir");
            Directory.CreateDirectory(subDir);
            File.WriteAllText(Path.Combine(testDir, "file1.txt"), "Hello");
            File.WriteAllText(Path.Combine(subDir, "file2.txt"), "World");

            var command = new DirectorySizeCommand(testDir);
            command.Execute();
            Assert.Equal(10, command.Size);

            Directory.Delete(testDir, true);
        }

        [Fact]
        public void FindFilesCommand_ShouldFindFilesInSubDirectories()
        {
            var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
            if(Directory.Exists(testDir))
            {
                Directory.Delete(testDir, true);
            }
            var subDir = Path.Combine(testDir, "SubDir");
            Directory.CreateDirectory(subDir);
            File.WriteAllText(Path.Combine(testDir, "file1.txt"), "Hello");
            File.WriteAllText(Path.Combine(subDir, "file2.txt"), "World");

            var command = new FindFilesCommand(testDir, "*.txt");
            command.Execute();
            Assert.Equal(2, command.FoundFiles.Count);

            Directory.Delete(testDir, true);
        }

        [Fact]
        public void DirectorySizeCommand_ShouldReturnZero()
        {
            var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
            if(Directory.Exists(testDir))
            {
                Directory.Delete(testDir, true);
            }
            Directory.CreateDirectory(testDir);
            
            var command = new DirectorySizeCommand(testDir);
            command.Execute();
            Assert.Equal(0, command.Size);

            Directory.Delete(testDir, true);
        }

        [Fact]
        public void FindFilesCommand_ShouldReturnEmpty()
        {
            var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
            if(Directory.Exists(testDir))
            {
                Directory.Delete(testDir, true);
            }
            Directory.CreateDirectory(testDir);
            File.WriteAllText(Path.Combine(testDir, "file1.log"), "log");

            var command = new FindFilesCommand(testDir, "*.txt");
            command.Execute();
            Assert.Empty(command.FoundFiles);

            Directory.Delete(testDir, true);
        }
    }
}

