public interface IRunnable
{
    string Id { get; }

    void Run(string fileId);

    void Sort();

    string StatusString(string label = null);
}
