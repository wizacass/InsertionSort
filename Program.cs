using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Lab1
{
    public class Program
    {
        private const string StandardFilePattern = "Data/Generated{0}.txt";
        private const string BinaryFilePattern = "Data/BinaryGenerated{0}.bin";
        private const int Generations = 10;

        private readonly List<IDataManager<Earnings>> _managers;
        private readonly IDataFactory<Earnings> _factory;
        private readonly List<IRunnable> _runners;

        public Program()
        {
            _managers = new List<IDataManager<Earnings>>
            {
                new TextDataManager<Earnings>(StandardFilePattern),
                new BinaryDataManager<Earnings>(BinaryFilePattern)
            };
            _factory = new DataFactory<Earnings>(
                new EarningsDataStringBuilder()
            );
            _runners = new List<IRunnable>
            {
                new ArraySorter<Earnings>(_managers[0]),
                new LinkedListSorter<Earnings>(_managers[0]),
                new ArraySorter<Earnings>(_managers[1]),
                new LinkedListSorter<Earnings>(_managers[1])
            };
        }

        public void Run()
        {
            // TODO: Write proper header
            Console.Write("C\t");
            foreach (var runner in _runners)
            {
                Console.Write(runner.Id + "\t");
            }

            Console.WriteLine();

            for (int i = 1; i <= Generations; i++)
            {
                Console.Write(CalculateEntries(i) + "\t");
                foreach (var runner in _runners)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    runner.Run(i.ToString());

                    var sw = new Stopwatch();
                    sw.Start();
                    runner.Sort();
                    sw.Stop();
                    Console.Write(sw.ElapsedMilliseconds + "\t");

                    //TODO: Calculate diff
                }

                Console.WriteLine();
                // TODO: Extract printing
            }

            Console.WriteLine();
        }

        public void Generate()
        {
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
            return 10 * (int) Math.Pow(2, i);
        }

        private static void Main(string[] args)
        {
            var p = new Program();
            //p.Generate();
            p.Run();
        }
    }
}
