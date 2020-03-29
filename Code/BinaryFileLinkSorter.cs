using System;
using System.IO;

namespace Lab1.Code
{
    public class BinaryFileLinkSorter<T> : IRunnable where T : IComparable<T>, ISerializable, new()
    {
        private sealed class Node
        {
            public T Data { get; }
            public int CurrentRef { get; }
            public int NextRef { get; set; }

            public Node(T data, int currentRef, int? nextRef = null)
            {
                Data = data;
                CurrentRef = currentRef;
                NextRef = nextRef ?? 0;
            }
        }

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

            int next = 0;
            do
            {
                var currentNode = ReadOne(reader, next);
                next = currentNode.NextRef;
                Console.WriteLine($"{currentNode.Data.ToString(),-18}-> {currentNode.NextRef}");
            } while (next != 0);

            // Node sorted = null;
            // var current = Head(reader);
            // do
            // {
            //     var next = ReadOne(reader, current.NextRef);
            //     WriteIndex(writer, 0, current.NextRef);
            //     sorted = SortedInsert(reader, writer, sorted, current);
            //     current = ReadOne(reader, next.CurrentRef);
            // } while (current.CurrentRef != 0);

            // WriteIndex(writer, sorted.CurrentRef, Head(reader).CurrentRef);

            fs.Close();
        }

        private Node Head(BinaryReader br)
        {
            return ReadOne(br, 0);
        }

        public string StatusString(string label = null)
        {
            throw new NotImplementedException();
        }

        private Node SortedInsert(BinaryReader br, BinaryWriter bw, Node sortedHead, Node newNode)
        {
            if (sortedHead == null)
            {
                sortedHead = newNode;
            }
            else if (sortedHead.Data.CompareTo(newNode.Data) >= 0)
            {
                WriteIndex(bw, sortedHead.CurrentRef, newNode.NextRef);
                //newNode.Next = sortedHead;
                //newNode.Next.Previous = newNode;
                sortedHead = newNode;
            }
            else
            {
                var current = sortedHead;
                while (
                    current.NextRef != 0 &&
                    ReadOne(br, current.NextRef).Data.CompareTo(newNode.Data) < 0
                )
                {
                    current = ReadOne(br, current.NextRef);
                }

                newNode.NextRef = current.NextRef;
                // if (current.NextRef != 0 && newNode.NextRef != 0)
                // {
                //     newNode.Next.Previous = newNode;
                // }

                WriteIndex(bw, newNode.CurrentRef, current.NextRef);
            }

            return sortedHead;
        }

        private Node ReadOne(BinaryReader br, int i)
        {
            var obj = new T();
            int k = GetPosition(i);
            br.BaseStream.Seek(k, SeekOrigin.Begin);
            int next = br.ReadInt32();
            obj.DeserializeFromBinary(br);

            return new Node(obj, i, next);
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
    }
}
