using System;
using System.Collections.Generic;

namespace Lab1
{
    public class Program
    {
        private string _filePattern = "Data/Generated{0}.txt";

        private IDataManager<Earnings> _manager;
        private IDataFactory<Earnings> _factory;
        private List<string> _files;

        public Program()
        {
            _manager = new ArrayDataManager<Earnings>();
            _factory = new DataFactory<Earnings>(
                    new EarningsDataStringBuilder()
                );
            // TODO: Get all text files from a specified directory
            _files = new List<string>();
        }

        public void Run()
        {
            var runner = new ArraySorter<Earnings>(_manager);
            foreach (var file in _files)
            {
                runner.Run(file);
                Console.WriteLine(runner.ToString());
                runner.Sort();
                Console.WriteLine(runner.ToString());
            }
        }

        public void Generate(int count)
        {
            var generator = new DataGenerator<Earnings>(_manager, _factory);
            for (int i = 1; i <= count; i++)
            {
                string filename = string.Format(_filePattern, i);
                _files.Add(filename);
                System.Console.WriteLine(filename);
                generator.Generate((int)Math.Pow(10, i), filename);
            }
        }

        private static void Main(string[] args)
        {
            var p = new Program();
            p.Generate(2);
            p.Run();
        }
    }
}
