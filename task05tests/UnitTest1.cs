using Xunit;
using Moq;

public class TestClass
{
    public int PublicField;
    private string _privateField;
    public int Property { get; set; }

    public void Method() {}
}
 [Serializable]
 public class AttributedClass {}

 public class TestParams
{
    public bool ParamsMethod(int age, string name)
    {
        return true;
    }
}

 public class ClassAnalyzerTests
{
    [Fact]
    public void GetPublicMethods_returnsCorrectMethods()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var methods = analyzer.GetPublicMethods();

        Assert.Contains("Method", methods);
    }

    [Fact]
    public void GetMethodParams_ReturnsVoid_WithoutParametrs()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var parametrs = analyzer.GetMethodParams("Method");

        Assert.Contains("Void", parametrs);
    }

    [Fact]
    public void GetMethodParams_ReturnsParamsAndType_WithParametrs()
    {
        var analyzer = new ClassAnalyzer(typeof(TestParams));
        var parametrs = analyzer.GetMethodParams("ParamsMethod");

        Assert.Contains("age", parametrs);
        Assert.Contains("name", parametrs);
        Assert.Contains("Boolean", parametrs);
    }

    [Fact]
    public void GetAllFields_IncludesPrivateFields()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var fields = analyzer.GetAllFields();

        Assert.Contains("_privateField", fields);
    }

    [Fact]
    public void GetProperties_ReturnsCorrectProperties()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var properties = analyzer.GetProperties();

        Assert.Contains("Property", properties);
    }

    [Fact]
    public void HasAttribute_ReturnsTrue_AttributeExists()
    {
        var analyzer = new ClassAnalyzer(typeof(AttributedClass));
        var HasAttribute = analyzer.HasAttribute<SerializableAttribute>();

        Assert.True(HasAttribute);
    }

    [Fact]
    public void HasAttribute_ReturnsFalse_AttributeDoesNotExists()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var HasAttribute = analyzer.HasAttribute<SerializableAttribute>();

        Assert.False(HasAttribute);
    }
}


