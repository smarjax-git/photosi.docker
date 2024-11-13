namespace photosi.api.Models
{
    public class CreatePickupPointDto
    {
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required string ZipCode { get; set; }
        public required string City { get; set; }
    }
}
