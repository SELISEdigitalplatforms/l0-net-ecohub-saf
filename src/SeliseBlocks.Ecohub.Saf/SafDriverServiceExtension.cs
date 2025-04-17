using System;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;

namespace SeliseBlocks.Ecohub.Saf;

public static class SafDriverServiceExtension
{
    public static void RegisterSafDriverServices(this IServiceCollection services, string baseUrl)
    {
        services.AddHttpClient("ApiClient", client =>
        {
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        });
        services.AddScoped<IHttpRequestGateway, HttpRequestGateway>();
        services.AddScoped<ISafAuthService, SafAuthService>();
        services.AddScoped<ISafDriverService, SafDriverService>();

    }
}
