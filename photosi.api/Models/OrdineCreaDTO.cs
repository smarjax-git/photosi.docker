namespace PhotoSi.API.Models
{
    public record OrdineCreaDTO
    {
        public System.Guid UserId { get; init; }
        public System.Guid PickupPointId { get; init; }
        public List<RigaCreaDTO>? RigheOrdine { get; init; }
    }
}
