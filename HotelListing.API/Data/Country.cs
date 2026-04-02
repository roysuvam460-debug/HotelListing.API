namespace HotelListing.API.Data
{
    public class Country
    {
        public int countryId { get; set; }
        public string? Name { get; set; }
        public string? ShortName { get; set; }
        public IList<Hotel> Hotels { get; set; } = [];
    }
}
