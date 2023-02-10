using HexaPokerNet.Adapter.Repositories.Kafka;

public class EnvironmentVariablesKafkaConfiguration: IKafkaConfiguration
{
    public string KafkaServer => Environment.GetEnvironmentVariable(AppEnvironmentVariables.KafkaServer) ?? "localhost:9092";
}