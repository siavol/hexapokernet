using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        var jsonBody = JToken.Parse(body);

        Assert.That(jsonBody["title"].Value<string>(), Is.EqualTo("My test story"));
        Assert.That(jsonBody["id"].Value<string>(), Is.Not.Empty);
    }
}
