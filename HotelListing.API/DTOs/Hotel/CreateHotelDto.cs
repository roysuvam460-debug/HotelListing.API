using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.DTOs.Hotel
{
    public class CreateHotelDto
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        [MaxLength(20)]
        public string? Address { get; set; }
        [Range(1, 5)]
        public double Rating { get; set; }
        [Required]
        public int CountryId { get; set; }
    }
    public record GetHotelsDto(
        int Id,
        string? Name,
        string? Address,
        double Rating,
        int CountryId
    );
    public record GetHotelDto(
        int Id,
        string? Name,
        string? Address,
        double Rating,
        int CountryId,
        string Country
    );
}

