using LibraryService.Queries;
using LibraryService.Models;
using LibraryService.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LibraryService.Controllers;

[ApiController]
[Route("api/books")]
[Produces("application/json")]
public class BookController(
    IRepository<Book> bookRepository,
    IRepository<Genre> genreRepository,
    IRepository<BookGenre> bookGenreRepository,
    IRepository<BookInstance> bookInstanceRepository,
    ILogger<BookController> logger) : ControllerBase
{
    /// <summary>
    /// Retrieve all books
    /// </summary>
    /// <param name="genreFilter" example="Fantasy">A list of genres to filter the books by, case insensitive</param>
    /// <param name="publishedAfter" example="1970">After the specified year</param>
    /// <param name="publishedBefore" example="2010" >Before the specified year</param>
    /// <param name="titleOrAuthorQuery" example="Harry">A fuzzy search query to filter books by title or author</param>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Book>>> GetAll(
        [FromQuery(Name = "fuzzySearch")] string? titleOrAuthorQuery,
        [FromQuery(Name = "publishedBefore")] int? publishedBefore,
        [FromQuery(Name = "publishedAfter")] int? publishedAfter,
        [FromQuery(Name = "genres")] params string[] genreFilter)
    {
        var books = await bookRepository.GetAll();
        
        if (genreFilter.Length != 0)
        {
            var genres = await genreRepository.GetAll();
            var bookGenres = await bookGenreRepository.GetAll();

            books = BookQueries.GetBooksByGenre(genreFilter, books, genres, bookGenres);
        }

        if (publishedBefore.HasValue)
        {
            books = BookQueries.PublishedBefore(books, publishedBefore.Value);
        }

        if (publishedAfter.HasValue)
        {
            books = BookQueries.PublishedAfter(books, publishedAfter.Value);
        }

        if (!string.IsNullOrEmpty(titleOrAuthorQuery))
        {
            books = BookQueries.FuzzySearchByTitleOrAuthor(books, titleOrAuthorQuery);
        }

        return Ok(books);
    }

    /// <summary>
    /// Retrieve a book by ID
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Book>> Get(int id)
    {
        var book = await bookRepository.GetById(id);
        return book == null ? NotFound() : Ok(book);
    }
    
    /// <summary>
    /// Retrieve the genres of the book by ID
    /// </summary>
    [HttpGet("{id:int}/genres")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<Genre>>> GetGenres(int id)
    {
        var book = await bookRepository.GetById(id);

        if (book == null)
        {
            return NotFound();
        }

        var genres = await genreRepository.GetAll();
        var bookGenres = await bookGenreRepository.GetAll();


        var genresOfBook = BookQueries.GetGenresOfBook(book.Id, genres, bookGenres);

        return Ok(genresOfBook);
    }
    
    /// <summary>
    /// Retrieve the instances of the book by ID
    /// </summary>
    [HttpGet("{id:int}/instances")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<BookInstance>>> GetInstances(int id)
    {
        var book = await bookRepository.GetById(id);

        if (book == null)
        {
            return NotFound();
        }

        var bookInstances = await bookInstanceRepository.GetAll();
        
        var instances = BookInstanceQueries.GetInstancesOfBook(book.Id, bookInstances);

        return Ok(instances);
    }
    
    /// <summary>
    /// Add a book
    /// </summary>
    /// <param name="amountOfCopies" example="2" >The amount of copies to add</param>
    /// <param name="genreNames" example="Thriller,Mystery">
    /// The genres to be set for the book.
    /// If there is no matches for the genre, it is ignored.
    /// </param>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<int>>> Add(Book? book,
        [FromQuery(Name = "amount")] int? amountOfCopies = 1,
        [FromQuery, BindRequired] params string[] genreNames)
    {
        if (book == null)
        {
            return BadRequest(book);
        }

        if (amountOfCopies < 0)
        {
            return BadRequest(new { message = "Amount of copies cannot be negative" });
        }

        if (genreNames.Length == 0)
        {
            return BadRequest(new { message = "Genres cannot be omitted when adding a book" });
        }
        
        var newBook = await bookRepository.Add(book);

        var genres = await genreRepository.GetAll();
        var validGenres = new List<Genre>(GenreQueries.GetValidGenres(genres, genreNames));
        
        foreach (var genre in validGenres)
        {
            await bookGenreRepository.Add(BookGenre.Create(book.Id, genre.Id));
        }
        
        for (var i = 0; i < amountOfCopies; i++)
        {
            var instance = BookInstance.Create(newBook.Id);
            await bookInstanceRepository.Add(instance);
        }
        
        return CreatedAtAction(nameof(Get), new { id = newBook.Id }, newBook);
    }
    
    /// <summary>
    /// Delete a book by ID, also deletes all instances of the book.
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        var bookToDelete = await bookRepository.GetById(id);
    
        if (bookToDelete == null)
        {
            return NotFound();
        }

        var bookInstances = await bookInstanceRepository.GetAll();
        
        var instancesToDelete = new List<BookInstance>(BookInstanceQueries.GetInstancesOfBook(bookToDelete.Id, bookInstances));
        
        foreach (var bookInstance in instancesToDelete)
        {
            await bookInstanceRepository.Delete(bookInstance.Id);
        }
        
        await bookRepository.Delete(bookToDelete.Id);
        
        return NoContent();
    }
}