using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace LibraryService.Models;

public enum BookStatusType { Available, InUse, Lost }

[Table("book_instance")]
[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
public class BookInstance : BaseModel
{
    [SwaggerSchema(ReadOnly = true)]
    [PrimaryKey("id")]
    public int Id { get; set; }
    
    [Column("created_at")]
    [SwaggerSchema(ReadOnly = true)]
    public DateTime CreatedAt { get; set; }
    
    [Column("book_id")]
    [JsonProperty("book_id")]
    public int BookId { get; set; }
    
    [Column("book_status_type")]
    [JsonProperty("book_status_type")]
    [JsonConverter(typeof(StringEnumConverter))]
    public BookStatusType StatusType { get; set; }

    public static BookInstance Create(int bookId)
    {
        return new BookInstance
        {
            BookId = bookId
        };
    }
}