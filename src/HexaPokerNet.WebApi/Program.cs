using System.Reflection;
using HexaPokerNet.Adapter;
using HexaPokerNet.Adapter.Repositories;using HexaPokerNet.Adapter.Repositories.Kafka;
using HexaPokerNet.Application.Repositories;
using HexaPokerNet.Domain;
using Microsoft.OpenApi.Models;

const string writableRepoEnvVar = "HPN_WRITABLE_REPO";
const string kafkaServerEnvVar = "HPN_KAFKA_SERVER";

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddJsonConsole();

// Add adapters and services to the container.
var writableRepositoryName = Environment.GetEnvironmentVariable(writableRepoEnvVar);
EWritableRepository writableRepositoryKind = EWritableRepository.InMemory;
if (!string.IsNullOrEmpty(writableRepositoryName))
{
    if (!Enum.TryParse(writableRepositoryName, true, out writableRepositoryKind))
    {
        throw new ApplicationException(
            $"Can not parse env variable ${writableRepoEnvVar}, value '${writableRepositoryName}' is unknown");
    }
}

var inMemoryRepository = new InMemoryRepository();
IEventStore eventStore;
switch (writableRepositoryKind)
{
    case EWritableRepository.InMemory:
        eventStore = inMemoryRepository;
        break;
    case EWritableRepository.Kafka:
        eventStore = new KafkaEventStore(Environment.GetEnvironmentVariable(kafkaServerEnvVar) ?? "localhost:9092");
        break;
    default:
        throw new ArgumentOutOfRangeException();
}
builder.Services
    .AddSingleton(eventStore)
    .AddSingleton<IReadableRepository>(inMemoryRepository)
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


/// <summary>
/// Web API program. 
/// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
public partial class Program { }