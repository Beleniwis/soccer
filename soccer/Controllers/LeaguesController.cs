using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Soccer.Data;
using Soccer.DTOs;
using Soccer.Models;
using Soccer.Validators;

namespace Soccer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeaguesController : ControllerBase
{
    private readonly SoccerDbContext _context;

    public LeaguesController(SoccerDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<League>>> GetLeagues()
    {
        var leagues = await _context.Leagues
            .Include(l => l.Country)
            .Where(l => l.Enabled)
            .ToListAsync();

        return Ok(leagues);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<League>> GetLeague(int id)
    {
        var league = await _context.Leagues
            .Include(l => l.Country)
            .Include(l => l.TeamLeagues)
                .ThenInclude(tl => tl.Team)
            .FirstOrDefaultAsync(l => l.Id == id);

        if (league == null)
        {
            return NotFound("La liga no existe.");
        }

        return Ok(league);
    }

    [HttpPost]
    public async Task<ActionResult<League>> CreateLeague(
        CreateLeagueDto dto)
    {
        var name = dto.Name.Trim();

        if (!NameValidation.IsValid(name))
        {
            return BadRequest(
                "El nombre de la liga debe tener al menos 2 letras y solo puede contener letras, espacios, guiones o apóstrofes.");
        }

        if (dto.StartDate > dto.EndDate)
        {
            return BadRequest(
                "La fecha de inicio no puede ser mayor que la fecha final.");
        }

        var countryExists = await _context.Countries
            .AnyAsync(c => c.Id == dto.CountryId && c.Enabled);

        if (!countryExists)
        {
            return BadRequest(
                "El país indicado no existe o está deshabilitado.");
        }

        var exists = await _context.Leagues
            .AnyAsync(l => l.Name.ToLower() == name.ToLower());

        if (exists)
        {
            return Conflict("Ya existe una liga con ese nombre.");
        }

        var league = new League
        {
            Name = name,
            CountryId = dto.CountryId,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Enabled = true,
            CreatedAt = DateTime.Now
        };

        _context.Leagues.Add(league);

        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetLeague),
            new { id = league.Id },
            league);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateLeague(
        int id,
        CreateLeagueDto dto)
    {
        var league = await _context.Leagues
            .FirstOrDefaultAsync(l => l.Id == id);

        if (league == null)
        {
            return NotFound("La liga no existe.");
        }

        var name = dto.Name.Trim();

        if (!NameValidation.IsValid(name))
        {
            return BadRequest(
                "El nombre de la liga debe tener al menos 2 letras y solo puede contener letras, espacios, guiones o apóstrofes.");
        }

        if (dto.StartDate > dto.EndDate)
        {
            return BadRequest(
                "La fecha de inicio no puede ser mayor que la fecha final.");
        }

        var countryExists = await _context.Countries
            .AnyAsync(c => c.Id == dto.CountryId && c.Enabled);

        if (!countryExists)
        {
            return BadRequest(
                "El país indicado no existe o está deshabilitado.");
        }

        var exists = await _context.Leagues
            .AnyAsync(l =>
                l.Id != id &&
                l.Name.ToLower() == name.ToLower());

        if (exists)
        {
            return Conflict("Ya existe otra liga con ese nombre.");
        }

        league.Name = name;
        league.CountryId = dto.CountryId;
        league.StartDate = dto.StartDate;
        league.EndDate = dto.EndDate;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteLeague(int id)
    {
        var league = await _context.Leagues
            .FirstOrDefaultAsync(l => l.Id == id);

        if (league == null)
        {
            return NotFound("La liga no existe.");
        }

        league.Enabled = false;

        await _context.SaveChangesAsync();

        return NoContent();
    }
}