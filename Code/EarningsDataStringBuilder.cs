using System;

public class EarningsDataStringBuilder : IDataStringBuilder
{
    private readonly Random _rnd;

    public EarningsDataStringBuilder()
    {
        _rnd = new Random(2019);
    }

    public EarningsDataStringBuilder(int seed)
    {
        _rnd = new Random(seed);
    }

    public string GenerateDataString()
    {
        int year = _rnd.Next(1900, 2005);
        int month = _rnd.Next(1, 12);
        int day = _rnd.Next(1, 28);
        decimal amount = _rnd.Next(0, 50000);
        return $"{year}.{month}.{day} {amount}";
    }
}
