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

//            int next = 0;
//            do
//            {
//                (var currentObj, int nextRef) = ReadOne(reader, next);
//                next = nextRef;
//                Console.WriteLine($"{currentObj.ToString(),-18}-> {nextRef}");
//            } while (next != 0);

            Tuple<T, int> sorted = null;
            var current = ReadOne(reader, 0);
            while (current.Item2 != 0)
            {
                var next = ReadOne(reader, current.Item2);
                WriteIndex(writer, 0, next.Item2);
                sorted = SortedInsert(writer, sorted, current);
                current = ReadOne(reader, next.Item2);
            }

            WriteIndex(writer, sorted.Item2, 0);

            fs.Close();
        }

        public string StatusString(string label = null)
        {
            throw new NotImplementedException();
        }

        private Tuple<T, int> SortedInsert(BinaryWriter bw, Tuple<T, int> sortedHead, Tuple<T, int> newNode)
        {
            if (sortedHead == null)
            {
                sortedHead = newNode;
            }
            else if ((sortedHead).Item1.CompareTo(newNode.Item1) >= 0)
            {
                WriteIndex(bw, sortedHead.Item2, newNode.Item2);
                newNode.Next = sortedHead;
                //newNode.Next.Previous = newNode;
                sortedHead = newNode;
            }
            else { }

            return sortedHead
        }

        private Tuple<T, int> ReadOne(BinaryReader br, int i)
        {
            var obj = new T();
            int k = GetPosition(i);
            br.BaseStream.Seek(k, SeekOrigin.Begin);
            int next = br.ReadInt32();
            obj.DeserializeFromBinary(br);

            return new Tuple<T, int>(obj, next);
        }

        private void WriteOne(BinaryWriter bw, T obj, int i)
        {
            int k = GetPosition(i);
            bw.BaseStream.Seek(k, SeekOrigin.Begin);
            obj.SerializeToBinary(bw);
        }

        private void WriteIndex(BinaryWriter bw, int index, int i)
        {
            int k = GetPosition(i);
            bw.BaseStream.Seek(k, SeekOrigin.Begin);
            bw.Write(index);
        }

        private int GetPosition(int i)
        {
            return i * (_typeInstance.ByteSize + IndexSize);
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
