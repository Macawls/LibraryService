using LibraryService.Models;
using LibraryService.Queries;
using LibraryService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LibraryService.Controllers;

[ApiController]
[Route("api/genres")]
[Produces("application/json")]
public class GenreController(
    IRepository<Genre> genreRepository,
    IRepository<BookGenre> bookGenreRepository,
    ILogger<GenreController> logger) : ControllerBase
{
    /// <summary>
    /// Retrieve all book genres
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Genre>>> Get()
    {
        var genres = await genreRepository.GetAll();
        return Ok(genres);
    }
    
    /// <summary>
    /// Retrieve a genre by ID
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Genre>> Get(int id)
    {
        var genre = await genreRepository.GetById(id);
        if (genre == null)
        {
            return NotFound();
        }
        return Ok(genre);
    }   
    
    /// <summary>
    /// Add a genre
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<Genre>> Add(Genre genre)
    {
        if (genre == null)
        {
            return BadRequest();
        }

        var genres = await genreRepository.GetAll();

        var valid = GenreQueries.GetValidGenres(genres, new[] { genre.Name });

        if (valid.Any())
        {
            return Conflict(new { message = "Genre name already exists" });
        }
        
        var newGenre = await genreRepository.Add(genre);
        
        return CreatedAtAction(nameof(Get), new { id = newGenre.Id }, newGenre);
    }
    
    /// <summary>
    /// Delete a genre by ID
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteGenre(int id)
    {
        var genreToDelete = await genreRepository.GetById(id);
        
        if (genreToDelete == null)
        {
            return NotFound();
        }
        
        var bookGenres = await bookGenreRepository.GetAll();
        var bookGenresOfBook = new List<BookGenre>(BookGenreQueries.GetBookGenresOfBook(id, bookGenres));
        
        foreach (var bookGenre in bookGenresOfBook)
        {
            await bookGenreRepository.Delete(bookGenre.Id);
        }
        
        await genreRepository.Delete(genreToDelete.Id);
        
        return NoContent();
    }
}