using LibraryService.Models;

namespace LibraryService.Rules;

public static class GenreRules
{
    public static bool IsUniqueName(Genre genre, IEnumerable<Genre> genres)
    {
        return genres.Select(entity => entity.Name.ToLower()).Any(e => e == genre.Name);
    }
}