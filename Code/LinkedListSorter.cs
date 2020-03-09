using System;
using System.Text;

public class LinkedListSorter<T> : IRunnable where T : IComparable<T>, IEquatable<T>
{
    private readonly IDataManager<T> _dataManager;
    private readonly MyLinkedList<T> _objects;

    public LinkedListSorter(IDataManager<T> dataManager)
    {
        _dataManager = dataManager;
        _objects = new MyLinkedList<T>();
    }

    public string Id => "LLS";

    public void Run(string fileId)
    {
        var data = _dataManager.Read(fileId);
        _objects.AddLast(data);
    }

    public void Sort()
    {
        _objects.Sort();
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
