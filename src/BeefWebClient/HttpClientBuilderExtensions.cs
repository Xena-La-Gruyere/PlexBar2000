using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;

namespace BeefWebClient
{
    public static class HttpClientBuilderExtensions
    {
        public static IHttpClientBuilder AddBeefWebClientService(this IHttpClientBuilder builder, string baseUrl)
        {
            return builder.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler()
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
                })
                .AddTypedClient<IBeefWebService, BeefWebService>();
        }
    }
}
