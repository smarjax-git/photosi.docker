using MediatR;

using Microsoft.EntityFrameworkCore;

using PhotoSi.Users.Data;

using System.ComponentModel.DataAnnotations;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using PhotoSi.Users.Models;
using Microsoft.AspNetCore.Authentication.BearerToken;
using FluentValidation;
using Microsoft.Identity.Client;

namespace PhotoSi.Users.Features.UserPickupPoints
{
    public class AddPickupPoint
    {
        private readonly IMediator _mediator;

        public AddPickupPoint(IMediator mediator) => _mediator = mediator;

        public record Command : IRequest<UserPickupPoint>
        {
            public required Guid UserId { get; init; }
            public required Guid PickupPointId { get; init; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(m => m.UserId).NotNull();
                RuleFor(m => m.PickupPointId).NotNull();
            }
        }

        public class CommandHandler : IRequestHandler<Command, UserPickupPoint>
        {
            private readonly UsersDbContext _db;
            private readonly AutoMapper.IConfigurationProvider _configuration;

            public CommandHandler(UsersDbContext db, AutoMapper.IConfigurationProvider configuration)
            {
                _db = db;
                _configuration = configuration;
            }

            public async Task<UserPickupPoint> Handle(Command message, CancellationToken token)
            {
                var existingPP = await _db.UserPickupPoints.Where(x => x.UserId == message.UserId && x.PickupPointId == message.PickupPointId).SingleOrDefaultAsync();

                if (existingPP != null)
                {
                    throw new InvalidOperationException();
                }

                var newPP = new UserPickupPoint
                {
                    Id = Guid.NewGuid(),
                    UserId = message.UserId,
                    PickupPointId = message.PickupPointId
                };

                _db.Entry(newPP).State = EntityState.Added;

                await _db.SaveChangesAsync();

                return newPP;
            }
        }
    }
}
