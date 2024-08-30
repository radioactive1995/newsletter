namespace Infrastructure.Configuration;

public record IdentityProviderConfig
{
    public required string Authority { get; init; }
    public required string ClientId { get; init; }
    public required string ClientSecret { get; init; }
    public required string SignInPolicy { get; init; }
    public required string PasswordResetPolicy { get; init; }
    public string SignInMetadataAddress => Path.Combine(Authority, ".well-known/openid-configuration?p=", SignInPolicy);
    public string PasswordResetMetadataAddress => Path.Combine(Authority, ".well-known/openid-configuration?p=", PasswordResetPolicy);
}

public static class IdentityProviderConstants 
{
    public const string IDENTITY_PROVIDER = "IdentityProvider";
    public const string LOGIN_CALLBACK = "auth/callback";
    public const string SIGNOUT_CALLBACK = "signout/callback";
    public const string PASSWORD_RESET_CALLBACK = "passwordreset/callback";

    public const string COOKIE_NAME = "NewsletterCookie";
    public const string PASSWORD_RESET_SCHEME = "passwordreset";
    public const string PASSWORD_RESET_OIDC_ERROR_CODE = "AADB2C90118";
}
