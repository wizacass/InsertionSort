public interface IDataManager<T>
{
    string Pattern { get; }

    T[] Read(string fileId);

    void Write(T[] data, string fileId);
}
