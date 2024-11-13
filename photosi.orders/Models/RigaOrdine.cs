using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace PhotoSi.Orders.Models
{
    [Table("OrdineRighe", Schema = "dbo")]
    public class RigaOrdine : IEntity
    {
        [Key]
        public required Guid Id { get; set; }

        [ForeignKey("OrdineId")]
        public required Guid OrdineId {  get; set; }

        public required Guid ProdottoId { get; set; }

        public required int NrRiga { get; set; }

        public required string Articolo { get; set; }

        public required string Descrizione { get; set; }

        public required decimal Quantita { get; set; }

        public required decimal Prezzo { get; set; }

        [JsonIgnore]
        public virtual Ordine Ordine { get; set; }
    }
}
