using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoSi.Users.Models
{
    [Table("Users", Schema = "dbo")]
    public class User : IEntity
    {
        [Key]
        public required Guid Id { get; set; }

        [MaxLength(255)]
        [Required(AllowEmptyStrings = false)]
        public required string Name { get; set; }

        [MaxLength(255)]
        [Required(AllowEmptyStrings = false)]
        public required string Surname { get; set; }

        [MaxLength(60)]
        [Required(AllowEmptyStrings = false)]
        public required string Email { get; set; }

        [MaxLength(1)]
        public required string Active { get; set; } = "S";

        public ICollection<UserPickupPoint> PreferredPickupPoints { get; set; } = [];
    }
}
