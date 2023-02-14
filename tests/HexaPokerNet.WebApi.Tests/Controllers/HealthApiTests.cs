using System.Net;
using HexaPokerNet.Application.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;

namespace HexaPokerNet.WebApi.Tests.Controllers;

[TestFixture]
public class HealthApiTests
{
    private WebApplicationFactory<Program> _webApplicationFactory;
    private HttpClient _client;
    private Mock<IHealthProvider> _healthProviderMock;

    [SetUp]
    public void Setup()
    {
        Environment.SetEnvironmentVariable("HPN_WRITABLE_REPO", "InMemory");
        _webApplicationFactory = new WebApplicationFactory<Program>();

        var aggregatedHealthProvider = _webApplicationFactory.Services.GetService<AggregatedHealthProvider>();
        _healthProviderMock = new Mock<IHealthProvider>();
        aggregatedHealthProvider.RegisterHealthProvider(_healthProviderMock.Object);

        _client = _webApplicationFactory.CreateClient();
    }

    [Test]
    public async Task GetHealthReturnsOkWhenAppIsHealthy()
    {
        _healthProviderMock.Setup(_ => _.HealthStatus).Returns(HealthStatus.Healthy);
        var response = await _client.GetAsync("/health");
        response.EnsureSuccessStatusCode();
    }

    [Test]
    public async Task GetHealthReturnsHttp503ServiceUnavailableWhenAppIsStarting()
    {
        _healthProviderMock.Setup(_ => _.HealthStatus).Returns(HealthStatus.Starting);
        var response = await _client.GetAsync("/health");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.ServiceUnavailable));
    }
}