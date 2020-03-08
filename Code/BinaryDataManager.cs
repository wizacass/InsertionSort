using System.IO;

public class BinaryDataManager<T> : IDataManager<T> where T : IParsable, ISerializable, new()
{
    public string Pattern { get; private set; }

    public BinaryDataManager(string pattern)
    {
        Pattern = pattern;
    }

    public T[] Read(string filename)
    {
        throw new System.NotImplementedException();
    }

    public void Write(T[] data, string number)
    {
        string filename = string.Format(Pattern, number);
        using var writer = new BinaryWriter(File.Open(filename, FileMode.Create));
        foreach (var item in data)
        {
            item.SerializeToBinary(writer);
        }
    }
}
