using Microsoft.AspNetCore.Mvc.Testing;

namespace HexaPokerNet.WebApi.Tests.Controllers;

[TestFixture]
public class HealthApiTests
{
    private WebApplicationFactory<Program> _webApplicationFactory;
    private HttpClient _client;

    [SetUp]
    public void Setup()
    {
        Environment.SetEnvironmentVariable("HPN_WRITABLE_REPO", "InMemory");
        _webApplicationFactory = new WebApplicationFactory<Program>();
        _client = _webApplicationFactory.CreateClient();
    }
    
    [Test]
    public async Task GetHealthReturnsOk()
    {
        var response = await _client.GetAsync("/health");
        response.EnsureSuccessStatusCode();
    }
}