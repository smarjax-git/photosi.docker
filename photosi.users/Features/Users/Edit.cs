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
    public class Edit
    {
        private readonly IMediator _mediator;

        public Edit(IMediator mediator) => _mediator = mediator;

        public record Command : IRequest<Unit>
        {
            public required Guid Id { get; init; }
            public string Name { get; init; }
            public string Surname { get; init; }
            public string Email { get; init; }
            public string Active { get; init; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(m => m.Id).NotNull();
                RuleFor(m => m.Name).NotNull().Length(1, 255);
                RuleFor(m => m.Surname).NotNull().Length(1, 255);
                RuleFor(m => m.Email).NotNull().Length(6, 60);
                RuleFor(m => m.Active).NotNull().Length(1);
            }
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

                user.Name = message.Name;
                user.Surname = message.Surname;
                user.Active = message.Active;
                user.Email = message.Email;

                _db.Entry(user).State = EntityState.Modified;

                await _db.SaveChangesAsync();

                return default;
            }
        }
    }
}
