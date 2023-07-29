using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;

namespace TeapotMiddleware.Tests;

// ReSharper disable once ClassNeverInstantiated.Global
public class HostFixture : IAsyncDisposable
{
    private readonly WebApplication _app;

    public HttpClient Client { get; set; }

    public HostFixture()
    {
        var builder = WebApplication.CreateBuilder();
        builder.WebHost.UseTestServer();
        var app = builder.Build();

        app.UseTeapot();
        app.MapGet("/", () => "Ok");
        app.MapPost("/", ([FromBody] string input) => $"{input}");

        _ = app.RunAsync();

        _app = app;
        Client = app.GetTestClient();
    }

    public async ValueTask DisposeAsync()
    {
        Client.Dispose();
        await _app.DisposeAsync();
    }
}