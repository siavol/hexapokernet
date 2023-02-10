using HexaPokerNet.Adapter;
using HexaPokerNet.Adapter.Repositories.Kafka;

public class AppConfiguration : IKafkaConfiguration
{
    private const string DefaultKafkaServer = "localhost:9092";

    public string KafkaServer => Environment.GetEnvironmentVariable(AppEnvironmentVariables.KafkaServer) ?? DefaultKafkaServer;

    public EWritableRepository RepositoryKind
    {
        get
        {
            var repositoryName = Environment.GetEnvironmentVariable(AppEnvironmentVariables.WritableRepoEnvVar);
            if (String.IsNullOrEmpty(repositoryName)) return EWritableRepository.InMemory;
            if (Enum.TryParse(repositoryName, true, out EWritableRepository repositoryKind)) return repositoryKind;
            throw new ApplicationException(
                $"Can not parse env variable ${AppEnvironmentVariables.WritableRepoEnvVar}, value '${repositoryName}' is unknown");
        }
    }
}