using System;

public interface IDataManager<T>
{
    public T[] Read(string filename);

    public void Write(string filename, T[] data);
}
