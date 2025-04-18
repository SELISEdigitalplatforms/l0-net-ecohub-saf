using System;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;

namespace SeliseBlocks.Ecohub.Saf;

public static class SafDriverServiceExtension
{
    public static void RegisterSafDriverServices(this IServiceCollection services, string baseUrl)
    {
        services.AddHttpClient<IHttpRequestGateway, HttpRequestGateway>(client =>
        {
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        });

        services.AddScoped<ISafAuthService, SafAuthService>();
        services.AddScoped<ISafApiService, SafApiService>();
        services.AddScoped<ISafEventService, SafEventService>();

    }
}
