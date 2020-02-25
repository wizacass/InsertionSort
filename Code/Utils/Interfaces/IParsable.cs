using System;

public interface IParsable
{
    public void Parse(string dataString);

    public void Parse(string[] dataArray);
}
