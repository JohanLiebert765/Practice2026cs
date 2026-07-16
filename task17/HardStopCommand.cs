namespace task17
{
    public class HardStopCommand : ICommand
    {
        private readonly ServerThread _server;
        public HardStopCommand(ServerThread server)
        {
            _server = server;
        }
        public void Execute()
        {
            if (Thread.CurrentThread != _server.Thread)
            {
                throw new InvalidOperationException("Ошибка! HardStop можно выполнить только в своём потоке");
            }
            _server.HardStop();
        }
    }
}

