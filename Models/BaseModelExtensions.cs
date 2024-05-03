using System.Reflection;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace LibraryService.Models;

public static class BaseModelExtensions
{
    public static void SetBaseModelMetadata<T>(this BaseModel model, int id, string createdAtPropertyName)
    {
        var properties = typeof(T).GetProperties();
        
        PropertyInfo? primaryKey = null;
        PropertyInfo? createdAt = null;
        
        foreach (var property in properties)
        {
            if (Attribute.IsDefined(property, typeof(PrimaryKeyAttribute)))
            {
                primaryKey = property;
            }
            else if (property.Name == createdAtPropertyName)
            {
                createdAt = property;
            }
        }
        
        primaryKey?.SetValue(model, id);
        createdAt?.SetValue(model, DateTime.Now);
    }
}