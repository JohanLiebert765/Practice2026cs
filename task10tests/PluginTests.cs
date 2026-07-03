using System.Reflection;
using CommandLib;
using Xunit;
using PluginRunner;

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
        Assert.True(indexB < indexA, "PluginB раньше чем PluginA");
        Assert.True(indexB < indexC, "PluginB раньше чем PluginC");
    }
}

