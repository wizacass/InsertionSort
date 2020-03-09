using System;
using System.IO;

namespace Lab1.Code
{
    public class BinaryFileSorter<T> : IRunnable where T : IComparable<T>, ISerializable, new()
    {
        public string Id => "NoMem";

        private string _filename;
        private readonly string _pattern;
        private readonly T _typeInstance;

        public BinaryFileSorter(string pattern)
        {
            _pattern = pattern;
            _typeInstance = new T();
        }

        public void Run(string fileId)
        {
            _filename = string.Format(_pattern, fileId);
        }

        public void Sort()
        {
            using var fs = new FileStream(_filename, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            using var reader = new BinaryReader(fs);
            using var writer = new BinaryWriter(fs);

            long elementsCount = fs.Length / _typeInstance.ByteSize;
            for (int i = 0; i < elementsCount; i++)
            {
                var current = ReadOne(reader, i);
                int j = i - 1;
                while (j >= 0 && ReadOne(reader, j).CompareTo(current) > 0)
                {
                    WriteOne(writer, ReadOne(reader, j), j + 1);
                    j--;
                }

                WriteOne(writer, current, j + 1);
            }

            fs.Close();
        }

        public string StatusString(string label = null)
        {
            throw new NotImplementedException();
        }

        private T ReadOne(BinaryReader br, int i)
        {
            var obj = new T();
            int k = i * _typeInstance.ByteSize;
            br.BaseStream.Seek(k, SeekOrigin.Begin);
            obj.DeserializeFromBinary(br);

            return obj;
        }

        private void WriteOne(BinaryWriter bw, T obj, int i)
        {
            int k = i * _typeInstance.ByteSize;
            bw.BaseStream.Seek(k, SeekOrigin.Begin);
            obj.SerializeToBinary(bw);
        }
    }
}
