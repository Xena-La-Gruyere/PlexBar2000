using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PlexClient.Models
{
    public partial class Directory
    {
        [JsonPropertyName("allowSync")]
        public bool AllowSync { get; set; }

        [JsonPropertyName("art")]
        public string Art { get; set; }

        [JsonPropertyName("composite")]
        public string Composite { get; set; }

        [JsonPropertyName("filters")]
        public bool Filters { get; set; }

        [JsonPropertyName("refreshing")]
        public bool Refreshing { get; set; }

        [JsonPropertyName("thumb")]
        public string Thumb { get; set; }

        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("agent")]
        public string Agent { get; set; }

        [JsonPropertyName("scanner")]
        public string Scanner { get; set; }

        [JsonPropertyName("language")]
        public string Language { get; set; }

        [JsonPropertyName("uuid")]
        public Guid Uuid { get; set; }

        [JsonPropertyName("updatedAt")]
        public long UpdatedAt { get; set; }

        [JsonPropertyName("createdAt")]
        public long CreatedAt { get; set; }

        [JsonPropertyName("scannedAt")]
        public long ScannedAt { get; set; }

        [JsonPropertyName("content")]
        public bool Content { get; set; }

        [JsonPropertyName("directory")]
        public bool IsDirectory { get; set; }

        [JsonPropertyName("contentChangedAt")]
        public long ContentChangedAt { get; set; }

        [JsonPropertyName("hidden")]
        public long Hidden { get; set; }

        [JsonPropertyName("Location")]
        public Location[] Location { get; set; }
    }

    public partial class Location
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("path")]
        public string Path { get; set; }
    }
}
