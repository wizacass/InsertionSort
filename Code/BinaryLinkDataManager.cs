using System;
using System.Collections.Generic;
using System.IO;

namespace Lab1.Code
{
    public class BinaryLinkDataManager<T> : IDataManager<T> where T : IParsable, ISerializable, new()
    {
        public string Id => "BinLink";
        public string Pattern { get; }

        private const int IndexSize = 4;

        private readonly T _typeInstance;

        private int ByteSize => _typeInstance.ByteSize + 2 * IndexSize;

        public BinaryLinkDataManager(string pattern)
        {
            Pattern = pattern;
            _typeInstance = new T();
        }

        public T[] Read(string fileId)
        {
            string filename = string.Format(Pattern, fileId);
            var items = new List<T>();

            using (var reader = new BinaryReader(File.Open(filename, FileMode.Open)))
            {
                int next = 0;
                do
                {
                    (var currentObj, int nextRef, int _) = ReadOne(reader, next);
                    next = nextRef;
                    items.Add(currentObj);
                } while (next != 0);
            }

            return items.ToArray();
        }

        public void Write(T[] data, string fileId)
        {
            int count = 0;

            string filename = string.Format(Pattern, fileId);
            using var writer = new BinaryWriter(File.Open(filename, FileMode.Create));
            foreach (var item in data)
            {
                int next = ++count == data.Length ? -1 : count;
                writer.Write(next);
                writer.Write(count - 2);
                item.SerializeToBinary(writer);
            }
        }

        private Tuple<T, int, int> ReadOne(BinaryReader br, int i)
        {
            var obj = new T();
            int k = i * ByteSize;
            br.BaseStream.Seek(k, SeekOrigin.Begin);
            int next = br.ReadInt32();
            int previous = br.ReadInt32();
            obj.DeserializeFromBinary(br);

            return new Tuple<T, int, int>(obj, next, previous);
        }
    }
}
