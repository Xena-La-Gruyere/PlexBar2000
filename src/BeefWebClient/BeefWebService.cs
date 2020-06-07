using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using BeefWebClient.Models;
using BeefWebClient.Parameters;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace BeefWebClient
{
    public class BeefWebService : IBeefWebService
    {
        private readonly HttpClient _client;
        private readonly ILogger<BeefWebService> _logger;

        public BeefWebService(HttpClient client, ILogger<BeefWebService> logger)
        {
            _client = client;
            _logger = logger;
            if (client.BaseAddress is null)
                throw new ArgumentException(paramName: nameof(client), message: $"{nameof(client)}.{nameof(client.BaseAddress)} must be set.");
        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            if (fi.GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] attributes && attributes.Any())
            {
                return attributes.First().Description;
            }

            return value.ToString();
        }

        public async Task<PlayerStateResult> GetPlayerState(params TrackInfoFields[] fields)
        {
            _logger.LogDebug($"{nameof(GetPlayerState)} called with fileds=[{string.Join(",", fields.Select(f => f.ToString()))}]");

            var requestUri = QueryHelpers.AddQueryString("player", "columns", string.Join(",", fields.Select(f => GetEnumDescription(f))));

            using var response = await _client.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            return JsonSerializer.Deserialize<PlayerStateResult>(await response.Content.ReadAsStringAsync());
        }

        private string BuildQueryForSetPlayerState(SetPlayerStateParameters parameters)
        {
            var queryParams = new Dictionary<string, string>();
            if (parameters.Volume.HasValue)
                queryParams.Add("volume", parameters.Volume.Value.ToString("F2", CultureInfo.InvariantCulture));

            if (parameters.IsMuted.HasValue)
                queryParams.Add("isMuted", parameters.IsMuted.Value.ToString());

            if (parameters.Position.HasValue)
                queryParams.Add("position", parameters.Position.Value.ToString());

            if (parameters.RelativePosition.HasValue)
                queryParams.Add("relativePosition", parameters.RelativePosition.Value.ToString());

            if (parameters.PlaybackMode.HasValue)
                queryParams.Add("playbackMode", parameters.PlaybackMode.Value.ToString());

            return QueryHelpers.AddQueryString("player", queryParams);
        }

        public async Task SetPlayerState(SetPlayerStateParameters parameters)
        {
            var requestUri = BuildQueryForSetPlayerState(parameters);

            using var response = await _client.PostAsync(requestUri, new StringContent(string.Empty));
            response.EnsureSuccessStatusCode();
        }

        public async Task Play()
        {
            using var response = await _client.PostAsync("player/play", new StringContent(string.Empty));
            response.EnsureSuccessStatusCode();
        }

        public async Task PlayRandom()
        {
            using var response = await _client.PostAsync("player/play/random", new StringContent(string.Empty));
            response.EnsureSuccessStatusCode();
        }

        public async Task Next()
        {
            using var response = await _client.PostAsync("player/next", new StringContent(string.Empty));
            response.EnsureSuccessStatusCode();
        }

        public async Task Previous()
        {
            using var response = await _client.PostAsync("player/previous", new StringContent(string.Empty));
            response.EnsureSuccessStatusCode();
        }

        public async Task Stop()
        {
            using var response = await _client.PostAsync("player/stop", new StringContent(string.Empty));
            response.EnsureSuccessStatusCode();
        }

        public async Task Pause()
        {
            using var response = await _client.PostAsync("player/pause", new StringContent(string.Empty));
            response.EnsureSuccessStatusCode();
        }

        public async Task TogglePause()
        {
            using var response = await _client.PostAsync("player/toggle", new StringContent(string.Empty));
            response.EnsureSuccessStatusCode();
        }

        public async Task Play(string playlistId, uint index)
        {
            using var response = await _client.PostAsync($"player/play/{playlistId}/{index}", new StringContent(string.Empty));
            response.EnsureSuccessStatusCode();
        }

        public async Task<PlaylistInfoResult> GetPlaylists()
        {
            using var response = await _client.GetAsync("playlists");
            response.EnsureSuccessStatusCode();
            return JsonSerializer.Deserialize<PlaylistInfoResult>(await response.Content.ReadAsStringAsync());
        }

        public async Task SetCurrentPlaylist(string id)
        {
            var requestUri = QueryHelpers.AddQueryString("playlists", "current", id);
            using var response = await _client.PostAsync(requestUri, new StringContent(string.Empty));
            response.EnsureSuccessStatusCode();
        }


        private string BuildRequestUriForAddPlaylist(string title, uint? index = null)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["title"] = title
            };
            if (index.HasValue)
                queryParams.Add("index", index.ToString());

            return QueryHelpers.AddQueryString("playlists/add", queryParams);
        }

        public async Task AddPlaylist(string title, uint? index = null)
        {
            var requestUri = BuildRequestUriForAddPlaylist(title, index);
            using var response = await _client.PostAsync(requestUri, new StringContent(string.Empty));
            response.EnsureSuccessStatusCode();
        }

        public async Task RemovePlaylist(string id)
        {
            using var response = await _client.PostAsync($"playlists/remove/{id}", new StringContent(string.Empty));
            response.EnsureSuccessStatusCode();
        }

        public async Task MovePlaylist(string id, uint index)
        {
            using var response = await _client.PostAsync($"playlists/move/{id}/{index}", new StringContent(string.Empty));
            response.EnsureSuccessStatusCode();
        }

        public async Task<PlaylistItemsResult> GetPlaylistItems(string id, int offset, int count, params TrackInfoFields[] fields)
        {
            var requestUri = QueryHelpers.AddQueryString(
                $"playlists/{id}/items/{offset}:{count}", 
                "columns", 
                string.Join(",", fields.Select(f => GetEnumDescription(f))));

            using var response = await _client.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            return JsonSerializer.Deserialize<PlaylistItemsResult>(await response.Content.ReadAsStringAsync());
        }

        public async Task RenamePlaylist(string id, string title)
        {
            var requestUri = QueryHelpers.AddQueryString($"playlists/{id}", "title", title);
            using var response = await _client.PostAsync(requestUri, new StringContent(string.Empty));
            response.EnsureSuccessStatusCode();
        }

        public async Task ClearPlaylist(string id)
        {
            using var response = await _client.PostAsync($"playlists/{id}/clear", new StringContent(string.Empty));
            response.EnsureSuccessStatusCode();
        }


        private string BuildRequestUriForAddItem(string id, bool async, int? position)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["async"] = async.ToString()
            };
            if (position.HasValue)
                queryParams.Add("position", position.Value.ToString());

            return QueryHelpers.AddQueryString($"playlists/{id}/items/add", queryParams);
        }

        public async Task AddItem(string id, bool async, int? position, params string[] paths)
        {
            var body = new ByteArrayContent(JsonSerializer.SerializeToUtf8Bytes(paths));
            var requestUri = BuildRequestUriForAddItem(id, async, position);

            using var response = await _client.PostAsync(requestUri, body);
            response.EnsureSuccessStatusCode();
        }

    }

}
