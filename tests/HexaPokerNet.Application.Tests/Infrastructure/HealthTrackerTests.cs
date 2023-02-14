using HexaPokerNet.Application.Infrastructure;
using NUnit.Framework;

namespace HexaPokerNet.Application.Tests.Infrastructure;

[TestFixture]
public class HealthTrackerTests
{
    [Test]
    public void ShouldReturnStartingWhenNoProvidersRegistered()
    {
        var healthTracker = new HealthTracker();
        Assert.That(healthTracker.HealthStatus, Is.EqualTo(HealthStatus.Starting));
    }

    [Test]
    public void ShouldReturnHealthyWhenReportedHealthyStatus()
    {
        var healthTracker = new HealthTracker();
        healthTracker.ReportHealthStatus(HealthStatus.Healthy);

        Assert.That(healthTracker.HealthStatus, Is.EqualTo(HealthStatus.Healthy));
    }
    
    [TestFixture]
    public class WhenHealthStatusIsHealthy
    {
        private HealthTracker _healthTracker;

        [SetUp]
        public void Setup()
        {
            _healthTracker = new HealthTracker();
            _healthTracker.ReportHealthStatus(HealthStatus.Healthy);
        }

        [Test]
        public void ShouldThrowWhenReportingStarting()
        {
            Assert.That(() => _healthTracker.ReportHealthStatus(HealthStatus.Starting), Throws.Exception);
        }
    }
}