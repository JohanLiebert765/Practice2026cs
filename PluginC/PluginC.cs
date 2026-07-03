using CommandLib;

namespace PluginC
{
    [PluginLoad("PluginB")]
    public class PluginC: ICommand
    {
        public void Execute()
        {
            Console.WriteLine("PluginC");
        }
    }
}

