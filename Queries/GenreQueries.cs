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
}