using System.Text.Json.Serialization;

namespace Soccer.Models;

public class Team
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int CountryId { get; set; }

    public bool Enabled { get; set; }

    public DateTime CreatedAt { get; set; }

    [JsonIgnore]
    public Country Country { get; set; } = null!;

    [JsonIgnore]
    public ICollection<Player> Players { get; set; } = new List<Player>();

    [JsonIgnore]
    public ICollection<TeamLeague> TeamLeagues { get; set; } = new List<TeamLeague>();
}