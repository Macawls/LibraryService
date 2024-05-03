using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace LibraryService.Models;

/// <summary>
/// A member of the library
/// </summary>
[Table("member")]
[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
public class Member : BaseModel
{
    [PrimaryKey("id")]
    [SwaggerSchema(ReadOnly = true)]
    public int Id { get; set; }
    
    [Column("created_at")]
    [SwaggerSchema(ReadOnly = true)]
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// The user's email.
    /// </summary>
    /// <example>john123@yahoo.com</example>
    [Column("email")]
    public string Email { get; set; }
    
    /// <summary>
    /// The user's first name
    /// </summary>
    /// <example>John</example>
    [Column("first_name")]
    public string FirstName { get; set; }
    
    /// <summary>
    /// The user's last name
    /// </summary>
    /// <example>Doe</example>
    [Column("last_name")]
    public string LastName { get; set; }
    
    /// <summary>
    /// The user's contact number
    /// </summary>
    /// <example>073 321 6890</example>
    [Column("phone_number")]
    public string PhoneNumber { get; set; }
}