using LibraryService.Models;

namespace LibraryService.Repositories;

public class InMemoryBookInstanceRepository(string filePath) : InMemoryRepositoryBase<BookInstance>(filePath)
{
    public override Task Update(BookInstance item)
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