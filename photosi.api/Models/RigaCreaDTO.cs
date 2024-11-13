namespace PhotoSi.API.Models
{
    public record RigaCreaDTO
    {
        public System.Guid OrdineId { get; init; }
        public System.Guid UserId { get; init; }
        public System.Guid ProdottoId { get; init; }
        public double Quantita { get; init; }
        public double Prezzo { get; init; }
    }
}
