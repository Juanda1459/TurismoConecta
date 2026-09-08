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

// 👇 Registra ItinerarioApiService igual que MunicipioApiService
builder.Services.AddHttpClient<ItinerarioApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7078");
})
.AddHttpMessageHandler<AuthorizationMessageHandler>();

await builder.Build().RunAsync();