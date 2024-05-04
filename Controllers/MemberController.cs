using LibraryService.Models;
using LibraryService.Queries;
using LibraryService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LibraryService.Controllers;

[ApiController]
[Route("api/members")]
[Produces("application/json")]
public class MemberController(
    IRepository<Member> memberRepository,
    ILogger<MemberController> logger) : ControllerBase
{
    /// <summary>
    /// Retrieve all members
    /// </summary>
    /// <param name="nameQuery" example="John">A fuzzy search query to filter members by first or last name</param>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Member>>> GetAll(
        [FromQuery(Name = "fuzzySearch")] string? nameQuery)
    {
        var members = await memberRepository.GetAll();

        if (!string.IsNullOrEmpty(nameQuery))
        {
            members = MemberQueries.FuzzySearchByNameAndLastName(members, nameQuery);
        }
            
        return Ok(members);
    }

    /// <summary>
    /// Retrieve a member by ID
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Member>> Get(int id)
    {
        var member = await memberRepository.GetById(id);
        if (member == null)
        {
            return NotFound();
        }
        return Ok(member);
    }

    /// <summary>
    /// Add a new member
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Member>> AddMember(Member member)
    {
        if (member == null)
        {
            return BadRequest();
        }
            
        var newMember = await memberRepository.Add(member);
        return CreatedAtAction(nameof(Get), new { id = newMember.Id }, newMember);
    }

    /// <summary>
    /// Delete a member by ID
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteMember(int id)
    {
        var memberToDelete = await memberRepository.GetById(id);
        if (memberToDelete == null)
        {
            return NotFound();
        }
        await memberRepository.Delete(memberToDelete.Id);
        return NoContent();
    }
}