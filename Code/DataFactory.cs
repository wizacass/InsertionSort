using System;

public class DataFactory<T> : IDataFactory<T> where T : IParsable, new()
{
    private IDataStringBuilder _dataStringBuilder;

    public DataFactory(IDataStringBuilder dataStringBuilder)
    {
        _dataStringBuilder = dataStringBuilder;
    }

    public T[] GenerateEntries(int count)
    {
        var objects = new T[count];
        for (int i = 0; i < count; i++)
        {
            objects[i] = GenerateEntry();
        }
        return objects;
    }

    public T GenerateEntry()
    {
        var obj = new T();
        obj.Parse(_dataStringBuilder.GenerateDataString());
        return obj;
    }
}
