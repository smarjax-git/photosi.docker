using MediatR;

using Microsoft.EntityFrameworkCore;

using PhotoSi.Orders.Data;
using PhotoSi.Orders.Models;

namespace PhotoSi.Orders.Features.OrdiniRighe
{
    public class Index
    {
        private readonly IMediator _mediator;

        public Index(IMediator mediator) => _mediator = mediator;

        public record Query : IRequest<List<RigaOrdine>>
        {
            public Guid UserId { get; init; }
            public Guid OrderId { get; init; }
        }

        public class QueryHandler : MediatR. IRequestHandler<Query, List<RigaOrdine>>
        {
            private readonly OrdersDbContext _db;
            private readonly AutoMapper.IConfigurationProvider _configuration;

            public QueryHandler(OrdersDbContext db, AutoMapper.IConfigurationProvider configuration)
            {
                _db = db;
                _configuration = configuration;
            }

            public async Task<List<RigaOrdine>> Handle(Query message, CancellationToken token)
            {
                var res =   await _db.Ordini.Where(x => x.UserId == message.UserId && x.Id == message.OrderId)
                                   .Join(_db.OrdiniRighe
                                            .Include(o => o.Ordine),
                                        o => o.Id,
                                        r => r.OrdineId,
                                        (o, r) => r
                                    ).ToListAsync();

                return res;
            }
        }
    }
}
