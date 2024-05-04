using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace LibraryService.Models;

/// <summary>
/// A book genre
/// </summary>
[Table("genre")]
[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
public class Genre : BaseModel
{
    [PrimaryKey("id")]
    [SwaggerSchema(ReadOnly = true)]
    public int Id { get; set; }
    
    /// <summary>
    /// The name of the genre
    /// </summary>
    /// <example>fiction</example>
    [Column("name")]
    public string Name { get; set; }
}