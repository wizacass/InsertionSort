using System;

public class Earnings : IComparable, IParsable
{
    public int Year { get; private set; }
    public int Month { get; private set; }
    public int Day { get; private set; }
    public decimal Amount { get; private set; }

    public Earnings() { }

    public Earnings(string data)
    {
        Parse(data);
    }

    public Earnings(string date, string amount)
    {
        Parse(new[] { date, amount });
    }

    public Earnings(DateTime date, decimal amount)
    {
        Year = date.Year;
        Month = date.Month;
        Day = date.Day;
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

    public void Parse(string dataString)
    {
        var data = dataString.Split();
        Parse(data);
    }

    public void Parse(string[] dataArray)
    {
        var dateValues = dataArray[0].Split('.');
        try
        {
            Year = int.Parse(dateValues[0]);
            Month = int.Parse(dateValues[1]);
            Day = int.Parse(dateValues[2]);
            Amount = int.Parse(dataArray[1]);
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"Error in pasing Earning data! {ex.Message}");
            throw;
        }
    }
}
