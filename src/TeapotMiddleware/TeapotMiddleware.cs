using Microsoft.AspNetCore.Http;

namespace TeapotMiddleware;

/// <summary>
/// A middleware that checks if an HTTP GET request has a body,
/// and if so, short-circuits the pipeline and returns a <see cref="StatusCodes.Status418ImATeapot"/>.
/// </summary>
public class TeapotMiddleware
{
    private readonly RequestDelegate _next;

    public TeapotMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (await GetRequestHasABody(context.Request))
        {
            context.Response.StatusCode = StatusCodes.Status418ImATeapot;
            return;
        }

        await _next(context);
    }

    /// <summary>
    /// An actual request checking method. Usually we need to call
    /// <see cref="HttpRequestRewindExtensions.EnableBuffering(HttpRequest)"/> but in this case it is unnecessary,
    /// so the request won't be buffred. If <see cref="HttpRequest.Body"/> is empty, we can simply discard it,
    /// and if it is not, there is no need to read it again.
    /// However big the request body is, this method reads only the first chunk of it since we are only interested in
    /// checking whether it contains any data. As a shallow research showed, this chunk should be around 21B
    /// so this shouldn't introduce a large performance overhead.
    /// </summary>
    /// <param name="request">An <see cref="HttpRequest"/> from the <see cref="HttpContext"/>.</param>
    /// <returns>True if a GET request has a body and false otherwise.</returns>
    private static async ValueTask<bool> GetRequestHasABody(HttpRequest request)
    {
        if (request.Method != HttpMethods.Get)
        {
            return false;
        }

        var reader = request.BodyReader;
        var readResult = await reader.ReadAsync();
        var buffer = readResult.Buffer;

        reader.AdvanceTo(buffer.Start, buffer.Start);
        await reader.CompleteAsync();

        return buffer.Length > 0;
    }
}