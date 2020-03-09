using System;
using System.IO;

public class Earnings : IComparable<Earnings>, IEquatable<Earnings>, IParsable, ISerializable
{
    public int ByteSize => 20;

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
        Parse(new[] {date, amount});
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

    public override string ToString()
    {
        return $"{Year}.{Month}.{Day} {Amount}";
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
            Console.WriteLine($"Error in parsing Earning data! {ex.Message}");
        }
    }

    public bool Equals(Earnings other)
    {
        if (other == null)
        {
            return false;
        }

        return
            Year == other.Year &&
            Month == other.Month &&
            Day == other.Day;
    }

    public int CompareTo(Earnings other)
    {
        return
            Year == other.Year
                ? Month == other.Month ? Day.CompareTo(other.Day) :
                Month.CompareTo(other.Month)
                : Year.CompareTo(other.Year);
    }

    public void SerializeToBinary(BinaryWriter bw)
    {
        bw.Write((short) Year); // 2 bytes
        bw.Write((byte) Month); // 1 byte
        bw.Write((byte) Day); // 1 byte
        bw.Write(Amount); // 16 bytes
    }

    public void DeserializeFromBinary(BinaryReader br)
    {
        Year = br.ReadInt16();
        Month = br.ReadByte();
        Day = br.ReadByte();
        Amount = br.ReadDecimal();
    }
}
