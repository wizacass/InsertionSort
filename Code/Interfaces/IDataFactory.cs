using System;

public interface IDataFactory<T>
{
    public T GenerateEntry();

    public T[] GenerateEntries(int count);
}
