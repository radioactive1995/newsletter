using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace Web.Core;

public class IdentityProviderService(IHttpContextAccessor accessor) : IIdentityProviderService
{
    public Task Challenge()
    {
        if (accessor.HttpContext == null) throw new InvalidOperationException("HttpContext is null");

        return accessor.HttpContext.ChallengeAsync(scheme: OpenIdConnectDefaults.AuthenticationScheme, properties: new() { RedirectUri = "/" });
    }

    public async Task Signout()
    {
        if (accessor.HttpContext == null) throw new InvalidOperationException("HttpContext is null");

        await accessor.HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        await accessor.HttpContext.SignOutAsync("passwordreset");
        await accessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme, properties: new() { RedirectUri = "/"});
    }
}