using System.IO;

public class BinaryDataManager<T> : IDataManager<T> where T : IParsable, ISerializable, new()
{
    public T[] Read(string filename)
    {
        throw new System.NotImplementedException();
    }

    public void Write(T[] data, string filename)
    {
        using var writer = new BinaryWriter(File.Open(filename, FileMode.Create));
        foreach (var item in data)
        {
            item.SerializeToBinary(writer);
        }
    }
}
