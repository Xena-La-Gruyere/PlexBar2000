using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PlexClient.Models;

namespace PlexClient
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
    }
}
