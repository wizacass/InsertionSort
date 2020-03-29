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

            //long elementsCount = fs.Length / (_typeInstance.ByteSize + IndexSize);
            //var currentNode = ReadOne(reader, 0);
            //int head = currentNode.Item2;
            int next = 0;
            do
            {
                (var currentObj, int nextRef) = ReadOne(reader, next); 
                next = nextRef;
                Console.WriteLine($"{currentObj.ToString(),-18}-> {nextRef}");
            } while (next != 0);


            /* for (int i = 0; i < elementsCount; i++)
            {
                var current = ReadOne(reader, i);
                Console.WriteLine(current.ToString());


                int j = i - 1;
                while (j >= 0 && ReadOne(reader, j).CompareTo(current) > 0)
                {
                    WriteOne(writer, ReadOne(reader, j), j + 1);
                    j--;
                }

                WriteOne(writer, current, j + 1);

            }*/

            fs.Close();
        }

        public string StatusString(string label = null)
        {
            throw new NotImplementedException();
        }

        private Tuple<T, int> ReadOne(BinaryReader br, int i)
        {
            var obj = new T();
            int k = i * (_typeInstance.ByteSize + IndexSize);
            br.BaseStream.Seek(k, SeekOrigin.Begin);
            int next = br.ReadInt32();
            obj.DeserializeFromBinary(br);

            return new Tuple<T, int>(obj, next);
        }

        private void WriteOne(BinaryWriter bw, T obj, int i)
        {
            int k = i * (_typeInstance.ByteSize + IndexSize);
            bw.BaseStream.Seek(k, SeekOrigin.Begin);
            obj.SerializeToBinary(bw);
        }

        private void Swap(BinaryReader br, BinaryWriter bw, int i, int j)
        {
            throw new NotImplementedException();

            int pos1 = i * (_typeInstance.ByteSize + IndexSize);
            int pos2 = j * (_typeInstance.ByteSize + IndexSize);
            
            br.BaseStream.Seek(pos1, SeekOrigin.Begin);
            int next = br.ReadInt32();
        }
    }
}
