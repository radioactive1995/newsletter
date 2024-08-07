using Application.Interfaces;

namespace Web.Core;

public class CurrentUserService(
    IHttpContextAccessor accessor) : ICurrentUserService
{
    public string? GetIpAddress()
    {
        return accessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
    }
}
