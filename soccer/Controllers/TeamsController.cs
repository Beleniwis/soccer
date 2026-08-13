using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Soccer.Data;
using Soccer.DTOs;
using Soccer.Models;
using Soccer.Validators;

namespace Soccer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeamsController : ControllerBase
{
    private readonly SoccerDbContext _context;

    public TeamsController(SoccerDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetTeams()
    {
        var teams = await _context.Teams
            .Include(t => t.Country)
            .Where(t => t.Enabled)
            .ToListAsync();

        return Ok(teams);
    }

    [HttpPost]
    public async Task<ActionResult<Team>> CreateTeam(CreateTeamDto dto)
    {
        var name = dto.Name.Trim();

        if (!NameValidation.IsValid(name))
        {
            return BadRequest("El nombre del equipo debe tener al menos 2 caracteres.");
        }

        var countryExists = await _context.Countries
            .AnyAsync(c => c.Id == dto.CountryId && c.Enabled);

        if (!countryExists)
        {
            return BadRequest("El país indicado no existe o está deshabilitado.");
        }

        var exists = await _context.Teams
            .AnyAsync(t => t.Name.ToLower() == name.ToLower());

        if (exists)
        {
            return Conflict("Ya existe un equipo con ese nombre.");
        }

        var team = new Team
        {
            Name = name,
            CountryId = dto.CountryId,
            Enabled = true,
            CreatedAt = DateTime.Now
        };

        _context.Teams.Add(team);

        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetTeam),
            new { id = team.Id },
            team);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Team>> GetTeam(int id)
    {
        var team = await _context.Teams
            .Include(t => t.Country)
            .Include(t => t.Players)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (team == null)
        {
            return NotFound("El equipo no existe.");
        }

        return Ok(team);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateTeam(
    int id,
    CreateTeamDto dto)
    {
        var team = await _context.Teams
            .FirstOrDefaultAsync(t => t.Id == id);

        if (team == null)
        {
            return NotFound("El equipo no existe.");
        }

        var name = dto.Name.Trim();

        if (string.IsNullOrWhiteSpace(name) || name.Length < 2)
        {
            return BadRequest("El nombre del equipo debe tener al menos 2 caracteres.");
        }

        var countryExists = await _context.Countries
            .AnyAsync(c => c.Id == dto.CountryId && c.Enabled);

        if (!countryExists)
        {
            return BadRequest("El país indicado no existe o está deshabilitado.");
        }

        var exists = await _context.Teams
            .AnyAsync(t =>
                t.Id != id &&
                t.Name.ToLower() == name.ToLower());

        if (exists)
        {
            return Conflict("Ya existe otro equipo con ese nombre.");
        }

        team.Name = name;
        team.CountryId = dto.CountryId;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTeam(int id)
    {
        var team = await _context.Teams
            .FirstOrDefaultAsync(t => t.Id == id);

        if (team == null)
        {
            return NotFound("El equipo no existe.");
        }

        team.Enabled = false;

        await _context.SaveChangesAsync();

        return NoContent();
    }
}