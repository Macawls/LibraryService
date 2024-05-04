using LibraryService.Models;

namespace LibraryService.Repositories;

public class InMemoryBookGenreRepository(string filePath) : InMemoryRepositoryBase<BookGenre>(filePath)
{
    public override Task Update(BookGenre item)
    {
        ArgumentNullException.ThrowIfNull(item);

        var existingGenre = Items.FirstOrDefault(e => e.Id == item.Id);

        if (existingGenre == null)
        {
            throw new InvalidOperationException($"Genre with ID {item.Id} not found.");
        }
        
        existingGenre.BookId = item.BookId;
        return Task.CompletedTask;
    }
}