using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace Web.Core;

public class IdentityProviderService(IHttpContextAccessor accessor) : IIdentityProviderService
{
    public Task Challenge(string? returnUrl)
    {
        if (accessor.HttpContext == null) throw new InvalidOperationException("HttpContext is null");

        var relevantRedirectUri = string.IsNullOrWhiteSpace(returnUrl) ? "/" : returnUrl;

        return accessor.HttpContext.ChallengeAsync(scheme: OpenIdConnectDefaults.AuthenticationScheme, properties: new() { RedirectUri = relevantRedirectUri });
    }

    public async Task Signout(string? returnUrl)
    {
        if (accessor.HttpContext == null) throw new InvalidOperationException("HttpContext is null");

        var relevantRedirectUri = string.IsNullOrWhiteSpace(returnUrl) ? "/" : returnUrl;

        await accessor.HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        await accessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme, properties: new() { RedirectUri = relevantRedirectUri });
    }
}