using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public static class CsvExporter
{
    private static string DataDir => "Data";

    private static string BooksPath => "Data/books.csv";
    private static string ClientsPath => "Data/clients.csv";
    private static string BorrowingsPath => "Data/borrowings.csv";

    // =========================
    // BACKUP
    // =========================
    public static void Backup()
    {
        Directory.CreateDirectory(DataDir);

        string backupDir = $"Data/backup_{DateTime.Now:yyyyMMdd_HHmmss}";
        Directory.CreateDirectory(backupDir);

        if (File.Exists(BooksPath))
            File.Copy(BooksPath, $"{backupDir}/books.csv");

        if (File.Exists(ClientsPath))
            File.Copy(ClientsPath, $"{backupDir}/clients.csv");

        if (File.Exists(BorrowingsPath))
            File.Copy(BorrowingsPath, $"{backupDir}/borrowings.csv");
    }

    // =========================
    // BOOKS
    // =========================
    public static void SaveBooks(List<Book> books)
    {
        Directory.CreateDirectory(DataDir);

        var sb = new StringBuilder();
        sb.AppendLine("Title;Author;Category;ISBN;Total;Borrowed");

        foreach (var b in books)
        {
            sb.AppendLine($"{b.Title};{b.Author};{b.Category};{b.Isbn};{b.TotalCopies};{b.BorrowedCopies}");
        }

        File.WriteAllText(BooksPath, sb.ToString());
    }

    public static List<Book> LoadBooks()
    {
        var list = new List<Book>();

        if (!File.Exists(BooksPath))
            return list;

        var lines = File.ReadAllLines(BooksPath);

        for (int i = 1; i < lines.Length; i++)
        {
            var line = lines[i];

            if (string.IsNullOrWhiteSpace(line))
                continue;

            var p = line.Split(';');

            if (p.Length < 6)
                continue;

            if (!int.TryParse(p[4], out int total))
                continue;

            if (!int.TryParse(p[5], out int borrowed))
                continue;

            list.Add(new Book
            {
                Title = p[0],
                Author = p[1],
                Category = p[2],
                Isbn = p[3],
                TotalCopies = total,
                BorrowedCopies = borrowed
            });
        }

        return list;
    }

    // =========================
    // CLIENTS
    // =========================
    public static void SaveClients(List<Client> clients)
    {
        Directory.CreateDirectory(DataDir);

        var sb = new StringBuilder();
        sb.AppendLine("FirstName;LastName;CardId;Date");

        foreach (var c in clients)
        {
            sb.AppendLine($"{c.FirstName};{c.LastName};{c.LibraryCardId};{c.CardIssueDate}");
        }

        File.WriteAllText(ClientsPath, sb.ToString());
    }

    public static List<Client> LoadClients()
    {
        var list = new List<Client>();

        if (!File.Exists(ClientsPath))
            return list;

        var lines = File.ReadAllLines(ClientsPath);

        for (int i = 1; i < lines.Length; i++)
        {
            var line = lines[i];

            if (string.IsNullOrWhiteSpace(line))
                continue;

            var p = line.Split(';');

            if (p.Length < 4)
                continue;

            if (!DateTime.TryParse(p[3], out DateTime date))
                continue;

            list.Add(new Client
            {
                FirstName = p[0],
                LastName = p[1],
                LibraryCardId = p[2],
                CardIssueDate = date
            });
        }

        return list;
    }

    // =========================
    // BORROWINGS
    // =========================
    public static void SaveBorrowings(List<Borrowing> list)
    {
        Directory.CreateDirectory(DataDir);

        var sb = new StringBuilder();
        sb.AppendLine("ClientId;ISBN;BorrowDate;ReturnDate");

        foreach (var b in list)
        {
            sb.AppendLine($"{b.ClientId};{b.Isbn};{b.BorrowDate};{b.ReturnDate}");
        }

        File.WriteAllText(BorrowingsPath, sb.ToString());
    }

    public static List<Borrowing> LoadBorrowings()
    {
        var list = new List<Borrowing>();

        if (!File.Exists(BorrowingsPath))
            return list;

        var lines = File.ReadAllLines(BorrowingsPath);

        for (int i = 1; i < lines.Length; i++)
        {
            var line = lines[i];

            if (string.IsNullOrWhiteSpace(line))
                continue;

            var p = line.Split(';');

            if (p.Length < 4)
                continue;

            if (!DateTime.TryParse(p[2], out DateTime borrowDate))
                continue;

            if (!DateTime.TryParse(p[3], out DateTime returnDate))
                continue;

            list.Add(new Borrowing
            {
                ClientId = p[0],
                Isbn = p[1],
                BorrowDate = borrowDate,
                ReturnDate = returnDate
            });
        }

        return list;
    }
}