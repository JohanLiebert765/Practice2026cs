using System.Reflection;
using CommandLib;
using FileSystemCommands;
using Xunit;

public class MetadataTests
{
    [Fact]
    public void DirectorySizeCommand_ShouldHaveDisplayNameAttribute()
    {
        var attribute = typeof(DirectorySizeCommand).GetCustomAttribute<DisplayNameAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("Вычисление размера каталога", attribute.DisplayName);
    }

    [Fact]
    public void DirectorySizeCommand_ShouldHaveVersionAttribute()
    {
        var attribute = typeof(DirectorySizeCommand).GetCustomAttribute<VersionAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal(1, attribute.Major);
        Assert.Equal(0, attribute.Minor);
    }

    [Fact]
    public void FindFilesCommand_ShouldHaveDisplayNameAttribute()
    {
         var attribute = typeof(FindFilesCommand).GetCustomAttribute<DisplayNameAttribute>();
         Assert.NotNull(attribute);
         Assert.Equal("Поиск файлов по маске", attribute.DisplayName);
    }

    [Fact]
    public void FindFilesCommand_ShouldHaveVersionAttribute()
    {
        var attribute = typeof(FindFilesCommand).GetCustomAttribute<VersionAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal(1, attribute.Major);
        Assert.Equal(0, attribute.Minor);
    }
}

