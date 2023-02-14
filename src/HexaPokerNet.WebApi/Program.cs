using System.Reflection;
using HexaPokerNet.Adapter;
using HexaPokerNet.Adapter.Repositories;
using HexaPokerNet.Adapter.Repositories.Kafka;
using HexaPokerNet.Application.Infrastructure;
using HexaPokerNet.Application.Repositories;
using HexaPokerNet.Domain;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);
AddHexapokernetServices(builder.Services, new AppConfiguration());
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
app.Services.GetRequiredService<IReadableRepository>().Start();

app.UseHttpLogging();

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


void AddHexapokernetServices(IServiceCollection services, AppConfiguration config)
{
    services.AddSingleton<IEntityIdGenerator, EntityIdGenerator>();
    services.AddSingleton<AggregatedHealthProvider>();

    switch (config.RepositoryKind)
    {
        case EWritableRepository.InMemory:
            var inMemoryRepository = new InMemoryRepository();
            services
                .AddSingleton<IEventStore>(inMemoryRepository)
                .AddSingleton<IReadableRepository>(inMemoryRepository);
            break;
        case EWritableRepository.Kafka:
            services
                .AddTransient<IKafkaConfiguration, AppConfiguration>()
                .AddSingleton<IEventStore, KafkaEventStore>()
                .AddSingleton<IReadableRepository, KafkaReadableRepository>();
            break;
        default:
            throw new ArgumentOutOfRangeException();
    }
}


/// <summary>
/// Web API program. 
/// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
public partial class Program { }