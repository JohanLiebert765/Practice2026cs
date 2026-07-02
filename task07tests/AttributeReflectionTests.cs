using System.Reflection;
using System.IO;
using System;
using Xunit;
using task07;

public class AttributeReflectionTests
{
    [Fact]
    public void Class_HasDisplayNameAttribute()
    {
        var type = typeof(SampleClass);
        var attribute = type.GetCustomAttribute<DisplayNameAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("Пример класса", attribute.DisplayName);
    }

    [Fact]
    public void Method_HasDisplayNameAttribute()
    {
        var method = typeof(SampleClass).GetMethod("TestMethod");
        var attribute = method.GetCustomAttribute<DisplayNameAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("Тестовый метод", attribute.DisplayName);
    }

    [Fact]
    public void Property_HasDisplayNameAttribute()
    {
        var prop = typeof(SampleClass).GetProperty("Number");
        var attribute = prop.GetCustomAttribute<DisplayNameAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("Числовое свойство", attribute.DisplayName);
    }

    [Fact]
    public void Class_HasVersionAttribute()
    {
        var type = typeof(SampleClass);
        var attribute = type.GetCustomAttribute<VersionAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal(1, attribute.Major);
        Assert.Equal(0, attribute.Minor);
    }

    [Fact]
    public void PrinttypeInfo_CorrectInfo()
    {
        var Out = Console.Out;
        using var writer = new StringWriter();
        Console.SetOut(writer);
        ReflectionHelper.PrintTypeInfo(typeof(SampleClass));
        Console.SetOut(Out);
        var output = writer.ToString();
        Assert.Contains("Пример класса", output);
        Assert.Contains("1.0", output);
        Assert.Contains("TestMethod", output);
        Assert.Contains("Тестовый метод", output);
        Assert.Contains("Number", output);
        Assert.Contains("Числовое свойство", output);
    }
}

