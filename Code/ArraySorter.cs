using System;
using System.Linq;
using System.Text;

public class ArraySorter<T>
{
    private IDataManager<T> _dataManager;
    private IDataFactory<T> _factory;
    private T[] _objects;

    public ArraySorter(IDataManager<T> dataManager, IDataFactory<T> factory)
    {
        _dataManager = dataManager;
        _factory = factory;
    }

    public void Run(string filename)
    {
        _objects = _dataManager.Read(filename);
    }

    public void Generate(int count, string filename)
    {
        var entries = _factory.GenerateEntries(count);
        _dataManager.Write(entries, filename);
    }

    public void Sort()
    {
        Array.Sort(_objects);
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (var item in _objects)
        {
            sb.AppendLine(item.ToString());
        }
        return sb.ToString();
    }
}
