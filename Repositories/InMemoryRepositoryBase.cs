using LibraryService.Models;
using Newtonsoft.Json;
using Supabase.Postgrest.Models;

namespace LibraryService.Repositories;

public abstract class InMemoryRepositoryBase<T> : IRepository<T> where T : BaseModel
{
    protected List<T> Items;
    protected string DefaultDateTimePropertyName => "CreatedAt";

    protected InMemoryRepositoryBase(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return;
        }
        
        var json = File.ReadAllText(filePath);
        
        if (!string.IsNullOrEmpty(json))
        {
            Items = JsonConvert.DeserializeObject<List<T>>(json)!;
        }
    }
    
    private int GenerateUniqueId()
    {
        return Items.Count == 0 ? 1 : Items.Max(item => (int)item.PrimaryKey.First().Value) + 1;
    }

    public Task<IEnumerable<T>> GetAll()
    {
        return Task.FromResult(Items as IEnumerable<T>);
    }

    public Task<T?> GetById(int id)
    {
        var item = Items.FirstOrDefault(e => (int)e.PrimaryKey.First().Value == id);
        return Task.FromResult(item);
    }

    public Task<T> Add(T value)
    {
        value.SetBaseModelMetadata<T>(id: GenerateUniqueId(), DefaultDateTimePropertyName);
        Items.Add(value);
        
        return Task.FromResult(value);
    }
    
    public abstract Task Update(T item);

    public virtual Task Delete(int id)
    {
        var itemToDelete = Items.FirstOrDefault(e => (int)e.PrimaryKey.First().Value == id);

        if (itemToDelete == null)
        {
            throw new InvalidOperationException($"Item with ID {id} not found.");
        }

        Items.Remove(itemToDelete);
        return Task.CompletedTask;
    }
}
