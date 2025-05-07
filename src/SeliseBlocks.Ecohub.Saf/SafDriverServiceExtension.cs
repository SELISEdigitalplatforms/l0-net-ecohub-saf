using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using SeliseBlocks.Ecohub.Saf.Services;

namespace SeliseBlocks.Ecohub.Saf;

/// <summary>
/// Provides extension methods for registering SAF driver services in the dependency injection container.
/// </summary>
public static class SafDriverServiceExtension
{
    /// <summary>
    /// Registers SAF driver services, including HTTP clients and scoped services, in the dependency injection container.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the services will be added.</param>
    /// <param name="baseUrl">The base URL for the SAF API.</param>
    public static void AddSafDriverServices(this IServiceCollection services, string baseUrl)
    {
        // Registers an HTTP client for making requests to the SAF API.
        services.AddHttpClient<IHttpRequestGateway, HttpRequestGateway>(client =>
        {
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        });

        // Registers scoped services for SAF authentication, API, and event handling.
        services.AddScoped<ISafAuthService, SafAuthService>();
        services.AddScoped<ISafApiService, SafApiService>();
        services.AddScoped<ISafEventService, SafEventService>();
    }
}
