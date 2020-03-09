using System.IO;

public interface ISerializable
{
    void SerializeToBinary(BinaryWriter bw);

    void DeserializeFromBinary(BinaryReader br);
}
