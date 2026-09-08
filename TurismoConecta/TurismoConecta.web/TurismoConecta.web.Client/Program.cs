using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TurismoConecta.web.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7078/")
});

builder.Services.AddTransient<AuthorizationMessageHandler>();

builder.Services.AddHttpClient<MunicipioApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7078");
})
.AddHttpMessageHandler<AuthorizationMessageHandler>();

// Registra ItinerarioApiService igual que MunicipioApiService
builder.Services.AddHttpClient<ItinerarioApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7078");
})
.AddHttpMessageHandler<AuthorizationMessageHandler>();

//Servicios clave para que <AuthorizeView> no se congele:
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthStateProvider>();

await builder.Build().RunAsync();