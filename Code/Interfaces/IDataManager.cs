using System;

public interface IDataManager<T>
{
    public T[] Read(string filename);

    public void Write(T[] data, string filename);
}
