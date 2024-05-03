using LibraryService.Models;
using LibraryService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LibraryService.Controllers;

[ApiController]
[Route("api/genres")]
public class GenreController(
    IRepository<Genre> genreRepository, 
    ILogger<GenreController> logger) : ControllerBase
{
    /// <summary>
    /// Retrieve all book genres
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Genre>>> Get()
    {
        var genres = await genreRepository.GetAll();
        return new ActionResult<IEnumerable<Genre>>(genres);
    }
    
    /// <summary>
    /// Retrieve a genre by ID
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Member>> Get(int id)
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
    public async Task<ActionResult<Member>> Add(Genre genre)
    {
        if (genre == null)
        {
            return BadRequest();
        }
        var newGenre = await genreRepository.Add(genre);
        return CreatedAtAction(nameof(Get), new { id = newGenre.Id }, newGenre);
    }
}