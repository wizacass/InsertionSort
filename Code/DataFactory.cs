using System.Collections.Generic;
using System.Linq;

namespace Lab1.Code
{
    public class DataFactory<T> : IDataFactory<T> where T : IParsable, new()
    {
        private readonly IDataStringBuilder _dataStringBuilder;

        public DataFactory(IDataStringBuilder dataStringBuilder)
        {
            _dataStringBuilder = dataStringBuilder;
        }

        public T[] GenerateEntries(int count)
        {
            var objects = new T[count];
            for (int i = 0; i < count; i++)
            {
                var obj = GenerateEntry();
                while (Contains(objects, obj))
                {
                    obj = GenerateEntry();
                }

                objects[i] = GenerateEntry();
            }

            return objects;
        }

        public T GenerateEntry()
        {
            var obj = new T();
            obj.Parse(_dataStringBuilder.GenerateDataString());
            return obj;
        }

        private static bool Contains(IEnumerable<T> objects, T obj)
        {
            return objects.Contains(obj);
        }
    }
}
