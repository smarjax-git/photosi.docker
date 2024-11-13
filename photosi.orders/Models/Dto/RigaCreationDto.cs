namespace PhotoSi.Orders.Models.Dto
{
    public record RigaCreationDto
    {
        public Guid OrdineId { get; init; }
        public Guid UserId { get; init; }

        public Guid ProdottoId { get; init; }

        public decimal Quantita { get; init; }

        public decimal Prezzo { get; init; }
    }
}
