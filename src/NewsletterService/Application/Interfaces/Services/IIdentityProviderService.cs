namespace Application.Interfaces.Services;

public interface IIdentityProviderService
{
    public Task Challenge();
    public Task Signout();
}
