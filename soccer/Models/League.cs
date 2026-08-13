using System.Text.Json.Serialization;

namespace Soccer.Models;

public class League
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int CountryId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool Enabled { get; set; }

    public DateTime CreatedAt { get; set; }

    public Country Country { get; set; } = null!;

    [JsonIgnore]
    public ICollection<League> Leagues { get; set; } = new List<League>();

    public ICollection<TeamLeague> TeamLeagues { get; set; } = new List<TeamLeague>();
}