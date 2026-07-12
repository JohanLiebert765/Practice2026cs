using CommandLib;

namespace PluginA
{
    [PluginLoad("PluginB")]
    public class PluginA: ICommand
    {
        public void Execute()
        {
            Console.WriteLine("PluginA");
        }
    }
}

