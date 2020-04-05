using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
        private const string CsvFilePattern = "Logs/log {0}.csv";

        private const int Generations = 8;

        private readonly List<IDataManager<Earnings>> _managers;
        private readonly IDataFactory<Earnings> _factory;
        private readonly List<IRunnable> _runners;

        private ConsoleLogger _logger;

        public Program()
        {
            _managers = new List<IDataManager<Earnings>>
            {
                //new TextDataManager<Earnings>(StandardFilePattern),
                new BinaryDataManager<Earnings>(BinaryFilePattern),
                new BinaryDataManager<Earnings>(BinaryArraySortableFilePattern),
                new BinaryLinkDataManager<Earnings>(BinaryLinkSortableFilePattern)
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
                new BinaryFileArraySorter<Earnings>(BinaryArraySortableFilePattern),
                new BinaryFileLinkSorter<Earnings>(BinaryLinkSortableFilePattern)
            };
        }

        public void Run(bool log = true)
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

            _logger = new ConsoleLogger(headers);
            if (log)
            {
                _logger.WriteHeader();
            }

            string path = string.Format(CsvFilePattern, DateTime.Now.ToString().Replace('/', '-')).Replace(':', '.');
            using var csvWriter = new StreamWriter(path);
            for (int i = 1; i <= Generations; i++)
            {
                if (!log)
                {
                    Console.WriteLine($"Running Generation {i}...");
                }

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

                if (log)
                {
                    _logger.WriteEntry(entries);
                }

                csvWriter.WriteLine(entries.Aggregate(string.Empty, (current, entry) => current + $"{entry.Item1};"));
            }

            if (!log)
            {
                Console.WriteLine("Test complete!");
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
            p.Run(false);
        }
    }
}
