using MediatR;

using Microsoft.EntityFrameworkCore;

using PhotoSi.Orders.Data;

using System.ComponentModel.DataAnnotations;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using PhotoSi.Orders.Models;

namespace PhotoSi.Orders.Features.Ordini
{
    public class Index
    {
        private readonly IMediator _mediator;

        public Index(IMediator mediator) => _mediator = mediator;

        public record Query : IRequest<List<Ordine>>
        { 
            public Guid UserId { get; init; }
            public Guid? OrderId { get; init; }
        }

        public class QueryHandler : MediatR. IRequestHandler<Query, List<Ordine>>
        {
            private readonly OrdersDbContext _db;
            private readonly AutoMapper.IConfigurationProvider _configuration;

            public QueryHandler(OrdersDbContext db, AutoMapper.IConfigurationProvider configuration)
            {
                _db = db;
                _configuration = configuration;
            }

            public async Task<List<Ordine>> Handle(Query message, CancellationToken token)
            {
                var query = _db.Ordini.Where(x => x.UserId == message.UserId);

                if (message.OrderId != null) 
                {
                    query = query.Where(x => x.Id == message.OrderId);
                }

                return await query.Include(x => x.RigheOrdine)
                                    .ToListAsync();
            }
        }
    }
}
