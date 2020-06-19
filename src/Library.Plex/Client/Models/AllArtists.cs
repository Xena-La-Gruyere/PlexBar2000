using System;
using System.Text.Json.Serialization;

namespace PlexClient.Client.Models
{
    public partial class AllArtists
    {
        [JsonPropertyName("MediaContainer")]
        public AllArtistsContainer MediaContainer { get; set; }
    }

    public partial class AllArtistsContainer
    {
        [JsonPropertyName("size")]
        public long Size { get; set; }

        [JsonPropertyName("allowSync")]
        public bool AllowSync { get; set; }

        [JsonPropertyName("art")]
        public string Art { get; set; }

        [JsonPropertyName("identifier")]
        public string Identifier { get; set; }

        [JsonPropertyName("librarySectionID")]
        public long LibrarySectionId { get; set; }

        [JsonPropertyName("librarySectionTitle")]
        public string LibrarySectionTitle { get; set; }

        [JsonPropertyName("librarySectionUUID")]
        public Guid LibrarySectionUuid { get; set; }

        [JsonPropertyName("mediaTagPrefix")]
        public string MediaTagPrefix { get; set; }

        [JsonPropertyName("mediaTagVersion")]
        public long MediaTagVersion { get; set; }

        [JsonPropertyName("nocache")]
        public bool Nocache { get; set; }

        [JsonPropertyName("thumb")]
        public string Thumb { get; set; }

        [JsonPropertyName("title1")]
        public string Title1 { get; set; }

        [JsonPropertyName("title2")]
        public string Title2 { get; set; }

        [JsonPropertyName("viewGroup")]
        public string ViewGroup { get; set; }

        [JsonPropertyName("viewMode")]
        public long ViewMode { get; set; }

        [JsonPropertyName("Metadata")]
        public Artist[] Metadata { get; set; }
    }
}
