using System.Reflection;
using CommandLib;
using PluginRunner;

if (args.Length == 0)
{
    Console.WriteLine("Укажите путь к папке с плагинами");
    return;
}
string PluginFolder = args[0];
var dllFiles = Directory.GetFiles(PluginFolder, "*.dll");
var PluginTypes = new Dictionary<string, Type>();
foreach (var dllFile in dllFiles)
{
    var assembly = Assembly.LoadFrom(dllFile);
    foreach (var type in assembly.GetTypes())
    {
        var attribute = type.GetCustomAttribute<PluginLoadAttribute>();
        if (attribute != null && type.IsClass)
        {
            PluginTypes.Add(type.Name, type);
        }
    }
}
var plugindependencies = new Dictionary<string, string[]>();
foreach (var plugin in PluginTypes)
{
    var attribute = plugin.Value.GetCustomAttribute<PluginLoadAttribute>();
    plugindependencies.Add(plugin.Key, attribute.PluginDependencies);
}
var loadOrder = PluginLoader.GraphSort(plugindependencies);
foreach (var PluginName in loadOrder)
{
    var type = PluginTypes[PluginName];
    var command = (ICommand)Activator.CreateInstance(type);
    Console.WriteLine($"Загрузка: {PluginName}");
    command.Execute();
}

