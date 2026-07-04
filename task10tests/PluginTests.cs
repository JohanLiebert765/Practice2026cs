using System.Reflection;
using CommandLib;
using Xunit;

public class PluginTests
{
    [Fact]
    public void PluginLoadAttribute_ShouldStoreEmptyDependencies()
    {
        var attribute = new PluginLoadAttribute();
        Assert.Empty(attribute.PluginDependencies);
    }

    [Fact]
    public void PluginLoadAttribute_ShouldStoreDependencies()
    {
        var attribute = new PluginLoadAttribute("PluginB", "PluginC");
        Assert.Equal(2, attribute.PluginDependencies.Length);
        Assert.Contains("PluginB", attribute.PluginDependencies);
        Assert.Contains("PluginC", attribute.PluginDependencies);
    }

    [Fact]
    public void PluginB_ShouldHavePluginLoadAttribute()
    {
        var type = typeof(PluginB.PluginB);
        var attribute = type.GetCustomAttribute<PluginLoadAttribute>();
        Assert.NotNull(attribute);
        Assert.Empty(attribute.PluginDependencies);
    }

    [Fact]
    public void PluginA_ShouldDependOnPluginB()
    {
        var type = typeof(PluginA.PluginA);
        var attribute = type.GetCustomAttribute<PluginLoadAttribute>();
        Assert.NotNull(attribute);
        Assert.Single(attribute.PluginDependencies);
        Assert.Contains("PluginB", attribute.PluginDependencies);
    }

    [Fact]
    public void PluginLoader_ShouldLoadInCorrectOrder()
    {
        var plugins = new Dictionary<string, string[]>
        {
            { "PluginA", new[] { "PluginB" } },
            { "PluginB", new string[] { } },
            { "PluginC", new[] { "PluginB" } }
        };
        var sorted = PluginLoader.GraphSort(plugins);
        var indexA = sorted.IndexOf("PluginA");
        var indexB = sorted.IndexOf("PluginB");
        var indexC = sorted.IndexOf("PluginC");
        Assert.True(indexB < indexA);
        Assert.True(indexB < indexC);
    }

    [Fact]
    public void LoadAndExecute_ShouldNoErrors()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "PluginTest");
        if (Directory.Exists(tempDir))
        {
            Directory.Delete(tempDir, true);
        }
        Directory.CreateDirectory(tempDir);
        var BasePath = AppDomain.CurrentDomain.BaseDirectory;
        File.Copy(Path.Combine(BasePath, "PluginA.dll"), Path.Combine(tempDir, "PluginA.dll"));
        File.Copy(Path.Combine(BasePath, "PluginB.dll"), Path.Combine(tempDir, "PluginB.dll"));
        File.Copy(Path.Combine(BasePath, "PluginC.dll"), Path.Combine(tempDir, "PluginC.dll"));
        File.Copy(Path.Combine(BasePath, "CommandLib.dll"), Path.Combine(tempDir, "CommandLib.dll"));
        var result = PluginLoader.LoadAndExecute(tempDir);
        var indexB = result.IndexOf("PluginB");
        var indexA = result.IndexOf("PluginA");
        var indexC = result.IndexOf("PluginC");
        Assert.True(indexB < indexA);
        Assert.True(indexB < indexC);
        Directory.Delete(tempDir, true);
    }
}

