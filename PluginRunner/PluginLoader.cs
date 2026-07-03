namespace PluginRunner
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
    }
}

