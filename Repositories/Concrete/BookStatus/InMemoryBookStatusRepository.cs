using LibraryService.Models;

namespace LibraryService.Repositories;

public class InMemoryBookStatusRepository(string filePath) : InMemoryRepositoryBase<BookStatus>(filePath)
{
    public override Task Update(BookStatus item)
    {
        ArgumentNullException.ThrowIfNull(item);

        var existing = Items.FirstOrDefault(e => e.Id == item.Id);

        if (existing == null)
        {
            throw new InvalidOperationException($"Book Status with ID {item.Id} not found.");
        }

        existing.BookId = item.Id;
        existing.StatusType = item.StatusType;
        return Task.CompletedTask;
    }
}