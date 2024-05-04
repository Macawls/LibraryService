using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace LibraryService.Models;

[Table("book_genre")]
[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
public class BookGenre : BaseModel
{
    [PrimaryKey("id")]
    public int Id { get; set; }
    
    [Column("book_id")]
    [JsonProperty("book_id")]
    public int BookId { get; set; }
    
    [Column("genre_id")]
    [JsonProperty("genre_id")]
    public int GenreId { get; set; }
    
    public static BookGenre Create(int bookId, int genreId)
    {
        return new BookGenre
        {
            BookId = bookId,
            GenreId = genreId
        };
    }
}