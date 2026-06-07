using System.Collections.Generic;

public interface IBookService
{
    void AddBook(Book book);
    void RemoveBook(string isbn);
    List<Book> GetBooks();
    Book GetByIsbn(string isbn);
    List<Book> SearchBooks(string keyword);
}