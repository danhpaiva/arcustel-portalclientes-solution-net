using ArcusTel.PortalClientes.Api.Data;
using ArcusTel.PortalClientes.Api.Helpers;
using ArcusTel.PortalClientes.Api.Interface;
using ArcusTel.PortalClientes.Api.Service;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AppDbContext")
        ?? throw new InvalidOperationException("Connection string 'AppDbContext' not found.")));

builder.Services
    .AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
        opts.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddHttpClient<IPartnerBrasilClient, PartnerBrasilClient>(client =>
{
    var url = builder.Configuration["Apis:PartnerBrasil"];
    if (string.IsNullOrWhiteSpace(url)) throw new InvalidOperationException("Apis:PartnerBrasil not configured");
    client.BaseAddress = new Uri(url);
});

builder.Services.AddHttpClient<IWorldTelClient, WorldTelClient>(client =>
{
    var url = builder.Configuration["Apis:WorldTel"];
    if (string.IsNullOrWhiteSpace(url)) throw new InvalidOperationException("Apis:WorldTel not configured");
    client.BaseAddress = new Uri(url);
});

builder.Services.AddScoped<IPartnerStatusHelper, PartnerStatusHelper>();
builder.Services.AddScoped<IDidNormalizer, DidNormalizer>();
builder.Services.AddScoped<IPrefixHelper, PrefixHelper>();

builder.Services.AddScoped<IDidOrchestrator, DidOrchestrator>();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();