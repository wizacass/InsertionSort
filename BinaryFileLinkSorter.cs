using System;
using System.IO;

namespace Lab1.Code
{
    public class BinaryFileLinkSorter<T> : IRunnable where T : IComparable<T>, ISerializable, new()
    {
        public string Id => "LLS_File";

        public void Run(string fileId)
        {
            throw new NotImplementedException();
        }

        public void Sort()
        {
            throw new NotImplementedException();
        }

        public string StatusString(string label = null)
        {
            throw new NotImplementedException();
        }
    }
}
