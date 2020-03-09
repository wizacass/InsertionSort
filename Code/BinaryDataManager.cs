using System.IO;

public class BinaryDataManager<T> : IDataManager<T> where T : IParsable, ISerializable, new()
{
    public string Pattern { get; }

    public BinaryDataManager(string pattern)
    {
        Pattern = pattern;
    }

    public T[] Read(string fileId)
    {
        throw new System.NotImplementedException();
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
