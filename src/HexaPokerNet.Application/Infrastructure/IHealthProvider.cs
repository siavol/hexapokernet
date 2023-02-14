namespace HexaPokerNet.Application.Infrastructure;

public interface IHealthProvider
{
    HealthStatus HealthStatus { get; }
}

public enum HealthStatus
{
    Starting,
    Healthy
}