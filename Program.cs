using System;

namespace Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var runner = new Sorter<Earnings>(
                new ArrayDataManager<Earnings>()
                );

            runner.Run("Data/small.txt");
            System.Console.WriteLine(runner.ToString());
            runner.Sort();
            System.Console.WriteLine(runner.ToString());
        }
    }
}
