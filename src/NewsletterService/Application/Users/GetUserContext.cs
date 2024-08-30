using Application.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.Users;

public static class GetUserContext
{
    public record Query : IRequest<ErrorOr<Response>>;
    public record Response(string UserId, string? UserName, string? Email);

    public class QueryHandler : IRequestHandler<Query, ErrorOr<Response>>
    {
        private readonly ICurrentUserService _currentUserService;

        public QueryHandler(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public async Task<ErrorOr<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            var userDto = _currentUserService.GetUserInformation();

            return userDto switch
            {
                null => Error.Unauthorized("GetUserContext.Unauthorized", "User is not authenticated"),
                { UserId: null } => Error.Unauthorized("GetUserContext.Unauthorized.UserId", "userId cannot be empty, user is not authenticated"),
                _ => new Response(userDto.UserId, userDto.UserName, userDto.Email),
            };
        }
    }
}
