using System.Net;
using System.Text;
using System.Text.Json;

namespace TeapotMiddleware.Tests;

public class TeapotMiddlewareIntegrationTests : IClassFixture<HostFixture>
{
    private readonly HostFixture _hostFixture;

    public TeapotMiddlewareIntegrationTests(HostFixture hostFixture)
    {
        _hostFixture = hostFixture;
    }

    [Fact]
    public async Task TeapotMiddleware_ShouldReturn418_WhenGetRequestHasABody()
    {
        var response = await _hostFixture.Client.SendAsync(
            new HttpRequestMessage
            {
                RequestUri = new Uri("/"),
                Method = HttpMethod.Get,
                Content = new StringContent("testcontent")
            }
        );

        Assert.Equal(418, (int)response.StatusCode);
    }

    [Fact]
    public async Task TeapotMiddleware_ShouldReturn200_WhenGetRequestDoesNotHaveABody()
    {
        var response = await _hostFixture.Client.GetAsync("/");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task TeapotMiddleware_ShouldReturnTheInputSent_WhenPostRequestIsReceived()
    {
        const string input = "testinput";

        var content = new StringContent(
            JsonSerializer.Serialize(input),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _hostFixture.Client.PostAsync("/", content);
        var responseString = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(input, responseString);
    }

    [Fact]
    public async Task TeapotMiddleware_ShouldReturn405_WhenMethodIsNotSupported()
    {
        var response = await _hostFixture.Client.DeleteAsync("/");
        Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);
    }

    [Fact]
    public async Task TeapotMiddleware_ShouldReturn404_WhenRouteIsUnknown()
    {
        var response = await _hostFixture.Client.GetAsync("/unknown");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}