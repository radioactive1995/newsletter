using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Users;
using MediatR;
using AccountType = Domain.Users.User.AccountType;

namespace Application.Users;

public static class UserLogin
{
    public const string OID_CLAIMTYPE = "http://schemas.microsoft.com/identity/claims/objectidentifier";
    public const string IDP_CLAIMTYPE = "http://schemas.microsoft.com/identity/claims/identityprovider";

    public record Command(string? ReturnUrl) : IRequest;
    public record Event(string[] Emails, string OidClaim, string? IdpClaim) : INotification
    {
        public AccountType AccountType => IdpClaim switch
        {
            "live.com" => AccountType.Microsoft,
            "google.com" => AccountType.Google,
            _ when string.IsNullOrWhiteSpace(IdpClaim) => AccountType.Local,
            _ => throw new ArgumentOutOfRangeException(nameof(IdpClaim), IdpClaim, "UserLogin.Event.IdpClaim, unknown value")
        };
    }

    public class CommandHandler(
        IIdentityProviderService identityProviderService) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            await identityProviderService.Challenge(request.ReturnUrl);
        }
    }


    public static class EventHandler
    {
        public class SyncUserLogin(IUserRepository userRepository) : INotificationHandler<Event>
        {
            public async Task Handle(Event @event, CancellationToken cancellationToken)
            {
                var user = await userRepository.GetUserByEmails(@event.Emails);

                if (user is null)
                {
                    user = User.CreateEntity(accountType: @event.AccountType, oidClaim: @event.OidClaim, emails: @event.Emails);
                    await userRepository.AddUser(user);
                    return;
                }

                user.UpdateUserSettings(@event.AccountType, @event.OidClaim, @event.Emails);
                await userRepository.UpdateUser(user);
            }
        }
    }
}
