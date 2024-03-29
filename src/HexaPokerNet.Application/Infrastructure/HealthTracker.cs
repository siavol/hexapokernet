namespace HexaPokerNet.Application.Infrastructure;

public class HealthTracker : IHealthProvider
{
    public HealthStatus HealthStatus { get; private set; } = HealthStatus.Starting;

    public virtual void ReportHealthStatus(HealthStatus status)
    {
        HealthStatus = HealthStatus switch
        {
            HealthStatus.Starting => status,
            HealthStatus.Healthy when status == HealthStatus.Starting => throw new InvalidOperationException(
                $"Can not transit health from {HealthStatus.Healthy} to {HealthStatus.Starting}"),
            HealthStatus.Healthy => status,
            _ => status
        };
    }
}
