using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace LibraryService.Models;

[Table("borrow_record")]
[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
public class BorrowRecord : BaseModel
{
    [PrimaryKey("id")]
    public int Id { get; set; }
    
    [Column("created_at")]
    [SwaggerSchema(ReadOnly = true)]
    public DateTime CreatedAt { get; set; }
    
    [Column("due_date")]
    public DateTime DueDate { get; set; }
    
    [Column("book_instance_id")]
    [JsonProperty("book_instance_id")]
    public int BookInstanceId { get; set; }
    
    [Column("member_id")]
    [JsonProperty("member_id")]
    public int MemberId { get; set; }
    
    public static BorrowRecord Create(DateTime dueDate, int bookInstanceId, int memberId)
    {
        return new BorrowRecord
        {
            BookInstanceId = bookInstanceId,
            MemberId = memberId,
            DueDate = dueDate
        };
    }
}