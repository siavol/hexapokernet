using System.Reflection;
using HexaPokerNet.Adapter;
using HexaPokerNet.Adapter.Repositories;
using HexaPokerNet.Application.Repositories;
using HexaPokerNet.Domain;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add adapters and services to the container.
builder.Services
    .AddSingleton<IWritableRepository, InMemoryRepository>()
    .AddSingleton<IEntityIdGenerator, EntityIdGenerator>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Hexagon Poker API",
        Description = "An ASP.NET Core Web API for the Hexagon Poker API"
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

// Configure the HTTP request pipeline.

// Always use Swagger, because this is a demo project.
// In prod filter with: 
// if (app.Environment.IsDevelopment())
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


/// <summary>
/// Web API program. 
/// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
public partial class Program { }