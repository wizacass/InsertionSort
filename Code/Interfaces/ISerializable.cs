using System.IO;

public interface ISerializable
{
    int ByteSize { get; }

    void SerializeToBinary(BinaryWriter bw);

    void DeserializeFromBinary(BinaryReader br);
}
