using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoSi.PickupPoint.Models
{
    [Table("PickUpPoints", Schema="dbo")]
    public class PickUpPoint
    {
        [Key]
        public required Guid Id {  get; set; }

        [MaxLength(255)]
        public required string Name { get; set; }

        [MaxLength(255)]
        public required string Address { get; set; }

        [MaxLength(5)]
        public required string ZipCode { get; set; }

        [MaxLength(255)]
        public required string City { get; set; }

        [MaxLength(1)]
        public required string Active { get; set; } = "S";
    }
}
