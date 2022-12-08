using HexaPokerNet.Application.Repositories;
using HexaPokerNet.Domain;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Linq;

namespace HexaPokerNet.WebApi.Tests;

[TestFixture]
public class StoryApiTests
{
    private readonly WebApplicationFactory<Program> _webApplicationFactory;
    private HttpClient _client;

    public StoryApiTests()
    {
        _webApplicationFactory = new WebApplicationFactory<Program>();
    }

    [SetUp]
    public void Setup()
    {
        _client = _webApplicationFactory.CreateClient();
    }

    [Test]
    public async Task PostStoryReturnsOk()
    {
        var response = await _client.PostAsync("/story",
            JsonContent.Create(new
            {
                title = "My test story"
            }));

        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        var jsonBody = JToken.Parse(body);

        Assert.That(jsonBody["id"].Value<string>(), Is.Not.Empty);
    }

    [Test]
    public async Task GetStoryByIdReturnsOk()
    {
        const string storyId = "story1";
        var writableRepository = _webApplicationFactory.Services.GetService<IWritableRepository>();
        await writableRepository!.AddStory(new Story(storyId, "My test story"));

        var response = await _client.GetAsync($"story/{storyId}");
        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadAsStringAsync();
        var jsonBody = JToken.Parse(body);

        Assert.That(jsonBody["title"].Value<string>(), Is.EqualTo("My test story"));
        Assert.That(jsonBody["id"].Value<string>(), Is.EqualTo(storyId));
    }

    [Test]
    public async Task GetStoryByIdReturnsNotFound()
    {
        var response = await _client.GetAsync("story/not-existing");
        Assert.That(response, Is.InstanceOf<HttpResponseMessage>());
    }
}
