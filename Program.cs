using System;
using System.Collections.Generic;

namespace Lab1
{
    public class Program
    {
        private const string FilePattern = "Data/Generated{0}.txt";

        private readonly IDataManager<Earnings> _manager;
        private readonly IDataFactory<Earnings> _factory;
        private readonly List<string> _files;
        private readonly List<IRunnable> _runners;

        public Program()
        {
            _manager = new ArrayDataManager<Earnings>();
            _factory = new DataFactory<Earnings>(
                new EarningsDataStringBuilder()
            );
            _files = new List<string>();
            _runners = new List<IRunnable>
            {
                new ArraySorter<Earnings>(_manager),
                new LinkedListSorter<Earnings>(_manager)
            };
        }

        public void Run()
        {
            foreach (var runner in _runners)
            {
                Console.WriteLine(runner.GetType());
                foreach (string file in _files)
                {
                    runner.Run(file);
                    Console.WriteLine(runner.StatusString("Before sorting"));
                    runner.Sort();
                    Console.WriteLine(runner.StatusString("After sorting"));
                }
            }
        }

        public void Generate(int count)
        {
            var generator = new DataGenerator<Earnings>(_manager, _factory);
            for (int i = 1; i <= count; i++)
            {
                string filename = string.Format(FilePattern, i);
                _files.Add(filename);
                Console.WriteLine(filename);
                generator.Generate((int) Math.Pow(10, i), filename);
            }
        }

        private static void Main(string[] args)
        {
            var p = new Program();
            p.Generate(1);
            p.Run();
        }
    }
}
