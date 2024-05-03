using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace LibraryService.Models;

/// <summary>
/// Represents a book
/// </summary>
[Table("book")]
[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
public class Book : BaseModel
{
    [PrimaryKey("id")]
    [SwaggerSchema(ReadOnly = true)]
    public int Id { get; set; }
    
    [Column("created_at")]
    [SwaggerSchema(ReadOnly = true)]
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// The title of the book
    /// </summary>
    /// <example>The Great Gatsby</example>
    [Column("title")]
    public string Title { get; set; }
    
    /// <summary>
    /// The author of the book
    /// </summary>
    /// <example>F. Scott Fitzgerald</example>
    [Column("author")]
    public string Author { get; set; }
    
    /// <summary>
    /// The year the book was published
    /// </summary>
    /// <example>1925</example>
    [Column("publication_year")]
    public int PublicationYear { get; set; }
   
    /// <summary>
    /// The ISBN of the book
    /// </summary>
    /// <example>9780743273565</example>
    [Column("isbn")]
    public string ISBN { get; set; }
}