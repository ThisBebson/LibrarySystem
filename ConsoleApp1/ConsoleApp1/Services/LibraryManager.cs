using System;
using System.Collections.Generic;
using System.Linq;

public sealed class LibraryManager : IBookService, IClientService, IBorrowService
{
    private List<Book> books;
    private List<Client> clients;
    private List<Borrowing> borrowings;

    public LibraryManager()
    {
        books = CsvExporter.LoadBooks();
        clients = CsvExporter.LoadClients();
        borrowings = CsvExporter.LoadBorrowings();
    }

    public void AddBook(Book book)
    {
        books.Add(book);
        CsvExporter.SaveBooks(books);
    }

    public void RemoveBook(string isbn)
    {
        var book = books.FirstOrDefault(x => x.Isbn == isbn);

        if (book != null)
        {
            books.Remove(book);
            CsvExporter.SaveBooks(books);
        }
    }

    public List<Book> GetBooks() => books;

    public Book GetByIsbn(string isbn)
        => books.FirstOrDefault(b => b.Isbn == isbn);

    public List<Book> SearchBooks(string keyword)
    {
        return books.Where(b =>
            b.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
            b.Author.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
            b.Category.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
            b.Isbn.Contains(keyword)
        ).ToList();
    }

    public void AddClient(Client client)
    {
        clients.Add(client);
        CsvExporter.SaveClients(clients);
    }

    public List<Client> GetClients() => clients;

    public List<Client> SearchClients(string keyword)
    {
        return clients.Where(c =>
            c.FirstName.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
            c.LastName.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
            c.LibraryCardId.Contains(keyword)
        ).ToList();
    }

    public void BorrowBook(string clientId, string isbn, DateTime returnDate)
    {
        var book = GetByIsbn(isbn);

        if (book == null || !book.IsAvailable)
            return;

        book.BorrowedCopies++;

        borrowings.Add(new Borrowing
        {
            ClientId = clientId,
            Isbn = isbn,
            BorrowDate = DateTime.Now,
            ReturnDate = returnDate
        });

        CsvExporter.SaveBooks(books);
        CsvExporter.SaveBorrowings(borrowings);
    }

    public void ReturnBook(string isbn)
    {
        var book = GetByIsbn(isbn);

        if (book == null)
            return;

        if (book.BorrowedCopies > 0)
            book.BorrowedCopies--;

        var borrowing = borrowings.FirstOrDefault(b => b.Isbn == isbn);

        if (borrowing != null)
            borrowings.Remove(borrowing);

        CsvExporter.SaveBooks(books);
        CsvExporter.SaveBorrowings(borrowings);
    }

    public List<Borrowing> GetBorrowings() => borrowings;
}