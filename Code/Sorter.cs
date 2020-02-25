using System;
using System.Text;

public class Sorter<T>
{
    private IDataManager<T> _dataManager;
    private T[] _objects;

    public Sorter(IDataManager<T> dataManager)
    {
        _dataManager = dataManager;
    }

    public void Run(string filename)
    {
        _objects = _dataManager.Read(filename);
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
