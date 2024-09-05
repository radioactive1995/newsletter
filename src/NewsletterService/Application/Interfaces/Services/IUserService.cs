using Application.Users;

namespace Application.Interfaces.Services;

public interface ICurrentUserService
{
    public UserInformationDto? GetUserInformation();
    public record UserInformationDto(string? Oid, string? UserName, IEnumerable<string> Emails);

    public string? GetIpAddress();
}

