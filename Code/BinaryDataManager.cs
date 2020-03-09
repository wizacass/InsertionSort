using System.Collections.Generic;
using System.IO;

public class BinaryDataManager<T> : IDataManager<T> where T : IParsable, ISerializable, new()
{
    public string Id => "Bin";
    public string Pattern { get; }

    public BinaryDataManager(string pattern)
    {
        Pattern = pattern;
    }

    public T[] Read(string fileId)
    {
        string filename = string.Format(Pattern, fileId);
        var items = new List<T>();

        using (var reader = new BinaryReader(File.Open(filename, FileMode.Open)))
        {
            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                var item = new T();
                item.DeserializeFromBinary(reader);
                items.Add(item);
            }
        }

        return items.ToArray();
    }

    public void Write(T[] data, string fileId)
    {
        string filename = string.Format(Pattern, fileId);
        using var writer = new BinaryWriter(File.Open(filename, FileMode.Create));
        foreach (var item in data)
        {
            item.SerializeToBinary(writer);
        }
    }
}
