using MediatR;

using Microsoft.EntityFrameworkCore;

using PhotoSi.Users.Data;

using System.ComponentModel.DataAnnotations;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using PhotoSi.Users.Models;
using Microsoft.AspNetCore.Authentication.BearerToken;
using FluentValidation;

namespace PhotoSi.Users.Features.Users
{
    public class Create
    {
        private readonly IMediator _mediator;

        public Create(IMediator mediator) => _mediator = mediator;

        public record Command : IRequest<User>
        {
            public required string Name { get; init; }
            public required string Surname { get; init; }
            public required string Email { get; init; }
            public List<Guid> PreferredPickupPoints { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(m => m.Name).NotNull().Length(1, 255);
                RuleFor(m => m.Surname).NotNull().Length(1, 255);
                RuleFor(m => m.Email).NotNull().Length(6, 60);
            }
        }

        public class CommandHandler : IRequestHandler<Command, User>
        {
            private readonly UsersDbContext _db;
            private readonly AutoMapper.IConfigurationProvider _configuration;

            public CommandHandler(UsersDbContext db, AutoMapper.IConfigurationProvider configuration)
            {
                _db = db;
                _configuration = configuration;
            }

            public async Task<User> Handle(Command message, CancellationToken token)
            {
                var user = new User()
                {
                    Id = Guid.NewGuid(),
                    Name = message.Name,
                    Surname = message.Surname,
                    Active = "S",
                    Email = message.Email
                };

                _db.Entry(user).State = EntityState.Added;

                if (message.PreferredPickupPoints != null)
                {
                    foreach (var pp in message.PreferredPickupPoints)
                    {
                        var newpp = new UserPickupPoint
                        {
                            Id = Guid.NewGuid(),
                            UserId = user.Id,
                            PickupPointId = pp
                        };

                        _db.Entry(newpp).State = EntityState.Added;
                    }
                }

                await _db.SaveChangesAsync();

                return user;
            }
        }
    }
}
