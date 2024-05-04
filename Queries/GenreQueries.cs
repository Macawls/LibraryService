using LibraryService.Models;

namespace LibraryService.Queries;

public static class GenreQueries
{
    public static IEnumerable<Genre> GetValidGenres(IEnumerable<Genre> genres, IEnumerable<string> namesToCompare)
    {
        return genres
            .Where(genre => namesToCompare.Select(e => e.ToLower())
            .Contains(genre.Name.ToLower()));
    }
    
    public static IEnumerable<Genre> GetGenresOfBook(int id, IEnumerable<Genre> genres, IEnumerable<BookGenre> bookGenres)
    {
        return from bookGenre in bookGenres
            join genre in genres on bookGenre.GenreId equals genre.Id
            where bookGenre.BookId == id
            select genre;
    }
}