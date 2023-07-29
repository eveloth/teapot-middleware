using Microsoft.AspNetCore.Builder;

namespace TeapotMiddleware;

/// <summary>
/// Extends <see cref="IApplicationBuilder"/> with methods used to configure <see cref="TeapotMiddleware"/>.
/// </summary>
public static class TeapotMiddlewareExtensions
{
    /// <summary>
    /// Adds a <see cref="TeapotMiddleware"/> to the middleware pipeline
    /// that will short-circuit it if a GET request has a body and return 418 "I'm a teapot".
    /// Add this in <c>Program.cs</c> (starting with .NET 6) or in <c>Startup.cs</c> (prior to .NET 6)
    /// where you want it to be placed. (And you probably want it to be placed after UseAuthentication.)
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <returns>The application builder.</returns>
    public static IApplicationBuilder UseTeapot(this IApplicationBuilder app) =>
        app.UseMiddleware<TeapotMiddleware>();
}