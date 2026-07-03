using System.Reflection;
using CommandLib;

if (args.Length == 0)
{
    Console.WriteLine("Укажите путь к DLL");
    return;
}
Assembly assembly = Assembly.LoadFrom(args[0]);
foreach (var type in assembly.GetTypes())
{
    if (type.IsClass)
    {
        var displayname = type.GetCustomAttribute<DisplayNameAttribute>();
        var version = type.GetCustomAttribute<VersionAttribute>();
        if(displayname != null)
        {
            Console.WriteLine($"Класс: {type.Name} ({displayname.DisplayName})");
        }
        else
        {
            Console.WriteLine($"Класс: {type.Name}");
        }
        if (version != null)
        {
            Console.WriteLine($"Версия: {version.Major}.{version.Minor}");
        }
        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        foreach (var method in methods.Where(m => !m.IsSpecialName))
        {
            var MethodDisplayName = method.GetCustomAttribute<DisplayNameAttribute>();
            var parameters = method.GetParameters();
            var ParametersList = string.Join(", ", parameters.Select(p => $"{p.ParameterType.Name} {p.Name}"));
            if (MethodDisplayName != null)
            {
                Console.WriteLine($"Метод: {method.Name} ({ParametersList}) ({MethodDisplayName.DisplayName})");
            }
            else
            {
                Console.WriteLine($"Метод: {method.Name} ({ParametersList})");
            }
        }
        var constructors = type.GetConstructors();
        foreach (var constructor in constructors)
        {
            var parameters = constructor.GetParameters();
            var ParametersList = string.Join(", ", parameters.Select(p => $"{p.ParameterType.Name} {p.Name}"));
            Console.WriteLine($"Конструктор: {type.Name} ({ParametersList})");
        }
        Console.WriteLine();
    }
}


