using System;
using System.Collections.Generic;
using System.Diagnostics;
using Lab1.Code;
using Lab1.Code.Models;

namespace Lab1
{
    public class Program
    {
        private const string StandardFilePattern = "Data/Generated{0}.txt";
        private const string BinaryFilePattern = "Data/BinaryGenerated{0}.bin";
        private const string BinaryArraySortableFilePattern = "Data/BinaryArraySortableGenerated{0}.bin";
        private const string BinaryLinkSortableFilePattern = "Data/BinaryLinkSortableGenerated{0}.bin";

        private const int Generations = 4;

        private readonly List<IDataManager<Earnings>> _managers;
        private readonly IDataFactory<Earnings> _factory;
        private readonly List<IRunnable> _runners;

        private Logger _logger;

        public Program()
        {
            _managers = new List<IDataManager<Earnings>>
            {
                //new TextDataManager<Earnings>(StandardFilePattern),
                new BinaryDataManager<Earnings>(BinaryFilePattern),
                new BinaryDataManager<Earnings>(BinaryArraySortableFilePattern)
            };
            _factory = new DataFactory<Earnings>(
                new EarningsDataStringBuilder()
            );
            _runners = new List<IRunnable>
            {
                //new ArraySorter<Earnings>(_managers[0]),
                //new LinkedListSorter<Earnings>(_managers[0]),
                new ArraySorter<Earnings>(_managers[1]),
                new LinkedListSorter<Earnings>(_managers[1]),
                new BinaryFileArraySorter<Earnings>(BinaryArraySortableFilePattern)
            };
        }

        public void Run()
        {
            Console.WriteLine($"Running Sorting test with {Generations} data sets.");
            const int offset = 2;
            int length = _runners.Count + offset;
            var headers = new string[length];
            headers[0] = "No.";
            headers[1] = "Count";
            for (int i = 0; i < _runners.Count; i++)
            {
                headers[i + offset] = _runners[i].Id;
            }

            _logger = new Logger(headers);
            _logger.WriteHeader();

            for (int i = 1; i <= Generations; i++)
            {
                int pos = offset;
                var entries = new Tuple<string, bool>[length];
                entries[0] = new Tuple<string, bool>(i.ToString(), true);
                entries[1] = new Tuple<string, bool>(CalculateEntries(i).ToString(), true);

                foreach (var runner in _runners)
                {
                    runner.Run(i.ToString());

                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    var sw = new Stopwatch();
                    sw.Start();
                    runner.Sort();
                    sw.Stop();
                    entries[pos++] = new Tuple<string, bool>(sw.ElapsedMilliseconds.ToString(), true);
                }

                _logger.WriteEntry(entries);
            }
        }

        public void Generate()
        {
            Console.WriteLine("Generating Entries...");
            for (int i = 1; i <= Generations; i++)
            {
                int entriesCount = CalculateEntries(i);
                var data = _factory.GenerateEntries(entriesCount);
                foreach (var manager in _managers)
                {
                    manager.Write(data, i.ToString());
                }
            }
        }

        private static int CalculateEntries(int i)
        {
            return 10 * (int)Math.Pow(2, i);
        }

        private static void Main()
        {
            var p = new Program();
            p.Generate();
            p.Run();
        }
    }
}
