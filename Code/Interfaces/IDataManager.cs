public interface IDataManager<T>
{
    T[] Read(string filename);

    void Write(T[] data, string filename);
}
