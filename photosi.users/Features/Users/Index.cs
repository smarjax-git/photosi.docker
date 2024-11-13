using MediatR;

using Microsoft.EntityFrameworkCore;

using PhotoSi.Users.Data;

using System.ComponentModel.DataAnnotations;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using PhotoSi.Users.Models;

namespace PhotoSi.Users.Features.Users
{
    public class Index
    {
        private readonly IMediator _mediator;

        public Index(IMediator mediator) => _mediator = mediator;

        public record Query : IRequest<List<User>>
        { 
            public Guid? Id { get; init; }
        }

        public class QueryHandler : MediatR. IRequestHandler<Query, List<User>>
        {
            private readonly UsersDbContext _db;
            private readonly AutoMapper.IConfigurationProvider _configuration;

            public QueryHandler(UsersDbContext db, AutoMapper.IConfigurationProvider configuration)
            {
                _db = db;
                _configuration = configuration;
            }

            public async Task<List<User>> Handle(Query message, CancellationToken token)
            {
                var query = _db.Users.Where(x => x.Active == "S");

                if(message.Id != null)
                {
                    query = query.Where(x => x.Id == message.Id);
                }

                return await query.ToListAsync();
            }
        }
    }
}
