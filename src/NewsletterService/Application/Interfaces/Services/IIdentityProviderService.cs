namespace Application.Interfaces.Services;

public interface IIdentityProviderService
{
    public Task Challenge(string? returnUrl);
    public Task Signout(string? returnUrl);
}
