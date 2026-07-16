namespace task17
{
    public interface IScheduler
    {
        bool HasCommand();
        ICommand Select();
        void Add(ICommand cmd);
        void Remove(ICommand cmd);
    }
}

