using System;
using System.Collections.Generic;
using System.IO;

public class ArrayDataManager<T> : IDataManager<T> where T : IParsable, new()
{
    public ArrayDataManager() { }

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

    public void Write(T[] data, string filename)
    {
        using (var writer = new StreamWriter(filename))
        {
            foreach (var item in data)
            {
                writer.WriteLine(item.ToString());
            }
        }
    }
}
