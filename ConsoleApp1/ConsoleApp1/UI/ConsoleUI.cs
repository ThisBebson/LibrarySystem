using System;

public static class ConsoleUI
{
    public static void Header(string title)
    {
        Console.WriteLine();
        Console.WriteLine("==================================");
        Console.WriteLine(title);
        Console.WriteLine("==================================");
        Console.WriteLine();
    }

    public static void Line()
    {
        Console.WriteLine("----------------------------------");
    }

    public static void Success(string msg)
    {
        Console.WriteLine("[OK] " + msg);
    }

    public static void Error(string msg)
    {
        Console.WriteLine("[ERROR] " + msg);
    }
}