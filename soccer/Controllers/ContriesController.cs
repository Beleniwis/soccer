using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Soccer.Data;
using Soccer.DTOs;
using Soccer.Models;
using Soccer.Validators;

namespace Soccer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CountriesController : ControllerBase
{
    private readonly SoccerDbContext _context;

    public CountriesController(SoccerDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Country>>> GetCountries()
    {
        var countries = await _context.Countries
            .Where(c => c.Enabled)
            .ToListAsync();

        return Ok(countries);
    }

    [HttpPost]
    public async Task<ActionResult<Country>> CreateCountry(CreateCountryDto dto)
    {
        var name = dto.Name.Trim();

        if (!NameValidation.IsValid(name))
        {
            return BadRequest(
                "El nombre debe tener al menos 2 letras y solo puede contener letras, espacios, guiones o apóstrofes.");
        }

        var exists = await _context.Countries
            .AnyAsync(c => c.Name.ToLower() == name.ToLower());

        if (exists)
        {
            return Conflict("Ya existe un país con ese nombre.");
        }

        var country = new Country
        {
            Name = name,
            Enabled = true,
            CreatedAt = DateTime.Now
        };

        _context.Countries.Add(country);

        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetCountries),
            new { id = country.Id },
            country);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Country>> GetCountry(int id)
    {
        var country = await _context.Countries
            .FirstOrDefaultAsync(c => c.Id == id);

        if (country == null)
        {
            return NotFound("El país no existe.");
        }

        return Ok(country);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateCountry(
    int id,
    CreateCountryDto dto)
    {
        var country = await _context.Countries
            .FirstOrDefaultAsync(c => c.Id == id);

        if (country == null)
        {
            return NotFound("El país no existe.");
        }

        var name = dto.Name.Trim();

        if (string.IsNullOrWhiteSpace(name) || name.Length < 2)
        {
            return BadRequest("El nombre debe tener al menos 2 caracteres.");
        }

        var exists = await _context.Countries
            .AnyAsync(c =>
                c.Id != id &&
                c.Name.ToLower() == name.ToLower());

        if (exists)
        {
            return Conflict("Ya existe otro país con ese nombre.");
        }

        country.Name = name;

        await _context.SaveChangesAsync();

        return NoContent();
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCountry(int id)
    {
        var country = await _context.Countries
            .FirstOrDefaultAsync(c => c.Id == id);

        if (country == null)
        {
            return NotFound("El país no existe.");
        }

        country.Enabled = false;

        await _context.SaveChangesAsync();

        return NoContent();
    }

}