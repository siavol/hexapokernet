using HexaPokerNet.Adapter;
using HexaPokerNet.Azure;
using HexaPokerNet.Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(
        worker => worker.ConfigureSystemTextJson()
    )
    .ConfigureServices(s =>
    {
        s.AddSingleton<IEntityIdGenerator, EntityIdGenerator>();
    })
    .Build();

host.Run();
