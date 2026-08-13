using System;
namespace Soccer.Api.Models;

public class TeamLeague
{
    public int TeamId { get; set; }
    public int LeagueId { get; set; }
    public Team Team { get; set; } = null!;
    public League League { get; set; } = null!;
}