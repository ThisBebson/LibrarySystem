using System;
using System.Linq;

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
            Console.WriteLine("9. Usun ksiazke");
            Console.WriteLine("10. Usun klienta");
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
                case "9": RemoveBook(); break;
                case "10": RemoveClient(); break;
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

        b.Isbn = library.GenerateNextBookId();

        Console.WriteLine($"Przydzielone ID ksiazki: {b.Isbn}");

        Console.Write("Ilosc: ");
        b.TotalCopies = int.Parse(Console.ReadLine());

        library.AddBook(b);

        ConsoleUI.Success("Dodano ksiazke");
    }

    private void ShowBooks()
    {
        ConsoleUI.Header("KSIAZKI");

        foreach (var b in library.GetBooks()
                         .OrderBy(b => b.Isbn))
        {
            int available = b.TotalCopies - b.BorrowedCopies;

            Console.WriteLine(
                $"ID: {b.Isbn} | {b.Title} | {b.Author} | Dostepne: {available}/{b.TotalCopies}"
            );
        }
    }

    private void AddClient()
    {
        var c = new Client();

        ConsoleUI.Header("DODAWANIE KLIENTA");

        Console.WriteLine("Imie:");
        c.FirstName = Console.ReadLine();

        Console.WriteLine("Nazwisko:");
        c.LastName = Console.ReadLine();

        c.LibraryCardId = library.GenerateNextClientId();

        Console.WriteLine($"Przydzielone ID: {c.LibraryCardId}");

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
        ConsoleUI.Header("KSIAZKI");

        foreach (var b in library.GetBooks()
                         .OrderBy(b => b.Isbn))
        {
            int available = b.TotalCopies - b.BorrowedCopies;

            Console.WriteLine(
                $"ID: {b.Isbn} | {b.Title} | {b.Author} | {available}/{b.TotalCopies}"
            );
        }

        Console.WriteLine();
        Console.Write("Podaj ID ksiazki: ");

        string id = Console.ReadLine();

        var book = library
            .GetBooks()
            .FirstOrDefault(b => b.Isbn == id);

        if (book == null)
        {
            ConsoleUI.Error("Nie znaleziono ksiazki");
            return;
        }

        Console.WriteLine();
        Console.WriteLine("=== DANE KSIAZKI ===");
        Console.WriteLine($"ID: {book.Isbn}");
        Console.WriteLine($"Tytul: {book.Title}");
        Console.WriteLine($"Autor: {book.Author}");
        Console.WriteLine($"Kategoria: {book.Category}");
        Console.WriteLine(
            $"Dostepne: {book.TotalCopies - book.BorrowedCopies}/{book.TotalCopies}"
        );
    }

    private void SearchClients()
    {
        ConsoleUI.Header("KLIENCI");

        foreach (var c in library.GetClients()
                         .OrderBy(c => c.LibraryCardId))
        {
            Console.WriteLine(
                $"ID: {c.LibraryCardId} | {c.FirstName} {c.LastName}"
            );
        }

        Console.WriteLine();
        Console.Write("Podaj ID klienta: ");

        string id = Console.ReadLine();

        var client = library
            .GetClients()
            .FirstOrDefault(c => c.LibraryCardId == id);

        if (client == null)
        {
            ConsoleUI.Error("Nie znaleziono klienta");
            return;
        }

        Console.WriteLine();
        Console.WriteLine("=== DANE KLIENTA ===");
        Console.WriteLine($"Imie: {client.FirstName}");
        Console.WriteLine($"Nazwisko: {client.LastName}");
        Console.WriteLine($"ID: {client.LibraryCardId}");
    }

    private void Backup()
    {
        CsvExporter.Backup();
        ConsoleUI.Success("Backup wykonany");
    }

    private void RemoveBook()
    {
        ConsoleUI.Header("KSIAZKI");

        foreach (var b in library.GetBooks()
                         .OrderBy(b => b.Isbn))
        {
            Console.WriteLine(
                $"ID: {b.Isbn} | {b.Title} | {b.Author}"
            );
        }

        Console.WriteLine();
        Console.Write("Podaj ID ksiazki do usuniecia: ");

        string id = Console.ReadLine();

        var book = library
            .GetBooks()
            .FirstOrDefault(b => b.Isbn == id);

        if (book == null)
        {
            ConsoleUI.Error("Nie znaleziono ksiazki");
            return;
        }

        if (book.BorrowedCopies > 0)
        {
            ConsoleUI.Error(
                "Nie mozna usunac ksiazki, poniewaz jest wypozyczona"
            );
            return;
        }

        library.RemoveBook(id);

        ConsoleUI.Success("Ksiazka usunieta");
    }

    private void RemoveClient()
    {
        ConsoleUI.Header("KLIENCI");

        foreach (var c in library.GetClients()
                         .OrderBy(c => c.LibraryCardId))
        {
            Console.WriteLine(
                $"ID: {c.LibraryCardId} | {c.FirstName} {c.LastName}"
            );
        }

        Console.WriteLine();
        Console.Write("Podaj ID klienta do usuniecia: ");

        string id = Console.ReadLine();

        var client = library
            .GetClients()
            .FirstOrDefault(c => c.LibraryCardId == id);

        if (client == null)
        {
            ConsoleUI.Error("Nie znaleziono klienta");
            return;
        }

        library.RemoveClient(id);

        ConsoleUI.Success("Klient usuniety");
    }
}