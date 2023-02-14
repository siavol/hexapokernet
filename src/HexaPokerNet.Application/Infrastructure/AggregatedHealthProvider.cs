namespace HexaPokerNet.Application.Infrastructure;

public class AggregatedHealthProvider : IHealthProvider
{
    private readonly List<IHealthProvider> _healthProviders = new();

    public HealthStatus HealthStatus
    {
        get
        {
            if (_healthProviders.Count == 0)
            {
                return HealthStatus.Healthy;
            }

            if (_healthProviders.All(_ => _.HealthStatus == HealthStatus.Healthy))
            {
                return HealthStatus.Healthy;
            }

            if (_healthProviders.Any(_ => _.HealthStatus == HealthStatus.Starting))
            {
                return HealthStatus.Starting;
            }

            throw new InvalidOperationException();
        }
    }

    public void RegisterHealthProvider(IHealthProvider healthProvider)
    {
        if (healthProvider == null) throw new ArgumentNullException(nameof(healthProvider));
        _healthProviders.Add(healthProvider);
    }
}