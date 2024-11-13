using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PhotoSi.Catalog.Models
{
    [Table("Categorie", Schema="dbo")]
    public class Categoria
    {
        [Key]
        public required Guid Id { get; set; }

        [MaxLength(255)]
        public required string Name { get; set; }

        [DefaultValue("S")]
        [MaxLength(1)]
        public required string Active { get; set; } = "S";

        [JsonIgnore]
        public virtual ICollection<Prodotto> Prodotti { get; set; }
    }
}
