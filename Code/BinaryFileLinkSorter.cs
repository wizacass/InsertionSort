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
            public int PrevRef { get; set; }

            public Node(T data, int currentRef, int? nextRef = null, int? prevRef = null)
            {
                Data = data;
                CurrentRef = currentRef;
                NextRef = nextRef ?? -1;
                PrevRef = prevRef ?? -1;
            }
        }

        public string Id => "LLS_File";

        private const int IndexSize = 4;

        private int ByteSize => _typeInstance.ByteSize + 2 * IndexSize;
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

            // int next = 0;
            // while (next != -1)
            // {
            //     var currentNode = ReadOne(reader, next);
            //     next = currentNode.NextRef;
            //     Console.WriteLine($"{currentNode.PrevRef,3} <- [{currentNode.Data.ToString(),-16}] -> {currentNode.NextRef}");
            // }

            var current = Head(fs, reader);
            int nextRef = current.NextRef;
            Console.WriteLine($"{current.PrevRef,3} <- [{current.Data.ToString(),-16}] -> {current.NextRef}");
            while (current.NextRef != -1)
            {
                var previous = current;
                current = ReadOne(reader, nextRef);
                nextRef = current.NextRef;
                Console.WriteLine($"{current.PrevRef,3} <- [{current.Data.ToString(),-16}] -> {current.NextRef}");

                while (previous.PrevRef != -1 && current.Data.CompareTo(previous.Data) < 0)
                {
                    //System.Console.WriteLine("Test!");
                    previous = ReadOne(reader, previous.PrevRef);
                }

                if (!ReadOne(reader, current.PrevRef).Equals(previous))
                {
                    System.Console.WriteLine("Nelygu!");
                    WriteNextIndex(writer, current.NextRef, current.PrevRef);
                    if (current.NextRef != -1)
                    {
                        WritePreviousIndex(writer, current.PrevRef, current.NextRef);
                    }
                    current.NextRef = -1;
                    current.PrevRef = -1;

                    current.NextRef = previous.NextRef;
                    current.PrevRef = previous.CurrentRef;
                    SaveIndexes(writer, current);

                    if (previous.NextRef != -1)
                    {
                        WritePreviousIndex(writer, current.CurrentRef, previous.NextRef);
                    }
                    WriteNextIndex(writer, current.CurrentRef, previous.CurrentRef);
                }

                // WritePreviousIndex(writer, current.PrevRef, current.NextRef);
                // WriteNextIndex(writer, current.NextRef, current.PrevRef);

                // current.NextRef = -1;
                // current.PrevRef = -1;

                // current.NextRef = previous.CurrentRef;
                // previous.PrevRef = current.CurrentRef;

            }

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

        private Node Head(FileStream fs, BinaryReader br)
        {
            long length = GetElementsCount(fs);
            for (int i = 0; i < length; i++)
            {
                var node = ReadOne(br, i);
                if (node.PrevRef == -1)
                {
                    return node;
                }
            }
            return null;
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
                WriteNextIndex(bw, sortedHead.CurrentRef, newNode.NextRef);
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

                WriteNextIndex(bw, newNode.CurrentRef, current.NextRef);
            }

            return sortedHead;
        }

        private Node ReadOne(BinaryReader br, int i)
        {
            if (i < 0)
            {
                return null;
            }

            var obj = new T();
            int k = i * ByteSize;
            br.BaseStream.Seek(k, SeekOrigin.Begin);
            int next = br.ReadInt32();
            int Previous = br.ReadInt32();
            obj.DeserializeFromBinary(br);

            return new Node(obj, i, next, Previous);
        }

        private void WriteOne(BinaryWriter bw, T obj, int i)
        {
            int k = i * ByteSize;
            bw.BaseStream.Seek(k, SeekOrigin.Begin);
            obj.SerializeToBinary(bw);
        }

        private void WriteNextIndex(BinaryWriter bw, int index, int i)
        {
            int k = i * ByteSize;
            bw.BaseStream.Seek(k, SeekOrigin.Begin);
            bw.Write(index);
        }

        private void WritePreviousIndex(BinaryWriter bw, int index, int i)
        {
            int k = i * ByteSize + IndexSize;
            bw.BaseStream.Seek(k, SeekOrigin.Begin);
            bw.Write(index);
        }

        private void SaveIndexes(BinaryWriter bw, Node node)
        {
            WriteNextIndex(bw, node.NextRef, node.CurrentRef);
            WritePreviousIndex(bw, node.PrevRef, node.CurrentRef);
        }

        private long GetElementsCount(FileStream fs)
        {
            return fs.Length / ByteSize;
        }
    }
}
