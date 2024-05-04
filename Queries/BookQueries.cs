using FuzzySharp;
using LibraryService.Models;

namespace LibraryService.Queries;

public static class BookQueries
{
    public static IEnumerable<Book> GetBooksByGenre(
        IEnumerable<string> desiredGenres, 
        IEnumerable<Book> books, 
        IEnumerable<Genre> genres, 
        IEnumerable<BookGenre> bookGenres)
    {
        return (from book in books
            join bookGenre in bookGenres on book.Id equals bookGenre.BookId
            join genre in genres on bookGenre.GenreId equals genre.Id
            where desiredGenres.Select(e => e.ToLower()).Contains(genre.Name.ToLower()) // case insensitive
            select book)
            .Distinct()
            .ToList();
    }
    
    public static IEnumerable<Genre> GetGenresOfBook(Book book, IEnumerable<Genre> genres, IEnumerable<BookGenre> bookGenres)
    {
        return from bookGenre in bookGenres
            join genre in genres on bookGenre.GenreId equals genre.Id
            where bookGenre.BookId == book.Id
            select genre;
    }
    
    public static IEnumerable<Book> FuzzySearchByTitleOrAuthor(IEnumerable<Book> books, string query, int minScore = 50)
    {
        return from book in books 
            let titleScore = Fuzz.PartialRatio(query, book.Title) 
            let authorScore = Fuzz.PartialRatio(query, book.Author) 
            where titleScore >= minScore || authorScore >= minScore 
            select book;
    }
    
    public static IEnumerable<Book> PublishedBefore(IEnumerable<Book> books, int year)
    {
        return books.Where(book => book.PublicationYear < year);
    }
    
    public static IEnumerable<Book> PublishedAfter(IEnumerable<Book> books, int year)
    {
        return books.Where(book => book.PublicationYear > year);
    }
}