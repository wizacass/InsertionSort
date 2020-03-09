using System;
using System.Collections.Generic;
using System.IO;

namespace Lab1.Code
{
    public class TextDataManager<T> : IDataManager<T> where T : IParsable, new()
    {
        public string Id => "Txt";
        public string Pattern { get; }

        public TextDataManager(string pattern)
        {
            Pattern = pattern;
        }

        public T[] Read(string fileId)
        {
            string filename = string.Format(Pattern, fileId);
            try
            {
                var objects = new List<T>();
                using (var reader = new StreamReader(filename))
                {
                    while (!reader.EndOfStream)
                    {
                        var obj = new T();
                        obj.Parse(reader.ReadLine());
                        objects.Add(obj);
                    }
                }

                return objects.ToArray();
            }
            catch (Exception)
            {
                Console.WriteLine($"File: {filename} not found!");
                return null;
            }
        }

        public void Write(T[] data, string fileId)
        {
            string filename = string.Format(Pattern, fileId);
            using var writer = new StreamWriter(filename);
            foreach (var item in data)
            {
                writer.WriteLine(item.ToString());
            }
        }
    }
}
