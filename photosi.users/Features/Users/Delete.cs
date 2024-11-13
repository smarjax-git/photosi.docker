using MediatR;

using Microsoft.EntityFrameworkCore;

using PhotoSi.Users.Data;

using System.ComponentModel.DataAnnotations;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using PhotoSi.Users.Models;
using Microsoft.AspNetCore.Authentication.BearerToken;

namespace PhotoSi.Users.Features.Users
{
    public class Delete
    {
        private readonly IMediator _mediator;

        public Delete(IMediator mediator) => _mediator = mediator;

        public record Command : IRequest<Unit>
        {
            public required Guid Id { get; init; }    
        }

        public class CommandHandler : IRequestHandler<Command, Unit>
        {
            private readonly UsersDbContext _db;
            private readonly AutoMapper.IConfigurationProvider _configuration;

            public CommandHandler(UsersDbContext db, AutoMapper.IConfigurationProvider configuration)
            {
                _db = db;
                _configuration = configuration;
            }

            public async Task<Unit> Handle(Command message, CancellationToken token)
            {
                var user = await _db.Users.Where(x => x.Id == message.Id).SingleOrDefaultAsync();

                if (user == null) 
                {
                    throw new InvalidOperationException();
                }

                _db.Entry(user).State = EntityState.Deleted;

                await _db.SaveChangesAsync();

                return default;
            }
        }
    }
}
