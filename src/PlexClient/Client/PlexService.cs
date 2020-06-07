using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PlexClient.Client.Models;

namespace PlexClient.Client
{
    public class PlexService : IPlexService
    {
        private readonly HttpClient _client;
        private readonly string _plexToken;
        private readonly ILogger<PlexService> _logger;

        public PlexService(HttpClient client, IOptions<PlexOptions> options, ILogger<PlexService> logger)
        {
            _client = client;
            _plexToken = options.Value.PlexToken;
            _logger = logger;

            if (client.BaseAddress is null)
                throw new ArgumentException(paramName: nameof(client), message: $"{nameof(client)}.{nameof(client.BaseAddress)} must be set.");

            if (string.IsNullOrEmpty(options.Value.PlexToken))
                throw new ArgumentException(paramName: nameof(options.Value.PlexToken), message: $"{nameof(options.Value.PlexToken)} must be set.");
        }

        public string GetThumbnailUri(string resource)
        {
            var uri = new Uri(_client.BaseAddress, resource).AbsoluteUri;

            return QueryHelpers.AddQueryString(uri, "X-Plex-Token", _plexToken);
        }

        public async Task<Sections> GetSections()
        {
            _logger.LogDebug($"{nameof(GetSections)} called");

            var requestUri = QueryHelpers.AddQueryString("library/sections", "X-Plex-Token", _plexToken);

            using var response = await _client.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            return JsonSerializer.Deserialize<Sections>(await response.Content.ReadAsStringAsync());
        }

        public async Task<AllArtists> GetAllArtists(string sectionKey)
        {
            _logger.LogDebug($"{nameof(GetAllArtists)} called with {nameof(sectionKey)}={sectionKey}");

            var requestUri = QueryHelpers.AddQueryString($"library/sections/{sectionKey}/all", "X-Plex-Token", _plexToken);

            using var response = await _client.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            return JsonSerializer.Deserialize<AllArtists>(await response.Content.ReadAsStringAsync());
        }

        public async Task<ArtistDetail> GetArtist(string ratingKey)
        {
            _logger.LogDebug($"{nameof(GetArtist)} called with {nameof(ratingKey)}={ratingKey}");

            var requestUri = QueryHelpers.AddQueryString($"library/metadata/{ratingKey}/children", "X-Plex-Token", _plexToken);

            using var response = await _client.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            return JsonSerializer.Deserialize<ArtistDetail>(await response.Content.ReadAsStringAsync());
        }

        public async Task<byte[]> GetThumbnail(string resource)
        {
            _logger.LogDebug($"{nameof(GetThumbnail)} called with {nameof(resource)}={resource}");

            var requestUri = QueryHelpers.AddQueryString(resource, "X-Plex-Token", _plexToken);

            using var response = await _client.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsByteArrayAsync();
        }
    }
}
