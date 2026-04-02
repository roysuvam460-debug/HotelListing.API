using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListing.API.Data;
using HotelListing.API.DTOs.Country;
using HotelListing.API.DTOs.Hotel;

namespace HotelListing.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CountriesController : ControllerBase
{
    private readonly HotelListingDbContext _context;

    public CountriesController(HotelListingDbContext context)
    {
        _context = context;
    }

    // GET: api/Countries
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetCountriesDto>>> GetCountries()
    {
        //return await _context.Countries.Include(c => c.Hotels).ToListAsync();
        var hotels = await _context.Countries.Select(c => new GetCountriesDto(c.countryId, c.Name, c.ShortName, c.Hotels.Select(h => new DTOs.Hotel.GetHotelsDto(
                h.Id, h.Name, h.Address, h.Rating, h.CountryId
            )).ToList())).ToListAsync();
        return Ok(hotels);
    }

    // GET: api/Countries/5
    [HttpGet("{id}")]
    public async Task<ActionResult<GetCountryDto>> GetCountry(int id)
    {
#pragma warning disable CS8604 // Possible null reference argument.
        var country = await _context.Countries.Where(c => c.countryId == id).Select(c => new GetCountryDto(c.countryId, c.Name, c.ShortName, c.Hotels.Select(h => new GetHotelDto(
                h.Id, h.Name, h.Address, h.Rating, h.CountryId, h.Country.Name
            )).ToList())).FirstOrDefaultAsync();
#pragma warning restore CS8604 // Possible null reference argument.

        if (country == null)
        {
            return NotFound();
        }

        return Ok(country);
    }

    // PUT: api/Countries/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCountry(int id, UpdateCountryDto countryDto)
    {
        if (id != countryDto.countryId)
        {
            return BadRequest();
        }

        var country = await _context.Countries.FindAsync(id);
        if (country == null)
        {
            return NotFound();
        }

        country.Name = countryDto.Name;
        country.ShortName = countryDto.ShortName;

        _context.Entry(country).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await CountryExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Countries
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Country>> PostCountry(CreateCountryDto countryDto)
    {
        var country = new Country
        {
            Name = countryDto.Name,
            ShortName = countryDto.ShortName
        };
        _context.Countries.Add(country);
        await _context.SaveChangesAsync();

        var resultDto = new GetCountryDto(
            country.countryId,
            country.Name,   
            country.ShortName,
            []
        );

        return CreatedAtAction("GetCountry", new { id = country.countryId }, country);
    }

    // DELETE: api/Countries/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCountry(int id)
    {
        var country = await _context.Countries.FindAsync(id);
        if (country == null)
        {
            return NotFound();
        }

        _context.Countries.Remove(country);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> CountryExists(int id)
    {
        return await _context.Countries.AnyAsync(e => e.countryId == id);
    }
}
