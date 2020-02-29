using System;
using System.Text;

public class ArraySorter<T> where T : IComparable<T>, IEquatable<T>
{
    private readonly IDataManager<T> _dataManager;
    private T[] _objects;

    public ArraySorter(IDataManager<T> dataManager)
    {
        _dataManager = dataManager;
    }

    public void Run(string filename)
    {
        _objects = _dataManager.Read(filename);
    }

    public void Sort()
    {
        for (int i = 0; i < _objects.Length; i++)
        {
            var current = _objects[i];
            int j = i - 1;

            while (j >= 0 && _objects[j].CompareTo(current) > 0)
            {
                _objects[j + 1] = _objects[j];
                j--;
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
