using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces;
using Infrastructure.Jobs;
using Infrastructure.Persistance;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Infrastructure.Configuration;
using System.Security.Claims;
using Application.Users;
using Ardalis.GuardClauses;

namespace Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IArticleRepository, ArticleRepository>();
        services.AddTransient<ISubscriberRepository, SubscriberRepository>();
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IEmailService, EmailService>();

        services.AddSingleton<ICacheService, CacheService>();
        services.AddSingleton<IEventBus, InMemoryEventBus>();

        services.AddHostedService<EventConsumerJob>();

        services.AddDbContextFactory<NewsletterContext>((provider, options) =>
        {

            var connectionString = Environment.GetEnvironmentVariable("SQLCONNSTR_DefaultConnection")
                               ?? configuration.GetConnectionString("DefaultConnection");

            
            options.UseSqlServer(connectionString, sqlServerOptionsAction: sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 15,
                    maxRetryDelay: TimeSpan.FromMinutes(1),
                    errorNumbersToAdd: [ -2, 1205, 40197, 40501, 40613, 10928, 10929 ]);
            });
        });

        services.AddDistributedMemoryCache();

        services.Configure<IdentityProviderConfig>(configuration.GetSection(nameof(IdentityProviderConfig)));
        var identityConfig = configuration.GetSection(nameof(IdentityProviderConfig)).Get<IdentityProviderConfig>();
        ArgumentNullException.ThrowIfNull(identityConfig);

        //add auth cookie with auth oidc
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        }).AddCookie(options =>
        {
            options.Cookie.Name = IdentityProviderConstants.COOKIE_NAME;
            options.Cookie.SameSite = SameSiteMode.Lax;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

            options.Events = new CookieAuthenticationEvents
            {
                OnRedirectToLogin = async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await Task.CompletedTask;
                }
            };

        }).AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
        {
            options.Authority = identityConfig.Authority;
            options.ClientId = identityConfig.ClientId;
            options.ClientSecret = identityConfig.ClientSecret;
            options.ResponseType = identityConfig.ResponseType;

            foreach (var scope in identityConfig.Scopes)
            {
                options.Scope.Add(scope);
            }

            options.SaveTokens = true;
            options.GetClaimsFromUserInfoEndpoint = true;
            options.MetadataAddress = identityConfig.SignInMetadataAddress;
            options.CallbackPath = IdentityProviderConstants.LOGIN_CALLBACK;
            options.SignedOutCallbackPath = IdentityProviderConstants.SIGNOUT_CALLBACK;

            options.Events = new OpenIdConnectEvents
            {
                OnRemoteFailure = context =>
                {
                    // Check if the error is due to the user canceling the self-asserted information process
                    if (context.Failure != null &&
                        context.Failure.Message.Contains(IdentityProviderConstants.CANCELLED_OIDC_ERROR_CODE))
                    {
                        // Redirect the user to the home page
                        context.Response.Redirect("/");
                        context.HandleResponse(); // Prevent further processing
                    }

                    return Task.CompletedTask;
                },

                OnTokenValidated = context =>
                {
                    var eventBus = context.HttpContext.RequestServices.GetRequiredService<IEventBus>();

                    Guard.Against.Null(context.Principal, message: "OIDC.OnTokenValidated.ClaimsPrincipal cannot be null");
                    Guard.Against.Null(context.Principal.Identity, message: "OIDC.OnTokenValidated.ClaimsPrincipal.IIdentity cannot be null");

                    var claimsIdentity = (ClaimsIdentity)context.Principal.Identity;

                    var emailClaims = claimsIdentity.FindAll("emails");
                    var oidClaim = claimsIdentity.FindFirst(UserLogin.OID_CLAIMTYPE);
                    var idpClaim = claimsIdentity.FindFirst(UserLogin.IDP_CLAIMTYPE);

                    Guard.Against.NullOrEmpty(emailClaims, message: "OIDC.OnTokenValidated.emailClaims cannot be null or empty");
                    Guard.Against.Null(oidClaim, message: "OIDC.OnTokenValidated.oidClaim cannot be null");
                    Guard.Against.NullOrWhiteSpace(oidClaim.Value, message: "OIDC.OnTokenValidated.oidClaim.Value cannot be null or empty");

                    eventBus.PublishAsync(new UserLogin.Event(Emails: emailClaims.Select(e => e.Value).ToArray(), OidClaim: oidClaim.Value, IdpClaim: idpClaim?.Value));

                    return Task.CompletedTask;
                },
            };
        });


        services.AddAuthorization();

        return services;
    }
}
