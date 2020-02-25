using System;

namespace Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var runner = new ArraySorter<Earnings>(
                new ArrayDataManager<Earnings>(),
                new DataFactory<Earnings>(
                    new EarningsDataStringBuilder()
                )
                );

            runner.Run("Data/small.txt");
            System.Console.WriteLine(runner.ToString());
            runner.Sort();
            System.Console.WriteLine(runner.ToString());
            runner.Generate(100, "Data/Generated1.txt");
            runner.Generate(1000, "Data/Generated2.txt");
        }
    }
}
