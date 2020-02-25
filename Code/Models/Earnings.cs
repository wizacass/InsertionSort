using System;

public class Earnings : IComparable
{
    public int Year { get; }
    public int Month { get; }
    public int Day { get; }
    public decimal Amount { get; }

    public Earnings(string date, decimal amount)
    {
        var dateValues = date.Split('.');
        try
        {
            Year = int.Parse(dateValues[0]);
            Month = int.Parse(dateValues[1]);
            Day = int.Parse(dateValues[2]);
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"Error in pasing Earning data! {ex.Message}");
            throw;
        }
        Amount = amount;
    }

    public Earnings(int year, int month, int day, decimal amount)
    {
        Year = year;
        Month = month;
        Day = day;
        Amount = amount;
    }

    public int CompareTo(object obj)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        return $"{Year}.{Month}.{Day} {Amount}$";
    }
}
