using Supabase.Postgrest.Models;

namespace LibraryService.Repositories;

public interface IRepository<T> where T : BaseModel
{
    Task<T?> GetById(int id);
    Task<T> Add(T value);
    Task Update(T item);
    Task Delete(int id);
    Task<IEnumerable<T>> GetAll();
}