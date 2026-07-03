using CommandLib;

namespace PluginB
{
    [PluginLoad]
    public class PluginB: ICommand
    {
        public void Execute()
        {
             Console.WriteLine("PluginB");
        }
    }
}

