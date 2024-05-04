using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace LibraryService.Models;

public enum BookStatusType { Available, InUse, Lost }

[Table("book_status")]
[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
public class BookStatus : BaseModel
{
    [SwaggerSchema(ReadOnly = true)]
    [PrimaryKey("id")]
    public int Id { get; set; }
    
    [Column("book_id")]
    [JsonProperty("book_id")]
    public int BookId { get; set; }
    
    [Column("book_status_type")]
    [JsonProperty("book_status_type")]
    [JsonConverter(typeof(StringEnumConverter))]
    public BookStatusType StatusType { get; set; }
}