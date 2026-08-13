namespace Soccer.Models;

public class Player
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int TeamId { get; set; }

    public bool Enabled { get; set; }

    public DateTime CreatedAt { get; set; }

    public Team Team { get; set; } = null!;
}