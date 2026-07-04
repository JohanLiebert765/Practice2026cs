using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;

namespace task11
{
    public static class Generator
    {
        public static ICalculator CreateCalculator(string code)
        {
            var SyntaxTree = CSharpSyntaxTree.ParseText(code);
            var references = new[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(ICalculator).Assembly.Location)
            };
            var compilation = CSharpCompilation.Create(
                "ClassCalculator",
                new[] { SyntaxTree }, references,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
            );
            var dllPath = Path.Combine(Path.GetTempPath(), "Calculator.dll");
            var result = compilation.Emit(dllPath);
            if (!result.Success)
            {
                throw new Exception($"Ошибка!");
            }
            var assembly = Assembly.LoadFrom(dllPath);
            var type = assembly.GetTypes().First(type => typeof(ICalculator).IsAssignableFrom(type));
            var calculator = (ICalculator)Activator.CreateInstance(type);
            return calculator;
        }
    }
}

