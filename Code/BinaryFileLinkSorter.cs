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

            bool sorted = false;
            while (!sorted)
            {
                sorted = true;
                var head = Head(reader);
                int next = head.NextRef;
                while (next != -1)
                {
                    var currentNode = ReadOne(reader, next);
                    var previousNode = ReadOne(reader, currentNode.PrevRef);
                    next = currentNode.NextRef;

                    if (currentNode.Data.CompareTo(previousNode.Data) >= 0)
                    {
                        continue;
                    }

                    sorted = false;
                    while (currentNode.Data.CompareTo(previousNode.Data) >= 0 && previousNode.PrevRef != -1)
                    {
                        previousNode = ReadOne(reader, previousNode.PrevRef);
                    }

                    Unhook(reader, writer, currentNode);

                    currentNode.NextRef = previousNode.CurrentRef;
                    currentNode.PrevRef = previousNode.PrevRef;
                    SaveIndexes(writer, currentNode);

                    previousNode = ReadOne(reader, previousNode.CurrentRef);
                    if (previousNode.PrevRef != -1)
                    {
                        var temp = ReadOne(reader, previousNode.PrevRef);
                        temp.NextRef = currentNode.CurrentRef;
                        SaveIndexes(writer, temp);
                    }

                    previousNode.PrevRef = currentNode.CurrentRef;
                    SaveIndexes(writer, previousNode);
                }
            }

            fs.Close();
        }

        private void Unhook(BinaryReader br, BinaryWriter bw, Node currentNode)
        {
            var temp = ReadOne(br, currentNode.PrevRef);
            temp.NextRef = currentNode.NextRef;
            SaveIndexes(bw, temp);
            if (currentNode.NextRef == -1)
            {
                return;
            }

            temp = ReadOne(br, currentNode.NextRef);
            temp.PrevRef = currentNode.PrevRef;
            SaveIndexes(bw, temp);
        }

        private Node Head(BinaryReader br)
        {
            long length = GetElementsCount(br.BaseStream);
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

        private long GetElementsCount(Stream stream)
        {
            return stream.Length / ByteSize;
        }
    }
}
