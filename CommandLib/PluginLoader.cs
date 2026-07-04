using System.Reflection;

namespace CommandLib
{
   public static class PluginLoader
    {
        public static List<string> GraphSort(Dictionary<string, string[]> plugins)
        {
            var loaded = new List<string>();
            while(loaded.Count < plugins.Count)
            {
                foreach (var plugin in plugins)
                {
                    if (loaded.Contains(plugin.Key))
                    {
                        continue;
                    }
                    var AllDependenciesLoaded = plugin.Value
                    .All(d => loaded.Contains(d));
                    if(AllDependenciesLoaded)
                    {
                        loaded.Add(plugin.Key);
                    }
                }
            }
            return loaded;
        }
        public static List<string> LoadAndExecute(string FolderPath)
        {
            var dllFiles = Directory.GetFiles(FolderPath, "*.dll");
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
            var loadOrder = GraphSort(plugindependencies);
            foreach (var PluginName in loadOrder)
            {
                var type = PluginTypes[PluginName];
                var command = (ICommand)Activator.CreateInstance(type);
                command.Execute();
            }
            return loadOrder;
        }
    }
}

