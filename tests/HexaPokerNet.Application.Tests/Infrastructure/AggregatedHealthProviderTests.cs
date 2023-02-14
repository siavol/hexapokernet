using HexaPokerNet.Application.Infrastructure;
using Moq;

namespace HexaPokerNet.Application.Tests.Infrastructure;

[TestFixture]
public class AggregatedHealthProviderTests
{
    [Test]
    public void ShouldReturnStartingWhenNoHealthTrackersRegistered()
    {
        var aggregatedHealthProvider = new AggregatedHealthProvider();
        Assert.That(aggregatedHealthProvider.HealthStatus, Is.EqualTo(HealthStatus.Starting));
    }

    [Test]
    public void ShouldReturnHealthStatusOfRegisteredTracker()
    {
        var aggregatedHealthProvider = new AggregatedHealthProvider();
        var healthProviderMock = new Mock<IHealthProvider>();
        aggregatedHealthProvider.RegisterHealthProvider(healthProviderMock.Object);
        healthProviderMock.Setup(_ => _.HealthStatus).Returns(HealthStatus.Healthy);
        
        Assert.That(aggregatedHealthProvider.HealthStatus, Is.EqualTo(HealthStatus.Healthy));
    }
}