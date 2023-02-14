namespace HexaPokerNet.Application.Infrastructure;

public interface IHealthProvider
{
    HealthStatus HealthStatus { get; }
    event EventHandler HealthStatusChanged;
}

public enum HealthStatus
{
    Starting,
    Healthy
}
