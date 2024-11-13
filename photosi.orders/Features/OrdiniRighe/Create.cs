using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

using PhotoSi.Orders.Data;
using PhotoSi.Orders.Models;

namespace PhotoSi.Orders.Features.OrdiniRighe
{
    public class Create
    {
        private readonly IMediator _mediator;

        public Create(IMediator mediator) => _mediator = mediator;

        public record Command : IRequest<RigaOrdine>
        {
            public required Guid OrdineId { get; init; }
            public required Guid UserId { get; init; }

            public required Guid ProdottoId { get; init; }

            public required decimal Quantita { get; init; }

            public required decimal Prezzo { get; init; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(m => m.OrdineId).NotNull();
                RuleFor(m => m.ProdottoId).NotNull();
                RuleFor(m => m.Quantita).GreaterThan(0);
                RuleFor(m => m.Prezzo).GreaterThan(0);
            }
        }

        public class CommandHandler : IRequestHandler<Command, RigaOrdine>
        {
            private readonly OrdersDbContext _db;
            private readonly AutoMapper.IConfigurationProvider _configuration;

            public CommandHandler(OrdersDbContext db, AutoMapper.IConfigurationProvider configuration)
            {
                _db = db;
                _configuration = configuration;
            }

            public async Task<RigaOrdine> Handle(Command message, CancellationToken token)
            {
                var ordineTrovato = await _db.Ordini.Where(x => x.Id == message.OrdineId && x.UserId == message.UserId).AnyAsync();

                if(!ordineTrovato)
                {
                    throw new InvalidOperationException($"L'ordine {message.OrdineId} non esiste per l'utente {message.UserId}");
                }

                int nextNrRiga = 0;

                if (_db.OrdiniRighe.Where(x => x.OrdineId == message.OrdineId).Any())
                {
                    nextNrRiga = _db.OrdiniRighe.Where(x => x.OrdineId == message.OrdineId).Max(x => x.NrRiga) + 1;
                }
                else
                {
                    nextNrRiga = 1;
                }

                var riga = new RigaOrdine()
                {
                    Id = Guid.NewGuid(),
                    Articolo = $"ARTICOLO {nextNrRiga}",
                    Descrizione = $"DESCRIZIONE ARTICOLO {nextNrRiga}",
                    NrRiga = nextNrRiga,
                    OrdineId = message.OrdineId,
                    Prezzo = message.Prezzo,
                    ProdottoId = message.ProdottoId, 
                    Quantita = message.Quantita
                };

                _db.Entry(riga).State = EntityState.Added;

                await _db.SaveChangesAsync();

                return riga;
            }
        }
    }
}
