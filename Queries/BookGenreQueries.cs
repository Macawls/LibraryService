using LibraryService.Models;

namespace LibraryService.Queries;

public static class BookGenreQueries
{
    public static IEnumerable<BookGenre> GetBookGenresOfBook(int bookId, IEnumerable<BookGenre> bookGenres)
    {
        return from bookGenre in bookGenres
            where bookGenre.BookId == bookId
            select bookGenre;
    }
}