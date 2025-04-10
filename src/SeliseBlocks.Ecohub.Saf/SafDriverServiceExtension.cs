using System;
using Microsoft.Extensions.DependencyInjection;

namespace SeliseBlocks.Ecohub.Saf;

public static class SafDriverServiceExtension
{
    public static void RegisterSafDriverServices(this IServiceCollection services)
    {
        services.AddSingleton<HttpClient>();
        services.AddSingleton<IHttpRequestGateway, HttpRequestGateway>();
        services.AddSingleton<ISafAuthService, SafAuthService>();
        services.AddSingleton<ISafDriverService, SafDriverService>();

    }
}
