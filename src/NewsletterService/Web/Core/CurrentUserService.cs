using Application.Interfaces.Services;
using System.Security.Claims;
using static Application.Interfaces.Services.ICurrentUserService;

namespace Web.Core;

public class CurrentUserService(
    IHttpContextAccessor accessor) : ICurrentUserService
{
    public string? GetIpAddress()
    {
        return accessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
    }

    UserInformationDto? ICurrentUserService.GetUserInformation()
    {
        if (accessor.HttpContext?.User?.Identity?.IsAuthenticated == false) return null;

        return new UserInformationDto(
            accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier),
            accessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name),
            accessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email)
        );
    }
}
