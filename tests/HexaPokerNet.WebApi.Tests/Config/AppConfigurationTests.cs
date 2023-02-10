using HexaPokerNet.Adapter;

namespace HexaPokerNet.WebApi.Tests.Config;

[TestFixture]
public class AppConfigurationTests
{
    [Test]
    public void RepositoryKind_Returns_InMemory_ByDefault()
    {
        Environment.SetEnvironmentVariable("HPN_WRITABLE_REPO", null);
        var config = new AppConfiguration();
        Assert.That(config.RepositoryKind, Is.EqualTo(EWritableRepository.InMemory));
    }

    [Test]
    public void RepositoryKind_Returns_Kafka_WhenEnvVariableSetToKafka()
    {
        Environment.SetEnvironmentVariable("HPN_WRITABLE_REPO", "Kafka");
        var config = new AppConfiguration();
        Assert.That(config.RepositoryKind, Is.EqualTo(EWritableRepository.Kafka));
    }

    [Test]
    public void RepositoryKind_Throws_WhenEnvVariableValueIsUnknown()
    {
        Environment.SetEnvironmentVariable("HPN_WRITABLE_REPO", "Wtf");
        var config = new AppConfiguration();
        Assert.That(() => config.RepositoryKind, Throws.Exception);
    }
}