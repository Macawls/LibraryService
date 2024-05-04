using LibraryService.Models;

namespace LibraryService.Repositories;

public class InMemoryGenreRepository(string filePath) : InMemoryRepositoryBase<Genre>(filePath)
{
    public override Task Update(Genre item)
    {
        ArgumentNullException.ThrowIfNull(item);

        var existingGenre = Items.FirstOrDefault(e => e.Id == item.Id);

        if (existingGenre == null)
        {
            throw new InvalidOperationException($"Genre with ID {item.Id} not found.");
        }
        
        existingGenre.Name = item.Name;
        return Task.CompletedTask;
    }
}