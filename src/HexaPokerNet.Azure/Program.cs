using HexaPokerNet.Adapter;
using HexaPokerNet.Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(s =>
    {
        s.AddSingleton<IEntityIdGenerator, EntityIdGenerator>();
    })
    .Build();

host.Run();
