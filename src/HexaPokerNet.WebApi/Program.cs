using HexaPokerNet.Adapter.Repositories;
using HexaPokerNet.Application.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add adapters and services to the container.
builder.Services.AddSingleton<IWritableRepository, InMemoryRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();