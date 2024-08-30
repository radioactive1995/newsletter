using Application.Users;
using MediatR;
using Web.Core.Interfaces;

namespace Web.Components.Pages.Login;

public class Endpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/login", async (IMediator mediator) =>
        {
            await mediator.Send(new UserLogin.Command());
        });

        endpoints.MapGet("/signout", async (IMediator mediator, HttpContext context) =>
        {
            var user = context.User;
            Console.WriteLine(user.Identity?.Name);
            await mediator.Send(new UserSignOut.Command());
        }).RequireAuthorization();
    }
}
