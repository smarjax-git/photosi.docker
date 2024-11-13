using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace PhotoSi.Orders.Models
{
    [Table("Ordini", Schema = "dbo")]
    public class Ordine : IEntity
    {
        public required Guid Id { get; set; }
        public required string NrOrdine { get; set; }
        public required DateTime Data { get; set; }
        public required string Stato { get; set; }
        public required Guid UserId { get; set; }
        public required Guid PickupPointId { get; set; }

        public ICollection<RigaOrdine> RigheOrdine { get; set; } = [];
    }
}