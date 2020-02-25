using System;

namespace Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var runner = new Sorter<Earnings>(
                new DataManager<Earnings>()
                );

            runner.Run("Data/small.txt");
            System.Console.WriteLine(runner.ToString());
        }
    }
}
