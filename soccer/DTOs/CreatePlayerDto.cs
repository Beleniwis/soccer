using System.ComponentModel.DataAnnotations;

namespace Soccer.DTOs;

public class CreatePlayerDto
{
    [Required]
    [MinLength(2)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public int TeamId { get; set; }
}