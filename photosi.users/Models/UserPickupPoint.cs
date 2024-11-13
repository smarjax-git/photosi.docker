using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoSi.Users.Models
{
    [Table("UserPickupPoints", Schema = "dbo")]
    public class UserPickupPoint : IEntity
    {
        [Key]
        public required Guid Id { get; set; }

        public required Guid UserId { get; set; }

        public required Guid PickupPointId { get; set; }
    }
}
