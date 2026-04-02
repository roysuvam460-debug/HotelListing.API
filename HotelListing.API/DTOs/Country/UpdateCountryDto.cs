using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.DTOs.Country
{
    public class UpdateCountryDto : CreateCountryDto
    {
        [Required]
        public int countryId { get; set; }
    }
}
