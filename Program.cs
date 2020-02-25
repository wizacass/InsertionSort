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

            for (int i = 1; i <= 2; i++)
            {
                string filename = $"Data/Generated{i}.txt";
                runner.Generate((int)Math.Pow(10, i), filename);
                runner.Run(filename);
                System.Console.WriteLine(runner.ToString());
                runner.Sort();
                System.Console.WriteLine(runner.ToString());
            }
        }
    }
}
