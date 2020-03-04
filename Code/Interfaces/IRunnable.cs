public interface IRunnable
{
    void Run(string filename);

    void Sort();

    string StatusString(string label = null);
}
