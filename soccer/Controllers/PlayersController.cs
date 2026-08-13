using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Soccer.Data;
using Soccer.DTOs;
using Soccer.Models;
using Soccer.Validators;

namespace Soccer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayersController : ControllerBase
{
    private readonly SoccerDbContext _context;

    public PlayersController(SoccerDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
    {
        var players = await _context.Players
            .Include(p => p.Team)
            .Where(p => p.Enabled)
            .ToListAsync();

        return Ok(players);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Player>> GetPlayer(int id)
    {
        var player = await _context.Players
            .Include(p => p.Team)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (player == null)
        {
            return NotFound("El jugador no existe.");
        }

        return Ok(player);
    }

    [HttpPost]
    public async Task<ActionResult<Player>> CreatePlayer(
        CreatePlayerDto dto)
    {
        var name = dto.Name.Trim();

        if (!NameValidation.IsValid(name))
        {
            return BadRequest(
                "El nombre debe tener al menos 2 letras y solo puede contener letras, espacios, guiones o apóstrofes.");
        }

        var teamExists = await _context.Teams
            .AnyAsync(t => t.Id == dto.TeamId && t.Enabled);

        if (!teamExists)
        {
            return BadRequest(
                "El equipo indicado no existe o está deshabilitado.");
        }

        var player = new Player
        {
            Name = name,
            TeamId = dto.TeamId,
            Enabled = true,
            CreatedAt = DateTime.Now
        };

        _context.Players.Add(player);

        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetPlayer),
            new { id = player.Id },
            player);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdatePlayer(
        int id,
        CreatePlayerDto dto)
    {
        var player = await _context.Players
            .FirstOrDefaultAsync(p => p.Id == id);

        if (player == null)
        {
            return NotFound("El jugador no existe.");
        }

        var name = dto.Name.Trim();

        if (!NameValidation.IsValid(name))
        {
            return BadRequest(
                "El nombre debe tener al menos 2 letras y solo puede contener letras, espacios, guiones o apóstrofes.");
        }

        var teamExists = await _context.Teams
            .AnyAsync(t => t.Id == dto.TeamId && t.Enabled);

        if (!teamExists)
        {
            return BadRequest(
                "El equipo indicado no existe o está deshabilitado.");
        }

        player.Name = name;
        player.TeamId = dto.TeamId;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePlayer(int id)
    {
        var player = await _context.Players
            .FirstOrDefaultAsync(p => p.Id == id);

        if (player == null)
        {
            return NotFound("El jugador no existe.");
        }

        player.Enabled = false;

        await _context.SaveChangesAsync();

        return NoContent();
    }
}