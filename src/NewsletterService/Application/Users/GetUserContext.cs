using Application.Interfaces.Services;
using Domain.Common;
using ErrorOr;
using MediatR;

namespace Application.Users;

//public static class GetUserContext
//{
//    public record Query : IRequest<Result<Response>>;
//    public record Response(string UserId, string? UserName, IEnumerable<string> Emails)
//    {
//        string DisplayName => string.Join(", ", Emails).GetHashCode().ToString();
//    }
//
//    public class QueryHandler : IRequestHandler<Query, Result<Response>>
//    {
//        private readonly ICurrentUserService _currentUserService;
//
//        public QueryHandler(ICurrentUserService currentUserService)
//        {
//            _currentUserService = currentUserService;
//        }
//
//        public async Task<Result<Response>> Handle(Query request, CancellationToken cancellationToken)
//        {
//            await Task.CompletedTask;
//
//            var userDto = _currentUserService.GetUserInformation();
//
//            return userDto switch
//            {
//                null => Error.Unauthorized("GetUserContext.Unauthorized", "User is not authenticated"),
//                { UserId: null } => Error.Unauthorized("GetUserContext.Unauthorized.UserId", "userId cannot be empty, user is not authenticated"),
//                _ => new Response(userDto.UserId, userDto.UserName, userDto.Email),
//            };
//        }
//    }
//}

//expand on GetUserContext so that we get the hash of the email addresses as a display name of the user
public static class GetUserContext
{
    public record Query : IRequest<Result<Response>>;
    public record Response(string UserId, string? UserName, IEnumerable<string> Emails)
    {
        public string DisplayName => Math.Abs(string.Join(", ", Emails).GetHashCode()).ToString();
    }

    public class QueryHandler : IRequestHandler<Query, Result<Response>>
    {
        private readonly ICurrentUserService _currentUserService;

        public QueryHandler(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public async Task<Result<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            var userDto = _currentUserService.GetUserInformation();

            return userDto switch
            {
                null => Error.Unauthorized("GetUserContext.Unauthorized", "User is not authenticated"),
                { Oid: null or "" } => Error.Unauthorized("GetUserContext.Unauthorized.Oid", "userId cannot be empty, user is not authenticated"),
                _ => new Response(userDto.Oid, userDto.UserName, userDto.Emails),
            };
        }
    }
}
