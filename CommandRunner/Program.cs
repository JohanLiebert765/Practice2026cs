using System.Reflection;
using ICommand = CommandLib.ICommand;

string dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FileSystemCommands.dll");
Assembly assembly = Assembly.LoadFrom(dllPath);

var CommandTypes = assembly.GetTypes()
.Where(type => type.GetInterfaces().Contains(typeof(ICommand)));

foreach (var type in CommandTypes)
{
    Console.WriteLine($"Найдена команда: {type.Name}");
    var constructor = type.GetConstructors()[0];
    var parametrs = constructor.GetParameters();
    var arguments = new  object[parametrs.Length];
    for(int i = 0; i < parametrs.Length; i++)
    {
        if (parametrs[i].Name == "DirectoryPath")
        {
            arguments[i] = Directory.GetCurrentDirectory();
        }
        else if (parametrs[i].Name == "Mask")
        {
            arguments[i] = "*.*";
        }
        else
        {
            arguments[i] = "";
        }
    }
    var command = (ICommand)Activator.CreateInstance(type, arguments);
    command.Execute();
    foreach(var property in type.GetProperties())
    {
        var value = property.GetValue(command);
        Console.WriteLine($"{property.Name}: {value}");
    }
    Console.WriteLine();
}

