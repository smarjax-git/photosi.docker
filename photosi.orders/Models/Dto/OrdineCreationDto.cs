using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoSi.Orders.Models.Dto
{
    public class OrdineCreationDto
    {
        public required Guid UserId { get; set; }
        public required Guid PickupPointId { get; set; }
    }
}
