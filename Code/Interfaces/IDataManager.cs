public interface IDataManager<T>
{
    T[] Read(string filename);

    public void Write(T[] data, string filename);
}
