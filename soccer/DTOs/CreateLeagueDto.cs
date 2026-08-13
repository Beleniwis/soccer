using System.ComponentModel.DataAnnotations;

namespace Soccer.DTOs;

public class CreateLeagueDto
{
    [Required]
    [MinLength(2)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public int CountryId { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }
}