using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoSi.Catalog.Models
{
    [Table("Prodotti", Schema="dbo")]
    public class Prodotto
    {
        [Key]
        public required Guid Id { get; set; }

        [MaxLength(20)]
        public required string Codice { get; set; }

        [MaxLength(255)]
        public required string Name { get; set; }

        [ForeignKey("CategoriaId")]
        public required Guid CategoriaId { get; set; }

        public required decimal Price { get; set; }

        [MaxLength(1)]
        [DefaultValue("S")]
        public required string Active { get; set; } = "S";

        public virtual Categoria Categoria { get; set; }

    }
}
