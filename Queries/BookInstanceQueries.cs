using LibraryService.Models;

namespace LibraryService.Queries;

public static class BookInstanceQueries
{
    public static IEnumerable<BookInstance> GetInstancesOfBook(int bookId, 
        IEnumerable<BookInstance> bookStatuses)
    {
        return from bookStatus in bookStatuses
            where bookId == bookStatus.BookId
            select bookStatus;
    }
}