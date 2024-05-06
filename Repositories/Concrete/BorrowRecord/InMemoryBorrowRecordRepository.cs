using LibraryService.Models;

namespace LibraryService.Repositories;

public class InMemoryBorrowRecordRepository(string filePath) : InMemoryRepositoryBase<BorrowRecord>(filePath)
{
    public override Task Update(BorrowRecord item)
    {
        ArgumentNullException.ThrowIfNull(item);

        var existingRecord = Items.FirstOrDefault(e => e.Id == item.Id);

        if (existingRecord == null)
        {
            throw new InvalidOperationException($"Borrow Record with ID {item.Id} not found.");
        }

        existingRecord.DueDate = item.DueDate;
        return Task.CompletedTask;
    }
}