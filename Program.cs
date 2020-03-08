using System;
using System.Collections.Generic;

namespace Lab1
{
    public class Program
    {
        private const string StandardFilePattern = "Data/Generated{0}.txt";
        private const string BinaryFilePattern = "Data/BinaryGenerated{0}.bin";
        private const int Generations = 1;

        private string[] FilePatterns = { StandardFilePattern, BinaryFilePattern };

        private readonly List<IDataManager<Earnings>> _managers;
        private readonly IDataFactory<Earnings> _factory;
        private readonly List<string> _files;
        private readonly List<IRunnable> _runners;

        public Program()
        {
            _managers = new List<IDataManager<Earnings>>();
            _managers.Add(new ArrayDataManager<Earnings>(StandardFilePattern));
            _managers.Add(new BinaryDataManager<Earnings>(BinaryFilePattern));
            _factory = new DataFactory<Earnings>(
                new EarningsDataStringBuilder()
            );
            _files = new List<string>();
            _runners = new List<IRunnable>
            {
                new ArraySorter<Earnings>(_managers[0]),
                new LinkedListSorter<Earnings>(_managers[0])
            };
        }

        public void Run()
        {
            // TODO: Tweak running to support Generations patameter
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

        public void Generate()
        {
            for (int i = 1; i <= Generations; i++)
            {
                string filename = string.Format(StandardFilePattern, i);
                _files.Add(filename);
                Console.WriteLine(filename);
                int entriesCount = 10 * (int)Math.Pow(2, i);
                var data = _factory.GenerateEntries(entriesCount);
                for (int j = 0; j < _managers.Count; j++)
                {
                    _managers[j].Write(data, i.ToString());
                }
            }
        }

        private static void Main(string[] args)
        {
            var p = new Program();
            p.Generate();
            //p.Run();
        }
    }
}
