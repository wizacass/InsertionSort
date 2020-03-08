using System.Collections.Generic;
using System.IO;

public class ArrayDataManager<T> : IDataManager<T> where T : IParsable, new()
{
    public string Pattern { get; private set; }

    public ArrayDataManager(string pattern)
    {
        Pattern = pattern;
    }

    public T[] Read(string filename)
    {
        var objects = new List<T>();
        using (var reader = new StreamReader(filename))
        {
            while (!reader.EndOfStream)
            {
                var obj = new T();
                obj.Parse(reader.ReadLine());
                objects.Add(obj);
            }
        }

        return objects.ToArray();
    }

    public void Write(T[] data, string number)
    {
        string filename = string.Format(Pattern, number);
        using var writer = new StreamWriter(filename);
        foreach (var item in data)
        {
            writer.WriteLine(item.ToString());
        }
    }
}
