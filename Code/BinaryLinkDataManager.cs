using System;
using System.Collections.Generic;
using System.IO;

namespace Lab1.Code
{
    public class BinaryLinkDataManager<T> : IDataManager<T> where T : IParsable, ISerializable, new()
    {
        public string Id => "BinLink";
        public string Pattern { get; }

        private int _count = 0;

        public BinaryLinkDataManager(string pattern)
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
                    int index = reader.ReadInt32();
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
                int next = (++_count % (data.Length)) ;
                writer.Write(next);
                item.SerializeToBinary(writer);
            }
        }
    }
}
