using System.ComponentModel.DataAnnotations;

namespace Soccer.DTOs;

public class CreateTeamDto
{
    [Required]
    [MinLength(2)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public int CountryId { get; set; }
}