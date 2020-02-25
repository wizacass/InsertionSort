using System;
using System.Collections.Generic;
using System.IO;

public class DataManager<T> : IDataManager<T>
{
    public T[] Read(string filename)
    {
        throw new NotImplementedException();
    }

    public void Write(string filename, T[] data)
    {
        throw new NotImplementedException();
    }
}
