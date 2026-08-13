
namespace Soccer.Models;

public class Country
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public bool Enabled { get; set; }

    public DateTime CreatedAt { get; set; }

    public ICollection<Team> Teams { get; set; } = new List<Team>();

    public ICollection<League> Leagues { get; set; } = new List<League>();
}