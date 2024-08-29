using Web.Components;
using Application;
using Infrastructure;
using Web.Core;
using Application.Interfaces.Services;
using System.Reflection;
using Web.Core.Interfaces;
using Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services
    .AddInfrastructure()
    .AddApplication();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IIdentityProviderService, IdentityProviderService>();

var app = builder.Build();

app.MapEndpoints();

app.UseExceptionHandler("/Error", createScopeForErrors: true);
app.UseHsts();

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.UseAuthentication();
app.UseAuthorization();


await app.RunAsync();

//comment