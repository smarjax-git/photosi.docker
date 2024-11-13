using MediatR;

using Microsoft.EntityFrameworkCore;

using PhotoSi.Users.Data;

using System.ComponentModel.DataAnnotations;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using PhotoSi.Users.Models;

namespace PhotoSi.Users.Features.UserPickupPoints
{
    public class Index
    {
        private readonly IMediator _mediator;

        public Index(IMediator mediator) => _mediator = mediator;

        public record Query : IRequest<List<UserPickupPoint>>
        { 
            public Guid? UserId { get; init; }
        }

        public class QueryHandler : MediatR. IRequestHandler<Query, List<UserPickupPoint>>
        {
            private readonly UsersDbContext _db;
            private readonly AutoMapper.IConfigurationProvider _configuration;

            public QueryHandler(UsersDbContext db, AutoMapper.IConfigurationProvider configuration)
            {
                _db = db;
                _configuration = configuration;
            }

            public async Task<List<UserPickupPoint>> Handle(Query message, CancellationToken token)
            {
                return await _db.UserPickupPoints.Where(x => x.UserId == message.UserId).ToListAsync();
            }
        }
    }
}
