using System.Collections.Concurrent;

namespace task17
{
    public class ServerThread
    {
        private readonly BlockingCollection<ICommand> queueCommand = new();
        private readonly Thread thread;
        public Action<ICommand, Exception> ExceptionHandler { get; set; }
        private readonly IScheduler scheduler = new Scheduler();
        private readonly AutoResetEvent NewCommandEvent = new AutoResetEvent(false);
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
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            queueCommand.Add(command);
            NewCommandEvent.Set();
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
                while (queueCommand.TryTake(out var command))
                {
                    scheduler.Add(command);
                }
                if (scheduler.HasCommand())
                {
                    var command = scheduler.Select();
                    try
                    {
                        command.Execute();
                    }
                    catch (Exception exception)
                    {
                        if(ExceptionHandler != null)
                        {
                            ExceptionHandler(command, exception);
                        }
                    }
                }
                else
                {
                    if (softStop)
                    {
                        break;
                    }
                    else
                    {
                        NewCommandEvent.Reset();
                        if (queueCommand.Count > 0)
                        {
                            continue;
                        }
                        NewCommandEvent.WaitOne();
                    }
                }
            }
        }
        public void HardStop()
        {
            hardStop = true;
            NewCommandEvent.Set();
        }
        public void SoftStop()
        {
            softStop = true;
            NewCommandEvent.Set();
        }
    }
}

