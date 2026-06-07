public class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
    public string Category { get; set; }
    public string Isbn { get; set; }

    public int TotalCopies { get; set; }
    public int BorrowedCopies { get; set; }

    public bool IsAvailable => (TotalCopies - BorrowedCopies) > 0;
}