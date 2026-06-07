using System;

public class Menu
{
    private readonly LibraryManager library = new();

    public void Start()
    {
        while (true)
        {
            ConsoleUI.Header("SYSTEM BIBLIOTEKI");

            Console.WriteLine("1. Dodaj ksiazke");
            Console.WriteLine("2. Pokaz ksiazki");
            Console.WriteLine("3. Dodaj klienta");
            Console.WriteLine("4. Wypozycz ksiazke");
            Console.WriteLine("5. Zwrot ksiazki");
            Console.WriteLine("6. Szukaj ksiazki");
            Console.WriteLine("7. Szukaj klienta");
            Console.WriteLine("8. Backup");
            Console.WriteLine("0. Wyjscie");

            Console.Write("Wybierz: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1": AddBook(); break;
                case "2": ShowBooks(); break;
                case "3": AddClient(); break;
                case "4": BorrowBook(); break;
                case "5": ReturnBook(); break;
                case "6": SearchBooks(); break;
                case "7": SearchClients(); break;
                case "8": Backup(); break;
                case "0": return;
            }

            Console.ReadLine();
        }
    }

    private void AddBook()
    {
        var b = new Book();

        ConsoleUI.Header("DODAWANIE KSIAZKI");

        Console.Write("Tytul: ");
        b.Title = Console.ReadLine();

        Console.Write("Autor: ");
        b.Author = Console.ReadLine();

        Console.Write("Kategoria: ");
        b.Category = Console.ReadLine();

        Console.Write("ISBN (np. 978-83-65458-89-6): ");
        b.Isbn = Console.ReadLine();

        Console.Write("Ilosc: ");
        b.TotalCopies = int.Parse(Console.ReadLine());

        library.AddBook(b);

        ConsoleUI.Success("Dodano ksiazke");
    }

    private void ShowBooks()
    {
        ConsoleUI.Header("KSIAZKI");

        foreach (var b in library.GetBooks())
            Console.WriteLine($"{b.Title} | {b.Author} | {b.Isbn} | {(b.IsAvailable ? "TAK" : "NIE")}");
    }

    private void AddClient()
    {
        var c = new Client();

        ConsoleUI.Header("DODAWANIE KLIENTA");

        Console.WriteLine("Imie:");
        c.FirstName = Console.ReadLine();

        Console.WriteLine("Nazwisko:");
        c.LastName = Console.ReadLine();

        Console.WriteLine("ID karty:");
        c.LibraryCardId = Console.ReadLine();

        c.CardIssueDate = DateTime.Now;

        library.AddClient(c);

        ConsoleUI.Success("Dodano klienta");
    }

    private void BorrowBook()
    {
        ConsoleUI.Header("WYPOZYCZENIE KSIAZKI");

        Console.WriteLine("Podaj ID klienta (np. CARD-001):");
        string clientId = Console.ReadLine();

        Console.WriteLine("Podaj ISBN ksiazki (np. 978-83-65458-89-6):");
        string isbn = Console.ReadLine();

        Console.WriteLine("Podaj date zwrotu (np. 2026-12-31):");
        string dateInput = Console.ReadLine();

        if (!DateTime.TryParse(dateInput, out DateTime date))
        {
            ConsoleUI.Error("Zly format daty");
            return;
        }

        library.BorrowBook(clientId, isbn, date);

        ConsoleUI.Success("Ksiazka wypozyczona");
    }

    private void ReturnBook()
    {
        ConsoleUI.Header("ZWROT KSIAZKI");

        Console.WriteLine("Podaj ISBN ksiazki do zwrotu:");
        Console.WriteLine("(np. 978-83-65458-89-6)");
        Console.WriteLine();

        string isbn = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(isbn))
        {
            ConsoleUI.Error("ISBN nie moze byc pusty");
            return;
        }

        library.ReturnBook(isbn);

        ConsoleUI.Success("Ksiazka zwrocona");
    }

    private void SearchBooks()
    {
        ConsoleUI.Header("SZUKANIE");

        var r = library.SearchBooks(Console.ReadLine());

        foreach (var b in r)
            Console.WriteLine($"{b.Title} | {b.Author}");
    }

    private void SearchClients()
    {
        ConsoleUI.Header("SZUKANIE KLIENTOW");

        var r = library.SearchClients(Console.ReadLine());

        foreach (var c in r)
            Console.WriteLine($"{c.FirstName} {c.LastName}");
    }

    private void Backup()
    {
        CsvExporter.Backup();
        ConsoleUI.Success("Backup wykonany");
    }
}