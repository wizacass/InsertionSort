using System;
using System.IO;

namespace Lab1.Code
{
    public class BinaryFileLinkSorter<T> : IRunnable where T : IComparable<T>, ISerializable, new()
    {
        public string Id => "LLS_File";

        private const int IndexSize = 4;

        private string _filename;
        private readonly string _pattern;
        private readonly T _typeInstance;

        public BinaryFileLinkSorter(string pattern)
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

            long elementsCount = fs.Length / (_typeInstance.ByteSize + IndexSize);
            for (int i = 0; i < elementsCount; i++)
            {
                var current = ReadOne(reader, i);
                Console.WriteLine(current.ToString());


                /*int j = i - 1;
                while (j >= 0 && ReadOne(reader, j).CompareTo(current) > 0)
                {
                    WriteOne(writer, ReadOne(reader, j), j + 1);
                    j--;
                }

                WriteOne(writer, current, j + 1);
    */        
            }

            fs.Close();
        }

        public string StatusString(string label = null)
        {
            throw new NotImplementedException();
        }

        // TODO: Return type Tuple<T, int>obj&nxt
        private T ReadOne(BinaryReader br, int i)
        {
            var obj = new T();
            int k = i * (_typeInstance.ByteSize + IndexSize);
            br.BaseStream.Seek(k, SeekOrigin.Begin);
            int next = br.ReadInt32();
            obj.DeserializeFromBinary(br);

            Console.WriteLine("Next: " + next);
            return obj;
        }

        private void WriteOne(BinaryWriter bw, T obj, int i)
        {
            int k = i * (_typeInstance.ByteSize + IndexSize);
            bw.BaseStream.Seek(k, SeekOrigin.Begin);
            obj.SerializeToBinary(bw);
        }

        // TODO: private void Swap(int i, int j)
    }
}
