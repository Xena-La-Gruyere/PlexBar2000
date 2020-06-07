using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using PlexClient.Client;
using PlexClient.Library;

namespace PlexClient
{
    public static class HttpClientBuilderExtensions
    {
        public static IServiceCollection AddPlexClientService(this IServiceCollection builder, string baseUrl, string plexToken)
        {
            builder.Configure<PlexOptions>(o => o.PlexToken = plexToken);

            builder.AddSingleton<IPlexLibraryService, PlexLibraryService>();

            builder.AddHttpClient<IPlexService, PlexService>()
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                {
                    UseCookies = false,
                    AllowAutoRedirect = true,
                    AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip,
                    PreAuthenticate = false,
                    UseDefaultCredentials = false,
                })
                .ConfigureHttpClient(client =>
                {
                    client.BaseAddress = new Uri(baseUrl.TrimEnd('/') + "/", UriKind.Absolute);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                });

            return builder;
        }
    }
}
