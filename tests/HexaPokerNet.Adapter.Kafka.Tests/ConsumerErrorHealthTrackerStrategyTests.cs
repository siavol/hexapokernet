using Confluent.Kafka;
using HexaPokerNet.Application.Infrastructure;
using Moq;

namespace HexaPokerNet.Adapter.Kafka.Tests;

[TestFixture]
public class ConsumerErrorHealthTrackerStrategyTests
{
    private ConsumerErrorHealthTrackerStrategy _strategy;
    private HealthTracker _healthTracker;
    private Mock<IKafkaEntityEventConsumerErrorStrategy> _innerStrategyMock;

    [SetUp]
    public void Setup()
    {
        _innerStrategyMock = new Mock<IKafkaEntityEventConsumerErrorStrategy>();
        _healthTracker = new HealthTracker();

        var healthySilenceTimeout = TimeSpan.FromMilliseconds(500);
        _strategy = new ConsumerErrorHealthTrackerStrategy(
            _healthTracker,
            _innerStrategyMock.Object,
            healthySilenceTimeout);
    }

    [Test]
    public void ShouldKeepStartingHealthWhenFirstConsumptionFails()
    {
        _strategy.GetErrorResult(CreateConsumeException());
        _strategy.GetErrorResult(CreateConsumeException());

        Assert.That(_healthTracker.HealthStatus, Is.EqualTo(HealthStatus.Starting));
    }

    [Test]
    public async Task ShouldReportHealthyStatusWhenNoConsumptionErrorsWithinTimeout()
    {
        _strategy.GetErrorResult(CreateConsumeException());
        _strategy.GetErrorResult(CreateConsumeException());
        await Task.Run(() =>
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(1000));

            Assert.That(_healthTracker.HealthStatus, Is.EqualTo(HealthStatus.Healthy));
        });
    }

    private static ConsumeException CreateConsumeException()
    {
        return new ConsumeException(new ConsumeResult<byte[], byte[]>(), new Error(ErrorCode.Unknown));
    }
}