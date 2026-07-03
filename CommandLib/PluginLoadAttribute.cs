namespace Commandlib
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PluginLoadAttribute: Attribute
    {
        public string[] PluginDependencies { get; }
        public PluginLoadAttribute(params string[] plugindependencies)
        {
            PluginDependencies = plugindependencies;
        }
    }
}

