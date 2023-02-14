namespace HexaPokerNet.Application.Infrastructure;

public sealed class HealthTracker: IHealthProvider
{
    private HealthStatus _healthStatus = HealthStatus.Starting;
    public event EventHandler? HealthStatusChanged;

    public HealthStatus HealthStatus
    {
        get => _healthStatus;
        private set
        {
            if (_healthStatus != value)
            {
                _healthStatus = value;
                RaiseHealthStatusChanged();
            }
        }
    }

    public void ReportHealthStatus(HealthStatus status)
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

    private void RaiseHealthStatusChanged()
    {
        HealthStatusChanged?.Invoke(this, EventArgs.Empty);
    }
}
