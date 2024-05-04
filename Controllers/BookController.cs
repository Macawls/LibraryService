using System.Net.Mime;
using LibraryService.Queries;
using LibraryService.Models;
using LibraryService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LibraryService.Controllers;

[ApiController]
[Route("api/books")]
[Produces(MediaTypeNames.Application.Json)]
public class BookController(
    IRepository<Book> bookRepository,
    IRepository<Genre> genreRepository,
    IRepository<BookGenre> bookGenreRepository,
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
    public async Task<ActionResult<IEnumerable<Book>>> Get(
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


        var genresOfBook = BookQueries.GetGenresOfBook(book, genres, bookGenres);

        return Ok(genresOfBook);
    }
    
    /// <summary>
    /// Add a book
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Book>> Add(Book? book)
    {
        if (book == null)
        {
            return BadRequest(book);
        }
        
        var newBook = await bookRepository.Add(book);
        return CreatedAtAction(nameof(Get), new { id = newBook.Id }, newBook);
    }
    
    /// <summary>
    /// Delete a book by ID
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

        await bookRepository.Delete(bookToDelete.Id);
        return NoContent();
    }
}