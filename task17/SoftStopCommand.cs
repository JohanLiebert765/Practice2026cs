namespace task17
{
    public class SoftStopCommand : ICommand
    {
        private readonly ServerThread _server;
        public SoftStopCommand(ServerThread server)
        {
            _server = server;
        }
        public bool IsComplete => true;
        public void Execute()
        {
            if (Thread.CurrentThread != _server.Thread)
            {
                throw new InvalidOperationException("Ошибка! SoftStop можно выполнить только в своём потоке");
            }
            _server.SoftStop();
        }
    }
}

