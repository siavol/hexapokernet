using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;

namespace HexaPokerNet.WebApi.Tests;

[TestFixture]
public class StoryApiTests
{
    private readonly WebApplicationFactory<Program> _webApplicationFactory;

    public StoryApiTests()
    {
        _webApplicationFactory = new WebApplicationFactory<Program>();
    }

    [Test]
    public async Task PostStoryReturnsOk()
    {
        var client = _webApplicationFactory.CreateClient();
        var response = await client.PostAsync("/story",
            JsonContent.Create(new
            {
                title = "My test story"
            }));

        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        Assert.That(body, Is.EqualTo(JsonConvert.SerializeObject(new
        {
            title = "My test story"
        })));
    }
}
