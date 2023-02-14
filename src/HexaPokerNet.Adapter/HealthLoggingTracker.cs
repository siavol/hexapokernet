using HexaPokerNet.Application.Infrastructure;
using Microsoft.Extensions.Logging;

namespace HexaPokerNet.Adapter;

public class HealthLoggingTracker : HealthTracker
{
    private readonly ILogger _logger;

    public HealthLoggingTracker(ILogger logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override void ReportHealthStatus(HealthStatus status)
    {
        _logger.LogInformation("Reporting health status {Status}", status);
        base.ReportHealthStatus(status);
    }
}