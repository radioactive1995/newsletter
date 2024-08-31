﻿using Application.Interfaces.Services;
using MediatR;

namespace Application.Users;

public static class UserLogin
{
    public record Command() : IRequest;

    public class CommandHandler(
        IIdentityProviderService identityProviderService) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            await identityProviderService.Challenge();
        }
    }
}