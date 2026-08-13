using System;

namespace Soccer.Api.Models;

public class Team
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int CountryId { get; set; }
    public bool Enabled { get; set; }
    public DateTime CreatedAt { get; set; }
    public Country Country { get; set; } = null!;
    public ICollection<Player> Players { get; set; } = new List<Player>();
    public ICollection<TeamLeague> TeamLeagues { get; set; } = new List<TeamLeague>();
}
