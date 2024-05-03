using LibraryService.Models;

namespace LibraryService.Repositories;

public class InMemoryBookRepositoryBase(string filePath) : InMemoryRepositoryBase<Book>(filePath)
{
    
    public override Task Update(Book item)
    {
        ArgumentNullException.ThrowIfNull(item);

        var existingBook = Items.FirstOrDefault(e => e.Id == item.Id);

        if (existingBook == null)
        {
            throw new InvalidOperationException($"Book with ID {item.Id} not found.");
        }
        
        existingBook.Title = item.Title;
        existingBook.Author = item.Author;
        existingBook.PublicationYear = item.PublicationYear;
        existingBook.ISBN = item.ISBN;
        
        return Task.CompletedTask;
    }
}