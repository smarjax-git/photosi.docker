namespace photosi.users.Models
{
    public record CreateUserPickupPointDto
    {
        public required Guid UserId { get; init; }
        public required Guid PickupPointId { get; init; }
    }
}
