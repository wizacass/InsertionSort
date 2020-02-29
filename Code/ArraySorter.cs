using System;
using System.Linq;
using System.Text;

public class ArraySorter<T> where T : IComparable<T>, IEquatable<T>
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
        for (int i = 1; i < _objects.Length; i++)
        {
            var current = _objects[i];
            int j = i - 1;

            while (j >= 0 && _objects[j].CompareTo(current) > 0)
            {
                _objects[j + 1] = _objects[j];
                j = j - 1;
            }
            _objects[j + 1] = current;
        }
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
