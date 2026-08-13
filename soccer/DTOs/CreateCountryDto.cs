using System.ComponentModel.DataAnnotations;
namespace Soccer.DTOs;

public class CreateCountryDto
{
    [Required]
    [MinLength(2)]
    public string Name { get; set; } = string.Empty;
}