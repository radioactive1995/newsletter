﻿using Application.Interfaces.Repositories;
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
using Microsoft.AspNetCore.Authentication;
using Infrastructure.Configuration;
using Microsoft.Extensions.Options;

namespace Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IArticleRepository, ArticleRepository>();
        services.AddTransient<ISubscriberRepository, SubscriberRepository>();
        services.AddTransient<IEmailService, EmailService>();

        services.AddSingleton<ICacheService, CacheService>();
        services.AddSingleton<IEventBus, InMemoryEventBus>();

        services.AddHostedService<EventConsumerJob>();

        services.AddDbContextFactory<NewsletterContext>((provider, options) =>
        {

            var connectionString = Environment.GetEnvironmentVariable("SQLCONNSTR_DefaultConnection")
                               ?? configuration.GetConnectionString("DefaultConnection");
            
            options.UseSqlServer(connectionString);
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
            options.Cookie.Name = "NewsletterCookie";
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
            options.Authority = "https://dzhumaev.b2clogin.com/c2bc9021-60ee-4a94-8474-a8536a17baf4/v2.0/";
            options.ClientId = identityConfig.ClientId;
            options.ClientSecret = identityConfig.ClientSecret;
            options.ResponseType = "code";
            options.Scope.Add("openid");
            options.Scope.Add("profile");
            options.Scope.Add("email");
            options.Scope.Add("https://dzhumaev.onmicrosoft.com/20d4be3b-5113-474c-8bec-09f482bf7ce4/Newsletter.ReadWrite");
            options.SaveTokens = true;
            options.GetClaimsFromUserInfoEndpoint = true;
            options.TokenValidationParameters.NameClaimType = "name";
            options.TokenValidationParameters.RoleClaimType = "role";
            options.MetadataAddress = "https://dzhumaev.b2clogin.com/dzhumaev.onmicrosoft.com/v2.0/.well-known/openid-configuration?p=b2c_1_dzhumaev_flow";
            options.CallbackPath = "/auth/callback"; // This is the default; you can customize it if needed

            // Define the URL to redirect to after logout
            options.SignedOutCallbackPath = "/signout/callback"; // Customize this as needed

            // Optionally, set the PostLogoutRedirectUri if you need a specific URL after logout
            //options.SignedOutRedirectUri = "/";
        });


        services.AddAuthorization();

        return services;
    }
}
