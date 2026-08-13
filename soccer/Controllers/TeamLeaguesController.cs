using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Soccer.Data;
using Soccer.DTOs;
using Soccer.Models;

namespace Soccer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeamLeaguesController : ControllerBase
{
    private readonly SoccerDbContext _context;

    public TeamLeaguesController(SoccerDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult> GetTeamLeagues()
    {
        var registrations = await _context.TeamLeagues
            .Include(tl => tl.Team)
            .Include(tl => tl.League)
            .Where(tl => tl.Team.Enabled && tl.League.Enabled)
            .Select(tl => new
            {
                teamId = tl.TeamId,
                teamName = tl.Team.Name,
                leagueId = tl.LeagueId,
                leagueName = tl.League.Name
            })
            .ToListAsync();

        return Ok(registrations);
    }

    [HttpPost]
    public async Task<ActionResult> RegisterTeam(
        RegisterTeamLeagueDto dto)
    {
        var team = await _context.Teams
            .Include(t => t.Players)
            .FirstOrDefaultAsync(t => t.Id == dto.TeamId);

        if (team == null || !team.Enabled)
        {
            return BadRequest(
                "El equipo no existe o está deshabilitado.");
        }

        var league = await _context.Leagues
            .FirstOrDefaultAsync(l => l.Id == dto.LeagueId);

        if (league == null || !league.Enabled)
        {
            return BadRequest(
                "La liga no existe o está deshabilitada.");
        }

        var playerCount = team.Players
            .Count(p => p.Enabled);

        if (playerCount < 11)
        {
            return BadRequest(
                $"El equipo tiene {playerCount} jugadores activos. " +
                "Debe tener al menos 11 jugadores para inscribirse en una liga.");
        }

        if (playerCount > 22)
        {
            return BadRequest(
                $"El equipo tiene {playerCount} jugadores activos. " +
                "No puede tener más de 22 jugadores para inscribirse en una liga.");
        }

        var alreadyRegistered = await _context.TeamLeagues
            .AnyAsync(tl =>
                tl.TeamId == dto.TeamId &&
                tl.LeagueId == dto.LeagueId);

        if (alreadyRegistered)
        {
            return Conflict(
                "El equipo ya está inscrito en esta liga.");
        }

        var registration = new TeamLeague
        {
            TeamId = dto.TeamId,
            LeagueId = dto.LeagueId
        };

        _context.TeamLeagues.Add(registration);

        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Equipo inscrito correctamente en la liga.",
            teamId = team.Id,
            teamName = team.Name,
            leagueId = league.Id,
            leagueName = league.Name,
            players = playerCount
        });
    }

    [HttpDelete]
    public async Task<ActionResult> UnregisterTeam(
        RegisterTeamLeagueDto dto)
    {
        var registration = await _context.TeamLeagues
            .FirstOrDefaultAsync(tl =>
                tl.TeamId == dto.TeamId &&
                tl.LeagueId == dto.LeagueId);

        if (registration == null)
        {
            return NotFound(
                "El equipo no está inscrito en esta liga.");
        }

        _context.TeamLeagues.Remove(registration);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}