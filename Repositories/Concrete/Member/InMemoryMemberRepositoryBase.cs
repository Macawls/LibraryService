using LibraryService.Models;

namespace LibraryService.Repositories;

public class InMemoryMemberRepositoryBase(string filePath) : InMemoryRepositoryBase<Member>(filePath)
{
    public override Task Update(Member item)
    {
        ArgumentNullException.ThrowIfNull(item);

        var existingMember = Items.FirstOrDefault(e => e.Id == item.Id);

        if (existingMember == null)
        {
            throw new InvalidOperationException($"Genre with ID {item.Id} not found.");
        }

        existingMember.Email = item.Email;
        existingMember.FirstName = item.FirstName;
        existingMember.LastName = item.LastName;
        existingMember.PhoneNumber = item.PhoneNumber;
        return Task.CompletedTask;
    }
}