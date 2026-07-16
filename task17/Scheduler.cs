namespace task17
{
    public class Scheduler : IScheduler
    {
        private readonly List<ICommand> commands = new();
        private int index = 0;
        public bool HasCommand()
        {
            return commands.Count > 0;
        }
        public void Add(ICommand cmd)
        {
            if (cmd == null)
            {
                throw new ArgumentNullException(nameof(cmd));
            }
            commands.Add(cmd);
        }
        public ICommand Select()
        {
            if (commands.Count == 0)
            {
                throw new InvalidOperationException("В планировщике нет команд ");
            }  
            if (index >= commands.Count)
            {
                index = 0;
            }
            var command = commands[index];
            commands.RemoveAt(index);
            if (commands.Count > 0 && index >= commands.Count)
            {
                index = 0;
            }
            return command;
        }
    }
}

