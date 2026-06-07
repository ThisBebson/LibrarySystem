using System;
using System.Collections.Generic;

public interface IBorrowService
{
    void BorrowBook(string clientId, string isbn, DateTime returnDate);
    void ReturnBook(string isbn);
    List<Borrowing> GetBorrowings();
}