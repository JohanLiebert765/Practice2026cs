using System.Collections.Concurrent;

namespace task17
{
    public class ServerThread
    {
        private readonly BlockingCollection<ICommand> queueCommand = new();
        private readonly Thread thread;
        public Action<ICommand, Exception> ExceptionHandler { get; set; }
        public ServerThread()
        {
            thread = new Thread(ProcessCommands)
            {
                IsBackground = true
            };
        }
        public Thread Thread => thread;
        public void Start()
        {
            thread.Start();
        }
        public void QueueCommand(ICommand command)
        {
            queueCommand.Add(command);
        }
        private volatile bool hardStop = false;
        private volatile bool softStop = false;
        private void ProcessCommands()
        {
            while (true)
            {
                if (hardStop)
                {
                    break;
                }
                if (softStop && queueCommand.Count == 0)
                {
                    break;
                }
                ICommand command = queueCommand.Take();
                try
                {
                    command.Execute();
                }
                catch (Exception exception)
                {
                    if (ExceptionHandler != null)
                    {
                        ExceptionHandler(command, exception);
                    }
                }
            }
        }
        public void HardStop()
        {
            hardStop = true;
        }
        public void SoftStop()
        {
            softStop = true;
        }
    }
}

