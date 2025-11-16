using Microsoft.EntityFrameworkCore;
using ArcusTel.PortalClientes.Api.Data;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddDbContext<AppDbContext>(options =>
    options
    .UseSqlServer(builder
    .Configuration
    .GetConnectionString("AppDbContext") ?? throw new InvalidOperationException("Connection string 'AppDbContext' not found.")));

builder.Services.AddControllers();
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
