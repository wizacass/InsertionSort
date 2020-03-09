using System;
using System.Text;

public class ArraySorter<T> : IRunnable where T : IComparable<T>, IEquatable<T>
{
    private readonly IDataManager<T> _dataManager;
    private T[] _objects;

    public ArraySorter(IDataManager<T> dataManager)
    {
        _dataManager = dataManager;
    }

    public string Id => "AS";

    public void Run(string fileId)
    {
        _objects = _dataManager.Read(fileId);
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

    public string StatusString(string label = null)
    {
        var sb = new StringBuilder();
        if (label != null)
        {
            sb.AppendLine(label);
        }

        foreach (var item in _objects)
        {
            sb.AppendLine(item.ToString());
        }

        return sb.ToString();
    }
}
