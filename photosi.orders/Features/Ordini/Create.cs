using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

using PhotoSi.Orders.Data;
using PhotoSi.Orders.Models;

namespace PhotoSi.Orders.Features.Ordini
{
    public class Create
    {
        private readonly IMediator _mediator;

        public Create(IMediator mediator) => _mediator = mediator;

        public record Command : IRequest<Ordine>
        {
            public required Guid UserId { get; set; }
            public required Guid PickupPointId { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(m => m.UserId).NotNull();
                RuleFor(m => m.PickupPointId).NotNull();
            }
        }

        public class CommandHandler : IRequestHandler<Command, Ordine>
        {
            private readonly OrdersDbContext _db;
            private readonly AutoMapper.IConfigurationProvider _configuration;

            public CommandHandler(OrdersDbContext db, AutoMapper.IConfigurationProvider configuration)
            {
                _db = db;
                _configuration = configuration;
            }

            public async Task<Ordine> Handle(Command message, CancellationToken token)
            {
                var ordine = new Ordine()
                {
                    Id = Guid.NewGuid(),
                    UserId = message.UserId,
                    NrOrdine = "2024-000001",       //* Logica creazione progressivo
                    Data = DateTime.Now,
                    Stato = "C",
                    PickupPointId = message.PickupPointId
                };

                _db.Entry(ordine).State = EntityState.Added;

                await _db.SaveChangesAsync();

                return ordine;
            }
        }
    }
}
