namespace task17
{
    public interface ICommand
    {
        void Execute();
        bool IsComplete { get; }
    }
}

